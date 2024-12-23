using Account.Services.gRPC.Services;
using System.Net;
using HttpProtocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 80, o => o.Protocols = HttpProtocols.Http1AndHttp2);
    options.Listen(IPAddress.Any, 5001, o => o.Protocols = HttpProtocols.Http2); // HTTP
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AccountService>();
app.MapGet("/", () => "Account Service is running...");

app.Run();