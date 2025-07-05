using Shouldly;
using Xunit;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.AddMovie;

public class DeciderTests
{
    [Fact]
    public void Decide_WhenStateIsInitial_ShouldReturnMovieAddedEvent()
    {
        // Arrange
        var command = new Command(
            Guid.NewGuid(),
            "Test Movie",
            100,
            DateTimeOffset.UtcNow,
            12.99
        );

        // Act
        var result = Decider.Decide(command);

        // Assert
        var expected = new MovieAdded(
            command.MovieId,
            command.Title,
            command.NumberOfSeats,
            command.DisplayTime,
            command.TicketPrice
        );
        result.ShouldBe(expected);
    }
}