using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Arrba.DTO;
using Arrba.ImageLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment _appEnvironment;

        public UploadController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [Authorize]
        [HttpGet]
        [Route("getGuid", Name = "generate Guid")]
        public async Task<ActionResult> Get()
        {
            string uniqueItemFolder = Guid.NewGuid().ToString("N");
            string rootPath = this._appEnvironment.WebRootPath;

            var result = await Task.Factory
                .StartNew(() => ImgFolderManager.GenerateTempUniqueItemFolder(uniqueItemFolder, rootPath).Exists);

            if (result)
            {
                return Ok(new
                {
                    uniqueItemFolder
                });
            }

            return StatusCode((int)HttpStatusCode.NotAcceptable);
        }

        [Authorize]
        [HttpPost]
        [Route("image/{uniqueItemFolder}", Name = "uploadImage")]
        public async Task<IActionResult> PostImage(string uniqueItemFolder)
        {
            string rootPath = this._appEnvironment.WebRootPath;
            string uniqueFolderPath = ImgFolderManager.GetTempUniqueItemFolder(uniqueItemFolder, rootPath);

            List<string> fileNames = new List<string>();

            IFormFile file = Request.Form.Files[0];

            if (file.Length <= 0) return BadRequest("Нет файлов в запросе");

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(uniqueFolderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            fileNames.Add(fileName);

            var uploadImage = new UploadImageDto();
            if (fileNames.Count == 1)
            {
                uploadImage.UniqueItemFolder = uniqueItemFolder;
                uploadImage.FileName = fileNames.SingleOrDefault();
            }

            uploadImage.FileNames = fileNames;

            return Ok(uploadImage);

        }

        [HttpDelete]
        [Route("{uniqueFolderName}/image/{fileName}/delete", Name = "delete temporary image file")]
        public async Task<ActionResult> Delete(string uniqueFolderName, string fileName)
        {
            string rootPath = this._appEnvironment.WebRootPath;
            string pathToFolderWithImages = ImgFolderManager.GetTempUniqueItemFolder(uniqueFolderName, rootPath);

            await Task.Factory.StartNew(() =>
            {
                if (new DirectoryInfo(pathToFolderWithImages).Exists)
                {
                    string filePath = Path.Combine(pathToFolderWithImages, fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            });

            return Ok();
        }

        [HttpDelete]
        [Route("{uniqueFolderName}/delete", Name = "delete temporary images by folder name")]
        public async Task<ActionResult> Delete(string uniqueFolderName)
        {
            string rootPath = this._appEnvironment.WebRootPath;
            string pathToFolderWithImages = ImgFolderManager.GetTempUniqueItemFolder(uniqueFolderName, rootPath);

            await Task.Factory.StartNew(() =>
            {
                if (new DirectoryInfo(pathToFolderWithImages).Exists)
                {
                    var filePathes = Directory.GetFiles(pathToFolderWithImages);

                    foreach (var filePath in filePathes)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    Directory.Delete(pathToFolderWithImages);
                }
            });

            return Ok();
        }
    }
}
