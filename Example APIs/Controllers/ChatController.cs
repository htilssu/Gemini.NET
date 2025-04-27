using GeminiDotNET;
using GeminiDotNET.API_Models.Enums;
using GeminiDotNET.Client_Models;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [HttpPost("GenerationWithChatHistory")]
        public async Task<IActionResult> GenerateContentWithChatHistory([FromBody] List<ChatMessage> chatMessages, string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey);

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithChatHistory(chatMessages)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.0-flash");
                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GenerationWithFile")]
        public async Task<IActionResult> GenerateContentWithFile(string apiKey, string prompt, string fileUri)
        {
            var generatorWithApiKey = new Generator(apiKey);

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithUploadedFile(fileUri, MimeType.MP4)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
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
