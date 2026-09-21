namespace DistriMosane.Api.Domain;

public class Customer
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string VatNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsArchived { get; set; }

    public List<Order> Orders { get; set; } = [];
}
