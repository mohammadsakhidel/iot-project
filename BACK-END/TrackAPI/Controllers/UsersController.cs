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

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Policies.CanCreateUser)]
        public async Task<IActionResult> Post(UserModel model) {
            try {
                
                var (succeeded, message) = await _userService.CreateAsync(model);
                if (!succeeded)
                    throw new ApplicationException(message);

                model.Password = null;
                return Created($"members/{message}", model);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpPut]
        [Authorize(Policies.CanUpdateUser)]
        public async Task<IActionResult> Put(UserModel model) {
            try {

                (var succeeded, var error) = await _userService.UpdateAsync(model);

                return Ok();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet]
        [Authorize(Policies.CanReadUser)]
        public async Task<IActionResult> Get(int skip, int take) {
            try {

                var users = await _userService.GetAsync(skip, take);

                return Ok(users);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policies.CanReadUser)]
        public async Task<IActionResult> Get(string id) {
            try {

                var user = await _userService.GetAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(user);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.CanDeleteUser)]
        public async Task<IActionResult> Delete(string id) {
            try {
                
                var (succeeded, error) = await _userService.RemoveAsync(id);
                if (!succeeded)
                    throw new ApplicationException(error);

                return NoContent();

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

    }

}
