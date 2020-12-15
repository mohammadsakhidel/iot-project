using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Extensions;
using TrackAPI.Models;
using TrackAPI.Services;
using TrackLib.Utils;

namespace TrackAPI.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase {

        private readonly IImageService _imageService;
        public ImagesController(IImageService imageService) {
            _imageService = imageService;
        }

        #region ------------------ GET -------------------
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id) {
            try {

                var model = await _imageService.GetAsync(id);
                if (model == null)
                    return NotFound();

                return File(model.Bytes, MediaTypeNames.Image.Jpeg);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

        #region ------------------ POST ------------------
        [Authorize]
        [HttpPost("square")]
        public async Task<IActionResult> PostAsync() {
            try {

                using var sr = new StreamReader(Request.Body);
                var base64Bytes = await sr.ReadToEndAsync();

                if (string.IsNullOrEmpty(base64Bytes))
                    throw new ApplicationException("Image cannot be null.");

                // Create Square Image:
                var bytes = Convert.FromBase64String(base64Bytes);
                using var ms = new MemoryStream(bytes);
                var originalImage = Image.FromStream(ms);
                var squareImage = ImageUtil.CreateSquareThumbnail(originalImage, Values.SQUARE_IMAGE_SIDES);

                // Save in database:
                ms.Position = 0;
                squareImage.Save(ms, ImageFormat.Jpeg);
                var model = new ImageModel {
                    Name = $"{TextUtil.RandomString(16)}.jpg",
                    Width = squareImage.Width,
                    Height = squareImage.Height,
                    Bytes = ms.ToArray()
                };

                // Return:
                (var done, var message) = await _imageService.CreateAsync(model);
                if (!done)
                    throw new ApplicationException(message);

                return Created($"/images/{message}", message);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }
        #endregion

    }

}
