var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.mcp_aspire_McpServer>("mcpserver")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
