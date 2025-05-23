using GeminiDotNET.ApiModels.Enums;

namespace GeminiDotNET.Helpers
{
    public static class MimeTypeHelper
    {
        private static readonly Dictionary<string, MimeType> _extensionToMimeType = BuildExtensionMapping();

        private static Dictionary<string, MimeType> BuildExtensionMapping()
        {
            var map = new Dictionary<string, MimeType>(StringComparer.OrdinalIgnoreCase)
            {
                ["jpg"] = MimeType.JPEG
            };

            foreach (MimeType mimeType in Enum.GetValues(typeof(MimeType)))
            {
                map[mimeType.ToString()] = mimeType;
            }

            return map;
        }

        public static MimeType GetMimeType(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is null or empty", nameof(filePath));

            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

            var extension = Path.GetExtension(filePath);

            if (string.IsNullOrEmpty(extension)) throw new NotSupportedException("File extension not found");

            extension = extension.TrimStart('.');

            if (!_extensionToMimeType.TryGetValue(extension, out var mimeType)) throw new NotSupportedException($"Unsupported MIME type");

            return mimeType;
        }
    }
}
