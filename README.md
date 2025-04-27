# Gemini.NET - Lightweight Gemini API Client Library for .NET 8

[![NuGet Version](https://img.shields.io/nuget/v/Gemini.NET)](https://www.nuget.org/packages/Gemini.NET)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Gemini.NET)](https://www.nuget.org/packages/Gemini.NET)

![Gemini.NET Logo](https://i.imgur.com/ee8T0gX.png)

**Gemini.NET** is a lightweight and user-friendly .NET 8 lubrary designed to simplify the integration of Google's Gemini API into your .NET 8 applications. It provides a straightforward and efficient way to generate content, manage API requests, and leverage Gemini's powerful features.

*   Building complex API requests using a fluent builder pattern.
*   Generating content using various Gemini models.
*   Uploading files for use in multimodal prompts.
*   Validating API keys.
*   Handling authentication via API Key or Google Cloud Credentials.

## **✨ Key Features**

*   **Fluent Request Building:** Easily construct API requests using the `ApiRequestBuilder`.
*   **Content Generation:** Generate text, interact in chat sessions, and analyze multimodal inputs with the `Generator` class.
*   **File Management:** Upload files using `FileUploader` and reference them in your requests.
*   **API Key Validation:** Check the format and validity of Gemini API keys using the `Validator`.
*   **Flexible Configuration:** Control generation parameters (temperature, tokens), safety settings, grounding, and response schemas.
*   **Authentication Options:** Supports both API Key and Google Cloud project-based authentication.

## **📦 Installation**

Install Gemini.NET using the **NuGet Package Manager** in Visual Studio or the .NET CLI.

**NuGet Package Manager:**

![alt text](https://i.imgur.com/6TFlTE4.png)

1. Open your project in Visual Studio.
2. Go to **Tools > NuGet Package Manager > Manage NuGet Packages for Solution...**
3. Search for `Gemini.NET` and install the package.

**Package Manager Console:**

```powershell
Install-Package Gemini.NET 
```

**.NET CLI:**

```bash
dotnet add package Gemini.NET
```

For more detailed instructions, visit the Gemini.NET's [NuGet Gallery page](https://www.nuget.org/packages/Gemini.NET).

---

## **Getting Started**

Here's a quick example of how to generate text using an API key:

```csharp
using GeminiDotNet;
using System.Threading.Tasks;

public class Example
{
    public static async Task Main(string[] args)
    {
        string apiKey = "YOUR_GEMINI_API_KEY"; // Replace with your actual Gemini API Key

        // 1. Validate the API Key format (optional but recommended)
        if (!Validator.CanBeValidApiKey(apiKey))
        {
            Console.WriteLine("Invalid API Key format.");
            return;
            // You might also want to check validity against the service:
            // bool isValid = await Validator.IsValidApiKeyAsync(apiKey);
        }

        // 2. Create a Generator instance
        var generator = new Generator(apiKey);

        // 3. Build the API request
        var request = new ApiRequestBuilder()
            .WithPrompt("Explain the concept of Large Language Models in simple terms.")
            .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512) // Optional configuration
            .Build();

        // 4. Generate content
        try
        {
            // Specify the model (using enum or string alias)
            ModelResponse response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash_Lite);

            // 5. Process the response (Assuming ModelResponse has a Text property)
            if (response != null && response.Text != null) // Adjust based on your ModelResponse structure
            {
                Console.WriteLine("Generated Content:");
                Console.WriteLine(response.Text);
            }
            else
            {
                Console.WriteLine("Failed to generate content or response was empty.");
                // You might want to inspect the response object for error details
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Handle exceptions appropriately (e.g., network issues, API errors)
        }
    }
}
```

## **Core Components Usage**

### `Validator`

Use the static `Validator` class to check API keys before making calls.

```csharp
string potentialKey = "SOME_KEY_STRING";

// Quick format check (synchronous)
bool looksValid = Validator.CanBeValidApiKey(potentialKey);
Console.WriteLine($"Key format looks valid: {looksValid}");

// Check against the Gemini service (asynchronous)
// Ensure you handle potential exceptions if the service call fails
try
{
    bool isValid = await Validator.IsValidApiKeyAsync(potentialKey);
    Console.WriteLine($"Key is actually valid: {isValid}");
}
catch (Exception ex)
{
     Console.WriteLine($"Error validating key: {ex.Message}");
}
```

### `FileUploader`

Upload files (such as images, PDF, video, audio) to be used in multimodal prompts. Files are typically referenced by a URI returned upon successful upload.

```csharp
string apiKey = "YOUR_API_KEY";
var fileUploader = new FileUploader(apiKey);
string filePath = "path/to/your/image.png"; // Replace with actual path
string? displayName = "My Image"; // Optional display name

try
{
    // Upload the file
    string fileUri = await fileUploader.UploadFileAsync(filePath, displayName);
    Console.WriteLine($"File uploaded successfully. URI: {fileUri}");

    // You can now use this fileUri with ApiRequestBuilder's WithUploadedFile method

    // Optionally, retrieve the URI later using the display name (if provided during upload)
    string? retrievedUri = await fileUploader.GetFileUriByDisplayNameAsync(displayName);
    Console.WriteLine($"Retrieved URI using display name: {retrievedUri}");
}
catch (Exception ex)
{
    Console.WriteLine($"File upload failed: {ex.Message}");
}
```

### `ApiRequestBuilder`

Construct your API request using the fluent builder methods. Chain methods to configure the request as needed.

```csharp
// --- Using File Uploader result ---
string apiKey = "YOUR_API_KEY";
var fileUploader = new FileUploader(apiKey);
string imagePath = "path/to/your/image.png";
string imageUri = await fileUploader.UploadFileAsync(imagePath); // Assume success

// --- Build Request ---
var requestBuilder = new ApiRequestBuilder();

// Example 1: Simple Text Prompt
var textRequest = requestBuilder
    .WithPrompt("What are the main benefits of using .NET?")
    .Build();

// Example 2: Multimodal Prompt (Text + Uploaded Image)
var multimodalRequest = requestBuilder
    .WithPrompt("Describe this image.")
    .WithUploadedFile(imageUri, MimeType.PNG) // Use the URI from FileUploader
    .Build();

// Example 3: Chat Conversation
var chatHistory = new List<ChatMessage>
{
    new ChatMessage { Role = "user", Content = "Hello!" },
    new ChatMessage { Role = "model", Content = "Hi there! How can I help you today?" },
    new ChatMessage { Role = "user", Content = "Tell me a short joke." }
};
var chatRequest = requestBuilder
    .WithChatHistory(chatHistory)
    .Build();

// Example 4: Including Local Images (by path or base64)
var imagePaths = new List<string> { "path/to/image1.jpg", "path/to/image2.png" };
// OR var base64Images = new List<string> { "base64_string_1", "base64_string_2" };
var localImageRequest = requestBuilder
    .WithPrompt("Compare these two images.")
    .WithImages(imagePaths) // OR .WithBase64Images(base64Images)
    .Build();


// Example 5: Advanced Configuration
var advancedRequest = requestBuilder
    .WithSystemInstruction("You are a helpful assistant that provides concise answers.")
    .WithPrompt("Explain quantum computing briefly.")
    .WithGenerationConfig(new GenerationConfig { /* Set specific config properties */ })
    // OR .WithDefaultGenerationConfig(temperature: 0.5f, maxOutputTokens: 256)
    .WithSafetySettings(new List<SafetySetting> { /* Define safety settings */ })
    // OR .DisableAllSafetySettings()
    .EnableGrounding() // If grounding is needed
    .WithResponseSchema(new { type = "object", properties = new { answer = new { type = "string" } } }) // Force JSON output matching OpenAPI schema
    .Build();

// Use the built request objects (textRequest, multimodalRequest, etc.) with the Generator
```

> **Important Notes:**
> - The `EnableGrounding` method is used for *Google Searching* tool, and `WithResponseSchema` is used to define the JSON schema for the generated content (reference to [Generate structured output with the Gemini API](https://ai.google.dev/gemini-api/docs/structured-output?lang=rest#json-schemas)).
> - The methods `EnableGrounding` and `WithResponseSchema` cannot be used at the same time.
> - Check the models support *Search* or *Structured outputs* at [Gemini models](https://ai.google.dev/gemini-api/docs/models).

### `Generator`

The `Generator` class sends the built request to the Gemini API and returns the response.

**Initialization:**

*   **Using API Key:**
    ```csharp
    string apiKey = "YOUR_API_KEY";
    var generator = new Generator(apiKey);
    ```
*   **Using Google Cloud Credentials:**
    ```csharp
    string projectName = "YOUR_GCP_PROJECT_NAME";
    string projectId = "YOUR_GCP_PROJECT_ID";
    string bearerToken = "YOUR_OAUTH_BEARER_TOKEN"; // Obtain this via gcloud auth print-access-token or client libraries
    double timeout = 180; // Optional timeout in seconds (default is 120)

    var generator = new Generator(projectName, projectId, bearerToken, timeout);
    ```

**Generating Content:**

```csharp
// Assume 'generator' is initialized and 'request' is built using ApiRequestBuilder

try
{
    // Option 1: Use ModelVersion enum
    ModelResponse response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_1_5_Pro_Latest);

    // Option 2: Use a specific model alias string (e.g., for fine-tuned models)
    // ModelResponse response = await generator.GenerateContentAsync(request, "gemini-2.0-flash");

    // Option 3: Specify a different timeout for this specific call
    // ModelResponse response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash_Lite, apiTimeOutInSecond: 60);

    // Process the response
    if (response != null && response.Text != null) // Adjust based on ModelResponse structure
    {
        Console.WriteLine("Response: " + response.Text);
    }
    else
    {
        Console.WriteLine("No content generated or error occurred.");
        // Inspect the response object for more details if available
    }
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Error: Missing required argument - {ex.ParamName}");
}
catch (InvalidOperationException ex)
{
     Console.WriteLine($"Error: Invalid operation - {ex.Message}"); // E.g., API key invalid
}
catch (Exception ex) // Catch other potential exceptions (network, timeouts, etc.)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}
```

## Authentication

This library supports two methods for authenticating with the Gemini API, determined by the `Generator` constructor you use:

1.  **API Key Authentication:**
    *   Simplest method for quick use and prototyping.
    *   Obtain your key from [Google AI Studio](https://aistudio.google.com/app/apikey).
    *   Use the `Generator(string apiKey)` constructor.

2.  **Google Cloud Project Authentication (OAuth Bearer Token):**
    *   Recommended for production applications and accessing project-specific resources or fine-tuned models.
    *   Requires Google Cloud Project details (Name, ID) and a valid OAuth Bearer token associated with a service account or user credentials that have the necessary permissions (e.g., AI Platform User role).
    *   You can obtain a Bearer token using `gcloud auth print-access-token` (for user credentials) or via Google Cloud client libraries for service accounts.
    *   Use the `Generator(string cloudProjectName, string cloudProjectId, string bearer, double apiTimeOutInSecond = 120)` constructor.

Choose the method that best suits your application's security and operational requirements.

## **🤝 Contributing**

We welcome contributions to Gemini.NET!  Whether you're fixing a bug, suggesting a new feature, improving documentation, or writing code, your help is appreciated.

**How to Contribute:**

1. **Fork the repository:** Start by forking the Gemini.NET repository to your own GitHub account.
2. **Create a branch:**  Create a new branch for your contribution (e.g., `feature/new-utility` or `fix/bug-ocr`).
3. **Make your changes:** Implement your feature, bug fix, or documentation update.
4. **Test your changes:** Ensure your changes are working correctly and don't introduce regressions.
5. **Submit a pull request:** Once you're happy with your contribution, submit a pull request to the main repository, explaining your changes in detail.

**Types of Contributions We Appreciate:**

* **Bug Reports:** If you find a bug, please report it by creating an issue. Provide clear steps to reproduce the bug and any relevant details about your environment.
* **Feature Requests:** Have a great idea for a new feature or utility?  Open an issue to discuss it!
* **Code Contributions:**  We welcome code contributions that add new features, improve existing functionality, or fix bugs.
* **Documentation Improvements:** Help us make the documentation clearer, more comprehensive, and easier to understand.
* **Examples and Tutorials:**  Creating examples and tutorials can help other developers get started with Gemini.NET more quickly.

**Contribution Guidelines:**

* Follow the existing code style and conventions.
* Write clear and concise commit messages.
* Ensure your code is well-tested.
* Be respectful and considerate in your interactions with other contributors.

Please note that by contributing to Gemini.NET, you agree to abide by the project's Code of Conduct (if applicable, otherwise standard open source community guidelines apply).

We look forward to your contributions!
