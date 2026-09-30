using TemporalDemo.ApiService.Endpoints;
using TemporalDemo.Contracts;
using Temporalio.Client;
using Temporalio.Exceptions;
using Temporalio.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var rawAddress = builder.Configuration["Temporal_Address"] ?? "localhost:7233";
var temporalAddress = rawAddress.Replace("http://", "").Replace("https://", "");

builder.Services.AddTemporalClient(client =>
{
    client.TargetHost = temporalAddress;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapOrderEndpoints();

app.Run();