using MassTransit;

namespace SagaOrchestration
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
