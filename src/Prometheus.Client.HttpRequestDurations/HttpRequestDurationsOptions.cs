using System.Collections.Generic;
using System.Text.RegularExpressions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.HttpRequestDurations
{
    /// <summary>
    ///     Options for RequestDurationsMiddleware
    /// </summary>
    public class HttpRequestDurationsOptions
    {
        /// <summary>
        ///      Metric name
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     HTTP status code (200, 400, 404 etc.)
        /// </summary>
        public bool IncludeStatusCode { get; set; }

        /// <summary>
        ///     HTTP method (GET, PUT, ...)
        /// </summary>
        public bool IncludeMethod { get; set; }

        /// <summary>
        ///     URL path
        /// </summary>
        public bool IncludePath { get; set; }

        /// <summary>
        ///     Ignore paths 
        /// </summary>
        public string[] IgnoreRoutesConcrete { get; set; }
        
        /// <summary>
        ///     Ignore paths 
        /// </summary>
        public string[] IgnoreRoutesContains { get; set; }

        /// <summary>
        ///     Ignore paths 
        /// </summary>
        public string[] IgnoreRoutesStartWith { get; set; }

        /// <summary>
        ///     Collector Registry
        /// </summary>
        public ICollectorRegistry CollectorRegistry { get; set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public HttpRequestDurationsOptions()
        {
            MetricName = "http_request_duration_seconds";

            IncludeStatusCode = true;
            IncludeMethod = false;
            IncludePath = false;
        }
    }
}