namespace DistriMosane.Api.Dtos;

public record OrderLineDto(
    int Id,
    string ProductLabel,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
