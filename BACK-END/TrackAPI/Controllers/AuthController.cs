using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Models;

namespace TrackAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase {

        
        public IActionResult Post(LoginModel model) {

            var user = HttpContext.User;

            return Ok(new { 
                Name = user.Claims.SingleOrDefault(c => c.Type == "GivenName")?.Value,
                Surname = user.Claims.SingleOrDefault(c => c.Type == "Surname")?.Value,
                UserName = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });
        }
    }
}
