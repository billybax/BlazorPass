using System.Text.Json;

namespace BlazorPass.Services;

public class GeminiService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

    public GeminiService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _apiKey = _configuration["GeminiApiKey"] ?? string.Empty;

        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("GeminiApiKey is not configured in appsettings or User Secrets.");
    }

    /// <summary>
    /// Generates a secure password using Gemini API
    /// </summary>
    public async Task<string> GeneratePasswordAsync(int length = 16, bool includeSpecial = true)
    {
        var prompt = $"Generate a random secure password with exactly {length} characters. " +
            (includeSpecial ? "Include uppercase, lowercase, numbers, and special characters." : "Use only uppercase, lowercase, and numbers.") +
            " Return ONLY the password, nothing else.";

        var response = await CallGeminiApiAsync(prompt);
        return response;
    }

    /// <summary>
    /// Checks password strength using Gemini
    /// </summary>
    public async Task<PasswordStrengthResult> CheckPasswordStrengthAsync(string password)
    {
        var prompt = $"Analyze the security strength of this password: '{password}'. " +
            "Rate it as 'Weak', 'Medium', or 'Strong'. " +
            "Provide a brief reason in 1-2 sentences. " +
            "Format your response as: RATING|REASON";

        var result = await CallGeminiApiAsync(prompt);
        var parts = result.Split('|');

        return new PasswordStrengthResult
        {
            Rating = parts.Length > 0 ? parts[0].Trim() : "Unknown",
            Reason = parts.Length > 1 ? parts[1].Trim() : "Unable to analyze"
        };
    }

    /// <summary>
    /// Gets password security recommendations using Gemini
    /// </summary>
    public async Task<string> GetSecurityRecommendationsAsync(string password)
    {
        var prompt = $"Provide security improvement recommendations for this password: '{password}'. " +
            "List 2-3 specific actionable recommendations. Keep it concise.";

        return await CallGeminiApiAsync(prompt);
    }

    private async Task<string> CallGeminiApiAsync(string prompt)
    {
        try
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var url = $"{GeminiApiUrl}?key={_apiKey}";
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return $"Error: {response.StatusCode} - {errorContent}";
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using (var doc = JsonDocument.Parse(responseContent))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var contentObj) &&
                        contentObj.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        var firstPart = parts[0];
                        if (firstPart.TryGetProperty("text", out var text))
                        {
                            return text.GetString() ?? "No response";
                        }
                    }
                }
            }

            return "Unable to parse response";
        }
        catch (Exception ex)
        {
            return $"Error calling Gemini API: {ex.Message}";
        }
    }
}

public class PasswordStrengthResult
{
    public string Rating { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}
