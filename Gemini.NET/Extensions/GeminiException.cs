using Models.Response.Failed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemini.NET.Extensions
{
    public class GeminiException : Exception
    {
        public ApiResponse? GeminiResponse { get; set; }
        public GeminiException(string message) : base(message) { }
        public GeminiException(string message, Exception innerException) : base(message, innerException) { }
        public GeminiException(string message, ApiResponse? geminiResponse) : base(message)
        {
            GeminiResponse = geminiResponse;
        }
        public GeminiException(string message, ApiResponse? geminiResponse, Exception innerException) : base(message, innerException)
        {
            GeminiResponse = geminiResponse;
        }
    }
}
