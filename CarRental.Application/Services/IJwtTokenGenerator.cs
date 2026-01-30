using CarRental.Domain.Entities;

namespace CarRental.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
