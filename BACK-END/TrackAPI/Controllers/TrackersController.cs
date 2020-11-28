using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;

namespace TrackAPI.Controllers {

    [Route("v1/[controller]")]
    [ApiController]
    public class TrackersController : ControllerBase {
        
        [HttpPost]
        public IActionResult Post() {
            return Ok();
        }

    }

}
