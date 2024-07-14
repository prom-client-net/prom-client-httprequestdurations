using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.HttpRequestDurations.Tests;

public class HttpRequestDurationsMiddlewareTests
{
    private readonly ICollectorRegistry _registry;
    private readonly IApplicationBuilder _app;
    private readonly HttpContext _ctx;

    public HttpRequestDurationsMiddlewareTests()
    {
        _registry = new CollectorRegistry();

        var services = new ServiceCollection();
        services.AddSingleton(_registry);
        _app = new ApplicationBuilder(services.BuildServiceProvider());
        _ctx = new DefaultHttpContext();
    }

    [Fact]
    public void Default_Register_DefaultCollector()
    {
        _app.UsePrometheusRequestDurations();
        _app.Build();


        _registry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out var collector);

        Assert.NotNull(collector);
    }

    [Fact]
    public void CustomName_NotRegister_DefaultCollector()
    {
        _app.UsePrometheusRequestDurations(q => q.MetricName = "custom_name");
        _app.Build();

        _registry.TryGet(HttpRequestDurationsOptions.DefaultMetricName, out var collector);

        Assert.Null(collector);
    }


    [Theory]
    [InlineData("custom_name")]
    [InlineData("http_seconds")]
    [InlineData("myapp_http_request_duration_seconds")]
    public void CustomName_Register_CustomCollector(string metricName)
    {
        _app.UsePrometheusRequestDurations(q => q.MetricName = metricName);
        _app.Build();

        _registry.TryGet(metricName, out var collector);

        Assert.NotNull(collector);
    }
}
