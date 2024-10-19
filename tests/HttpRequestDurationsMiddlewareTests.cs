using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class HttpRequestDurationsMiddlewareTests
{
    private readonly ICollectorRegistry _registry;
    private readonly IApplicationBuilder _app;

    public HttpRequestDurationsMiddlewareTests()
    {
        _registry = new CollectorRegistry();

        var services = new ServiceCollection();
        services.AddSingleton(_registry);
        _app = new ApplicationBuilder(services.BuildServiceProvider());
    }

    [Fact]
    public void Use_DefaultCollectorNotNull()
    {
        UseBuildApp();

        _registry.TryGet(Defaults.MetricName, out var collector);

        Assert.NotNull(collector);
    }

    [Theory]
    [InlineData("custom_name")]
    [InlineData("http_seconds")]
    [InlineData("myapp_http_request_duration_seconds")]
    public void Use_WithCustomMetricName_CustomCollectorNotNull(string metricName)
    {
        UseBuildApp(q => q.MetricName = metricName);

        _registry.TryGet(metricName, out var collector);

        Assert.NotNull(collector);
    }

    [Fact]
    public void Use_WithCustomMetricName_DefaultCollectorIsNull()
    {
        UseBuildApp(q => q.MetricName = "custom_name");

        _registry.TryGet(Defaults.MetricName, out var collector);

        Assert.Null(collector);
    }

    [Fact]
    public void Collector_IsHistogram()
    {
        UseBuildApp();

        _registry.TryGet(Defaults.MetricName, out var collector);
        Assert.IsAssignableFrom<IMetricFamily<IHistogram>>(collector);
    }

    [Fact]
    public void Metric_ContainsStatusCodeLabel()
    {
        UseBuildApp(q => q.IncludeStatusCode = true);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains(Defaults.LabelNames.StatusCode, metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainStatusCodeLabel()
    {
        UseBuildApp(q => q.IncludeStatusCode = false);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain(Defaults.LabelNames.StatusCode, metric.LabelNames);
    }

    [Fact]
    public void Metric_ContainsMethodLabel()
    {
        UseBuildApp(q => q.IncludeMethod = true);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains(Defaults.LabelNames.Method, metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainMethodLabel()
    {
        UseBuildApp(q => q.IncludeMethod = false);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain(Defaults.LabelNames.Method, metric.LabelNames);
    }

    [Fact]
    public void Metric_ContainsControllerLabel()
    {
        UseBuildApp(q => q.IncludeController = true);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains(Defaults.LabelNames.Controller, metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainControllerLabel()
    {
        UseBuildApp(q => q.IncludeController = false);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain(Defaults.LabelNames.Controller, metric.LabelNames);
    }

    [Fact]
    public void Metric_ContainsActionLabel()
    {
        UseBuildApp(q => q.IncludeAction = true);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains(Defaults.LabelNames.Action, metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainActionLabel()
    {
        UseBuildApp(q => q.IncludeAction = false);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain(Defaults.LabelNames.Action, metric.LabelNames);
    }

    [Fact]
    public void Metric_ContainsPathLabel()
    {
        UseBuildApp(q => q.IncludePath = true);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains(Defaults.LabelNames.Path, metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainPathLabel()
    {
        UseBuildApp(q => q.IncludePath = false);

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain(Defaults.LabelNames.Path, metric.LabelNames);
    }

    [Fact]
    public void Metric_ContainsCustomLabel()
    {
        UseBuildApp(q => q.CustomLabels = new Dictionary<string, Func<string>>
        {
            {
                "custom_label", () => "custom_value"
            }
        });

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.Contains("custom_label", metric.LabelNames);
        Assert.DoesNotContain("custom_value", metric.LabelNames);
    }

    [Fact]
    public void Metric_DoesNotContainCustomLabel()
    {
        UseBuildApp();

        _registry.TryGet(Defaults.MetricName, out var collector);
        var metric = (IMetricFamily<IHistogram>)collector;
        Assert.DoesNotContain("custom_label", metric.LabelNames);
    }

    private void UseBuildApp(Action<HttpRequestDurationsOptions> setupOptions = null)
    {
        _app.UsePrometheusRequestDurations(setupOptions).Build();
    }
}
