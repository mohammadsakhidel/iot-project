using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Extensions {
    public static class ExceptionExtensions {
        public static IActionResult GetActionResult(this Exception exception) {
            return new ObjectResult(exception.Message) {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
