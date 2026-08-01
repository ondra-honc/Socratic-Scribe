namespace Socratic_Academic_Writing_Assistant.Helpers;

public static class TextSimilarity
{
  public static double CalculateSimilarity(string source, string target)
  {
    if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0.0;

    string s = source.Trim().ToLowerInvariant();
    string t = target.Trim().ToLowerInvariant();

    if (s == t) return 1.0;
    if (s.Length == 0 || t.Length == 0) return 0.0;

    int distance = ComputeLevenshteinDistance(s, t);
    int maxLength = Math.Max(s.Length, t.Length);

    return 1.0 - ((double)distance / maxLength);
  }

  private static int ComputeLevenshteinDistance(string source, string target)
  {
    int sLen = source.Length;
    int tLen = target.Length;

    if (sLen == 0) return tLen;
    if (tLen == 0) return sLen;

    int[] v0 = new int[tLen + 1];
    int[] v1 = new int[tLen + 1];

    for (int i = 0; i <= tLen; i++)
    {
      v0[i] = i;
    }

    for (int i = 0; i < sLen; i++)
    {
      v1[0] = i + 1;

      for (int j = 0; j < tLen; j++)
      {
        int cost = (source[i] == target[j]) ? 0 : 1;
        v1[j + 1] = Math.Min(
            Math.Min(v1[j] + 1, v0[j + 1] + 1),
            v0[j] + cost
        );
      }

      var temp = v0;
      v0 = v1;
      v1 = temp;
    }
    return v0[tLen];
  }
}