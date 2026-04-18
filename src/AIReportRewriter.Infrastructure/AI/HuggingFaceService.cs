using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AIReportRewriter.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AIReportRewriter.Infrastructure.AI;

public class HuggingFaceService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public HuggingFaceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["HuggingFace:ApiKey"];
    }

    public async Task<string> RewriteAsync(string content, string tone)
    {
        var prompt = $@"Rewrite the financial report in {tone} tone. ONLY return the final rewritten sentence. DO NOT repeat instructions. Text:{content}";

        return await CallAI(prompt);
    }

    public async Task<string> SummarizeAsync(string content)
    {
        var prompt = $@"Summarize the following financial report in one short sentence. ONLY return the summary. Text:{content}";

        return await CallAI(prompt);
    }

    private async Task<string> CallAI(string prompt)
    {
        int maxRetries = 3;
        int delayMs = 1000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://router.huggingface.co/hf-inference/models/facebook/bart-large-cnn");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var requestBody = new
                {
                    inputs = prompt
                };

                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HF Error: {error}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseContent);

                return jsonDoc.RootElement[0]
                    .GetProperty("summary_text")
                    .GetString() ?? "No response";
            }
            catch (Exception ex)
            {
                if (attempt == maxRetries)
                    throw;

                await Task.Delay(delayMs * attempt);

                Console.WriteLine($"Retry {attempt} failed: {ex.Message}");
            }
        }

        throw new Exception("AI call failed after retries");
    }

}