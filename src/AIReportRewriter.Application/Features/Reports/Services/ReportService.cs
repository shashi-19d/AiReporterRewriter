using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AIReportRewriter.Application.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;
    private readonly IAIService _aiService;
    private readonly ILogger<ReportService> _logger;

    public ReportService(IAIService aiService, IReportRepository repository, ILogger<ReportService> logger)
    {
        _aiService = aiService;
        _repository = repository;
        _logger = logger;
    }

    public async Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request)
    {
        _logger.LogInformation("Processing report rewrite request");

        var rewritten = await _aiService.RewriteAsync(request.Content, request.Tone);
        _logger.LogInformation("Rewrite completed");

        var summary = await _aiService.SummarizeAsync(request.Content);
        _logger.LogInformation("Summary generated");

        rewritten = CleanText(rewritten);
        summary = CleanText(summary);

        var report = new FinancialReport
        {
            OriginalContent = request.Content,
            RewrittenContent = rewritten,
            Summary = summary,
            Tone = request.Tone
        };

        await _repository.AddAsync(report);

        return new RewriteReportResponseDto
        {
            RewrittenContent = rewritten,
            Summary = summary
        };
    }

    private string CleanText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        var junkPhrases = new[]
        {
          "Rewrite the following",
          "ONLY return",
          "DO NOT",
          "Text:",
          "Provide a short",
          "Summarize",
          "Re-write",
          "explanation",
          "call the Samaritans",
         "National Suicide Prevention"
        };

        foreach (var junk in junkPhrases)
        {
            text = text.Replace(junk, "", StringComparison.OrdinalIgnoreCase);
        }

        var firstSentence = text.Split('.', '!', '?').FirstOrDefault();

        return firstSentence?.Trim() + ".";
    }
}