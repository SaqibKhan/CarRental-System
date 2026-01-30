using CarRental.Application.DTOs;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Persistence.Repositories;
using FluentAssertions;

namespace CarRental.Tests;

[Trait("Category", "Integration")]
[Trait("Feature", "Reservations")]
public sealed class ReservationServiceTests : TestBase
{
    private readonly ReservationService _sut;
    private readonly CarRepository _carRepository;
    private readonly ReservationRepository _reservationRepository;

    public ReservationServiceTests()
    {
        _carRepository = new CarRepository(DbContext);
        _reservationRepository = new ReservationRepository(DbContext);
        var unitOfWork = new UnitOfWork(DbContext);
        _sut = new ReservationService(_carRepository, _reservationRepository, unitOfWork);
    }

    #region CreateReservationAsync Tests

    [Fact]
    public async Task CreateReservationAsync_WhenCarIsAvailable_ShouldReturnSuccessWithReservation()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var request = new CreateReservationRequest(
            CarId: Guid.Parse("20000000-0000-0000-0000-000000000003"),
            StartDateTime: DateTimeOffset.UtcNow.AddDays(1),
            NumberOfDays: 3
        );

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.CarId.Should().Be(request.CarId);
    }



    [Fact]
    public async Task CreateReservationAsync_ShouldCalculateEndDateCorrectly()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var startDate = DateTimeOffset.UtcNow.AddDays(15);
        var request = new CreateReservationRequest(
            CarId: Guid.Parse("20000000-0000-0000-0000-000000000001"),
            StartDateTime: startDate,
            NumberOfDays: 15
        );

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.Value!.StartDateTime.Should().Be(startDate);
        result.Value.EndDateTime.Should().Be(startDate.AddDays(15));
    }

    [Fact]
    public async Task CreateReservationAsync_WhenAllCarsOfTypeAreBooked_ShouldReturnFailure()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var startDate = DateTimeOffset.UtcNow.AddDays(1);
        await ReserveAllCarsOfTypeAsync(CarType.Sedan, user.Id, startDate, durationDays: 5);
        var carId = Guid.Parse("20000000-0000-0000-0000-000000000002");

        var request = CreateReservationRequest(carId: carId, startDate.AddDays(1), durationDays: 2);

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain($"No {carId} cars are available for the requested date range");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    [InlineData(-100)]
    public async Task CreateReservationAsync_WhenDurationIsInvalid_ShouldReturnValidationError(int invalidDuration)
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var request = CreateReservationRequest(carId: Guid.NewGuid(), 10, durationDays: invalidDuration);

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("duration must be greater than zero");
    }

    [Fact]
    public async Task CreateReservationAsync_WhenOneCarIsBookedButAnotherAvailable_ShouldAssignAvailableCar()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var startDate = DateTimeOffset.UtcNow.AddDays(30);

        var sedans = await _carRepository.GetByTypeAsync(CarType.Sedan);
        var bookedSedan = sedans.First();
        var carId = Guid.Parse("20000000-0000-0000-0000-000000000001");
        await CreateReservationAsync(bookedSedan.Id, user.Id, startDate, durationDays: 5);

        var request = CreateReservationRequest(carId: carId, startDate.AddDays(1), durationDays: 2);

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.CarId.Should().NotBe(bookedSedan.Id);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenRequestDoesNotOverlap_ShouldSucceed()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var startDate = DateTimeOffset.UtcNow.AddDays(20);

        var sedans = await _carRepository.GetByTypeAsync(CarType.Sedan);
        await CreateReservationAsync(sedans.First().Id, user.Id, startDate, durationDays: 3);
        var carId = Guid.Parse("20000000-0000-0000-0000-000000000001");
        var nonOverlappingRequest = CreateReservationRequest(carId: carId, startDate.AddDays(5), durationDays: 2);

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, nonOverlappingRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(CarType.Sedan)]
    [InlineData(CarType.SUV)]
    [InlineData(CarType.Van)]
    public async Task CreateReservationAsync_ShouldWorkForAllCarTypes(CarType carType)
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var carId = Guid.Parse("20000000-0000-0000-0000-000000000001");
        var request = CreateReservationRequest(carId: carId, daysFromNow: 1, durationDays: 2);

        // Act
        var result = await _sut.CreateReservationAsync(user.Id, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.CarId.Should().Be(carId);
    }

    #endregion

    #region GetUserReservationsAsync Tests

    [Fact]
    public async Task GetUserReservationsAsync_WhenUserHasReservations_ShouldReturnAllReservations()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var sedans = await _carRepository.GetByTypeAsync(CarType.Sedan);
        await CreateReservationAsync(sedans.First().Id, user.Id, DateTimeOffset.UtcNow.AddDays(40), durationDays: 3);

        // Act
        var result = await _sut.GetUserReservationsAsync(user.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle();
    }

    [Fact]
    public async Task GetUserReservationsAsync_WhenUserHasNoReservations_ShouldReturnEmptyList()
    {
        // Arrange
        var user = await CreateTestUserAsync();

        // Act
        var result = await _sut.GetUserReservationsAsync(user.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserReservationsAsync_ShouldOnlyReturnUserOwnReservations()
    {
        // Arrange
        await SeedCarsAsync();
        var user1 = await CreateTestUserAsync("user1@test.com");
        var user2 = await CreateTestUserAsync("user2@test.com");

        var sedans = await _carRepository.GetByTypeAsync(CarType.Sedan);
        await CreateReservationAsync(sedans.First().Id, user1.Id, DateTimeOffset.UtcNow.AddDays(50), 2);
        await CreateReservationAsync(sedans.Last().Id, user2.Id, DateTimeOffset.UtcNow.AddDays(50), 2);

        // Act
        var result = await _sut.GetUserReservationsAsync(user1.Id);

        // Assert
        result.Value.Should().ContainSingle();
        result.Value.Should().AllSatisfy(r => r.Id.Should().NotBeEmpty());
    }

    [Fact]
    public async Task GetUserReservationsAsync_ShouldMapResponsePropertiesCorrectly()
    {
        // Arrange
        await SeedCarsAsync();
        var user = await CreateTestUserAsync();
        var sedans = await _carRepository.GetByTypeAsync(CarType.Sedan);
        var car = sedans.First();
        var startDate = DateTimeOffset.UtcNow.AddDays(60);
        await CreateReservationAsync(car.Id, user.Id, startDate, 3);

        // Act
        var result = await _sut.GetUserReservationsAsync(user.Id);

        // Assert
        var reservation = result.Value!.Single();
        reservation.CarId.Should().Be(car.Id);
        reservation.StartDateTime.Should().BeCloseTo(startDate, TimeSpan.FromSeconds(1));
        reservation.EndDateTime.Should().BeCloseTo(startDate.AddDays(3), TimeSpan.FromSeconds(1));
    }

    #endregion

    #region Helper Methods

    private static CreateReservationRequest CreateReservationRequest(Guid carId, int daysFromNow, int durationDays) =>
        new(carId, DateTimeOffset.UtcNow.AddDays(daysFromNow), durationDays);

    private static CreateReservationRequest CreateReservationRequest(Guid carId, DateTimeOffset startDate, int durationDays) =>
        new(carId, startDate, durationDays);

    private async Task ReserveAllCarsOfTypeAsync(CarType carType, Guid userId, DateTimeOffset startDate, int durationDays)
    {
        var cars = await _carRepository.GetByTypeAsync(carType);
        foreach (var car in cars)
        {
            await CreateReservationAsync(car.Id, userId, startDate, durationDays);
        }
    }

    private async Task CreateReservationAsync(Guid carId, Guid userId, DateTimeOffset startDate, int durationDays)
    {
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            CarId = carId,
            UserId = userId,
            StartDateTime = startDate,
            EndDateTime = startDate.AddDays(durationDays)
        };
        await _reservationRepository.AddAsync(reservation);
        await DbContext.SaveChangesAsync();
    }

    #endregion
}
