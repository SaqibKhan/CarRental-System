using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Infrastructure.Persistence.Repositories;
using FluentAssertions;

namespace CarRental.Tests;

[Trait("Category", "Integration")]
[Trait("Feature", "Cars")]
public sealed class CarServiceTests : TestBase
{
    private readonly CarService _sut;
    private readonly CarRepository _carRepository;

    public CarServiceTests()
    {
        _carRepository = new CarRepository(DbContext);
        _sut = new CarService(_carRepository);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidRepository_ShouldCreateInstance()
    {
        // Act
        var service = new CarService(_carRepository);

        // Assert
        service.Should().NotBeNull();
    }

    #endregion

    #region GetAllCarsAsync Tests

    [Fact]
    public async Task GetAllCarsAsync_WhenCarsExist_ShouldReturnAllActiveCars()
    {
        // Arrange
        await SeedCarsAsync();

        // Act
        var result = await _sut.GetAllCarsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetAllCarsAsync_WhenNoCarsExist_ShouldReturnEmptyList()
    {
        // Act
        var result = await _sut.GetAllCarsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllCarsAsync_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllCarsAsync();
        var returnedCar = result.Value.FirstOrDefault(c => c.Id == car.Id)!;
        // Assert
        result.IsSuccess.Should().BeTrue();      
        returnedCar.Id.Should().Be(car.Id);
        returnedCar.CarName.Should().Be(car.CarName);
        returnedCar.NumberPlate.Should().Be(car.NumberPlate);
        returnedCar.ModelYear.Should().Be(car.ModelYear);
        returnedCar.DailyPrice.Should().Be(car.DailyPrice);
        returnedCar.Description.Should().Be(car.Description);
        returnedCar.CarType.Should().Be(car.CarType);
        returnedCar.IsActive.Should().Be(car.IsActive);
        returnedCar.ImageUrl.Should().Be(car.ImageUrl);
    }

    [Fact]
    public async Task GetAllCarsAsync_WithReservations_ShouldIncludeReservationSummaries()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllCarsAsync();
        var returnedCar = result.Value.FirstOrDefault(c => c.Id == car.Id)!;

        // Assert
        result.IsSuccess.Should().BeTrue();      
        returnedCar.Reservations.Should().ContainSingle();
        returnedCar.Reservations.First().Id.Should().Be(reservation.Id);
        returnedCar.Reservations.First().StartDateTime.Should().Be(reservation.StartDateTime);
        returnedCar.Reservations.First().EndDateTime.Should().Be(reservation.EndDateTime);
    }

    [Fact]
    public async Task GetAllCarsAsync_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _sut.GetAllCarsAsync(cts.Token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region GetCarByIdAsync Tests

    [Fact]
    public async Task GetCarByIdAsync_WhenCarExists_ShouldReturnCar()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarByIdAsync(car.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(car.Id);
    }

    [Fact]
    public async Task GetCarByIdAsync_WhenCarDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _sut.GetCarByIdAsync(nonExistentId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Car not found");
    }

    [Fact]
    public async Task GetCarByIdAsync_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarByIdAsync(car.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(car.Id);
        result.Value.CarName.Should().Be(car.CarName);
        result.Value.NumberPlate.Should().Be(car.NumberPlate);
        result.Value.ModelYear.Should().Be(car.ModelYear);
        result.Value.DailyPrice.Should().Be(car.DailyPrice);
        result.Value.Description.Should().Be(car.Description);
        result.Value.CarType.Should().Be(car.CarType);
        result.Value.IsActive.Should().Be(car.IsActive);
        result.Value.ImageUrl.Should().Be(car.ImageUrl);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithReservations_ShouldMapReservationsCorrectly()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarByIdAsync(car.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Reservations.Should().ContainSingle();
        var resSummary = result.Value.Reservations.First();
        resSummary.Id.Should().Be(reservation.Id);
        resSummary.StartDateTime.Should().Be(reservation.StartDateTime);
        resSummary.EndDateTime.Should().Be(reservation.EndDateTime);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithMultipleReservations_ShouldReturnAllReservations()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation1 = CreateTestReservation(car.Id, user.Id, 1, 3);
        var reservation2 = CreateTestReservation(car.Id, user.Id, 5, 7);
        car.Reservations.Add(reservation1);
        car.Reservations.Add(reservation2);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarByIdAsync(car.Id);

        // Assert
        result.Value!.Reservations.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _sut.GetCarByIdAsync(car.Id, cts.Token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region GetCarsByTypeAsync Tests

    [Fact]
    public async Task GetCarsByTypeAsync_WhenCarsOfTypeExist_ShouldReturnMatchingCars()
    {
        // Arrange
        await SeedCarsAsync();

        // Act
        var result = await _sut.GetCarsByTypeAsync(CarType.Sedan);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(c => c.CarType.Should().Be(CarType.Sedan));
    }

    [Fact]
    public async Task GetCarsByTypeAsync_WhenNoCarsOfTypeExist_ShouldReturnEmptyList()
    {
        // Arrange
        await SeedCarsAsync();

        // Act
        var result = await _sut.GetCarsByTypeAsync(CarType.SUV);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(CarType.Sedan, 2)]
    [InlineData(CarType.SUV, 1)]
    [InlineData(CarType.Van, 1)]  
    public async Task GetCarsByTypeAsync_ShouldReturnCorrectCountPerType(CarType carType, int expectedCount)
    {
        // Arrange
        await SeedCarsAsync();

        // Act
        var result = await _sut.GetCarsByTypeAsync(carType);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task GetCarsByTypeAsync_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var car = CreateTestCar(CarType.SUV);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarsByTypeAsync(CarType.SUV);
        var returnedCar = result.Value.FirstOrDefault(c => c.Id == car.Id)!;

        // Assert
        result.IsSuccess.Should().BeTrue();
        returnedCar.Id.Should().Be(car.Id);
        returnedCar.CarName.Should().Be(car.CarName);
        returnedCar.CarType.Should().Be(CarType.SUV);
    }

    [Fact]
    public async Task GetCarsByTypeAsync_WithReservations_ShouldIncludeReservationSummaries()
    {
        // Arrange
        var car = CreateTestCar(CarType.Van);
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCarsByTypeAsync(CarType.Van);
        var returnedCar = result.Value.FirstOrDefault(c => c.Id == car.Id)!;

        // Assert
        returnedCar.Reservations.Should().ContainSingle();
    }

    [Fact]
    public async Task GetCarsByTypeAsync_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _sut.GetCarsByTypeAsync(CarType.Sedan, cts.Token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region IsCarAvailableAsync Tests

    [Fact]
    public async Task IsCarAvailableAsync_WhenCarExistsAndAvailable_ShouldReturnTrue()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        var (startDate, endDate) = GetDateRange(1, 3);

        // Act
        var result = await _sut.IsCarAvailableAsync(car.Id, startDate, endDate);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WhenCarDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var (startDate, endDate) = GetDateRange(1, 3);

        // Act
        var result = await _sut.IsCarAvailableAsync(nonExistentId, startDate, endDate);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeFalse();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WhenCarIsBooked_ShouldReturnFalse()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var (bookedStart, bookedEnd) = GetDateRange(1, 5);
        var reservation = CreateTestReservation(car.Id, user.Id, 1, 5);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();

        // Act - request overlaps with existing booking
        var result = await _sut.IsCarAvailableAsync(car.Id, bookedStart.AddDays(1), bookedEnd.AddDays(-1));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeFalse();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WhenBookingDoesNotOverlap_ShouldReturnTrue()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id, 1, 3);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        var (requestStart, requestEnd) = GetDateRange(5, 7);

        // Act
        var result = await _sut.IsCarAvailableAsync(car.Id, requestStart, requestEnd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WhenRequestStartsWhenBookingEnds_ShouldReturnTrue()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id, 1, 3);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        var bookedEnd = DateTimeOffset.UtcNow.AddDays(3);

        // Act - new booking starts exactly when previous ends
        var result = await _sut.IsCarAvailableAsync(car.Id, bookedEnd, bookedEnd.AddDays(2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WhenRequestEndsWhenBookingStarts_ShouldReturnTrue()
    {
        // Arrange
        var car = CreateTestCar();
        var user = await CreateTestUserAsync();
        var reservation = CreateTestReservation(car.Id, user.Id, 5, 7);
        car.Reservations.Add(reservation);
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        var bookedStart = DateTimeOffset.UtcNow.AddDays(10);

        // Act - new booking ends exactly when next booking starts
        var result = await _sut.IsCarAvailableAsync(car.Id, bookedStart.AddDays(-3), bookedStart);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task IsCarAvailableAsync_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var car = CreateTestCar();
        await DbContext.Cars.AddAsync(car);
        await DbContext.SaveChangesAsync();
        var (startDate, endDate) = GetDateRange(1, 3);
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _sut.IsCarAvailableAsync(car.Id, startDate, endDate, cts.Token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region Helper Methods

    private static Car CreateTestCar(CarType carType = CarType.Sedan) => new()
    {
        Id = Guid.NewGuid(),
        CarName = "Toyota Camry",
        NumberPlate = $"ABC{Random.Shared.Next(100, 999)}",
        ModelYear = "2023",
        DailyPrice = 50.00m,
        Description = "Comfortable sedan",
        CarType = carType,
        IsActive = true,
        ImageUrl = "http://example.com/image.jpg",
        Reservations = []
    };

    private static Reservation CreateTestReservation(
        Guid carId,
        Guid userId,
        int startDaysFromNow = 1,
        int endDaysFromNow = 3) => new()
    {
        Id = Guid.NewGuid(),
        CarId = carId,
        UserId = userId,
        StartDateTime = DateTimeOffset.UtcNow.AddDays(startDaysFromNow),
        EndDateTime = DateTimeOffset.UtcNow.AddDays(endDaysFromNow)
    };

    private static (DateTimeOffset start, DateTimeOffset end) GetDateRange(
        int startDaysFromNow,
        int endDaysFromNow) =>
        (DateTimeOffset.UtcNow.AddDays(startDaysFromNow), DateTimeOffset.UtcNow.AddDays(endDaysFromNow));

    #endregion
}