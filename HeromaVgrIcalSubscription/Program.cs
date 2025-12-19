using HeromaVgrIcalSubscription.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables();

builder.Services
    .Configure<CalendarOptions>(builder.Configuration.GetSection("CalendarOptions"))
    .Configure<SeleniumOptions>(builder.Configuration.GetSection("SeleniumOptions"));

builder.Services
    .AddTransient<ISchemaService, SchemaService>()
    .AddTransient<ICalendarService, CalendarService>()
    .AddTransient<ISeleniumTokenService, SeleniumTokenService>();

builder.Services
    .AddHttpClient<ICalendarService, CalendarService>()
    .Services
    .AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.MapGet("/Schema", () => "Hello there!");

app.MapGet("/Schema/{user}/{password}/{months}", async (string user, string password, int months,
    ISchemaService schemaService, IMemoryCache cache, ILogger<Program> logger) =>
{
    logger.LogInformation("New incoming request from {User}", user);
    return await cache.GetOrCreateAsync($"{user}-{password}-{months}", async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);
        return await schemaService.GetCalendarAsync(new(user, password, months));
    });
});

app.Run();