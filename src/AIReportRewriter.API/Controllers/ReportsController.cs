using AIReportRewriter.Application.Features.Reports.DTOs;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using AIReportRewriter.Application.Features.Reports.Services;

namespace AIReportRewriter.API.Controllers;

[EnableRateLimiting("fixed")]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReports([FromQuery] GetReportsQuery query)
    {
        var result = await _reportService.GetReportsAsync(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reportService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("rewrite")]
    public async Task<IActionResult> RewriteReport([FromBody] RewriteReportRequestDto request)
    {
        var result = await _reportService.ProcessReportAsync(request);
        return Ok(new
        {
            success = true,
            data = result
        });
    }
}