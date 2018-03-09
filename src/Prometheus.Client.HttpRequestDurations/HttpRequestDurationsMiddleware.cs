using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Prometheus.Client.HttpRequestDurations
{
    /// <summary>
    ///     Middleware for collect http responses
    /// </summary>
    public class HttpRequestDurationsMiddleware
    {
        private readonly string _metricHelpText = "duration histogram of http responses labeled with: ";

        private readonly RequestDelegate _next;
        private readonly HttpRequestDurationsOptions _options;
        private readonly Histogram _histogram;

        public HttpRequestDurationsMiddleware(RequestDelegate next, HttpRequestDurationsOptions options)
        {
            _next = next;
            _options = options;

            var labels = new List<string>();

            if (_options.IncludeStatusCode)
                labels.Add("status_code");

            if (_options.IncludeMethod)
                labels.Add("method");

            if (_options.IncludePath)
                labels.Add("path");

            _metricHelpText += string.Join(", ", labels);
            _histogram = _options.CollectorRegistry == null
                ? Metrics.CreateHistogram(options.MetricName, _metricHelpText, labels.ToArray())
                : Metrics.WithCustomRegistry(options.CollectorRegistry).CreateHistogram(options.MetricName, _metricHelpText, labels.ToArray());
        }

        public async Task Invoke(HttpContext context)
        {
            var route = context.Request.Path.ToString().ToLower();

            if (_options.IgnoreRoutesStartWith != null && _options.IgnoreRoutesStartWith.Any(i => route.StartsWith(i)))
            {
                await _next.Invoke(context);
                return;
            }

            if (_options.IgnoreRoutesContains != null && _options.IgnoreRoutesContains.Any(i => route.Contains(i)))
            {
                await _next.Invoke(context);
                return;
            }

            if (_options.IgnoreRoutesConcrete != null && _options.IgnoreRoutesConcrete.Any(i => route == i))
            {
                await _next.Invoke(context);
                return;
            }

            var watch = Stopwatch.StartNew();
            await _next.Invoke(context);
            watch.Stop();

            var labelValues = new List<string>();
            if (_options.IncludeStatusCode)
                labelValues.Add(context.Response.StatusCode.ToString());

            if (_options.IncludeMethod)
                labelValues.Add(context.Request.Method);

            if (_options.IncludePath)
                labelValues.Add(route);

            _histogram.Labels(labelValues.ToArray()).Observe(watch.Elapsed.TotalSeconds);
        }
    }
}