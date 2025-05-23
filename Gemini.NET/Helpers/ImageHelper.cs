using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;

namespace GeminiDotNET.Helpers
{
    /// <summary>
    /// Helper class for image operations
    /// </summary>
    public static class ImageHelper
    {
        private static readonly List<MimeType> _supportedMimeTypes =
        [
            MimeType.PNG,
            MimeType.JPEG,
            MimeType.HEIC,
            MimeType.HEIF,
            MimeType.WEBP,
        ];

        public static ImageData Base64ToImageData(string base64Image)
        {
            MimeType? mimeType;
            if (base64Image.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                var imageFormat = base64Image[..base64Image.IndexOf(';')].Split('/')[^1];
                mimeType = _supportedMimeTypes.FirstOrDefault(t => t.ToString().EndsWith(imageFormat, StringComparison.OrdinalIgnoreCase));
                if (mimeType == default)
                {
                    mimeType = MimeType.JPEG;
                }
                if (base64Image.Contains(','))
                {
                    base64Image = base64Image.Split(',')[1];
                }
            }
            else
            {
                mimeType = MimeType.JPEG;
                if (base64Image.Contains(','))
                {
                    base64Image = base64Image.Split(',')[1];
                }
            }

            return new ImageData
            {
                Base64Data = base64Image,
                MimeType = mimeType.Value
            };
        }

        public static ImageData ImagePathToBase64(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Image not found", filePath);
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(filePath);
                var base64 = Convert.ToBase64String(imageBytes);
                return Base64ToImageData(base64);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while converting '{filePath}' to base64", ex);
            }
        }
    }
}
