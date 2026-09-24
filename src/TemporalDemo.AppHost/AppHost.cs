var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging").WithManagementPlugin();

var temporal = builder.AddContainer("temporal", "temporalio/temporal")
                    .WithArgs("server", "start-dev", "--ip", "0.0.0.0")
                    .WithEndpoint(port: 7233, targetPort: 7233, name: "grpc", scheme: "http")
                    .WithHttpEndpoint(port: 8233, targetPort: 8233, name: "ui");

builder.AddProject<Projects.TemporalDemo_Worker>("worker")
        .WithReference(messaging)
        .WithReference(temporal.GetEndpoint("grpc"))
        .WithEnvironment("Temporal_Address", temporal.GetEndpoint("grpc"))
        .WaitFor(messaging)
        .WaitFor(temporal);

builder.AddProject<Projects.TemporalDemo_ApiService>("apiservice")
        .WithReference(messaging)
        .WaitFor(messaging);

builder.Build().Run();
