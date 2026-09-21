namespace DistriMosane.Api.Dtos;

public record CustomerListItemDto(
    int Id,
    string CompanyName,
    string VatNumber,
    string City,
    int OrderCount,
    decimal TotalRevenue,
    DateTime? LastOrderDate);
