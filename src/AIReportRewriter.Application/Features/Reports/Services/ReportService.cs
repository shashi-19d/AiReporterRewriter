using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Domain.Entities;
using System.Text.Json;

namespace AIReportRewriter.Application.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;
    private readonly IAIService _aiService;

    public ReportService(IAIService aiService, IReportRepository repository)
    {
        _aiService = aiService;
        _repository = repository;
    }

    public async Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request)
    {
        var rewritten = await _aiService.RewriteAsync(request.Content, request.Tone);
        var summary = await _aiService.SummarizeAsync(request.Content);

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

        if (text.Contains(":"))
        {
            text = text.Substring(text.LastIndexOf(":") + 1);
        }

        return text.Trim();
    }
}