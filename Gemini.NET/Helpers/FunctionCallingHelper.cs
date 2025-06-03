using GeminiDotNET.ApiModels.Response.Success.FunctionCalling;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace GeminiDotNET.Helpers
{
    public static class FunctionCallingHelper
    {
        public static T? GetParameterValue<T>(FunctionCall? functionCallingInfo, string parameterName)
        {
            if (functionCallingInfo == null) return default;

            var parameterModel = functionCallingInfo.Args;

            if (parameterModel == null || string.IsNullOrEmpty(parameterName)) return default;

            if (parameterModel is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue(parameterName, out var val) && val is T tVal) return tVal;
                else if (dict.TryGetValue(parameterName, out val)) return ConvertValue<T>(val);
            }

            if (parameterModel is JObject jObject)
            {
                JToken? token = jObject.SelectToken(parameterName);
                if (token != null) return token.ToObject<T>();
            }

            var prop = parameterModel.GetType().GetProperty(parameterName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                var val = prop.GetValue(parameterModel);
                return ConvertValue<T>(val);
            }

            return default;
        }

        public static FunctionResponse CreateResponse(Delegate del, string output) => new()
        {
            Name = nameof(del.Method),
            Response = new Response
            {
                Output = output
            }
        };

        public static FunctionResponse CreateResponse(string name, string output) => new()
        {
            Name = name,
            Response = new Response { Output = output }
        };

        private static T? ConvertValue<T>(object? val)
        {
            if (val == null) return default;
            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
}
