using TemporalDemo.Worker.Activities;
using TemporalDemo.Worker.Workflows;
using Temporalio.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var rawAddress = builder.Configuration["Temporal_Address"] ?? "localhost:7233";
var temporalAddress = rawAddress.Replace("http://", "").Replace("https://", "");

builder.Services.AddTemporalClient(opts =>
    {
        opts.TargetHost = temporalAddress;
    })
    .AddHostedTemporalWorker("orders-queue")
    .AddWorkflow<OrderWorkflow>()
    .AddScopedActivities<OrderActivities>();

var host = builder.Build();
host.Run();
