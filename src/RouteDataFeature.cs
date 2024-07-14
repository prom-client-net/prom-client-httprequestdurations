using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations;

internal class RouteDataFeature
{
    public RouteValueDictionary Values { get; } = new();
}
