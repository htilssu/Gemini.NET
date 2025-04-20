using Gemini.NET.API_Models.API_Request;
using GeminiDotNET;
using GeminiDotNET.API_Models.Enums;
using GeminiDotNET.Client_Models;
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

        [HttpPost("AskFile")]
        public async Task<IActionResult> AskFile(string apiKey, string filePath)
        {
            var uploader = new FileUploader(apiKey);

            try
            {
                var fileUri = await uploader.UploadFileAsync(filePath);
                var generatorWithApiKey = new Generator(apiKey);

                var apiRequest = new ApiRequestBuilder()
                    .WithPrompt("Summarize the context")
                    .WithUploadedFile(fileUri, MimeType.ApplicationPdf)
                    .WithDefaultGenerationConfig()
                    .DisableAllSafetySettings()
                    .Build();

                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.0-flash");

                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
