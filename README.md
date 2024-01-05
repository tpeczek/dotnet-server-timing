# Server Timing API support for .NET
[![NuGet Version](https://img.shields.io/nuget/v/Lib.AspNetCore.ServerTiming?label=Lib.AspNetCore.ServerTiming&logo=nuget)](https://www.nuget.org/packages/Lib.AspNetCore.ServerTiming)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Lib.AspNetCore.ServerTiming?label=⭳)](https://www.nuget.org/packages/Lib.AspNetCore.ServerTiming)

[![NuGet Version](https://img.shields.io/nuget/v/Lib.Azure.Functions.Worker.ServerTiming?label=Lib.Azure.Functions.Worker.ServerTiming&logo=nuget)](https://www.nuget.org/packages/Lib.Azure.Functions.Worker.ServerTiming)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Lib.Azure.Functions.Worker.ServerTiming?label=⭳)](https://www.nuget.org/packages/Lib.Azure.Functions.Worker.ServerTiming)

Server Timing API provides a convenient way to communicate performance metrics about the request-response cycle to the user agent (which conveniently includes developer tools in the browser). Here you can find a set of libraries that simplify the onboarding of Server Timing API in .NET projects:
- Lib.AspNetCore.ServerTiming for ASP.NET Core
- Lib.Azure.Functions.Worker.ServerTiming for isolated worker process Azure Functions

## Installation

All libraries are available as NuGet packages.

```
PM>  Install-Package Lib.AspNetCore.ServerTiming
```

```
PM>  Install-Package Lib.Azure.Functions.Worker.ServerTiming
```

Once you install the correct library for your scenario, please refer to "Getting Started" article which will get you further:
- [Getting Started (ASP.NET Core)](https://tpeczek.github.io/dotnet-server-timing/articles/getting-started-aspnetcore.html)
- [Getting Started (Isolated Worker Process Azure Functions)](https://tpeczek.github.io/dotnet-server-timing/articles/getting-started-azurefunctions.html)

## Demos

The project repository contains demos for the libraries:
- [ASP.NET Core](https://github.com/tpeczek/Lib.AspNetCore.ServerTiming/tree/main/demos/Demo.AspNetCore.ServerTiming)
- [Isolated Worker Process Azure Functions](https://github.com/tpeczek/Lib.AspNetCore.ServerTiming/tree/main/demos/Demo.Azure.Functions.Worker.ServerTiming)

## Additional Resources

There are some blog posts available which describe implementation details:

- [Feeding Server Timing API from ASP.NET Core](https://www.tpeczek.com/2017/06/feeding-server-timing-api-from-aspnet.html)
- [Little Known ASP.NET Core Features - HTTP Trailers](https://www.tpeczek.com/2020/09/little-known-aspnet-core-features-http.html)

## Donating

My blog and open source projects are result of my passion for software development, but they require a fair amount of my personal time. If you got value from any of the content I create, then I would appreciate your support by [sponsoring me](https://github.com/sponsors/tpeczek) (either monthly or one-time).

## Copyright and License

Copyright © 2017 - 2024 Tomasz Pęczek

Licensed under the [MIT License](https://github.com/tpeczek/Lib.AspNetCore.ServerTiming/blob/master/LICENSE.md)
