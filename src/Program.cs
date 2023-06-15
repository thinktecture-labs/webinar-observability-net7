using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Thinktecture.Webinars.SampleApi;
using Thinktecture.Webinars.SampleApi.Entities;
using Thinktecture.Webinars.SampleApi.Repositories;
using Thinktecture.Webinars.SampleApi.Services;

var builder = WebApplication.CreateBuilder(args);
// logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options => { options.FormatterName = ConsoleFormatterNames.Json; });

// metrics
builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.AddMeter(Observability.Meter.Name);
        b.AddRuntimeInstrumentation();
        b.AddAspNetCoreInstrumentation();
        b.AddPrometheusExporter();
    });

// traces
builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
    {
        b.AddSource(Observability.ServiceName);
        b.ConfigureResource(rb =>
        {
            rb.AddService(Observability.ServiceName, serviceVersion: Observability.ServiceVersion);
        });
        b.AddAspNetCoreInstrumentation();
        b.AddEntityFrameworkCoreInstrumentation();
        b.AddOtlpExporter();
    });

builder.Services.AddDbContext<SampleApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SampleDb")));
// Add services to the container.
builder.Services
    .AddScoped<ProductsRepository>()
    .AddScoped<CategoriesRepository>()
    .AddScoped<ProductsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var ctx = services.GetRequiredService<SampleApiContext>();
    ctx.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.Run();
