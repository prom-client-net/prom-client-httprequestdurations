#if HasRoutes
using Microsoft.AspNetCore.Routing;
#endif

namespace Prometheus.Client.HttpRequestDurations
{
    interface ICapturedRouteDataFeature
    {
#if HasRoutes
        RouteValueDictionary Values { get; }
#endif
    }
}
