using AIReportRewriter.Domain.Entities;

public interface IReportRepository
{
    Task AddAsync(FinancialReport report);
}