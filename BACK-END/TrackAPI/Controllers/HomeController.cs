using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Controllers {

    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase {

        [HttpGet]
        public IActionResult Get() {
            return Ok("Welcome to AussieTracker API.");
        }

        [HttpPost]
        public IActionResult Post(dynamic data) {
            return Ok(data ?? "");
        }

    }

}
