using EventSourcing.Functional.Movie;
using Shouldly;

namespace EventSourcing.Functional.GetSeatSelection;

public class MovieSelectionTests
{
    [Fact]
    public void Test()
    {
        var ticketPrice = TicketPrice.Create(15).UnwrapSuccess().Type;
        var movieId = Guid.NewGuid();
        var displayTime = DateTimeOffset.Now;
        object[] events =
        [
            new MovieAdded(movieId, "Inception", 100, displayTime, ticketPrice),
        ];

        var state = MovieProjection.Apply(events);
        state.Title.ShouldBe("Inception");
        state.DisplayTime.ShouldBe(displayTime);
    }
}