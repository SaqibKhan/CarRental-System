namespace CarRental.Application.DTOs;

public record CreateReservationRequest(Guid CarId, DateTimeOffset StartDateTime, int NumberOfDays);

public record ReservationSummary(
    Guid Id,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);

public record ReservationResponse(
    Guid Id,
    Guid CarId,
    Guid UserId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime);
