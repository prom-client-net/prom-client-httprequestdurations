# Prometheus.Client.HttpRequestDurations

[![ci](https://img.shields.io/github/actions/workflow/status/prom-client-net/prom-client-httprequestdurations/ci.yml?branch=main&label=ci&logo=github&style=flat-square)](https://github.com/prom-client-net/prom-client-httprequestdurations/actions/workflows/ci.yml)
[![nuget](https://img.shields.io/nuget/v/Prometheus.Client.HttpRequestDurations?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![nuget](https://img.shields.io/nuget/dt/Prometheus.Client.HttpRequestDurations?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![codecov](https://img.shields.io/codecov/c/github/prom-client-net/prom-client-httprequestdurations?logo=codecov&style=flat-square)](https://app.codecov.io/gh/prom-client-net/prom-client-httprequestdurations)
[![codefactor](https://img.shields.io/codefactor/grade/github/prom-client-net/prom-client-httprequestdurations?logo=codefactor&style=flat-square)](https://www.codefactor.io/repository/github/prom-client-net/prom-client-httprequestdurations)
[![license](https://img.shields.io/github/license/prom-client-net/prom-client-httprequestdurations?style=flat-square)](https://github.com/prom-client-net/prom-client-httprequestdurations/blob/main/LICENSE)

## Installation

```sh
dotnet add package Prometheus.Client.HttpRequestDurations
```

## Use

There are [Examples](https://github.com/prom-client-net/prom-examples)

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
