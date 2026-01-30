using CarRental.Application.Common;
using CarRental.Application.DTOs;
using CarRental.Domain.Enums;
using CarRental.Domain.Repositories;

namespace CarRental.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<Result<IReadOnlyList<CarResponse>>> GetAllCarsAsync(CancellationToken cancellationToken = default)
    {
        var cars = await _carRepository.GetAllActiveAsync(cancellationToken);
        var response = cars.Select(c => new CarResponse(
            c.Id,
            c.CarName,
            c.NumberPlate,
            c.ModelYear,
            c.DailyPrice,
            c.Description,
            c.CarType,
            c.IsActive,
            c.ImageUrl,
            c.Reservations.Select(r => new ReservationSummary(r.Id, r.StartDateTime, r.EndDateTime)).ToList()
        )).ToList();

        
        return Result<IReadOnlyList<CarResponse>>.Success(response);
    }

    public async Task<Result<IReadOnlyList<CarResponse>>> GetCarsByTypeAsync(CarType carType, CancellationToken cancellationToken = default)
    {
        var cars = await _carRepository.GetByTypeAsync(carType, cancellationToken);
        var response = cars.Select(c => new CarResponse(
            c.Id,
            c.CarName,
            c.NumberPlate,
            c.ModelYear,
            c.DailyPrice,
            c.Description,
            c.CarType,
            c.IsActive,
            c.ImageUrl,
            c.Reservations.Select(r => new ReservationSummary(r.Id, r.StartDateTime, r.EndDateTime)).ToList()
        )).ToList();
        return Result<IReadOnlyList<CarResponse>>.Success(response);
    }

    public async Task<Result<bool>> IsCarAvailableAsync(
        Guid carId, 
        DateTimeOffset startDateTime, 
        DateTimeOffset endDateTime, 
        CancellationToken cancellationToken = default)
    {
        var availableCar = await _carRepository.GetAvailableCarAsync(carId, startDateTime, endDateTime, cancellationToken);
        return Result<bool>.Success(availableCar is not null);
    }

    public async Task<Result<CarResponse>> GetCarByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await _carRepository.GetByIdAsync(id, cancellationToken);
        
        if (car is null)
        {
            return Result<CarResponse>.Failure("Car not found.");
        }
        
        var response = new CarResponse(
            car.Id,
            car.CarName,
            car.NumberPlate,
            car.ModelYear,
            car.DailyPrice,
            car.Description,
            car.CarType,
            car.IsActive,
            car.ImageUrl,
            car.Reservations.Select(r => new ReservationSummary(r.Id, r.StartDateTime, r.EndDateTime)).ToList()
        );
        return Result<CarResponse>.Success(response);
    }
}
