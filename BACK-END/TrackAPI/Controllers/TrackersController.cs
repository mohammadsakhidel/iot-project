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

        private readonly ITrackerService _trackerService;
        public TrackersController(ITrackerService trackerService) {
            _trackerService = trackerService;
        }

        [HttpPost]
        [Authorize(Policies.CanCreateTracker)]
        public async Task<IActionResult> PostAsync(TrackerModel model) {
            try {

                await _trackerService.CreateAsync(model);

                var createdModel = await _trackerService.GetAsync(model.Id);
                return Created($"trackers/{model.Id}", createdModel);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

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

    }

}
