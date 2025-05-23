using GeminiDotNET;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ApiModels.Response.Success.FunctionCalling;
using GeminiDotNET.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Example_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionCalling : ControllerBase
    {
        [HttpPost("ChatWithFunctionCalling")]
        public async Task<IActionResult> ChatWithFunctionCalling(string apiKey, string prompt)
        {
            var generatorWithApiKey = new Generator(apiKey).EnableChatHistory();
            var functionDeclaration = new FunctionDeclaration
            {
                Name = "GetWeather",
                Description = "Get the current weather for a given location at the given hour of the day.",
                Parameters = new Parameters
                {
                    Properties = new
                    {
                        City = new
                        {
                            type = "string"
                        },
                        Hour = new
                        {
                            type = "integer"
                        },
                    }
                }
            };

            try
            {
                var apiRequest = new ApiRequestBuilder()
                    .WithPrompt(prompt)
                    .WithFunctionDeclarations([functionDeclaration], FunctionCallingMode.ANY)
                    .WithDefaultGenerationConfig()
                    .DisableAllSafetySettings()
                    .Build();

                var responseWithFunctionCall = await generatorWithApiKey.GenerateContentAsync(apiRequest, "gemini-2.0-flash-lite");

                var functionResponses = new List<FunctionResponse>();

                foreach (var function in responseWithFunctionCall.FunctionCalls)
                {
                    if (function.Name == "GetWeather")
                    {
                        var city = FunctionCallingHelper.GetParameterValue<string>(function, "City");
                        var hour = FunctionCallingHelper.GetParameterValue<int>(function, "Hour");
                        var weather = @"{ 'Weather' : 'sunny', 'Temperature' : 38 }";
                        var functionResponse = new FunctionResponse
                        {
                            Name = function.Name,
                            Response = new Response
                            {
                                Output = weather,
                            }
                        };
                        functionResponses.Add(functionResponse);
                    }
                }

                var apiRequestWithFunction = new ApiRequestBuilder()
                    .WithFunctionResponses(functionResponses)
                    .WithDefaultGenerationConfig()
                    .DisableAllSafetySettings()
                    .Build();

                var responseWithFunctionResponse = await generatorWithApiKey.GenerateContentAsync(apiRequestWithFunction, "gemini-2.0-flash-lite");

                return Ok(responseWithFunctionResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
