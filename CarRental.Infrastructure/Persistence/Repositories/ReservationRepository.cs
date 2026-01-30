using CarRental.Domain.Entities;
using CarRental.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly CarRentalDbContext _context;

    public ReservationRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Include(r => r.Car)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.Car)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetByCarIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(r => r.CarId == carId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingReservationAsync(
        Guid carId, 
        DateTimeOffset startDateTime, 
        DateTimeOffset endDateTime, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AnyAsync(r => r.CarId == carId && 
                          startDateTime < r.EndDateTime && 
                          endDateTime > r.StartDateTime, 
                     cancellationToken);
    }

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
    }
}
