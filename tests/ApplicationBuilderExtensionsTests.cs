using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class ApplicationBuilderExtensionsTests
{
    private readonly ServiceCollection _services = new();

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

        Assert.True(Metrics.DefaultCollectorRegistry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out var defaultCollector));

        // Cleanup
        Metrics.DefaultCollectorRegistry?.Remove(defaultCollector);
    }

    [Fact]
    public void With_DICollecorRegistry()
    {
        var registry = new CollectorRegistry();
        _services.AddSingleton<ICollectorRegistry>(registry);
        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        app.UsePrometheusRequestDurations();
        app.Build();

        Assert.True(registry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out _));

        Assert.False(Metrics.DefaultCollectorRegistry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out _));
    }

    [Fact]
    public void With_CustomCollecorRegistry()
    {
        var registry = new CollectorRegistry();

        var app = new ApplicationBuilder(_services.BuildServiceProvider());
        app.UsePrometheusRequestDurations(q => q.CollectorRegistry = registry);
        app.Build();

        Assert.True(registry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out _));

        Assert.False(Metrics.DefaultCollectorRegistry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out _));
    }
}
