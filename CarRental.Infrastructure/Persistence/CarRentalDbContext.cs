using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence;

public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.CarType).HasConversion<string>();
            entity.HasMany(c => c.Reservations)
                  .WithOne(r => r.Car)
                  .HasForeignKey(r => r.CarId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasMany(u => u.Reservations)
                  .WithOne(r => r.User)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.StartDateTime).IsRequired();
            entity.Property(r => r.EndDateTime).IsRequired();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Deterministic GUIDs for seed data
        var carIds = Enumerable.Range(0, 300)
            .Select(i => Guid.Parse($"20000000-0000-0000-0000-{i.ToString("D12")}"))
            .ToArray();

        var user1Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var user2Id = Guid.Parse("10000000-0000-0000-0000-000000000002");

        // Password hashes produced using PBKDF2-SHA256 with 100000 iterations and 16-byte salts, 32-byte hashes,
        // matching CarRental.Infrastructure.Authentication.PasswordHasher.
        // Precomputed to keep HasData deterministic:
        // user1 ("123")
        var user1Hash = "bCW7U4jwJs71FiGK9DZK8Q==.WoPsW82LMwGdtRMwsBdcX9+QkpCkZd/ZzOSgGgo7/cc=";
        // user2 ("123")
        var user2Hash = "bCW7U4jwJs71FiGK9DZK8Q==.WoPsW82LMwGdtRMwsBdcX9+QkpCkZd/ZzOSgGgo7/cc=";

        // Seed Users
        modelBuilder.Entity<User>().HasData(new[]
        {
            new User
            {
                Id = user1Id,
                Email = "user1@example.com",
                PasswordHash = user1Hash
            },
            new User
            {
                Id = user2Id,
                Email = "user2@example.com",
                PasswordHash = user2Hash
            }
        });

        // Seed Cars (300 records)
        var makes = new[] { "Toyota", "Honda", "Ford", "BMW", "Audi", "Chevrolet", "Nissan", "Hyundai", "Kia", "Mercedes" };
        var models = new[] { "Corolla", "Civic", "Focus", "3 Series", "A4", "Malibu", "Altima", "Elantra", "Rio", "C Class" };
        var carTypes = new[] { CarType.Sedan, CarType.SUV, CarType.Van };

        var cars = new List<Car>(capacity: 300);
        for (int i = 0; i < 300; i++)
        {
            var id = carIds[i];
            var make = makes[i % makes.Length];
            var model = models[i % models.Length];
            var year = (2010 + (i % 15)).ToString(); // "2010".."2024"
            var plate = $"REG-{i + 1000:D6}";
            var price = 30m + (i % 70); // 30..99
            var type = carTypes[i % carTypes.Length];

            cars.Add(new Car
            {
                Id = id,
                CarName = $"{make} {model}",
                NumberPlate = plate,
                ModelYear = year,
                DailyPrice = price,
                Description = $"Sample {make} {model} {year} for testing.",
                CarType = type,
                IsActive = true,
                ImageUrl = "/Images/00288d00-588a-4285-86b5-c26bd0b72531.jpg"
            });
        }
        modelBuilder.Entity<Car>().HasData(cars);

        // Seed Reservations (250 records)
        var reservations = new List<Reservation>(capacity: 250);
        var baseStart = new DateTimeOffset(2025, 1, 1, 8, 0, 0, TimeSpan.Zero);

        for (int i = 0; i < 250; i++)
        {
            var id = Guid.Parse($"30000000-0000-0000-0000-{i.ToString("D12")}");
            var carId = carIds[i % carIds.Length];
            var userId = (i % 2 == 0) ? user1Id : user2Id;
            var start = baseStart.AddDays(i % 60).AddHours(i % 12);
            var end = start.AddDays(2).AddHours(i % 4);

            reservations.Add(new Reservation
            {
                Id = id,
                CarId = carId,
                UserId = userId,
                StartDateTime = start,
                EndDateTime = end
            });
        }
        modelBuilder.Entity<Reservation>().HasData(reservations);
    }
}
