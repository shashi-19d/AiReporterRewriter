using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AIReportRewriter.Application.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;
    private readonly IAIService _aiService;
    private readonly ILogger<ReportService> _logger;
    private readonly IMemoryCache _cache;

    public ReportService(IAIService aiService, IReportRepository repository, ILogger<ReportService> logger, IMemoryCache cache)
    {
        _aiService = aiService;
        _repository = repository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request)
    {
        _logger.LogInformation("Processing report for Tone: {Tone}", request.Tone);

        // STEP A: Create cache key
        var cacheKey = $"{request.Content}_{request.Tone}";

        //  STEP B: Check cache BEFORE AI call
        if (_cache.TryGetValue(cacheKey, out RewriteReportResponseDto cachedResponse))
        {
            _logger.LogInformation("Returning response from cache");
            return cachedResponse;
        }

        //  STEP C: Call AI (only if cache miss)
        var rewritten = await _aiService.RewriteAsync(request.Content, request.Tone);
        var summary = await _aiService.SummarizeAsync(request.Content);

        //  STEP D: Clean output
        rewritten = CleanText(rewritten);
        summary = CleanText(summary);

        //  STEP E: Prepare response
        var response = new RewriteReportResponseDto
        {
            RewrittenContent = rewritten,
            Summary = summary
        };

        //  STEP F: SAVE TO DB (existing logic)
        var report = new FinancialReport
        {
            OriginalContent = request.Content,
            RewrittenContent = rewritten,
            Summary = summary,
            Tone = request.Tone,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(report);

        //  STEP G: Store in cache (AFTER success)
        _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
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