using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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

    [Route("v1/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase {

        private readonly IWebHostEnvironment _host;
        private readonly IImageService _imageService;
        public ImagesController(IImageService imageService, IWebHostEnvironment host) {
            _imageService = imageService;
            _host = host;
        }

        #region ------------------ GET -------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id, string d) {
            try {

                var model = await _imageService.GetAsync(id);
                if (model == null) {

                    // No default image requested?
                    if (string.IsNullOrEmpty(d))
                        return NotFound();

                    // Default image requested
                    var folder = PathUtil.Resolve(_host.ContentRootPath, Values.IMAGES_FOLDER);
                    var filePath = Path.Combine(folder, $"{d.ToLower()}.jpg");
                    if (!System.IO.File.Exists(filePath))
                        return NotFound();

                    return File(System.IO.File.ReadAllBytes(filePath), MediaTypeNames.Image.Jpeg);
                }

                return File(model.Bytes, MediaTypeNames.Image.Jpeg);

            } catch (Exception ex) {
                return ex.GetActionResult();
            }
        }

        [HttpGet("icons")]
        public IActionResult GetIcons() {
            try {

                var folder = PathUtil.Resolve(_host.ContentRootPath, Values.IMAGES_FOLDER);
                var allIcons = Directory.GetFiles(folder)
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToArray();

                return Ok(allIcons);
                

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
