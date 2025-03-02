using Models.Request;
using System.Reflection;

namespace Gemini.NET.Helpers
{
    public static class OpenApiSchemaGenerator
    {
        public static ResponseSchema AsOpenApiSchema<T>()
        {
            return ConvertToSchema(typeof(T), new HashSet<Type>());
        }

        private static ResponseSchema ConvertToSchema(Type type, HashSet<Type> processedTypes)
        {
            if (processedTypes.Contains(type))
            {
                return new ResponseSchema("object");
            }
            processedTypes.Add(type);

            var schema = new ResponseSchema("object")
            {
                Properties = [],
                Required = []
            };

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                string propName = prop.Name;
                Type propType = prop.PropertyType;
                ResponseSchema propSchema;

                // Xử lý đặc biệt cho SearchSuggestions
                if (propName == "SearchSuggestions")
                {
                    propSchema = new ResponseSchema("string");
                }
                else
                {
                    // Lấy kiểu của property theo định nghĩa OpenAPI
                    var (openApiType, itemsSchema, enumValues) = GetOpenApiType(propType, processedTypes);
                    propSchema = new ResponseSchema(openApiType);

                    if (enumValues != null)
                    {
                        propSchema.Enum = enumValues;
                    }
                    if (openApiType == "array" && itemsSchema != null)
                    {
                        propSchema.Items = itemsSchema;
                    }
                    // Nếu kiểu là object (không phải primitive hay string) thì tiến hành convert nested object
                    if (openApiType == "object" && !propType.IsPrimitive && propType != typeof(string))
                    {
                        propSchema = ConvertToSchema(propType, processedTypes);
                    }
                }

                // Nếu property được đánh dấu là required (ví dụ dùng RequiredMemberAttribute)
                if (Attribute.IsDefined(prop, typeof(System.Runtime.CompilerServices.RequiredMemberAttribute)))
                {
                    schema.Required.Add(propName);
                }
                schema.Properties[propName] = propSchema;
            }

            if (schema.Required.Count == 0)
            {
                schema.Required = null;
            }

            if (schema.Properties.Count == 0)
            {
                schema.Properties = null;
            }

            return schema;
        }

        // Hàm trả về kiểu openapi, schema của items (nếu array) và danh sách enum nếu có
        private static (string type, ResponseSchema? itemsSchema, List<string>? enumValues) GetOpenApiType(Type type, HashSet<Type> processedTypes)
        {
            List<string>? enumValues = null;
            ResponseSchema? itemsSchema = null;

            if (type.IsEnum)
            {
                enumValues = Enum.GetNames(type).ToList();
                return ("string", null, enumValues);
            }

            if (type == typeof(string))
                return ("string", null, null);
            if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(sbyte))
                return ("integer", null, null);
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return ("number", null, null);
            if (type == typeof(bool))
                return ("boolean", null, null);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = type.GetGenericArguments()[0];
                var (itemOpenApiType, innerItemsSchema, innerEnumValues) = GetOpenApiType(itemType, processedTypes);
                itemsSchema = new ResponseSchema(itemOpenApiType);
                if (innerEnumValues != null)
                {
                    itemsSchema.Enum = innerEnumValues;
                }
                // Nếu item là object (và không phải primitive hay string) thì chuyển đổi nested object
                if (itemOpenApiType == "object" && !itemType.IsPrimitive && itemType != typeof(string))
                {
                    itemsSchema = ConvertToSchema(itemType, processedTypes);
                }
                return ("array", itemsSchema, null);
            }

            return ("object", null, null);
        }
    }
}
