using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.HttpRequestDurations.Tools;

namespace Prometheus.Client.HttpRequestDurations
{
    /// <summary>
    ///     Middleware for collect http responses
    /// </summary>
    public class HttpRequestDurationsMiddleware
    {
        private readonly IMetricFamily<IHistogram> _histogram;
        private readonly string _metricHelpText = "duration histogram of http responses labeled with: ";

        private readonly RequestDelegate _next;
        private readonly HttpRequestDurationsOptions _options;

        public HttpRequestDurationsMiddleware(RequestDelegate next, HttpRequestDurationsOptions options)
        {
            _next = next;
            _options = options;
            var metricFactory = new MetricFactory(options.CollectorRegistry);

            var labels = new List<string>();

            if (_options.IncludeStatusCode)
                labels.Add("status_code");

            if (_options.IncludeMethod)
                labels.Add("method");

            if (_options.IncludePath)
                labels.Add("path");

            if (_options.IncludeCustomLabels)
                labels.AddRange(_options.CustomLabels.Select(customLabel => customLabel.Key));

            _metricHelpText += string.Join(", ", labels);
            _histogram = metricFactory.CreateHistogram(options.MetricName, _metricHelpText, options.IncludeTimestamp, options.Buckets, labels.ToArray());
        }

        public async Task Invoke(HttpContext context)
        {
            var path = string.Empty;
#if HasRoutes
            if(_options.UseRouteName)
                path = context.GetRouteName();
#endif
            if (string.IsNullOrEmpty(path))
                path = context.Request.Path.ToString();

            path = NormalizePath.Execute(path, _options);

            if (_options.IgnoreRoutesStartWith != null && _options.IgnoreRoutesStartWith.Any(i => path.StartsWith(i)))
            {
                await _next.Invoke(context);
                return;
            }

            if (_options.IgnoreRoutesContains != null && _options.IgnoreRoutesContains.Any(i => path.Contains(i)))
            {
                await _next.Invoke(context);
                return;
            }

            if (_options.IgnoreRoutesConcrete != null && _options.IgnoreRoutesConcrete.Any(i => path == i))
            {
                await _next.Invoke(context);
                return;
            }

            if (_options.ShouldMeasureRequest != null && !_options.ShouldMeasureRequest(context.Request))
            {
                await _next.Invoke(context);
                return;
            }

            string statusCode = null;
            var method = context.Request.Method;
            var ts = Stopwatch.GetTimestamp();

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception)
            {
                statusCode = "500";
                throw;
            }
            finally
            {
                if (string.IsNullOrEmpty(statusCode))
                    statusCode = context.Response.StatusCode.ToString();

                double ticks = Stopwatch.GetTimestamp() - ts;
                WriteMetrics(statusCode, method, path, ticks / Stopwatch.Frequency);
            }
        }

        private void WriteMetrics(string statusCode, string method, string path, double elapsedSeconds)
        {
            // Order is important

            var labelValues = new List<string>();
            if (_options.IncludeStatusCode)
                labelValues.Add(statusCode);

            if (_options.IncludeMethod)
                labelValues.Add(method);

            if (_options.IncludePath)
                labelValues.Add(path);

            if (_options.CustomLabels != null)
            {
                foreach (var customLabel in _options.CustomLabels)
                    labelValues.Add(customLabel.Value());
            }

            _histogram.WithLabels(labelValues.ToArray()).Observe(elapsedSeconds);
        }
    }
}
