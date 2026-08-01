using Microsoft.AspNetCore.Mvc;
using Socratic_Academic_Writing_Assistant.DTOs;
using Socratic_Academic_Writing_Assistant.Helpers;
using Socratic_Academic_Writing_Assistant.Services;
using System.Text;
namespace Socratic_Academic_Writing_Assistant.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AnalysisController : ControllerBase
  {
    private readonly IAIAnalysisService _aiAnalysisService;
    public AnalysisController(IAIAnalysisService service)
    {
      _aiAnalysisService = service;
    }

    [HttpPost]
    async public Task Analyze([FromBody] AnalyzeTextRequest textReq)
    {
      Response.ContentType = "text/event-stream";
      Response.Headers.CacheControl = "no-cache";
      const double SimilarityThreshold = 0.60;
      var sentenceBuffer = new StringBuilder();

      try
      {
        await foreach (var token in _aiAnalysisService.Analyze(textReq.InputText, textReq.IsDeepMode, HttpContext.RequestAborted))
        {
          if (token.Contains(".") || token.Contains("?") || token.Contains("!"))
          {
            string sentence = sentenceBuffer.ToString();
            sentenceBuffer.Clear();
            double similarity = TextSimilarity.CalculateSimilarity(textReq.InputText, sentence);
            string outputText = sentence;

            if (similarity > SimilarityThreshold)
            {
              outputText = " [Guardrail: Let's focus on analyzing your writing step-by-step rather than rewriting it.] ";
            }
          }

          string SSEFrame = $"event: hint-token\ndata: {token}\n\n";
          await Response.WriteAsync(SSEFrame);
          await Response.Body.FlushAsync();

          if (sentenceBuffer.Length > 0)
          { 
            string finalSentence = sentenceBuffer.ToString();
            double finalSimilarity = TextSimilarity.CalculateSimilarity(textReq.InputText, finalSentence);
            string outputText = finalSimilarity > SimilarityThreshold ? " [Guardrail: Direct rewrite detected.] " : finalSentence;

            string sseFrame = $"event: hint-token\ndata: {outputText}\n\n";
            await Response.WriteAsync(sseFrame);
            await Response.Body.FlushAsync();
          }
        }
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("User canceled streaming");
      }
      catch (Exception)
      {
        string SSEException = "event: error\ndata: An error occurred while generating Socratic hints.\n\n";
        await Response.WriteAsync(SSEException);
        await Response.Body.FlushAsync();
      }

      string SSEFrameComplete = "event: stream-complete\ndata: [DONE]\n\n";
      await Response.WriteAsync(SSEFrameComplete);
      await Response.Body.FlushAsync();
    }
  }
}