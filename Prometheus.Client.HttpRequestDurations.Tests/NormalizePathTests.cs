using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.HttpRequestDurations.Tools;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests
{
    public class NormalizePathTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(156842)]
        public void Normalize_Int_Center(uint id) // ID - can't be less 0
        {
            var pathString = new PathString($"/path/to/{id}/next");
            
            var options = new HttpRequestDurationsOptions()
            {
                CustomNormalizePath = new Dictionary<Regex, string>()
                {
                    { new Regex(@"\/[0-9]{1,}(?![a-z])"), "/id" }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/id/next", path);
        }
    }
}
