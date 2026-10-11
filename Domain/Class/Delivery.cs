using System.Diagnostics.CodeAnalysis;
using Domain.DTO;

namespace Domain.Class;

public class Delivery
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public DateTime DateReceived { get; set; }

    public Delivery(){}

    [SetsRequiredMembers]
    public Delivery(DeliveryDto dto)
    {
        Id = 0;
        Number = dto.Number;
        DateReceived = dto.DateReceived;
    }
}