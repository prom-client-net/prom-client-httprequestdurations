using System;
using Microsoft.AspNetCore.Builder;

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
            return app.UseMiddleware<HttpRequestDurationsMiddleware>(options);
        }
    }
}
