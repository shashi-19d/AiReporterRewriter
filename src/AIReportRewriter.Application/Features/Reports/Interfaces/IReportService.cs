using AIReportRewriter.Application.Features.Reports.DTOs;

namespace AIReportRewriter.Application.Features.Reports.Interfaces;

public interface IReportService
{
    Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request);
}