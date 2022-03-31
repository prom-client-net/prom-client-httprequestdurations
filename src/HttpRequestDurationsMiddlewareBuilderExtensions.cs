using System;
using Microsoft.AspNetCore.Builder;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.HttpRequestDurations
{
    public static class PrometheusMiddlewareBuilderExtensions
    {
        /// <summary>
        ///     Metrics logging of request durations
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        public static IApplicationBuilder UsePrometheusRequestDurations(this IApplicationBuilder app)
        {
            return UsePrometheusRequestDurations(app, null);
        }

        /// <summary>
        ///     Metrics logging of request durations
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="setupOptions">Setup Options</param>
        public static IApplicationBuilder UsePrometheusRequestDurations(this IApplicationBuilder app, Action<HttpRequestDurationsOptions> setupOptions)
        {
            var options = new HttpRequestDurationsOptions();
            setupOptions?.Invoke(options);

            options.CollectorRegistry
                ??= (ICollectorRegistry)app.ApplicationServices.GetService(typeof(ICollectorRegistry))
                    ?? Metrics.DefaultCollectorRegistry;

#if HasRoutes
            // If we are going to set labels for Controller or Action -- then we need to make them readily available
            if (options.IncludeController || options.IncludeAction)
                app.UseMiddleware<CaptureRouteDataMiddleware>();
#endif

            return app.UseMiddleware<HttpRequestDurationsMiddleware>(options);
        }
    }
}
