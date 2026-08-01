namespace Socratic_Academic_Writing_Assistant.DTOs
{
  public record OpenRouterResponseDelta(string? Content);
  public record OpenRouterResponseChoice(OpenRouterResponseDelta Delta);
  public record OpenRouterResponseRoot(List<OpenRouterResponseChoice> Choices);
}
