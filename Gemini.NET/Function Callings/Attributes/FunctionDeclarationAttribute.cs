namespace GeminiDotNET.FunctionCallings.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class FunctionDeclarationAttribute(string name, string description) : Attribute
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
    }
}
