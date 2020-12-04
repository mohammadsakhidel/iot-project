using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Models;
using TrackAPI.Services;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class TrackersController : ControllerBase {

        #region -------------- CONSTRUCTION ---------------
        private readonly ITrackerService _trackerService;
        public TrackersController(ITrackerService trackerService) {
            _trackerService = trackerService;
        }
        #endregion

        #region -------------- GET ACTIONS ----------------
        [HttpGet]
        [Authorize(Policies.CanReadTracker)]
        public async Task<IActionResult> GetAsync(int skip = 0, int take = Values.PAGESIZE) {
            try {

                var trackers = await _trackerService.TakeAsync(skip, take);
                return Ok(trackers);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policies.CanReadTracker)]
        public async Task<IActionResult> GetAsync(string id) {
            try {

                var tracker = await _trackerService.GetAsync(id);
                if (tracker == null)
                    return NotFound();

                return Ok(tracker);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{trackerId}/reports/{date?}")]
        public async Task<IActionResult> Reports(string trackerId, string date) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound();

                var hasDate = DateTime.TryParse(date, out var reportDate);
                var reports = await _trackerService.GetReportsAsync(trackerId, hasDate ? reportDate : null);

                return Ok(reports);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

        #region -------------- POST ACTIONS ---------------
        [HttpPost]
        [Authorize(Policies.CanCreateTracker)]
        public async Task<IActionResult> PostAsync(TrackerModel model) {
            try {

                (var done, var trackerId) = await _trackerService.CreateAsync(model);

                var createdModel = await _trackerService.GetAsync(trackerId);
                return Created($"trackers/{trackerId}", createdModel);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("search")]
        [Authorize(Policies.CanReadTracker)]
        public async Task<IActionResult> Search(TrackerSearchModel model) {
            try {

                var users = await _trackerService.SearchAsync(model);

                return Ok(users);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

        #region -------------- PUT ACTIONS ----------------
        [HttpPut]
        [Authorize(Policies.CanUpdateTracker)]
        public async Task<IActionResult> PutAsync(TrackerModel model) {
            try {

                (var done, var error) = await _trackerService.UpdateAsync(model);
                if (!done)
                    throw new ApplicationException(error);

                return Ok();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

        #region ------------- DELETE ACTIONS --------------
        [HttpDelete("{id}")]
        [Authorize(Policies.CanDeleteTracker)]
        public async Task<IActionResult> Delete(string id) {
            try {

                var (done, error) = await _trackerService.RemoveAsync(id);
                if (!done)
                    throw new ApplicationException(error);

                return NoContent();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

    }

}
