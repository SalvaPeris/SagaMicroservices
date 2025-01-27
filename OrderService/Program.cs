using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host("rabbitmq://localhost");
        configuration.Message<OrderPlaced>(x => x.SetEntityName("order-topic-exchange"));
        configuration.Publish<OrderPlaced>(x => x.ExchangeType = "topic");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.orderId, order.quantity);
    await bus.Publish(orderPlacedMessage, context =>
    {
        context.SetRoutingKey(order.quantity > 10 ? "order.shipping" : "order.regular.tracking");
    });

    return Results.Created($"/orders/{order.orderId}", orderPlacedMessage);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();


public record OrderRequest(Guid orderId, int quantity);