namespace AIReportRewriter.Application.Interfaces;

public interface IAIService
{
    Task<string> RewriteAsync(string content, string tone);
    Task<string> SummarizeAsync(string content);
}