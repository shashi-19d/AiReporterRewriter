using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Domain.Entities;

namespace AIReportRewriter.Application.Features.Reports.Interfaces;

public interface IReportService
{
    Task<RewriteReportResponseDto> ProcessReportAsync(RewriteReportRequestDto request);

    Task<PagedResponse<RewriteReportResponseDto>> GetReportsAsync(GetReportsQuery query);

    Task<RewriteReportResponseDto?> GetByIdAsync(int id);
}