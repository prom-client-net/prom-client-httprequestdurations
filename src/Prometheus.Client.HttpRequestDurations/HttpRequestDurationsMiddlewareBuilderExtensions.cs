using System;
using Microsoft.AspNetCore.Builder;

namespace Prometheus.Client.HttpRequestDurations
{
    public static class PrometheusMiddlewareBuilderExtensions
    {
        public static IApplicationBuilder UsePrometheusRequestDurations(this IApplicationBuilder app)
        {
            return UsePrometheusRequestDurations(app, null);
        }
        
        public static IApplicationBuilder UsePrometheusRequestDurations(this IApplicationBuilder app, Action<HttpRequestDurationsOptions> setupAction)
        {
            var options = new HttpRequestDurationsOptions();
            setupAction?.Invoke(options);
            return app.UseMiddleware<HttpRequestDurationsMiddleware>(options);
        }
    }
}
