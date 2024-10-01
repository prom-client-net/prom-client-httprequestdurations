namespace Prometheus.Client.HttpRequestDurations;

internal static class Defaults
{
    internal const string MetricName = "http_request_duration_seconds";

    internal static class LabelNames
    {
        internal const string StatusCode = "status_code";
        internal const string Method = "method";
        internal const string Controller = "controller";
        internal const string Action = "action";
        internal const string Path = "path";
    }
}
