using CarRental.Infrastructure.Authentication;
using FluentAssertions;

namespace CarRental.Tests;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void Hash_ShouldReturnDifferentHashesForSamePassword()
    {
        // Arrange
        var password = "123!";

        // Act
        var hash1 = _passwordHasher.Hash(password);
        var hash2 = _passwordHasher.Hash(password);

        // Assert
        hash1.Should().NotBe(hash2); // Different salts should produce different hashes
    }

    [Fact]
    public void Verify_ShouldReturnTrue_WhenPasswordMatchesHash()
    {
        // Arrange
        var password = "123!";
        var hash = _passwordHasher.Hash(password);

        // Act
        var result = _passwordHasher.Verify(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
    {
        // Arrange
        var password = "123!";
        var hash = _passwordHasher.Hash(password);

        // Act
        var result = _passwordHasher.Verify("WrongPassword", hash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Verify_ShouldReturnFalse_WhenHashFormatIsInvalid()
    {
        // Arrange
        var invalidHash = "invalid-hash-without-separator";

        // Act
        var result = _passwordHasher.Verify("password", invalidHash);

        // Assert
        result.Should().BeFalse();
    }
}
