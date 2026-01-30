using CarRental.Domain.Entities;

namespace CarRental.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingReservationAsync(Guid carId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, CancellationToken cancellationToken = default);
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
