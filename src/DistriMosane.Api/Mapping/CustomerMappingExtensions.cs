using DistriMosane.Api.Domain;
using DistriMosane.Api.Dtos;

namespace DistriMosane.Api.Mapping;

public static class CustomerMappingExtensions
{
    public static CustomerListItemDto ToListItem(this Customer customer)
    {
        var orders = customer.Orders;

        return new CustomerListItemDto(
            customer.Id,
            customer.CompanyName,
            customer.VatNumber,
            customer.City,
            orders.Count,
            orders.Sum(order => order.ComputeTotal()),
            orders.Count == 0 ? null : orders.Max(order => order.OrderDate));
    }

    public static CustomerDetailDto ToDetail(this Customer customer)
    {
        return new CustomerDetailDto(
            customer.Id,
            customer.CompanyName,
            customer.VatNumber,
            customer.Email,
            customer.City,
            customer.CreatedAt,
            customer.Orders
                .OrderByDescending(order => order.OrderDate)
                .Select(order => order.ToSummary())
                .ToList());
    }
}
