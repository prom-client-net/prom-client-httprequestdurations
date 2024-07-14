using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations;

internal class CapturedRouteDataFeature
{
    public RouteValueDictionary Values { get; } = new();
}
