using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations.Tools;

internal interface ICapturedRouteDataFeature
{
    RouteValueDictionary Values { get; }
}
