namespace DistriMosane.Api.Dtos;

public record OrderDetailDto(
    int Id,
    DateTime OrderDate,
    string Status,
    int CustomerId,
    string CustomerName,
    IReadOnlyList<OrderLineDto> Lines,
    decimal Total);
