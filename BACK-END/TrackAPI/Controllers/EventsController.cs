using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackAPI.Services;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {

        private readonly IAccessCodeService _accessCodeService;
        private readonly ITrackerService _trackerService;
        private readonly AppSettings _appSettings;
        public EventsController(IAccessCodeService accessCodeService, 
            ITrackerService trackerService, IOptions<AppSettings> options) {
            _accessCodeService = accessCodeService;
            _trackerService = trackerService;
            _appSettings = options.Value;
        }

        [Authorize]
        [HttpGet("accesscode")]
        public async Task<IActionResult> GetAccessCode() {
            try {

                var userId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimNames.USER_ID)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new ApplicationException("UserID cannot be null.");

                var accessCode = new AccessCodeModel {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    CreationTime = DateTime.UtcNow
                };
                await _accessCodeService.AddAsync(accessCode);

                var userTrackers = await _trackerService.GetUserTrackers(userId);
                var servers = userTrackers
                    .Select(t => _appSettings.Worker.GetHost(t.LastConnectedServer))
                    .Select(s => new object[] { s.IP, s.UserPort })
                    .GroupBy(a => a[0])
                    .Select(ag => ag.First())
                    .ToArray();

                return Ok(new {
                    AccessCode = accessCode.Id,
                    Servers = servers
                });

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

    }

}
