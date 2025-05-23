using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
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
        public async Task<IActionResult> GetFile(string apiKey, string name)
        {
            var uploader = new FileUploader(apiKey);

            try
            {
                var file = await uploader.GetFileAsync(name);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AskFile")]
        public async Task<IActionResult> AskFile(string apiKey, string filePath)
        {
            var uploader = new FileUploader(apiKey);

            try
            {
                //var fileUri = await uploader.UploadFileAsync(filePath);
                var generatorWithApiKey = new Generator(apiKey);

                var apiRequest = new ApiRequestBuilder()
                    .WithPrompt("Summarize the context")
                    .WithUploadedFile("https://generativelanguage.googleapis.com/v1beta/files/brnqkf71i8b9", MimeType.PDF)
                    .WithDefaultGenerationConfig()
                    .DisableAllSafetySettings()
                    .Build();

                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.5-pro-exp-03-25");

                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
