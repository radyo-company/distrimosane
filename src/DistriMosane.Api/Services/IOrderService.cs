using DistriMosane.Api.Dtos;

namespace DistriMosane.Api.Services;

public interface IOrderService
{
    Task<OrderDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
