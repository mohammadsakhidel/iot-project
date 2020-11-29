﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackAPI.Services;
using TrackAPI.Sockets;
using TrackLib.Constants;
using TrackLib.Commands;

namespace TrackAPI.Controllers {
    [Route("v1/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {

        private readonly ITrackerService _trackerService;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        public CommandsController(ITrackerService trackerService, ICommandExecutor commandExecutor,
            IUserService userService, IOptions<AppSettings> options) {

            _trackerService = trackerService;
            _commandExecutor = commandExecutor;
            _userService = userService;
            _appSettings = options.Value;

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync(ExecuteCommandModel model) {
            try {

                #region Validate:
                // Tracker
                var tracker = await _trackerService.GetAsync(model.TrackerId);
                if (tracker == null)
                    return BadRequest("Tracker ID is not valid.");

                // Command Type:
                var commandSet = CommandSets.All()[tracker.CommandSet];
                if (!commandSet.ContainsKey(model.CommandType))
                    return BadRequest("Invalid Command.");

                // User:
                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                var isAdming = HttpContext.User.Claims.Any(c => c.Type == ClaimNames.ISADMIN);
                if (!isAdming && (string.IsNullOrEmpty(userId) || tracker.UserId != userId))
                    return BadRequest("User not able to execute commands on this tracker.");
                #endregion

                #region Arrange:
                // Command
                var command = new CommandRequest {
                    TrackerID = model.TrackerId,
                    Type = commandSet[model.CommandType],
                    Payload = model.Payload ?? ""
                };

                // Host
                var host = _appSettings.Worker.UseTrackerLastConnection && !string.IsNullOrEmpty(tracker.LastConnectedServer)
                    ? tracker.LastConnectedServer : _appSettings.Worker.DefaultServer;
                #endregion

                #region Execute Command:
                var response = await _commandExecutor.SendAsync(command, host);
                if (response == null)
                    throw new ApplicationException("Command execution failed.");
                #endregion

                return Ok(new ApiResult {
                    Done = response.Done,
                    Data = response.Payload,
                    Error = response.Error
                });
            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("sets")]
        [Authorize]
        public IActionResult GetCommandSets() {
            try {

                var sets = CommandSets.All()
                    .ToDictionary(r => r.Key, r => r.Value.Select(kv => kv.Key)
                    .ToArray());

                return Ok(sets);
            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

    }
}