using CarRental.Application.Common;
using CarRental.Application.DTOs;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace CarRental.Tests;

[Trait("Category", "Unit")]
[Trait("Feature", "Authentication")]
public sealed class AuthServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        
        _sut = new AuthService(_userRepository, _unitOfWork, _passwordHasher, _jwtTokenGenerator);
    }

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccessWithToken()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password123");
        var user = CreateUser(request.Email);
        const string expectedToken = "jwt-token";

        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.Verify(request.Password, user.PasswordHash).Returns(true);
        _jwtTokenGenerator.GenerateToken(user).Returns(expectedToken);

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Token.Should().Be(expectedToken);
        result.Value.Email.Should().Be(user.Email);
        result.Value.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentEmail_ShouldReturnFailure()
    {
        // Arrange
        var request = new LoginRequest("nonexistent@example.com", "password123");
        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid email or password");
    }

    [Fact]
    public async Task LoginAsync_WithIncorrectPassword_ShouldReturnFailure()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "wrongpassword");
        var user = CreateUser(request.Email);

        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.Verify(request.Password, user.PasswordHash).Returns(false);

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid email or password");
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task LoginAsync_ShouldNotGenerateToken_WhenPasswordVerificationFails()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "wrongpassword");
        var user = CreateUser(request.Email);

        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        // Act
        await _sut.LoginAsync(request);

        // Assert
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    #endregion

    #region RegisterAsync Tests

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldCreateUserAndReturnToken()
    {
        // Arrange
        var request = new RegisterRequest("newuser@example.com", "securepassword");
        const string hashedPassword = "hashed-password";
        const string expectedToken = "jwt-token";

        _userRepository.ExistsAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _passwordHasher.Hash(request.Password).Returns(hashedPassword);
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns(expectedToken);

        // Act
        var result = await _sut.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Token.Should().Be(expectedToken);
        result.Value.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var request = new RegisterRequest("existing@example.com", "password");
        _userRepository.ExistsAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("User with this email already exists");
    }

    [Fact]
    public async Task RegisterAsync_ShouldHashPassword_BeforeStoringUser()
    {
        // Arrange
        var request = new RegisterRequest("newuser@example.com", "plainpassword");
        const string hashedPassword = "hashed-password";

        _userRepository.ExistsAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _passwordHasher.Hash(request.Password).Returns(hashedPassword);
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns("token");

        // Act
        await _sut.RegisterAsync(request);

        // Assert
        _passwordHasher.Received(1).Hash(request.Password);
        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.PasswordHash == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RegisterAsync_ShouldCallSaveChanges_AfterAddingUser()
    {
        // Arrange
        var request = new RegisterRequest("newuser@example.com", "password");
        _userRepository.ExistsAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed");
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns("token");

        // Act
        await _sut.RegisterAsync(request);

        // Assert
        Received.InOrder(() =>
        {
            _userRepository.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>());
        });
    }

    [Fact]
    public async Task RegisterAsync_ShouldNotAddUser_WhenEmailExists()
    {
        // Arrange
        var request = new RegisterRequest("existing@example.com", "password");
        _userRepository.ExistsAsync(request.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        await _sut.RegisterAsync(request);

        // Assert
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    #endregion

    #region Helper Methods

    private static User CreateUser(string email = "test@example.com") => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        PasswordHash = "existing-hash"
    };

    #endregion
}
