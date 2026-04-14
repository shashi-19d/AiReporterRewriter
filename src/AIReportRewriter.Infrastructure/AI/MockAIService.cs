using AIReportRewriter.Application.Interfaces;

namespace AIReportRewriter.Infrastructure.AI;

public class MockAIService : IAIService
{
    public Task<string> RewriteAsync(string content, string tone)
    {
        return Task.FromResult($"[{tone}] Rewritten: {content}");
    }

    public Task<string> SummarizeAsync(string content)
    {
        var shortText = content.Length > 50
            ? content.Substring(0, 50) + "..."
            : content;

        return Task.FromResult($"Summary: {shortText}");
    }
}