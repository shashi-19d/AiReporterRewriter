using AIReportRewriter.Domain.Entities;
using AIReportRewriter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FinancialReport report)
    {
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<FinancialReport>, int)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? tone)
    {
        var query = _context.Reports.AsQueryable();

        if (!string.IsNullOrEmpty(tone))
            query = query.Where(x => x.Tone == tone);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    public async Task<FinancialReport?> GetByIdAsync(int id)
    {
        return await _context.Reports.FindAsync(id);
    }
}