using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InventoryReservedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ReceiveEndpoint("inventory-reserved", e =>
        {
            e.Consumer<InventoryReservedConsumer>();
        });
    });
});

var app = builder.Build();
app.Run();

public class InventoryReservedConsumer : IConsumer<InventoryReserved>
{
    public async Task Consume(ConsumeContext<InventoryReserved> context)
    {
        Console.WriteLine($"Payment processed for Order {context.Message.OrderId}");
        await context.Publish(new PaymentCompleted(context.Message.OrderId));
    }
}