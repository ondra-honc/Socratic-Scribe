using System.ComponentModel.DataAnnotations;

namespace Socratic_Academic_Writing_Assistant.DTOs
{
  public class AnalyzeTextRequest
  {
    [StringLength(5000, ErrorMessage = "Wrong Text Length" ,MinimumLength = 25)]
    public required string InputText { get; set; }
    public bool IsDeepMode { get; set; }
  }
}
