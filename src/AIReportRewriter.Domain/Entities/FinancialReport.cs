namespace AIReportRewriter.Domain.Entities;

public class FinancialReport
{
    public Guid Id { get; set; }

    public string OriginalContent { get; set; }

    public string RewrittenContent { get; set; }

    public string Summary { get; set; }

    public string Tone { get; set; }

    public DateTime CreatedAt { get; set; }
}