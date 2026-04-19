using AIReportRewriter.Domain.Entities;

public interface IReportRepository
{
    Task AddAsync(FinancialReport report);

    Task<(IEnumerable<FinancialReport>, int)> GetPagedAsync(int pageNumber, int pageSize, string? tone);

    Task<FinancialReport?> GetByIdAsync(int id);
}