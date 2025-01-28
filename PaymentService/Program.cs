using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ReceiveEndpoint("order-placed", e =>
        {
            e.Consumer<OrderPlacedConsumer>();
        });
    });
});

var app = builder.Build();
app.Run();

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine($"Inventory reserved for Order {context.Message.OrderId}");
        await context.Publish(new InventoryReserved(context.Message.OrderId));
        return Task.CompletedTask;
    }
}