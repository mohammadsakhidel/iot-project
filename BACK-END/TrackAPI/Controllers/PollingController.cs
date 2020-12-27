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
using TrackAPI.Models.Polling;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class PollingController : ControllerBase {

        AppSettings _appSettings;
        public PollingController(IOptions<AppSettings> options) {
            _appSettings = options.Value;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAllEvents(PollingInput input) {
            try {

                var timeoutTask = Task.Delay(_appSettings.Polling.TimeoutSeconds * 1000);

                var statusChangeTask = Task.Run(async () => {

                    var statusCheckTasks = new List<Task<PollingEvent>>();
                    foreach (var currentStatus in input.TrackersStatus) {
                        if (currentStatus.Value.ToLower() == TrackerStatusValues.ONLINE.ToLower()) {
                            statusCheckTasks.Add(WaitForOfflineEvent(currentStatus.Key));
                        } else if (currentStatus.Value.ToLower() == TrackerStatusValues.OFFLINE.ToLower()) {
                            statusCheckTasks.Add(WaitForOnlineEvent(currentStatus.Key));
                        } else {
                            statusCheckTasks.Add(FindTrackerCurrentStatus(currentStatus.Key));
                        }
                    }
                    var completedTask = await Task.WhenAny(statusCheckTasks);
                    var statusChangedEvent = completedTask.Result;

                });

                await Task.WhenAny(timeoutTask, statusChangeTask);

                return Ok("done");

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }


        #region Private Mehotds:
        private Task<PollingEvent> WaitForOnlineEvent(string trackerId) {
            throw new NotImplementedException();
        }

        private Task<PollingEvent> WaitForOfflineEvent(string trackerId) {
            throw new NotImplementedException();
        }

        private Task<PollingEvent> FindTrackerCurrentStatus(string trackerId) {
            throw new NotImplementedException();
        }
        #endregion
    }

}
