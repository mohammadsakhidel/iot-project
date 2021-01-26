using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Models;
using TrackAPI.Services;
using TrackLib.Commands;
using TrackLib.Constants;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class TrackersController : ControllerBase {

        #region -------------- CONSTRUCTION ---------------
        private readonly ITrackerService _trackerService;
        private readonly ICommandService _commandService;
        private readonly IUserService _userService;

        public TrackersController(ITrackerService trackerService,
            ICommandService commandService, IUserService userService) {
            _trackerService = trackerService;
            _commandService = commandService;
            _userService = userService;
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
        [Authorize(Policies.CanReadTracker)]
        public async Task<IActionResult> Messages(string trackerId, string date) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound();

                var hasDate = DateTime.TryParse(date, out var reportDate);
                var messages = await _trackerService.GetMessagesAsync(trackerId, hasDate ? reportDate : null);

                return Ok(messages);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{trackerId}/commandlogs/{date?}")]
        [Authorize(Policies.CanReadTracker)]
        public async Task<IActionResult> Logs(string trackerId, string date) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound();

                var hasDate = DateTime.TryParse(date, out var logsDate);
                var logs = await _commandService.GetLogsAsync(trackerId, hasDate ? logsDate : null);

                return Ok(logs);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<IActionResult> GetUserTrackers() {
            try {

                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new ApplicationException("UserID cannot be null.");

                var trackers = await _trackerService.GetUserTrackers(userId);

                // Set Commands Property:
                trackers.ForEach(t => {
                    var set = CommandSet.Get(t.CommandSet, HttpContext.RequestServices);
                    t.Commands = (set != null
                        ? set.SupportedCommands.Select(c => c.CommonName).ToArray()
                        : Array.Empty<string>()
                    );
                });

                return Ok(trackers);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{trackerId}/permissions")]
        [Authorize]
        public async Task<IActionResult> GetPermittedUsers(string trackerId) {
            try {

                // Tracker Valid?
                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound("Tracker not found.");

                // User Owner?
                var userId = HttpContext.User.Claims.SingleOrDefault(
                    c => c.Type == ClaimNames.USER_ID
                    )?.Value;
                if (userId != tracker.UserId)
                    return BadRequest("User is not tracker's owner.");

                var permittedUsers = await _trackerService.GetPermittedUsersAsync(trackerId);
                
                return Ok(permittedUsers.ToArray());


            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{trackerId}/configs")]
        [Authorize]
        public async Task<IActionResult> GetConfigs(string trackerId) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound("Invalid tracker ID");

                return Ok(tracker.GetConfigsDic());
                

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{trackerId}/configs/contacts")]
        [Authorize]
        public async Task<IActionResult> GetContacts(string trackerId) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound("Invalid tracker ID");

                var configs = tracker.GetConfigsDic();
                if (!configs.ContainsKey("contacts"))
                    return Ok(Array.Empty<object>());

                return Ok(configs["contacts"]);


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

        [HttpPost("{trackerId}/permissions")]
        [Authorize]
        public async Task<IActionResult> AddPermittedUserAsync(TrackerPermittedUserModel model) {
            try {

                // Validate Tracker & User IDs:
                var tracker = await _trackerService.GetAsync(model.TrackerId);
                if (tracker == null)
                    return BadRequest("Invalid tracker Id.");

                var userToBePermitted = _userService.GetAsync(model.UserId);
                if (userToBePermitted == null)
                    return BadRequest("Invalid user Id.");

                // Check caller user access:
                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (userId != tracker.UserId)
                    return BadRequest("Caller is not the owner.");

                await _trackerService.AddPermittedUser(model);

                return Ok();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPost("{trackerId}/configs/contacts")]
        [Authorize]
        public async Task<IActionResult> AddContact(string trackerId, ContactModel model) {
            try {

                // Validate Tracker ID:
                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return BadRequest("Invalid tracker Id.");

                // Remove Contact:
                var configs = tracker.GetConfigsDic();

                var contactsObj = configs["contacts"];
                var contactsJson = JsonSerializer.Serialize(contactsObj);
                var contacts = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(contactsJson);
                if (contacts.Any(c => c["number"] == model.Number))
                    return BadRequest(ErrorCodes.ALREADY_ADDED);

                contacts.Add(new Dictionary<string, string> {
                    { "name", model.Name },
                    { "number", model.Number }
                });
                configs["contacts"] = contacts;

                // Save:
                await _trackerService.SaveConfigsAsync(trackerId, JsonSerializer.Serialize(configs));

                return Ok();

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

        [HttpPut("{serialNumber}/user")]
        public async Task<IActionResult> AssignUser(string serialNumber) {
            try {

                var tracker = await _trackerService.FindBySerialAsync(serialNumber);
                if (tracker == null)
                    return NotFound();

                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new ApplicationException("UserID cannot be null.");

                var (done, messageOrAssignedTracker) = await _trackerService.AssignUser(tracker.Id, userId);
                if (!done)
                    return BadRequest(messageOrAssignedTracker);

                return Ok(messageOrAssignedTracker);

            } catch (Exception e) {
                return e.GetActionResult();
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

        [HttpDelete("{trackerId}/user")]
        public async Task<IActionResult> UnassignUser(string trackerId) {
            try {

                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return NotFound();

                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new ApplicationException("UserID cannot be null.");

                var (done, message) = await _trackerService.UnassignUser(trackerId, userId);
                if (!done)
                    return BadRequest(message);

                return Ok();

            } catch (Exception e) {
                return e.GetActionResult();
            }
        }

        [HttpDelete("{trackerId}/permissions/{userId}")]
        [Authorize]
        public async Task<IActionResult> RemovePermittedUserAsync(string trackerId, string userId) {
            try {

                // Validate Tracker & User IDs:
                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return BadRequest("Invalid tracker Id.");

                var userToBePermitted = _userService.GetAsync(userId);
                if (userToBePermitted == null)
                    return BadRequest("Invalid user Id.");

                // Check caller user access:
                var apiUserId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (apiUserId != tracker.UserId)
                    return BadRequest("Caller is not the owner.");

                await _trackerService.RemovePermittedUser(trackerId, userId);

                return Ok();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpDelete("{trackerId}/configs/contacts/{number}")]
        [Authorize]
        public async Task<IActionResult> RemoveContact(string trackerId, string number) {
            try {

                // Validate Tracker ID:
                var tracker = await _trackerService.GetAsync(trackerId);
                if (tracker == null)
                    return BadRequest("Invalid tracker Id.");

                // Remove Contact:
                var configs = tracker.GetConfigsDic();

                var contactsObj = configs["contacts"];
                var contactsJson = JsonSerializer.Serialize(contactsObj);
                var contacts = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(contactsJson);
                var toBeRemoved = contacts.Where(c => c["number"] == number).ToList();
                toBeRemoved.ForEach(c => contacts.Remove(c));

                configs["contacts"] = contacts;

                // Save:
                await _trackerService.SaveConfigsAsync(trackerId, JsonSerializer.Serialize(configs));

                return Ok();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

    }

}
