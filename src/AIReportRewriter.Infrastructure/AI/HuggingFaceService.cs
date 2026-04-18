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
        var url = "https://router.huggingface.co/hf-inference/models/facebook/bart-large-cnn";

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestBody = new
        {
            inputs = prompt
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"HuggingFace Error: {responseContent}");
        }

        using var jsonDoc = JsonDocument.Parse(responseContent);

        var root = jsonDoc.RootElement;

        if (root.ValueKind == JsonValueKind.Array)
        {
            var firstItem = root[0];

            if (firstItem.TryGetProperty("summary_text", out var summary))
            {
                return summary.GetString() ?? "";
            }
        }

        return responseContent;
    }

}