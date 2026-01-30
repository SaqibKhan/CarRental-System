using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

public class Car
{
    public Guid Id { get; set; }
    public string CarName { get; set; }
    public string NumberPlate { get; set; }
    public string ModelYear { get; set; }
    public decimal DailyPrice { get; set; }
    public string Description { get; set; }
    public CarType CarType { get; set; }
    public bool IsActive { get; set; } = true;
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
