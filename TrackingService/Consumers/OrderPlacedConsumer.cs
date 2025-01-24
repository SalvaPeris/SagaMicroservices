using MassTransit;
using SharedMessages.Messages;

namespace TrackingService.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlaced>
    {
        public Task Consume(ConsumeContext<OrderPlaced> context)
        {
            Console.WriteLine($"Order received for tracking order number {context.Message.OrderId} and quantity {context.Message.Quantity}");
            return Task.CompletedTask;
        }
    }
}
