using IdentityServer4.Models;
using IdentityServer4.Test;
using IDS;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
  .AddInMemoryClients(Config.Clients)
  .AddInMemoryIdentityResources(Config.IdentityResources)
  .AddInMemoryApiResources(Config.ApiResources)
  .AddInMemoryApiScopes(Config.ApiScopes)
  .AddTestUsers(Config.Users)
  .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
