using Shouldly;
using WebApplication1.Oop.Writes.Movie;
using Xunit;

namespace WebApplication1.Oop.Reads.GetAvailableMovies;

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
            new TicketPriceIncreased(movieId, 5.0), // price should now be 20
            new TicketPriceDecreased(movieId, 2.0) // price should now be 18
        ];

        var readModel = new ReadModel();
        readModel.Apply(events);

        readModel.TicketPrice.ShouldBe(18.0);
        readModel.Id.ShouldBe(movieId);
        readModel.NumberOfAvailableSeats.ShouldBe(numberOfSeats);
        readModel.Title.ShouldBe(title);
        readModel.DisplayTime.ShouldBe(displayTime);
        
    }
}