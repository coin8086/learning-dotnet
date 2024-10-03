# Memo

```mermaid
flowchart BT
  IAppBuilder[IApplicationBuilder]
  IEpBuilder[IEndpointRouteBuilder]
  IHost[IHost]
  webApp[WebApplication]

  webAppBuilder[WebApplicationBuilder]
  IHostAppBuilder[IHostApplicationBuilder]

  webApp --> IAppBuilder
  webApp --> IEpBuilder
  webApp --> IHost

  IAppBuilder .-> |New| IAppBuilder
  IEpBuilder .-> |CreateApplicationBuilder| IAppBuilder

  webAppBuilder --> IHostAppBuilder

```