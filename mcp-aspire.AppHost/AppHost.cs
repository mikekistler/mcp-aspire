var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.mcp_aspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
