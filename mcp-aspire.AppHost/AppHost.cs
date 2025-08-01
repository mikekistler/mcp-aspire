var builder = DistributedApplication.CreateBuilder(args);

var mcpServer = builder.AddProject<Projects.mcp_aspire_McpServer>("mcpserver")
    .WithHttpHealthCheck("/health");

var inspector = builder.AddMcpInspector("inspector")
    .WithMcpServer(mcpServer);

builder.Build().Run();
