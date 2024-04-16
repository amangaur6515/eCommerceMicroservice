using Ocelot.Configuration.Creator;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Ocelot.Provider.Polly;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);
var routes = "Routes.dev";
#if DEBUG
routes = "Routes.dev";
#else
routes = "Routes.prod";
#endif
;
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddOcelot(routes,builder.Environment)
    .AddEnvironmentVariables();


builder.Services.AddOcelot(builder.Configuration).AddEureka().AddPolly() ;

builder.Services.AddServiceDiscovery(o => o.UseEureka());

var app = builder.Build();

await app.UseOcelot();



app.Run();
