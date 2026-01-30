using CarRental.Application.Common;
using CarRental.Application.DTOs;
using CarRental.Domain.Entities;
using CarRental.Domain.Repositories;

namespace CarRental.Application.Services;

public class ReservationService : IReservationService
{
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(
        ICarRepository carRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReservationResponse>> CreateReservationAsync(
        Guid userId, 
        CreateReservationRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (request.NumberOfDays <= 0)
        {
            return Result<ReservationResponse>.Failure("Reservation duration must be greater than zero");
        }

        var startDateTime = request.StartDateTime;
        var endDateTime = startDateTime.AddDays(request.NumberOfDays);

        var availableCar = await _carRepository.GetAvailableCarAsync(
            request.CarId, 
            startDateTime, 
            endDateTime, 
            cancellationToken);

        if (availableCar is null)
        {
            return Result<ReservationResponse>.Failure(
                $"No {request.CarId} cars are available for the requested date range");
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            CarId = availableCar.Id,
            UserId = userId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime
        };

        await _reservationRepository.AddAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ReservationResponse>.Success(new ReservationResponse(
            reservation.Id,
            reservation.CarId,
            reservation.UserId,
            reservation.StartDateTime,
            reservation.EndDateTime));
    }

    public async Task<Result<IReadOnlyList<ReservationResponse>>> GetUserReservationsAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var reservations = await _reservationRepository.GetByUserIdAsync(userId, cancellationToken);

        var response = reservations.Select(r => new ReservationResponse(
            r.Id,
            r.CarId,
            r.UserId,
            r.StartDateTime,
            r.EndDateTime)).ToList();

        return Result<IReadOnlyList<ReservationResponse>>.Success(response);
    }
}
