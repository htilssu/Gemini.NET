using Gemini.NET;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiCodeAssistController : ControllerBase
    {
        [HttpPost("GenerateContent")]
        public async Task<IActionResult> GenerateContent(string bearer, string cloudProjectId, string prompt)
        {
            var generatorWithApiKey = new GeminiCodeAssistGenerator
            {
                CloudProjectId = cloudProjectId
            };

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(bearer, apiRequest, ModelVersion.Gemini_25_Flash);
                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SetPrivacyMode")]
        public async Task SetPrivacyMode(string bearer, string cloudProjectId, bool enableDataCollection)
        {
            var generatorWithApiKey = new GeminiCodeAssistGenerator
            {
                CloudProjectId = cloudProjectId
            };

            await generatorWithApiKey.SetPrivacySettingAsync(bearer, enableDataCollection);
        }
    }
}
