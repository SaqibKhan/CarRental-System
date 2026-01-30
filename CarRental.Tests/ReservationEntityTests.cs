using CarRental.Domain.Entities;
using FluentAssertions;

namespace CarRental.Tests;

public class ReservationEntityTests
{
    [Fact]
    public void OverlapsWith_ShouldReturnTrue_WhenRequestedStartIsBeforeExistingEnd_AndRequestedEndIsAfterExistingStart()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 12, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 18, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void OverlapsWith_ShouldReturnTrue_WhenRequestedPeriodIsWithinExisting()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 12, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void OverlapsWith_ShouldReturnTrue_WhenExistingPeriodIsWithinRequested()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 12, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void OverlapsWith_ShouldReturnFalse_WhenRequestedEndsBeforeExistingStarts()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 5, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void OverlapsWith_ShouldReturnFalse_WhenRequestedStartsAfterExistingEnds()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 25, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void OverlapsWith_ShouldReturnFalse_WhenRequestedEndsExactlyWhenExistingStarts()
    {
        // Arrange (adjacent periods - not overlapping)
        var reservation = new Reservation
        {
            StartDateTime = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero)
        };

        var requestedStart = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero);
        var requestedEnd = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = reservation.OverlapsWith(requestedStart, requestedEnd);

        // Assert
        result.Should().BeFalse();
    }
}
