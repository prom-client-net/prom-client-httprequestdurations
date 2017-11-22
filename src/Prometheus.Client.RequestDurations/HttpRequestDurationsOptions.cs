using System.Collections.Generic;

namespace Prometheus.Client.Owin.RequestDurations
{
    /// <summary>
    ///     Options for RequestDurationsMiddleware
    /// </summary>
    public class RequestDurationsOptions
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
        public List<string> ExcludeRoutes { get; set; }
        
        /// <summary>
        ///     Constructor
        /// </summary>
        public RequestDurationsOptions()
        {
            MetricName = "http_request_duration_seconds";
            
            IncludeStatusCode = true;
            IncludeMethod = false;
            IncludePath = false;
            
            ExcludeRoutes = new List<string>();
        }
    }
}
