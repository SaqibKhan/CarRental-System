using CarRental.Application.Common;
using CarRental.Application.DTOs;

namespace CarRental.Application.Services;

public interface IReservationService
{
    Task<Result<ReservationResponse>> CreateReservationAsync(Guid userId, CreateReservationRequest request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ReservationResponse>>> GetUserReservationsAsync(Guid userId, CancellationToken cancellationToken = default);
}
