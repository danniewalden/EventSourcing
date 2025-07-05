using Shouldly;
using Xunit;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.IncreateTicketPrice;

public class Tests
{
    [Fact]
    public void Decide_WhenStateIsValid_ShouldReturnTicketPriceIncreasedEvent()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var state = new MovieState.PendingScreening { Id = movieId, TicketPrice = 10.0 };
        var command = new Command(10.0);

        // Act
        var result = Decider.Decide(state, command);

        // Assert
        result.ShouldBe(new TicketPriceIncreased(movieId, 10.0));
    }
}