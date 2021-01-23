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

                return await ValidateAndExecuteCommandAsync(model);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        #region Commands Actions:
        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> PasswordCommand(ExecuteCommandModel model) {
            try {

                Func<Task> saveToConfigsFunc = async () => {

                    var tracker = await _trackerService.GetAsync(model.TrackerId);
                    var configs = tracker.GetConfigsDic();
                    configs[TrackerConfigFields.PASSWORD] = model.Payload;

                    await _trackerService.SaveConfigsAsync(model.TrackerId, JsonSerializer.Serialize(configs));

                };

                return await ValidateAndExecuteCommandAsync(model, saveToConfigsFunc);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("center")]
        [Authorize]
        public async Task<IActionResult> CenterNumberCommand(ExecuteCommandModel model) {
            try {

                Func<Task> saveToConfigsFunc = async () => {

                    var tracker = await _trackerService.GetAsync(model.TrackerId);
                    var configs = tracker.GetConfigsDic();
                    configs[TrackerConfigFields.CENTER] = model.Payload;

                    await _trackerService.SaveConfigsAsync(model.TrackerId, JsonSerializer.Serialize(configs));

                };

                return await ValidateAndExecuteCommandAsync(model, saveToConfigsFunc);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("call")]
        [Authorize]
        public async Task<IActionResult> MakeCallCommand(ExecuteCommandModel model) {
            try {

                Func<Task> saveToConfigsFunc = async () => {

                    var tracker = await _trackerService.GetAsync(model.TrackerId);
                    var configs = tracker.GetConfigsDic();
                    configs[TrackerConfigFields.CALL] = model.Payload;

                    await _trackerService.SaveConfigsAsync(model.TrackerId, JsonSerializer.Serialize(configs));

                };

                return await ValidateAndExecuteCommandAsync(model, saveToConfigsFunc);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadIntervalCommand(ExecuteCommandModel model) {
            try {

                Func<Task> saveToConfigsFunc = async () => {

                    var tracker = await _trackerService.GetAsync(model.TrackerId);
                    var configs = tracker.GetConfigsDic();
                    configs[TrackerConfigFields.UPLOAD] = model.Payload;

                    await _trackerService.SaveConfigsAsync(model.TrackerId, JsonSerializer.Serialize(configs));

                };

                return await ValidateAndExecuteCommandAsync(model, saveToConfigsFunc);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

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

                var commandType = CommandSet.COMMAND_GET_STATUS;
                var commandSet = CommandSet.Get(tracker.CommandSet, HttpContext.RequestServices);
                if (!commandSet.IsCommandSupported(commandType))
                    return BadRequest("Check status command not supported.");
                #endregion

                #region Arrange:
                var versionQueryCommand = commandSet[commandType];

                // Command
                var command = new CommandRequest(
                    tracker.Id,
                    versionQueryCommand,
                    string.Empty
                );

                // Host
                var host = _appSettings.Worker.GetHost(tracker.LastConnectedServer);
                #endregion

                #region Execute Command:
                var response = await _commandExecutor.ExecuteAsync(command, host);
                if (response == null)
                    throw new ApplicationException("Command execution failed.");
                #endregion

                var trackerModel = _mapper.Map<TrackerModel>(tracker);
                var errorData = (response.Error == ErrorCodes.TRACKER_OFFLINE ? tracker.LastConnection?.ToString(SharedValues.DATETIME_FORMAT) : string.Empty);

                return Ok(new ApiResult {
                    Done = response.Done,
                    Error = response.Error ?? string.Empty,
                    Data = response.Done ? JsonSerializer.Serialize(trackerModel) : errorData
                });

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("sets/{name?}")]
        [Authorize]
        public IActionResult GetCommandSets(string name) {
            try {

                if (!string.IsNullOrEmpty(name)) {
                    var set = CommandSet.Get(name, HttpContext.RequestServices);
                    if (set == null)
                        return NotFound($"Set '{name}' not found.");

                    return Ok(set.SupportedCommands.Select(c => c.CommonName).ToArray());

                } else {
                    var sets = CommandSet.GetAllSets(HttpContext.RequestServices)
                        .ToDictionary(
                            cs => cs.Name,
                            cs => cs.SupportedCommands.Select(c => c.CommonName).ToArray()
                        );
                    return Ok(sets);
                }
            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        #region Private Methods:
        private async Task<IActionResult> ValidateAndExecuteCommandAsync(ExecuteCommandModel model, params Func<Task>[] functions) {

            //Validate:
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Message);

            // Execute & Log:
            var response = await SendCommandToDeviceAsync(model);

            // Run Actions:
            if (response.Done && functions != null && functions.Any()) {
                foreach (var func in functions) {
                    await func();
                }
            }

            // Return:
            return Ok(new ApiResult {
                Done = response.Done,
                Data = response.Payload ?? string.Empty,
                Error = response.Error ?? string.Empty
            });

        }

        private async Task<(bool IsValid, string Message)> ValidateAsync(ExecuteCommandModel model) {

            // Tracker
            var tracker = await _trackerService.GetAsync(model.TrackerId);
            if (tracker == null)
                return (false, "Tracker ID is not valid.");

            // Command Type:
            var commandSet = CommandSet.Get(tracker.CommandSet, HttpContext.RequestServices);
            if (commandSet == null || !commandSet.IsCommandSupported(model.CommandType))
                return (false, "Invalid Command.");

            // User:
            var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
            var isAdming = HttpContext.User.Claims.Any(c => c.Type == ClaimNames.ISADMIN);
            if (!isAdming && (string.IsNullOrEmpty(userId) || tracker.UserId != userId))
                return (false, "User not able to execute commands on this tracker.");

            return (true, null);
        }

        private async Task<CommandResponse> SendCommandToDeviceAsync(ExecuteCommandModel model) {

            #region Arrange:
            // Tracker & User:
            var tracker = await _trackerService.GetAsync(model.TrackerId);
            var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;

            // Command
            var command = new CommandRequest(
                model.TrackerId,
                model.CommandType,
                model.Payload ?? ""
            );

            // Host
            var host = _appSettings.Worker.GetHost(tracker.LastConnectedServer);
            #endregion

            #region Execute & Log Command:
            // Send command:
            var response = await _commandExecutor.ExecuteAsync(command, host);

            // Add Log:
            var log = new CommandLogModel {
                TrackerId = tracker.Id,
                Type = command.Type,
                Payload = command.Payload,
                UserId = userId,
                Response = response != null ? JsonSerializer.Serialize(response) : "",
                CreationTime = DateTime.UtcNow.ToString(SharedValues.DATETIME_FORMAT)
            };
            await _commandService.AddLogAsync(log);

            if (response == null)
                throw new ApplicationException("Command execution failed.");
            #endregion

            return response;

        }
        #endregion

    }
}
