using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using BR.MadenIlan.Photo.Api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Photo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public PhotoController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo?.Length <= 0) return BadRequest("Photo is NULL");

            string photosFolderName = "Photos";
            string randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            string folderPath = Path.Combine(webHostEnvironment.WebRootPath, photosFolderName);
            string path = Path.Combine(folderPath, randomFileName);

            using FileStream stream = new(path, FileMode.Create);
            await photo.CopyToAsync(stream, cancellationToken);
            string returnFileName = Path.Combine(photosFolderName, randomFileName);

            return Ok(new { Url = returnFileName });
        }

        [HttpDelete]
        public IActionResult Delete(PhotoDeleteDto model)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, model.Url);
            if (!System.IO.File.Exists(path)) return BadRequest();

            System.IO.File.Delete(path);

            return NoContent();
        }
    }
}
