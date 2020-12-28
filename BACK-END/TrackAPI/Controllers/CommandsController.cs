using Microsoft.AspNetCore.Authorization;
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
using AutoMapper;
using System.Text.Json;

namespace TrackAPI.Controllers {
    [Route("v1/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {

        private readonly ITrackerService _trackerService;
        private readonly ICommandService _commandService;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public CommandsController(ITrackerService trackerService, ICommandExecutor commandExecutor,
            ICommandService commandService, IMapper mapper, IOptions<AppSettings> options) {

            _trackerService = trackerService;
            _commandService = commandService;
            _commandExecutor = commandExecutor;
            _mapper = mapper;
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
                var commandSet = CommandSet.Get(tracker.CommandSet, HttpContext.RequestServices);
                if (!commandSet.IsCommandSupported(model.CommandType))
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

                #region Execute & Log Command:
                // Send command:
                var response = await _commandExecutor.SendAsync(command, host);

                // Add Log:
                var log = new CommandLogModel {
                    TrackerId = tracker.Id,
                    Type = command.Type,
                    Payload = command.Payload,
                    UserId = userId,
                    Response = response != null ? JsonSerializer.Serialize(response) : "",
                    CreationTime = DateTime.UtcNow.ToString(Values.DATETIME_FORMAT)
                };
                await _commandService.AddLogAsync(log);

                if (response == null)
                    throw new ApplicationException("Command execution failed.");
                #endregion

                return Ok(new ApiResult {
                    Done = response.Done,
                    Data = response.Payload ?? string.Empty,
                    Error = response.Error ?? string.Empty
                });
            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("connect/{id}")]
        [Authorize]
        public async Task<IActionResult> Connection(string id) {
            try {

                #region Validate:
                // Tracker
                var tracker = await _trackerService.GetAsync(id);
                if (tracker == null)
                    return BadRequest("Tracker ID is not valid.");

                // User:
                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                var isAdming = HttpContext.User.Claims.Any(c => c.Type == ClaimNames.ISADMIN);
                if (!isAdming && (string.IsNullOrEmpty(userId) || tracker.UserId != userId))
                    return BadRequest("User not able to execute commands on this tracker.");

                var commandType = CommandSet.GetAllQueryCommands().First();
                var commandSet = CommandSet.Get(tracker.CommandSet, HttpContext.RequestServices);
                if (!commandSet.IsCommandSupported(commandType))
                    return BadRequest("Version query command not supported.");
                #endregion

                #region Arrange:
                var versionQueryCommand = commandSet[commandType];

                // Command
                var command = new CommandRequest {
                    TrackerID = tracker.Id,
                    Type = versionQueryCommand,
                    Payload = ""
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

                var trackerModel = _mapper.Map<TrackerModel>(tracker);
                var errorData = (response.Error == ErrorCodes.TRACKER_OFFLINE ? tracker.LastConnection?.ToString(Values.DATETIME_FORMAT) : string.Empty);

                return Ok(new ApiResult {
                    Done = response.Done,
                    Error = response.Error ?? string.Empty,
                    Data = response.Done ? JsonSerializer.Serialize(trackerModel) : errorData
                });

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("sets")]
        [Authorize]
        public IActionResult GetCommandSets() {
            try {

                var sets = CommandSet.GetAllSets(HttpContext.RequestServices)
                    .ToDictionary(
                        cs => cs.Name,
                        cs => cs.SupportedCommands.Select(c => c.CommonName).ToArray()
                    );

                return Ok(sets);
            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

    }
}
