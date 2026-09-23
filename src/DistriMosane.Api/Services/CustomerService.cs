using DistriMosane.Api.Data;
using DistriMosane.Api.Domain;
using DistriMosane.Api.Dtos;
using DistriMosane.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DistriMosane.Api.Services;

public class CustomerService(AppDbContext context) : ICustomerService
{
    public async Task<IReadOnlyList<CustomerListItemDto>> SearchAsync(
        string? search,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query = context.Customers
            .AsNoTracking()
            .Where(customer => !customer.IsArchived)
            .Include(customer => customer.Orders)
                .ThenInclude(order => order.Lines);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(customer => EF.Functions.Like(customer.CompanyName, $"%{term}%"));
        }

        var customers = await query
            .OrderBy(customer => customer.CompanyName)
            .ToListAsync(cancellationToken);

        return customers
            .Select(customer => customer.ToListItem())
            .ToList();
    }

    public async Task<CustomerDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await context.Customers
            .AsNoTracking()
            .Include(c => c.Orders)
                .ThenInclude(order => order.Lines)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return customer?.ToDetail();
    }
}
