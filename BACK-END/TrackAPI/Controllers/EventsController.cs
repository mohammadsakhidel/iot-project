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
    public class EventsController : ControllerBase {

        private readonly IAccessCodeService _accessCodeService;
        public EventsController(IAccessCodeService accessCodeService) {
            _accessCodeService = accessCodeService;
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

                return Ok(accessCode.Id);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

    }

}
