using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Prometheus.Client.HttpRequestDurations.Tools;

internal static class HttpContextExtensions
{
    public static string GetRouteName(this HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var endpoint = httpContext.GetEndpoint();

        var result = endpoint?.Metadata.GetMetadata<RouteNameMetadata>()?.RouteName;

        if (string.IsNullOrEmpty(result))
        {
            var routeAttribute = endpoint?.Metadata.GetMetadata<RouteAttribute>();
            var methodAttribute = endpoint?.Metadata.GetMetadata<HttpMethodAttribute>();

            result = $"{routeAttribute?.Template}{methodAttribute?.Template}";
        }

        return result;
    }

    /// <summary>
    /// Attempt to determine the controller name
    /// </summary>
    /// <param name="httpContext">Http Context</param>
    /// <returns>Name of the controller if identified; returns 'NotAvailable' if unable to be located.</returns>
    public static string GetControllerName(this HttpContext httpContext) => GetRouteDataKeyValue(httpContext, "controller");

    /// <summary>
    /// Attempt to determine the action (method) name
    /// </summary>
    /// <param name="httpContext">Http Context</param>
    /// <returns>Name of the action if identified; returns 'NotAvailable' if unable to be located.</returns>
    public static string GetActionName(this HttpContext httpContext) => GetRouteDataKeyValue(httpContext, "action");

    private static string GetRouteDataKeyValue(this HttpContext httpContext, string routeDataKey)
    {
        const string notFoundResult = "NotAvailable";

        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var routeData = httpContext.Features.Get<ICapturedRouteDataFeature>()?.Values;

        // If we have captured route data, we always prefer it.
        // Otherwise, we extract new route data right now.
        if (routeData == null)
            routeData = httpContext.GetRouteData()?.Values;

        if (routeData?.Values.Count > 0)
        {
            var result = string.Empty;

            foreach (var item in routeData)
            {
                if (item.Key == routeDataKey)
                {
                    result = item.Value?.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(result))
                return result;
        }

        return notFoundResult;
    }
}
