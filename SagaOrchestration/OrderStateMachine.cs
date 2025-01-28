using MassTransit;
using SharedMessages.Messages;

namespace SagaOrchestration
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; set; }
        public State InventoryReserved { get; set; }
        public State PaymentCompleted { get; set; }


        public Event<OrderPlaced> OrderPlacedEvent { get; private set; }
        public Event<InventoryReserved> InventoryReservedEvent { get; private set; }
        public Event<PaymentCompleted> PaymentCompletedEvent { get; private set; }


        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderPlacedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => InventoryReservedEvent, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));

            Initially(
                When(OrderPlacedEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Quantity = context.Message.Quantity;
                    Console.WriteLine($"Order #{context.Saga.OrderId} placed succesfully.");

                }).TransitionTo(Submitted));

            During(Submitted,
                When(InventoryReservedEvent)
                .TransitionTo(InventoryReserved));

            During(InventoryReserved,
                When(PaymentCompletedEvent)
                .Then(context =>
                {
                    Console.WriteLine($"Order #{context.Saga.OrderId} completed successfully.");
                })
                .TransitionTo(PaymentCompleted)
                .Finalize());

            SetCompletedWhenFinalized();
        }
    }
}
