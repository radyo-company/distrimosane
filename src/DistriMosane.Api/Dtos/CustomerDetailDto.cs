namespace DistriMosane.Api.Dtos;

public record CustomerDetailDto(
    int Id,
    string CompanyName,
    string VatNumber,
    string Email,
    string City,
    DateTime CreatedAt,
    IReadOnlyList<OrderSummaryDto> Orders);
