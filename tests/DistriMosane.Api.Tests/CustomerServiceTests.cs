using DistriMosane.Api.Domain;
using DistriMosane.Api.Services;

namespace DistriMosane.Api.Tests;

public class CustomerServiceTests
{
    [Fact]
    public async Task SearchAsync_WithoutTerm_ReturnsEveryCustomerOrderedByCompanyName()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync(null);

        Assert.Equal(
            ["Atelier Graphique Wauters", "Fiduciaire Namuroise", "Garage Dupuis SA", "Imprimerie Colson"],
            result.Select(customer => customer.CompanyName));
    }

    [Fact]
    public async Task SearchAsync_WithPartialTerm_ReturnsOnlyMatchingCustomers()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync("Graph");

        var customer = Assert.Single(result);
        Assert.Equal("Atelier Graphique Wauters", customer.CompanyName);
        Assert.Equal("Namur", customer.City);
    }

    [Fact]
    public async Task SearchAsync_IgnoresTermCasing()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync("fiduciaire");

        var customer = Assert.Single(result);
        Assert.Equal("Fiduciaire Namuroise", customer.CompanyName);
    }

    [Fact]
    public async Task SearchAsync_WithBlankTerm_ReturnsEveryCustomer()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync("   ");

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task SearchAsync_ReportsOrderCountAndLastOrderDate()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync("Imprimerie");

        var customer = Assert.Single(result);
        Assert.Equal(2, customer.OrderCount);
        Assert.Equal(new DateTime(2026, 3, 4), customer.LastOrderDate);
    }

    [Fact]
    public async Task SearchAsync_ForCustomerWithoutOrder_ReturnsNoLastOrderDate()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.SearchAsync("Garage");

        var customer = Assert.Single(result);
        Assert.Equal(0, customer.OrderCount);
        Assert.Null(customer.LastOrderDate);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ReturnsNull()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);

        var result = await service.GetByIdAsync(404);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCustomerDetailsWithOrdersMostRecentFirst()
    {
        using var database = CreateDatabase();
        var service = new CustomerService(database.Context);
        var customerId = database.Context.Customers.Single(c => c.CompanyName == "Imprimerie Colson").Id;

        var result = await service.GetByIdAsync(customerId);

        Assert.NotNull(result);
        Assert.Equal("BE0216709183", result.VatNumber);
        Assert.Equal("commandes@imprimerie-colson.be", result.Email);
        Assert.Equal(
            [new DateTime(2026, 3, 4), new DateTime(2025, 11, 18)],
            result.Orders.Select(order => order.OrderDate));
        Assert.Equal(["Shipped", "Delivered"], result.Orders.Select(order => order.Status));
    }

    private static TestDatabase CreateDatabase()
    {
        var database = new TestDatabase();

        database.Context.Customers.AddRange(
            new Customer
            {
                CompanyName = "Imprimerie Colson",
                VatNumber = "BE0216709183",
                Email = "commandes@imprimerie-colson.be",
                City = "Namur",
                CreatedAt = new DateTime(2023, 2, 9),
                Orders =
                [
                    new Order
                    {
                        OrderDate = new DateTime(2025, 11, 18),
                        Status = OrderStatus.Delivered,
                        Lines = [new OrderLine { ProductLabel = "Classeur à levier A4 dos 8 cm", Quantity = 12, UnitPrice = 4.35m }]
                    },
                    new Order
                    {
                        OrderDate = new DateTime(2026, 3, 4),
                        Status = OrderStatus.Shipped,
                        Lines =
                        [
                            new OrderLine { ProductLabel = "Ramette papier A4 80g (carton de 5)", Quantity = 20, UnitPrice = 22.90m },
                            new OrderLine { ProductLabel = "Post-it 76x76 mm (lot de 12)", Quantity = 4, UnitPrice = 14.30m }
                        ]
                    }
                ]
            },
            new Customer
            {
                CompanyName = "Atelier Graphique Wauters",
                VatNumber = "BE0373254218",
                Email = "studio@wauters-graphic.be",
                City = "Namur",
                CreatedAt = new DateTime(2024, 6, 21),
                Orders =
                [
                    new Order
                    {
                        OrderDate = new DateTime(2026, 1, 15),
                        Status = OrderStatus.Delivered,
                        Lines = [new OrderLine { ProductLabel = "Cartouche toner laser couleur 2500 pages", Quantity = 3, UnitPrice = 124.50m }]
                    }
                ]
            },
            new Customer
            {
                CompanyName = "Fiduciaire Namuroise",
                VatNumber = "BE0921072507",
                Email = "info@fiduciaire-namuroise.be",
                City = "Namur",
                CreatedAt = new DateTime(2022, 10, 3),
                Orders =
                [
                    new Order
                    {
                        OrderDate = new DateTime(2025, 9, 30),
                        Status = OrderStatus.Delivered,
                        Lines = [new OrderLine { ProductLabel = "Enveloppes C4 blanches (boîte de 250)", Quantity = 6, UnitPrice = 34.60m }]
                    }
                ]
            },
            new Customer
            {
                CompanyName = "Garage Dupuis SA",
                VatNumber = "BE0999315279",
                Email = "administration@garagedupuis.be",
                City = "Andenne",
                CreatedAt = new DateTime(2025, 4, 2)
            });

        database.Context.SaveChanges();

        return database;
    }
}
