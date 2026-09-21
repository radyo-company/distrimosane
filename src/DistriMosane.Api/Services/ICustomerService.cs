using DistriMosane.Api.Dtos;

namespace DistriMosane.Api.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerListItemDto>> SearchAsync(string? search, CancellationToken cancellationToken = default);

    Task<CustomerDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
