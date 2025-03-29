using Gemini.NET;
using Gemini.NET.Client_Models;
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
                .WithResponseSchema(new
                {
                    type = "object",
                    properties = new
                    {
                        response = new
                        {
                            type = "string"
                        }
                    }
                })
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.0-flash");
                return Ok(response.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
