using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations.Tools;

internal class CapturedRouteDataFeature : ICapturedRouteDataFeature
{
    public RouteValueDictionary Values { get; } = new();
}
