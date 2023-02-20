using WeatherMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddMemoryCache(); // This is needed if using DI.

builder.Services.Configure<IdentityServerSettings>(
  builder.Configuration.GetSection(nameof(IdentityServerSettings))
);
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options => {
  options.DefaultScheme = "cookie";
  options.DefaultChallengeScheme = "oidc";
})
  .AddCookie("cookie")
  .AddOpenIdConnect("oidc", options => {
    var config = builder.Configuration;
    const string sectionPrefix = "InteractiveClientSettings:";
    options.Authority = config[sectionPrefix + "AuthorityUrl"];
    options.ClientId = config[sectionPrefix + "ClientId"];
    options.ClientSecret = config[sectionPrefix + "ClientSecret"];
    options.Scope.Add(config[sectionPrefix + "Scopes:0"]);
    options.ResponseType = "code";
    options.UsePkce = true;
    options.SaveTokens = true;
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
