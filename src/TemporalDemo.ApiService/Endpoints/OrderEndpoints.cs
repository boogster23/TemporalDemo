using TemporalDemo.Contracts;
using Temporalio.Client;
using Temporalio.Exceptions;

namespace TemporalDemo.ApiService.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/orders");

        group.MapPost("/", SubmitOrderAsync).WithName("SubmitOrder");
        group.MapGet("/{id:guid}", GetOrderStatusAsync).WithName("GetOrderStatus");

        return routes;
    }

    private static async Task<IResult> SubmitOrderAsync(SubmitOrder order, ITemporalClient temporalClient)
    {
        var orderToSubmit = order with
        {
            OrderId = order.OrderId == Guid.Empty 
                ? Guid.NewGuid()
                : order.OrderId
        };

        await temporalClient.StartWorkflowAsync(
            (IOrderWorkflow wf) => 
                wf.RunAsync(orderToSubmit),
                new WorkflowOptions(
                    id: $"order-{orderToSubmit.OrderId}",
                    taskQueue: "orders-queue"
                ));
        
        return Results.Accepted($"api/orders/{orderToSubmit.OrderId}", orderToSubmit);
    }

    private static async Task<IResult> GetOrderStatusAsync(Guid id, ITemporalClient temporalClient)
    {
        try
        {
            var handle = temporalClient.GetWorkflowHandle<IOrderWorkflow>($"order-{id}");
            var status = await handle.QueryAsync(wf => wf.GetStatus());

            return Results.Ok(status);
        }
        catch(RpcException ex) when (ex.Code == RpcException.StatusCode.NotFound)
        {
            return Results.NotFound(new
            {
                message = $"Order {id} not found."
            });
        }
    }
}
