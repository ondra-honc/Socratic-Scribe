namespace Socratic_Academic_Writing_Assistant.Services
{
  public interface IAIAnalysisService
  {
    IAsyncEnumerable<string> Analyze(string input, bool deepMode, CancellationToken cancelToken = default);
  }
}
