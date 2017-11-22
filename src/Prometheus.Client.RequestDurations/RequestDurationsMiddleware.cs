using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Prometheus.Client.Owin.RequestDurations
{
    public class RequestDurationsMiddleware
    {
        private string _metricHelpText = "duration histogram of http responses labeled with: ";

        private readonly RequestDelegate _next;
        private readonly RequestDurationsOptions _options;
        private Histogram _histogram;

        public RequestDurationsMiddleware(RequestDelegate next, RequestDurationsOptions options)
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
           
            // create histogram with Registry            
        }

        public async Task Invoke(HttpContext context)
        {
            var route = context.Request.Path.ToString();
            if (_options.ExcludeRoutes.Any(i => route.Contains(i)))
            {
                await _next.Invoke(context);
                return;
            }
            var watch = Stopwatch.StartNew();

            await _next.Invoke(context);

            var method = context.Request.Method;
            var statusCode = context.Response.StatusCode.ToString();

            watch.Stop();
            var seconds = watch.Elapsed.Seconds;
            if (seconds > 0)
            {
                // write metric
            }
        }
    }
}