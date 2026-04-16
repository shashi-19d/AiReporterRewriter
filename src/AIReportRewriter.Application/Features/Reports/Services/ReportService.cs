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
        var rewrittenJson = await _aiService.RewriteAsync(request.Content, request.Tone);
        var summaryJson = await _aiService.SummarizeAsync(request.Content);

        var rewrittenDoc = JsonDocument.Parse(rewrittenJson);
        var summaryDoc = JsonDocument.Parse(summaryJson);

        return new RewriteReportResponseDto
        {
            RewrittenContent = rewrittenDoc.RootElement.GetProperty("rewrittenContent").GetString(),
            Summary = summaryDoc.RootElement.GetProperty("summary").GetString()
        };
    }
}