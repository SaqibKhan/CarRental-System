using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly CarRentalDbContext DbContext;

    protected TestBase()
    {
        var options = new DbContextOptionsBuilder<CarRentalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        DbContext = new CarRentalDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    protected async Task SeedCarsAsync()
    {
        var cars = new List<Car>
        {
            new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), CarName = "Test Car 1", NumberPlate = "ABC-001", ModelYear = "2024", Description = "Test description 1", CarType = CarType.Sedan, IsActive = true },
            new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), CarName = "Test Car 2", NumberPlate = "ABC-002", ModelYear = "2024", Description = "Test description 2", CarType = CarType.Sedan, IsActive = true },
            new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), CarName = "Test Car 3", NumberPlate = "ABC-003", ModelYear = "2024", Description = "Test description 3", CarType = CarType.SUV, IsActive = true },
            new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), CarName = "Test Car 4", NumberPlate = "ABC-004", ModelYear = "2024", Description = "Test description 4", CarType = CarType.Van, IsActive = true },
        };

        DbContext.Cars.RemoveRange(DbContext.Cars);
        await DbContext.SaveChangesAsync();
        
        await DbContext.Cars.AddRangeAsync(cars);
        await DbContext.SaveChangesAsync();
    }

    protected async Task<User> CreateTestUserAsync(string email = "test@example.com", string passwordHash = "hashedpassword")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash
        };

        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();
        return user;
    }

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
