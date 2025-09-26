using EventSourcing.Functional.Movie;
using Shouldly;

namespace EventSourcing.Functional.GetAvailableMovies;

public class AvailableMoviesTests
{
    private static readonly TicketPrice Price = TicketPrice.Create(10).UnwrapSuccess().Type;

    [Fact]
    public void Test_Projection()
    {
        var movieId = Guid.NewGuid();
        var displayTime = DateTimeOffset.Now;
        const string title = "Inception";
        const int numberOfSeats = 100;

        MovieEvent[] events =
        [
            new MovieAdded(movieId, title, numberOfSeats, displayTime, Price),
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