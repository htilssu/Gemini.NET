using Gemini.NET.Helpers;
using GeminiDotNET.Helpers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace GeminiDotNET
{
    public class FileUploader
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://generativelanguage.googleapis.com";

        public FileUploader(string apiKey)
        {
            if (!Validator.CanBeValidApiKey(apiKey)) throw new ArgumentNullException(nameof(apiKey), "Invalid or expired API key.");

            _apiKey = apiKey.Trim();
            _httpClient = new();
        }

        public async Task<string> UploadFileAsync(string filePath, string? displayName = null, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);
            var safeDisplayName = string.IsNullOrEmpty(displayName) ? Path.GetFileName(filePath) : displayName.Trim();
            var existingFileUri = await GetFileUriByDisplayNameAsync(safeDisplayName, cancellationToken);
            if (!string.IsNullOrEmpty(existingFileUri))
            {
                return existingFileUri;
            }

            var mimeTypeValue = MimeTypeHelper.GetMimeType(filePath);
            var mimeType = EnumHelper.GetDescription(mimeTypeValue);
            var fileSize = new FileInfo(filePath).Length;

            var startUrl = $"{BaseUrl}/upload/v1beta/files?key={_apiKey}";
            var startRequest = new HttpRequestMessage(HttpMethod.Post, startUrl);
            startRequest.Headers.Add("X-Goog-Upload-Protocol", "resumable");
            startRequest.Headers.Add("X-Goog-Upload-Command", "start");
            startRequest.Headers.Add("X-Goog-Upload-Header-Content-Length", fileSize.ToString());
            startRequest.Headers.Add("X-Goog-Upload-Header-Content-Type", mimeType);
            startRequest.Content = new StringContent(
                $"{{\"file\": {{\"display_name\": \"{safeDisplayName}\"}}}}", Encoding.UTF8, "application/json"
            );

            using var startResponse = await _httpClient.SendAsync(startRequest, cancellationToken);
            startResponse.EnsureSuccessStatusCode();

            if (!startResponse.Headers.TryGetValues("X-Goog-Upload-URL", out var uploadUrlValues)) throw new InvalidOperationException("Upload URL not found in response");

            var uploadUrl = uploadUrlValues.First();

            using var fileStream = File.OpenRead(filePath);
            var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
            uploadRequest.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
            uploadRequest.Headers.Add("X-Goog-Upload-Offset", "0");
            uploadRequest.Content = new StreamContent(fileStream);
            uploadRequest.Content.Headers.ContentLength = fileSize;
            uploadRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

            using var uploadResponse = await _httpClient.SendAsync(uploadRequest, cancellationToken);
            uploadResponse.EnsureSuccessStatusCode();

            var responseJson = await uploadResponse.Content.ReadAsStringAsync(cancellationToken);
            using var jsonDoc = JsonDocument.Parse(responseJson);

            var root = jsonDoc.RootElement;
            if (!root.TryGetProperty("file", out var fileElement) || !fileElement.TryGetProperty("uri", out var uriElement))
            {
                throw new InvalidOperationException("Could not extract file URI");
            }

            var fileUri = uriElement.GetString();

            if (mimeType.StartsWith("video/"))
            {
                await WaitForFileToBeActiveAsync(fileElement, 5, cancellationToken);
            }

            return fileUri!;
        }

        private async Task WaitForFileToBeActiveAsync(JsonElement fileElement, int delayTimeInSecond, CancellationToken cancellationToken)
        {
            if (!fileElement.TryGetProperty("name", out var nameElement))
            {
                throw new InvalidOperationException("File name not found in the response");
            }

            var fileName = nameElement.GetString();
            var statusUrl = $"{BaseUrl}/v1beta/files/{HttpUtility.UrlEncode(fileName)}?key={_apiKey}";

            string? state = null;
            sbyte retryCount = 0;

            do
            {
                await Task.Delay(delayTimeInSecond / 1000, cancellationToken);
                var resp = await _httpClient.GetAsync(statusUrl, cancellationToken);
                var json = await resp.Content.ReadAsStringAsync(cancellationToken);

                using var statusDoc = JsonDocument.Parse(json);
                state = statusDoc.RootElement.GetProperty("file").GetProperty("state").GetString();

                retryCount++;
                if (retryCount > 20)
                {
                    throw new TimeoutException($"File processing took too long. Failed after {retryCount} attemps.");
                }

            }
            while (state != "ACTIVE");
        }

        public async Task<string?> GetFileUriByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default)
        {
            var listUrl = $"{BaseUrl}/v1beta/files?key={_apiKey}";
            using var response = await _httpClient.GetAsync(listUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("files", out var filesElement))
            {
                throw new InvalidOperationException("Could not extract files from response");
            }

            foreach (var file in filesElement.EnumerateArray())
            {
                if (file.TryGetProperty("displayName", out var nameElement) && nameElement.GetString()?.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true && file.TryGetProperty("uri", out var uriElement))
                {
                    return uriElement.GetString();
                }
            }

            return null;
        }
    }
}
