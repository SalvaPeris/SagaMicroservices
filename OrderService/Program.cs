using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host("rabbitmq://localhost");
        configuration.Publish<OrderPlaced>();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders", async (OrderDto order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.OrderId, order.Quantity);
    await bus.Publish(orderPlacedMessage);
    return Results.Created($"/orders/{order.OrderId}", orderPlacedMessage);
});

app.Run();

public record OrderDto(Guid OrderId, int Quantity);