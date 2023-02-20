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

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseIdentityServer();
app.UseAuthorization();

// app.MapGet("/", () => "Hello World!");
app.MapDefaultControllerRoute();

app.Run();
