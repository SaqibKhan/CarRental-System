using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Repositories;

public interface ICarRepository
{
    Task<Car?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Car>> GetByTypeAsync(CarType carType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Car>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetAvailableCarAsync(Guid carId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, CancellationToken cancellationToken = default);
    Task AddAsync(Car car, CancellationToken cancellationToken = default);
}
