# Prometheus.Client.HttpRequestDurations

[![Build status](https://ci.appveyor.com/api/projects/status/e1uhigqp9gxpw9it?svg=true)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client-httprequestdurations)
[![MyGet](https://img.shields.io/myget/phnx47-beta/vpre/Prometheus.Client.HttpRequestDurations.svg)](https://www.myget.org/feed/phnx47-beta/package/nuget/Prometheus.Client.HttpRequestDurations)
[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

## Installation

	dotnet add package Prometheus.Client.HttpRequestDurations
	

#### Quik start:

There are [Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples/tree/master/HttpRequestDurations)

```csharp
app.UsePrometheusRequestDurations(q =>
{
    q.IncludePath = true;
    q.IncludeMethod = true;
    q.IgnoreRoutesConcrete = new[]
    {
        "/favicon.ico",
        "/robots.txt",
        "/"
    };
    q.IgnoreRoutesStartWith = new[]
    {
        "/swagger"
    };
    q.CustomLabels = new Dictionary<string, string>
    {
        { "service_name", "example" }
    };
});
```


## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).


