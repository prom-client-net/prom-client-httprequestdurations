using System;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class HttpContextExtensionsTests
{
    [Fact]
    public void GetRouteName_When_HttpContextIsNull_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((HttpContext)null).GetRouteName());
    }

    [Fact]
    public void GetControllerName_When_HttpContextIsNull_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((HttpContext)null).GetControllerName());
    }

    [Fact]
    public void GetActionName_When_HttpContextIsNull_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((HttpContext)null).GetActionName());
    }
}
