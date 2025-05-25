using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class ApplicationBuilderExtensionsTests : IDisposable
{
    private readonly ServiceCollection _services = [];

    [Fact]
    public void AppBuilderIsNull_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((ApplicationBuilder)null).UsePrometheusRequestDurations());
    }

    [Fact]
    public void TargetIsType_HttpRequestDurationsMiddleware()
    {
        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        _services.AddSingleton<ICollectorRegistry, CollectorRegistry>();
        app.UsePrometheusRequestDurations();

        Assert.IsType<HttpRequestDurationsMiddleware>(app.Build().Target);
    }

    [Fact]
    public void With_DefaultCollectorRegistry()
    {
        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        app.UsePrometheusRequestDurations();
        app.Build();

        Assert.True(Metrics.DefaultCollectorRegistry.TryGet(Defaults.MetricName, out _));
    }

    [Fact]
    public void With_DICollectorRegistry()
    {
        var registry = new CollectorRegistry();
        _services.AddSingleton<ICollectorRegistry>(registry);
        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        app.UsePrometheusRequestDurations();
        app.Build();

        Assert.True(registry.TryGet(Defaults.MetricName, out _));

        Assert.False(Metrics.DefaultCollectorRegistry.TryGet(Defaults.MetricName, out _));
    }

    [Fact]
    public void With_CustomCollectorRegistry()
    {
        var registry = new CollectorRegistry();

        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        app.UsePrometheusRequestDurations(q => q.CollectorRegistry = registry);
        app.Build();

        Assert.True(registry.TryGet(Defaults.MetricName, out _));

        Assert.False(Metrics.DefaultCollectorRegistry.TryGet(Defaults.MetricName, out _));
    }

    public void Dispose()
    {
        Metrics.DefaultCollectorRegistry?.Remove(Defaults.MetricName);
    }
}
