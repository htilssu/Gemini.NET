using GeminiDotNET.ApiModels.Response.Success;
using GeminiDotNET.Helpers;
using System.Net.Http.Headers;
using System.Text;

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

            var mimeTypeValue = MimeTypeHelper.GetMimeType(filePath);
            var mimeType = mimeTypeValue.GetDescription();
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
            var dto = JsonHelper.AsObject<FileUploaderResponse>(responseJson);

            if (dto == null || dto?.File == null || dto?.File?.Uri == null)
            {
                throw new InvalidOperationException("Could not extract file URI");
            }

            var fileUri = dto.File.Uri;

            if (fileSize > 50 * 1024 * 1024)
            {
                string? state = null;
                sbyte retryCount = 0;
                do
                {
                    var uploadingFile = await GetFileAsync(dto.File.Name);
                    state = uploadingFile?.State;
                    retryCount++;
                    if (retryCount > 20)
                    {
                        throw new TimeoutException($"File processing took too long. Failed after {retryCount} attemps.");
                    }
                }
                while (state != "ACTIVE");
            }

            return fileUri;
        }

        public async Task<FileMetaData?> GetFileAsync(string name)
        {
            try
            {
                var listUrl = $"{BaseUrl}/v1beta/{name}?key={_apiKey}";
                using var response = await _httpClient.GetAsync(listUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                return JsonHelper.AsObject<FileMetaData>(json);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<FileMetaData>> GetAllFilesAsync()
        {
            var listUrl = $"{BaseUrl}/v1beta/files?key={_apiKey}";
            using var response = await _httpClient.GetAsync(listUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var dto = JsonHelper.AsObject<FileUploaderResponse>(json);
            if (dto == null || dto?.Files == null)
            {
                return [];
            }

            return dto.Files;
        }
    }
}
