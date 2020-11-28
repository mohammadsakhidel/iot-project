using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackAPI.Services;

namespace TrackAPI.Controllers {
    [Route("v1/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase {

        private readonly IUserService _userService;
        public AuthController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> TokenAsync(LoginModel model) {
            try {
                (var isValid, var token) = await _userService.ValidateUserAsync(model.UserName, model.Password);
                if (!isValid)
                    return BadRequest("Invalid username or password.");

                return Ok(token);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
    }
}
