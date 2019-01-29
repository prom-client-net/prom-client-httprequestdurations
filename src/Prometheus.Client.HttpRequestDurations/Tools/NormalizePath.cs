using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;

[assembly: InternalsVisibleTo("Prometheus.Client.HttpRequestDurations.Tests")]

namespace Prometheus.Client.HttpRequestDurations.Tools
{
    internal static class NormalizePath
    {
        public static string Execute(PathString pathString, HttpRequestDurationsOptions options)
        {
            var result = pathString.ToString().ToLowerInvariant();
            if (options.IncludeCustomNormalizePath)
                foreach (var normalizePath in options.CustomNormalizePath)
                    result = normalizePath.Key.Replace(result, normalizePath.Value);
            return result;
        }
    }
}
