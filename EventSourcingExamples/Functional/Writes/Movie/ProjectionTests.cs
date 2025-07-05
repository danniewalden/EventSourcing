using Shouldly;
using Xunit;

namespace WebApplication1.Functional.Writes.Movie;

public class ProjectionTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        
        MovieEvent[] events =
        {
            new MovieAdded(movieId, "Inception", 100, DateTimeOffset.Now, 15.0),
            new TicketPriceIncreased(movieId, 5.0), // price should now be 20
            new TicketPriceDecreased(movieId, 2.0) // price should now be 18       
        };
        
        // Act
        var movieState = Projection.Apply(events);

        // Assert
        movieState.ShouldBeOfType<MovieState.PendingScreening>().TicketPrice.ShouldBe(18);
    }
}