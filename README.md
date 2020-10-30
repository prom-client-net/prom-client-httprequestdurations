# Prometheus.Client.HttpRequestDurations

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![CI](https://github.com/PrometheusClientNet/Prometheus.Client.HttpRequestDurations/workflows/CI/badge.svg)](https://github.com/PrometheusClientNet/Prometheus.Client.HttpRequestDurations/actions?query=workflow%3ACI)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

## Installation
```shell script
dotnet add package Prometheus.Client.HttpRequestDurations
```

#### Use:

There are [Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples/tree/master/HttpRequestDurations)

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

* Report any bugs or issues you find on the [issue tracker](https://github.com/PrometheusClientNet/Prometheus.Client.HttpRequestDurations/issues).
* You can grab the source code at the package's [git repository](https://github.com/PrometheusClientNet/Prometheus.Client.HttpRequestDurations).

## Support

I would also very much appreciate your support:

<a href="https://www.buymeacoffee.com/phnx47"><img width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/bmac0.png" alt="Buy Me A Coffee"></a>
<a href="https://ko-fi.com/phnx47"><img width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/kofi0.png" alt="Support me on ko-fi"></a>
<a href="https://www.patreon.com/phnx47"><img width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/patreon0.png" alt="Support me on Patreon"></a>
## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).


