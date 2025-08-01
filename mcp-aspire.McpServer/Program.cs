using EverythingServer.Tools;
using EverythingServer.Resources;
using EverythingServer.Prompts;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

HashSet<string> subscriptions = [];
var _minimumLoggingLevel = LoggingLevel.Debug;

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<AddTool>()
    .WithTools<AnnotatedMessageTool>()
    .WithTools<EchoTool>()
    .WithTools<LongRunningTool>()
    .WithTools<PrintEnvTool>()
    .WithTools<SampleLlmTool>()
    .WithTools<TinyImageTool>()
    .WithPrompts<ComplexPromptType>()
    .WithPrompts<SimplePromptType>()
    .WithResources<SimpleResourceType>();

// Configure session timeout
builder.Services.Configure<HttpServerTransportOptions>(options =>
{
    options.IdleTimeout = Timeout.InfiniteTimeSpan; // Never timeout
});

ResourceBuilder resource = ResourceBuilder.CreateDefault().AddService("everything-server");
builder.Services.AddOpenTelemetry()
    .WithTracing(b => b.AddSource("*").AddHttpClientInstrumentation().SetResourceBuilder(resource))
    .WithMetrics(b => b.AddMeter("*").AddHttpClientInstrumentation().SetResourceBuilder(resource))
    .WithLogging(b => b.SetResourceBuilder(resource));
    // .UseOtlpExporter();

builder.Services.AddSingleton(subscriptions);

builder.Services.AddSingleton<Func<LoggingLevel>>(_ => () => _minimumLoggingLevel);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapMcp();

app.Run();
