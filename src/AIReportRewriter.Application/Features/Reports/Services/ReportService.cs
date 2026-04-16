using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Interfaces;
using System.Text.Json;

namespace AIReportRewriter.Application.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IAIService _aiService;

    public ReportService(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request)
    {
        var rewrittenText = await _aiService.RewriteAsync(request.Content, request.Tone);
        var summaryText = await _aiService.SummarizeAsync(request.Content);

        return new RewriteReportResponseDto
        {
            RewrittenContent = CleanText(rewrittenText),
            Summary = CleanText(summaryText)
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