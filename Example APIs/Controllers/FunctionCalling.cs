using Gemini.NET.API_Models.API_Request.Configurations.Tools;
using GeminiDotNET;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionCalling : ControllerBase
    {
        [HttpPost("ChatToGetFunctionDeclarations")]
        public async Task<IActionResult> ChatToGetFunctionDeclarations(string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey);
            var functionDeclaration = new FunctionDeclaration
            {
                Name = "GetWeather",
                Description = "Get the current weather for a given location.",
                Parameters = new Parameters
                {
                    Properties = new
                    {
                        City = new
                        {
                            type = "string"
                        }
                    }
                }
            };

            var apiRequest = new ApiRequestBuilder()
                .WithPrompt(prompt)
                .WithFunctionDeclarations([functionDeclaration])
                .WithDefaultGenerationConfig()
                .DisableAllSafetySettings()
                .Build();

            try
            {
                var response = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.0-flash-lite");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
