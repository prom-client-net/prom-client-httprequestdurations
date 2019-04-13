# Prometheus.Client.HttpRequestDurations

[![MyGet](https://img.shields.io/myget/prometheus-client-net/vpre/Prometheus.Client.HttpRequestDurations.svg?label=myget)](https://www.myget.org/feed/prometheus-client-net/package/nuget/Prometheus.Client.HttpRequestDurations)
[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.HttpRequestDurations.svg)](https://www.nuget.org/packages/Prometheus.Client.HttpRequestDurations)

[![Build status](https://ci.appveyor.com/api/projects/status/e1uhigqp9gxpw9it?svg=true)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client-httprequestdurations)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/PrometheusClientNet/prometheus-client-httprequestdurations.svg)](https://ci.appveyor.com/project/PrometheusClientNet/prometheus-client-httprequestdurations/build/tests)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

## Installation

	dotnet add package Prometheus.Client.HttpRequestDurations
	

#### Use:

There are [Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples/tree/master/HttpRequestDurations)

```csharp
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

If you are having problems, send a mail to [prometheus@phnx47.net](mailto://prometheus@phnx47.net). I will try to help you.

I would also very much appreciate your support by buying me a coffee.

<a href="https://www.buymeacoffee.com/phnx47" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>


## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).


