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
        var prompt = $"Rewrite this financial report in a {tone} tone:\n{content}";
        return await CallAI(prompt);
    }

    public async Task<string> SummarizeAsync(string content)
    {
        var prompt = $"Summarize the following text:\n{content}";
        return await CallAI(prompt);
    }
}