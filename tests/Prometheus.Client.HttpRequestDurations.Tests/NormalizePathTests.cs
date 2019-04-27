using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.HttpRequestDurations.Tools;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests
{
    public class NormalizePathTests
    {
        private static readonly Regex _intRegex = new Regex(@"\/[0-9]{1,}(?![a-z])", RegexOptions.Compiled);
        private const string _intValue = "/id";

        private static readonly Regex _guidRegex = new Regex(@"\/[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}", RegexOptions.Compiled);
        private const string _guidValue = "/guid";

        public static IEnumerable<object[]> GetInt()
        {
            // ID - can't be less 0
            yield return new object[] { 2 };
            yield return new object[] { 0 };
            yield return new object[] { 4 };
            yield return new object[] { 648 };

            var rnd = new Random();

            yield return new object[] { rnd.Next(0, int.MaxValue) };
            yield return new object[] { rnd.Next(0, int.MaxValue) };
            yield return new object[] { rnd.Next(0, int.MaxValue) };
        }

        public static IEnumerable<object[]> GetGuid()
        {
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
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
                    { _intRegex, _intValue }
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
                    { _intRegex, _intValue }
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
                    { _intRegex, _intValue }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/id/", path);
        }

        [Theory]
        [MemberData(nameof(GetGuid))]
        public void Guid_Center(string guid)
        {
            var pathString = new PathString($"/path/to/{guid}/next");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { _guidRegex, _guidValue }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/guid/next", path);
        }

        [Theory]
        [MemberData(nameof(GetGuid))]
        public void Guid_Right(string guid)
        {
            var pathString = new PathString($"/path/to/{guid}");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { _guidRegex, _guidValue }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/guid", path);
        }

        [Theory]
        [MemberData(nameof(GetGuid))]
        public void Guid_Right_WithSlash(string guid)
        {
            var pathString = new PathString($"/path/to/{guid}/");

            var options = new HttpRequestDurationsOptions
            {
                CustomNormalizePath = new Dictionary<Regex, string>
                {
                    { _guidRegex, _guidValue }
                }
            };

            var path = NormalizePath.Execute(pathString, options);
            Assert.Equal($"/path/to/guid/", path);
        }
    }
}
