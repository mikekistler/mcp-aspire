var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.mcp_aspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.mcp_aspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
