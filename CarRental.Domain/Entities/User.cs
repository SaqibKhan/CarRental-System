using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(256)]  // Ensure sufficient length for Base64-encoded salt.hash
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
