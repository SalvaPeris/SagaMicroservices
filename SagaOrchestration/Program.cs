using MassTransit;
using SagaOrchestration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ReceiveEndpoint("saga-service", e =>
        {
            e.StateMachineSaga<OrderState>(context);
        });
    });
});

var app = builder.Build();
app.Run();
