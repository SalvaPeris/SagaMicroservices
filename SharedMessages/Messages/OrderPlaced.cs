namespace SharedMessages.Messages
{
    public record OrderPlaced(Guid OrderId, int Quantity);

    public record InventoryReserved(Guid OrderId);

    public record PaymentCompleted(Guid OrderId);
}
