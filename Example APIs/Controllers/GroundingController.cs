using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroundingController : ControllerBase
    {
        [HttpPost("GenerationWithGrounding")]
        public async Task<IActionResult> GenerateContentWithGrounding(string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey);

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, ModelVersion.Gemini_20_Flash_Lite);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
