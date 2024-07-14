using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations.Tools;

internal class CapturedRouteDataFeature
{
    public RouteValueDictionary Values { get; } = new();
}
