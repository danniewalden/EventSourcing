using Shouldly;
using Xunit;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.DecreaseTicketPrice;

public class Tests
{
    [Fact]
    public void Decide_WhenStateIsValid_ShouldReturnTicketPriceDecreasedEvent()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var state = new MovieState.PendingScreening { TicketPrice = 20, Id = movieId };
        var command = new Command(10.0);

        // Act
        var result = Decider.Decide(state, command);

        // Assert
        result.ShouldBe(new TicketPriceDecreased(movieId, 10.0));
    }
}