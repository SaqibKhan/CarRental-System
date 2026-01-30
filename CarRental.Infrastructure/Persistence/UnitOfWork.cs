using CarRental.Domain.Repositories;

namespace CarRental.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarRentalDbContext _context;

    public UnitOfWork(CarRentalDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
