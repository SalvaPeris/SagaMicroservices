using MassTransit;
using SharedMessages.Messages;

namespace SagaOrchestration
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; set; }
        public State InventoryReserver { get; set; }
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

        }
    }
}
