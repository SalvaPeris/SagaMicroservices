using MassTransit;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("shipping-order-queue", e =>
        {
            e.Consumer<OrderPlacedConsumer>(context);

            e.Bind("order-placed-exchange", x =>
            {
                x.RoutingKey = "order.created";
                x.ExchangeType = "direct";
            });

            e.UseKillSwitch(options =>
            {
                options.SetActivationThreshold(2);
                options.SetTripThreshold(0.1);
                options.SetRestartTimeout(m: 1);
            });
        });
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

app.Run();
