using CliveBot.Database;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using CliveBot.Web.Events;
using CliveBot.Application;
using CliveBot.Azure;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CliveBot.Web.Middleware;
using CliveBot.Web.Policies;

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

#if !DEBUG
builder.Logging.AddApplicationInsights();
builder.Services.AddApplicationInsightsTelemetry();
#endif

builder.Logging.AddSerilog();

builder.Services.AddHealthChecks();

builder.Services.AddScoped<XCookieAuthEvents>();

var connectionString = builder.Configuration.GetConnectionString("POSTGRES");

builder.Services.AddDbContext<ApplicationDbContext>((config) =>
{
#if DEBUG
    config.EnableSensitiveDataLogging();
#endif
});
builder.Services.RegisterMediatR();

var azureConfig = builder.Configuration.GetSection("Azure");
var blobConnectionString = azureConfig.GetValue<string>("BlobConnectionString") ?? "";

builder.Services.RegisterAzureBlobServices(blobConnectionString);

var discordLogin = builder.Configuration.GetSection("DiscordLogin");
var discordClientId = discordLogin.GetValue<string>("ClientId") ?? "";
var discordClientSecret = discordLogin.GetValue<string>("ClientSecret") ?? "";

var cookies = builder.Configuration.GetSection("Cookies");
var cookieDomain = cookies.GetValue<string>("Domain");

var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

builder.Services.AddCors((o) => {
    if (string.IsNullOrEmpty(frontendUrl)) return;
    o.AddDefaultPolicy(p => p
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(frontendUrl)
    );
});

builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddDiscord(options =>
    {
        options.AccessDeniedPath = "/error/accessdenied";
        options.ClientId = discordClientId;
        options.ClientSecret = discordClientSecret;
        
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (options) =>
    {
        options.EventsType = typeof(XCookieAuthEvents);
        options.AccessDeniedPath = "/error/accessdenied";
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = false; // Needs for JavaScript
        if (cookieDomain != null)
        {
            options.Cookie.Domain = cookieDomain;
        }
        options.Events.OnSignedIn = (ctx) => {
            if (string.IsNullOrEmpty(frontendUrl))
            {
                return Task.CompletedTask;
            }
            ctx.Response.Redirect(frontendUrl);
            return Task.CompletedTask;
        };

        options.Events.OnSigningOut = (ctx) => {
            if (string.IsNullOrEmpty(frontendUrl))
            {
                return Task.CompletedTask;
            }
            ctx.Response.Redirect(frontendUrl);
            return Task.CompletedTask;
        };
    });
    //.AddBearerToken();

builder.Services.AddAuthorization(o =>
{
    o.AddCustomPolicies();
});
builder.Services.AddPolicyHandlers();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CliveBot Web API",
        Description = "A Web NextJX Static site with .NET Core Backend",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Thomas Aunvik",
            Url = new Uri("mailto:contact@thaun.dev")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/ThomasAunvik/FFXVI-Clive-Bot/blob/master/LICENSE")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.CustomSchemaIds(type => type.ToString().Replace("+", "."));
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        await ErrorHandlingMiddleware.HandleExceptionAsync(context);
    });
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseHealthChecks("/healthz");
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.UseSwagger(options =>
{
    options.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});

try
{
    app.Run();
}catch(Exception e)
{
    Log.Logger.Fatal("Error! {e}", e);
}
