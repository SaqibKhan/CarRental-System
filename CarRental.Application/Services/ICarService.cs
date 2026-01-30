using CarRental.Application.Common;
using CarRental.Application.DTOs;
using CarRental.Domain.Enums;

namespace CarRental.Application.Services;

public interface ICarService
{
    Task<Result<IReadOnlyList<CarResponse>>> GetAllCarsAsync(CancellationToken cancellationToken = default);
    Task<Result<CarResponse>> GetCarByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CarResponse>>> GetCarsByTypeAsync(CarType carType, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsCarAvailableAsync(Guid carId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, CancellationToken cancellationToken = default);
}
