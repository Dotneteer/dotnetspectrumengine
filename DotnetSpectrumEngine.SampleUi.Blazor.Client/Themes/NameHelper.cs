using System.Text;

namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// Helper functions for name conversion
    /// </summary>
    public static class NameHelper
    {
        public static string ToCssName(string name)
        {
            var sb = new StringBuilder(100);
            var lastCharWasLowerCase = false;
            foreach (var ch in name)
            {
                if (char.IsLower(ch))
                {
                    sb.Append(ch);
                    lastCharWasLowerCase = true;
                }
                else
                {
                    if (lastCharWasLowerCase)
                    {
                        sb.Append("-");
                    }
                    sb.Append(char.ToLower(ch));
                    lastCharWasLowerCase = false;
                }
            }
            return sb.ToString();
        }
    }
}
