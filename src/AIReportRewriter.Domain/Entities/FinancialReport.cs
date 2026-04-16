namespace AIReportRewriter.Domain.Entities;

public class FinancialReport
{
    public Guid Id { get; set; }

    public required string OriginalContent { get; set; }

    public required string RewrittenContent { get; set; }

    public required string Summary { get; set; }
    public required string Tone { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}