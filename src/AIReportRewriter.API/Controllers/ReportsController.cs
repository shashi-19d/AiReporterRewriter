using Microsoft.AspNetCore.Mvc;
using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;

namespace AIReportRewriter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("rewrite")]
    public async Task<IActionResult> RewriteReport([FromBody] RewriteReportRequestDto request)
    {
        var result = await _reportService.ProcessReportAsync(request);
        return Ok(result);
    }
}