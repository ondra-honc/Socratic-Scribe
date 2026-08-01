namespace Socratic_Academic_Writing_Assistant.DTOs
{
  public record OpenRouterMessage(string Role, string Content);
  public record OpenRouterRequest(string Model, List<OpenRouterMessage> Messages, bool Stream = true);
}
