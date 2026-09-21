using DistriMosane.Api.Domain;
using DistriMosane.Api.Dtos;

namespace DistriMosane.Api.Mapping;

public static class OrderMappingExtensions
{
    public static decimal ComputeTotal(this Order order)
    {
        return order.Lines.Sum(line => line.Quantity * line.UnitPrice);
    }

    public static OrderSummaryDto ToSummary(this Order order)
    {
        return new OrderSummaryDto(
            order.Id,
            order.OrderDate,
            order.Status.ToString(),
            order.Lines.Count,
            order.ComputeTotal());
    }

    public static OrderDetailDto ToDetail(this Order order)
    {
        return new OrderDetailDto(
            order.Id,
            order.OrderDate,
            order.Status.ToString(),
            order.CustomerId,
            order.Customer?.CompanyName ?? string.Empty,
            order.Lines.Select(line => line.ToDto()).ToList(),
            order.ComputeTotal());
    }

    private static OrderLineDto ToDto(this OrderLine line)
    {
        return new OrderLineDto(
            line.Id,
            line.ProductLabel,
            line.Quantity,
            line.UnitPrice,
            line.Quantity * line.UnitPrice);
    }
}
