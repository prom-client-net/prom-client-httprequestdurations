using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Prometheus.Client.HttpRequestDurations
{
    /// <summary>
    ///     Middleware for collect http responses
    /// </summary>
    public class HttpRequestDurationsMiddleware
    {
        private readonly Histogram _histogram;
        private readonly string _metricHelpText = "duration histogram of http responses labeled with: ";

        private readonly RequestDelegate _next;
        private readonly HttpRequestDurationsOptions _options;

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

            if (_options.IncludeCustomLabels)
                foreach (var customLabel in _options.CustomLabels)
                    labels.Add(customLabel.Key);


            _metricHelpText += string.Join(", ", labels);
            _histogram = _options.CollectorRegistry == null
                ? Metrics.CreateHistogram(options.MetricName, _metricHelpText, _options.Buckets, labels.ToArray())
                : Metrics.WithCustomRegistry(options.CollectorRegistry)
                    .CreateHistogram(options.MetricName, _metricHelpText, _options.Buckets, labels.ToArray());
        }

        public async Task Invoke(HttpContext context)
        {
            string route = context.Request.Path.ToString().ToLower();

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

            
            if (_options.IncludeCustomLabels)
                foreach (var customLabel in _options.CustomLabels)
                    labelValues.Add(customLabel.Value);
            
            if(_options.IncludePath && _options.IncludeNormalizePath)
                foreach (var normalizePath in _options.NormalizePath)
                    route = normalizePath.Key.Replace(route, normalizePath.Value);
                
            if (_options.IncludePath)
                labelValues.Add(route);

          
            _histogram.Labels(labelValues.ToArray()).Observe(watch.Elapsed.TotalSeconds);
        }
    }
}
