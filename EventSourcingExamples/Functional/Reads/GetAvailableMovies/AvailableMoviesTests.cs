using Shouldly;
using WebApplication1.Functional.Writes.Movie;
using Xunit;

namespace WebApplication1.Functional.Reads.GetAvailableMovies;

public class AvailableMoviesTests
{
    [Fact]
    public void Test_Projection()
    {
        var movieId = Guid.NewGuid();
        var displayTime = DateTimeOffset.Now;
        const string title = "Inception";
        const int numberOfSeats = 100;

        MovieEvent[] events =
        [
            new MovieAdded(movieId, title, numberOfSeats, displayTime, 15.0),
        ];

        var readModel = Projection.Apply(events);

        readModel.ShouldBe(
            new ReadModel
            {
                Id = movieId,
                Title = title,
                NumberOfAvailableSeats = numberOfSeats,
                DisplayTime = displayTime,
                TicketPrice = 18.0
            }
        );
    }
}