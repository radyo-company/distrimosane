namespace DistriMosane.Api.Dtos;

public record OrderSummaryDto(
    int Id,
    DateTime OrderDate,
    string Status,
    int LineCount,
    decimal Total);
