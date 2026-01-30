using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence.Repositories;

public class CarRepository : ICarRepository
{
    private readonly CarRentalDbContext _context;

    public CarRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Car?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Car>> GetByTypeAsync(CarType carType, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Where(c => c.CarType == carType && c.IsActive)
            .Include(c => c.Reservations)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Car>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Car?> GetAvailableCarAsync(
        Guid carId,
        DateTimeOffset startDateTime, 
        DateTimeOffset endDateTime, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Where(c => c.Id == carId && c.IsActive)
            .Where(c => !c.Reservations.Any(r => 
                startDateTime < r.EndDateTime && endDateTime > r.StartDateTime))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await _context.Cars.AddAsync(car, cancellationToken);
    }
}
