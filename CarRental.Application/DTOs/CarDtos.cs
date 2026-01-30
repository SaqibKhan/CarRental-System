using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs;

public record CarResponse(
    Guid Id,
    string CarName,
    string NumberPlate,
    string ModelYear,
    decimal DailyPrice,
    string Description,
    CarType CarType,
    bool IsActive,
    string ImageUrl,
    IReadOnlyList<ReservationSummary> Reservations);
