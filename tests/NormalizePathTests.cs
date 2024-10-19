using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class NormalizePathTests
{
    private const string _intValue = "/int";
    private const string _guidValue = "/guid";

    private static readonly Regex _intRegex =
        new(@"\/[0-9]{1,}(?![a-z])", RegexOptions.Compiled);

    private static readonly Regex _guidRegex =
        new(@"\/[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}", RegexOptions.Compiled);

    private static readonly Random _rnd = new();

    public static TheoryData<int> IntData = new()
    {
        1,
        0,
        4,
        648,
        _rnd.Next(0, int.MaxValue),
        _rnd.Next(0, int.MaxValue),
        _rnd.Next(0, int.MaxValue)
    };

    public static TheoryData<string> GuidData = new()
    {
        "b50c8511-ec3c-4271-88bc-d557efc2d6a3",
        "b3cd56ab-d5f8-40eb-ae0a-c99e14399e48",
        "6cc98e89-1bb1-4c6a-8277-399061368a27",
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString()
    };

    [Theory]
    [MemberData(nameof(IntData))]
    public void Int_Center(int id)
    {
        var pathString = $"/path/to/{id}/next";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _intRegex, _intValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/int/next", path);
    }

    [Theory]
    [MemberData(nameof(IntData))]
    public void Int_Right(int id)
    {
        var pathString = $"/path/to/{id}";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _intRegex, _intValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/int", path);
    }

    [Theory]
    [MemberData(nameof(IntData))]
    public void Int_Right_WithSlash(int id)
    {
        var pathString = $"/path/to/{id}/";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _intRegex, _intValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/int/", path);
    }

    [Theory]
    [MemberData(nameof(GuidData))]
    public void Guid_Center(string guid)
    {
        var pathString = $"/path/to/{guid}/next";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _guidRegex, _guidValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/guid/next", path);
    }

    [Theory]
    [MemberData(nameof(GuidData))]
    public void Guid_Right(string guid)
    {
        var pathString = $"/path/to/{guid}";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _guidRegex, _guidValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/guid", path);
    }

    [Theory]
    [MemberData(nameof(GuidData))]
    public void Guid_Right_WithSlash(string guid)
    {
        var pathString = $"/path/to/{guid}/";

        var options = new HttpRequestDurationsOptions
        {
            CustomNormalizePath = new Dictionary<Regex, string> { { _guidRegex, _guidValue } }
        };

        var path = NormalizePath.Execute(pathString, options);
        Assert.Equal("/path/to/guid/", path);
    }
}
