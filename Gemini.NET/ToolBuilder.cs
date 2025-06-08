using Gemini.NET.API_Models.API_Request.Configurations.Tools;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling;
using GeminiDotNET.ApiModels.Shared;
using GeminiDotNET.Helpers;

namespace GeminiDotNET
{
    public class ToolBuilder
    {
        private bool _googleSearchEnabled = false;
        private readonly List<FunctionDeclaration> _functionDeclarations = [];

        public ToolBuilder EnableGoogleSearch()
        {
            _googleSearchEnabled = true;
            return this;
        }

        public ToolBuilder AddFunctionDeclarations(IEnumerable<FunctionDeclaration> functionDeclarations)
        {
            if (functionDeclarations == null) throw new ArgumentNullException(nameof(functionDeclarations), "Function declarations cannot be null.");
            _functionDeclarations.AddRange(functionDeclarations);
            return this;
        }

        public ToolBuilder AddFunctionDeclaration(Delegate del, Parameters? parameters = null)
        {
            var attr = del.GetFunctionDeclarationAttribute();

            if (_functionDeclarations.Exists(fd => fd.Name.Equals(attr.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return this;
            }

            _functionDeclarations.Add(new FunctionDeclaration
            {
                Name = attr.Name,
                Description = attr.Description,
                Parameters = parameters
            });

            return this;
        }

        public List<Tool> Build()
        {
            var tools = new List<Tool>();

            if (_googleSearchEnabled)
            {
                tools.Add(new Tool
                {
                    GoogleSearch = new GoogleSearch()
                });

                tools.Add(new Tool
                {
                    UrlContext = new UrlContext()
                });
            }

            if (_functionDeclarations.Count != 0)
            {
                tools.Add(new Tool
                {
                    FunctionDeclarations = _functionDeclarations
                });
            }

            if (tools.Count == 0)
            {
                throw new InvalidOperationException("At least one tool must be enabled or added.");
            }

            return tools;
        }
    }
}
