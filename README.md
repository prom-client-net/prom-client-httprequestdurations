# Prometheus.Client.HttpRequestDurations

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![CI](https://img.shields.io/github/workflow/status/prom-client-net/prom-client-httprequestdurations/%F0%9F%92%BF%20CI%20Master?label=CI&logo=github)](https://github.com/prom-client-net/prom-client-httprequestdurations/actions/workflows/master.yml)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)

## Installation

```shell
dotnet add package Prometheus.Client.HttpRequestDurations
```

## Use

There are [Examples](https://github.com/prom-client-net/prom-examples/tree/master/HttpRequestDurations)

```c#
app.UsePrometheusRequestDurations(q =>
{
    q.IncludePath = true;
    q.IncludeMethod = true;
    q.IgnoreRoutesConcrete = new[] // Ignore some concrete routes
    {
        "/favicon.ico",
        "/robots.txt",
        "/"
    };
    q.IgnoreRoutesStartWith = new[]
    {
        "/swagger" // Ignore '/swagger/*'
    };
    q.CustomNormalizePath = new Dictionary<Regex, string>
    {
        { new Regex(@"\/[0-9]{1,}(?![a-z])"), "/id" } // Replace 'int' in Route
    };
});
```

## Contribute

Contributions to the package are always welcome!

* Report any bugs or issues you find on the [issue tracker](https://github.com/prom-client-net/prom-client-httprequestdurations/issues).
* You can grab the source code at the package's [git repository](https://github.com/prom-client-net/prom-client-httprequestdurations).

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
