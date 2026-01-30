namespace CarRental.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }

    public Car Car { get; set; } = null!;
    public User User { get; set; } = null!;

    public bool OverlapsWith(DateTimeOffset requestedStart, DateTimeOffset requestedEnd)
    {
        return requestedStart < EndDateTime && requestedEnd > StartDateTime;
    }
}
