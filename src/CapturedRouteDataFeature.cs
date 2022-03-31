#if HasRoutes
using Microsoft.AspNetCore.Routing;
#endif

namespace Prometheus.Client.HttpRequestDurations
{
    sealed class CapturedRouteDataFeature : ICapturedRouteDataFeature
    {
#if HasRoutes
        public RouteValueDictionary Values { get; } = new RouteValueDictionary();
#endif
    }
}
