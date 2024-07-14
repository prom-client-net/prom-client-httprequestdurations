using System.Linq;

namespace Prometheus.Client.HttpRequestDurations;

internal static class NormalizePath
{
    public static string Execute(string pathString, HttpRequestDurationsOptions options)
    {
        var result = pathString.ToLowerInvariant();
        if (options.IncludeCustomNormalizePath)
            result = options.CustomNormalizePath.Aggregate(result, (current, normalizePath) => normalizePath.Key.Replace(current, normalizePath.Value));

        return result;
    }
}
