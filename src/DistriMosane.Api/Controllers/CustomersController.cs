using DistriMosane.Api.Dtos;
using DistriMosane.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistriMosane.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerListItemDto>>> Search(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var customers = await customerService.SearchAsync(search, cancellationToken);
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}
