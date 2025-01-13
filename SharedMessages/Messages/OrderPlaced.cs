namespace SharedMessages.Messages
{
    public sealed record class OrderPlaced(Guid OrderId, int Quantity);
}
