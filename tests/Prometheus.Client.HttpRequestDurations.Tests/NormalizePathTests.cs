using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.HttpRequestDurations.Tools;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests
{
    public class NormalizePathTests
    {
        public static IEnumerable<object[]> GetInt()
        {
            // ID - can't be less 0
            yield return new object[] { 2 };
            yield return new object[] { 0 };
            yield return new object[] { 4 };
            yield return new object[] { 648 };
        }


        [Theory]
        [MemberData(nameof(GetInt))]
        public void Int_Center(uint id)
        {
            var pathString = new PathString($"/path/to/{id}/next");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { new Regex(@"\/[0-9]{1,}(?![a-z])"), "/id" }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/id/next", path);
        }

        [Theory]
        [MemberData(nameof(GetInt))]
        public void Int_Right(uint id) // ID - can't be less 0
        {
            var pathString = new PathString($"/path/to/{id}");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { new Regex(@"\/[0-9]{1,}(?![a-z])"), "/id" }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/id", path);
        }

        [Theory]
        [MemberData(nameof(GetInt))]
        public void Int_Right_WithSlash(uint id) // ID - can't be less 0
        {
            var pathString = new PathString($"/path/to/{id}/");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { new Regex(@"\/[0-9]{1,}(?![a-z])"), "/id" }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/id/", path);
        }
    }
}
