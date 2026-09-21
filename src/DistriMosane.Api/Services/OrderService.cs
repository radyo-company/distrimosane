using DistriMosane.Api.Data;
using DistriMosane.Api.Dtos;
using DistriMosane.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DistriMosane.Api.Services;

public class OrderService(AppDbContext context) : IOrderService
{
    public async Task<OrderDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return order?.ToDetail();
    }
}
