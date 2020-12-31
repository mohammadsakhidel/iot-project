﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackAPI.Models.Polling;
using TrackAPI.Services;
using TrackAPI.Sockets;
using TrackLib.Commands;
using TrackLib.Constants;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class PollingController : ControllerBase {

        AppSettings _appSettings;
        ICommandExecutor _commandExecutor;
        ITrackerService _trackerService;
        public PollingController(IOptions<AppSettings> options, ICommandExecutor commandExecutor,
            ITrackerService trackerService) {
            _appSettings = options.Value;
            _commandExecutor = commandExecutor;
            _trackerService = trackerService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAllEvents(PollingInput input) {
            var cts = new CancellationTokenSource();
            try {

                #region Start Tasks:
                var timeoutTask = Task.Run(() => {
                    PollingEvent @event = null;
                    Thread.Sleep(_appSettings.Polling.TimeoutSeconds * 1000);
                    return @event;
                });

                var statusCheckTasks = new List<Task<PollingEvent>>();
                foreach (var tStatus in input.TrackersStatus) {
                    tStatus.Status ??= string.Empty;
                    if (tStatus.Status.ToLower() == TrackerStatusValues.ONLINE.ToLower()) {
                        statusCheckTasks.Add(WaitForOfflineEvent(tStatus.TrackerId, cts.Token));
                    } else if (tStatus.Status.ToLower() == TrackerStatusValues.OFFLINE.ToLower()) {
                        statusCheckTasks.Add(WaitForOnlineEvent(tStatus.TrackerId, cts.Token));
                    } else {
                        statusCheckTasks.Add(FindTrackerCurrentStatus(tStatus.TrackerId, cts.Token));
                    }
                }
                #endregion

                // Wait for an event or timeout:
                var completedTask = await Task.WhenAny(
                    statusCheckTasks.Concat(new Task<PollingEvent>[] { timeoutTask })
                );
                var isTimeout = completedTask == timeoutTask;
                var @event = completedTask.Result;


                // Cancel running tasks:
                cts.Cancel();

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                return Ok(new ApiResult {
                    Done = @event != null,
                    Data = @event != null ? JsonSerializer.Serialize(@event, jsonOptions) : string.Empty,
                    Error = isTimeout ? ErrorCodes.POLLING_TIMEOUT : string.Empty
                });

            } catch (Exception ex) {
                try { cts.Cancel(); } catch { }
                return ex.GetActionResult();
            }
        }


        #region Private Mehotds:
        private Task<PollingEvent> WaitForOnlineEvent(string trackerId, CancellationToken token) {
            return Task.Run(async () => {
                PollingEvent result = null;
                TrackerModel tracker = null;
                lock (_trackerService) {
                    tracker = _trackerService.GetAsync(trackerId).Result;
                }

                while (!token.IsCancellationRequested) {

                    // Send Request to check status:
                    var request = new CommandRequest(tracker.Id, CommandSet.COMMAND_GET_STATUS);
                    var host = _appSettings.Worker.GetHost(tracker.LastConnectedServer);
                    var response = await _commandExecutor.ExecuteAsync(request, host);

                    if (response != null && response.Done) {
                        result = new StatusChangedEvent(tracker.Id, TrackerStatusValues.ONLINE);
                        break;
                    }

                    Thread.Sleep(_appSettings.Polling.StatusCheckDelaySeconds * 1000);

                }
                return result;
            }, token);
        }

        private Task<PollingEvent> WaitForOfflineEvent(string trackerId, CancellationToken token) {
            return Task.Run(async () => {
                PollingEvent result = null;
                TrackerModel tracker = null;
                lock (_trackerService) {
                    tracker = _trackerService.GetAsync(trackerId).Result;
                }

                while (!token.IsCancellationRequested) {

                    // Send Request to check status:
                    var request = new CommandRequest(tracker.Id, CommandSet.COMMAND_GET_STATUS);
                    var host = _appSettings.Worker.GetHost(tracker.LastConnectedServer);
                    var response = await _commandExecutor.ExecuteAsync(request, host);

                    if (response != null && !response.Done && response.Error == ErrorCodes.TRACKER_OFFLINE) {
                        result = new StatusChangedEvent(
                            tracker.Id, 
                            TrackerStatusValues.OFFLINE, 
                            tracker.LastConnection.HasValue ? tracker.LastConnection.Value.ToString(Values.DATETIME_FORMAT) : string.Empty
                        );
                        break;
                    }

                    Thread.Sleep(_appSettings.Polling.StatusCheckDelaySeconds * 1000);

                }
                return result;
            }, token);
        }

        private Task<PollingEvent> FindTrackerCurrentStatus(string trackerId, CancellationToken token) {
            return Task.Run(async () => {
                PollingEvent result = null;
                TrackerModel tracker = null;
                lock (_trackerService) {
                    tracker = _trackerService.GetAsync(trackerId).Result;
                }

                while (!token.IsCancellationRequested) {

                    // Send Request to check status:
                    var request = new CommandRequest(tracker.Id, CommandSet.COMMAND_GET_STATUS);
                    var host = _appSettings.Worker.GetHost(tracker.LastConnectedServer);
                    var response = await _commandExecutor.ExecuteAsync(request, host);

                    if (response != null && response.Done) {
                        result = new StatusChangedEvent(tracker.Id, TrackerStatusValues.ONLINE);
                        break;
                    } else if (response != null && !response.Done && response.Error == ErrorCodes.TRACKER_OFFLINE) {
                        result = new StatusChangedEvent(
                            tracker.Id, 
                            TrackerStatusValues.OFFLINE,
                            tracker.LastConnection.HasValue ? tracker.LastConnection.Value.ToString(Values.DATETIME_FORMAT) : string.Empty
                        );
                        break;
                    }

                    Thread.Sleep(_appSettings.Polling.StatusCheckDelaySeconds * 1000);

                }
                return result;
            }, token);
        }
        #endregion
    }

}
