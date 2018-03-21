using System.Text.RegularExpressions;

namespace Prometheus.Client.HttpRequestDurations
{
    internal static class NormalizeHelper
    {
        public static string Replace(string subjectString, string pattern, string replacement)
        {
            return Regex.Replace(subjectString,
                pattern,
                replacement,
                RegexOptions.IgnoreCase);
        }
    }
}