using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;

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
        var rewritten = await _aiService.RewriteAsync(request.Content, request.Tone);
        var summary = await _aiService.SummarizeAsync(request.Content);

        return new RewriteReportResponseDto
        {
            RewrittenContent = rewritten,
            Summary = summary
        };
    }
}