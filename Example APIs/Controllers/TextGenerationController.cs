using GeminiDotNET;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextGenerationController : ControllerBase
    {
        [HttpPost("GenerationWithImages")]
        public async Task<IActionResult> GenerateContentWithImages([FromBody] List<string> base64Images, string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey);

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithBase64Images(base64Images)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, Generator.GetLatestStableModelVersion());
                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GenerationWithSystemInstruction")]
        public async Task<IActionResult> GenerateContentWithSystemInstruction([FromBody] string instruction, string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey);

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithSystemInstruction(instruction)
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, Generator.GetLatestStableModelVersion());
                return Ok(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
