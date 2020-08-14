using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Prometheus.Client.HttpRequestDurations.Tools
{
    internal static class NormalizePath
    {
        public static string Execute(PathString pathString, HttpRequestDurationsOptions options)
        {
            var result = pathString.ToString().ToLowerInvariant();
            if (options.IncludeCustomNormalizePath)
                result = options.CustomNormalizePath.Aggregate(result, (current, normalizePath) => normalizePath.Key.Replace(current, normalizePath.Value));

            return result;
        }
    }
}
