using GeminiDotNET;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(string apiKey, string filePath)
        {
            var uploader = new FileUploader(apiKey);

            try
            {
                var fileUri = await uploader.UploadFileAsync(filePath);
                return Ok(fileUri);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetFile")]
        public async Task<IActionResult> GetFile(string apiKey, string displayName)
        {
            var uploader = new FileUploader(apiKey);

            try
            {
                var name = await uploader.GetFileUriByDisplayNameAsync(displayName);
                return Ok(name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
