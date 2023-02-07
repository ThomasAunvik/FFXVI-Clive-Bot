using CliveBot.Database;
using NextjsStaticHosting.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.File(
            Path.Combine(Directory.GetCurrentDirectory(), "logs/web.log"),
            rollingInterval: RollingInterval.Day,
            shared: true
        )
        .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}")
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Logging.AddApplicationInsights();
builder.Logging.AddSerilog();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<ApplicationDbContext>((config) =>
{
#if DEBUG
    config.EnableSensitiveDataLogging();
#endif
});

var discordLogin = builder.Configuration.GetSection("DiscordLogin");
var discordClientId = discordLogin.GetValue<string>("ClientId") ?? "";
var discordClientSecret = discordLogin.GetValue<string>("ClientSecret") ?? "";


builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
).AddDiscord(options =>
    {
        options.ClientId = discordClientId;
        options.ClientSecret = discordClientSecret;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (options) => {
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = false; // Needs for JavaScript
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.Configure<NextjsStaticHostingOptions>(
    builder.Configuration.GetSection("NextjsStaticHosting")
);
builder.Services.AddNextjsStaticHosting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/healthz");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapNextjsStaticHtmls();
});

app.UseNextjsStaticHosting();

try
{
    app.Run();
}catch(Exception e)
{
    Log.Logger.Fatal(e, "Error! {e}");
}
