using DistriMosane.Api.Domain;
using DistriMosane.Api.Services;

namespace DistriMosane.Api.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ReturnsNull()
    {
        using var database = CreateDatabase();
        var service = new OrderService(database.Context);

        var result = await service.GetByIdAsync(404);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOrderWithItsCustomerAndLines()
    {
        using var database = CreateDatabase();
        var service = new OrderService(database.Context);
        var orderId = database.Context.Orders.Single(order => order.Status == OrderStatus.Shipped).Id;

        var result = await service.GetByIdAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal("Imprimerie Colson", result.CustomerName);
        Assert.Equal("Shipped", result.Status);
        Assert.Equal(new DateTime(2026, 3, 4), result.OrderDate);
        Assert.Equal(
            ["Ramette papier A4 80g (carton de 5)", "Post-it 76x76 mm (lot de 12)"],
            result.Lines.Select(line => line.ProductLabel));
    }

    [Fact]
    public async Task GetByIdAsync_MultipliesQuantityByUnitPriceOnEachLine()
    {
        using var database = CreateDatabase();
        var service = new OrderService(database.Context);
        var orderId = database.Context.Orders.Single(order => order.Status == OrderStatus.Shipped).Id;

        var result = await service.GetByIdAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal([458.00m, 57.20m], result.Lines.Select(line => line.LineTotal));
    }

    [Fact]
    public async Task GetByIdAsync_SumsTheLinesOfTheOrder()
    {
        using var database = CreateDatabase();
        var service = new OrderService(database.Context);
        var orderId = database.Context.Orders.Single(order => order.Status == OrderStatus.Shipped).Id;

        var result = await service.GetByIdAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal(515.20m, result.Total);
    }

    [Fact]
    public async Task GetByIdAsync_ExposesTheStatusAsText()
    {
        using var database = CreateDatabase();
        var service = new OrderService(database.Context);
        var orderId = database.Context.Orders.Single(order => order.Status == OrderStatus.Cancelled).Id;

        var result = await service.GetByIdAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal("Cancelled", result.Status);
    }

    private static TestDatabase CreateDatabase()
    {
        var database = new TestDatabase();

        database.Context.Customers.Add(new Customer
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
                    OrderDate = new DateTime(2026, 3, 4),
                    Status = OrderStatus.Shipped,
                    Lines =
                    [
                        new OrderLine { ProductLabel = "Ramette papier A4 80g (carton de 5)", Quantity = 20, UnitPrice = 22.90m },
                        new OrderLine { ProductLabel = "Post-it 76x76 mm (lot de 12)", Quantity = 4, UnitPrice = 14.30m }
                    ]
                },
                new Order
                {
                    OrderDate = new DateTime(2026, 1, 9),
                    Status = OrderStatus.Cancelled,
                    Lines = [new OrderLine { ProductLabel = "Cartouche toner laser couleur 2500 pages", Quantity = 3, UnitPrice = 124.50m }]
                }
            ]
        });

        database.Context.SaveChanges();

        return database;
    }
}
