using Shouldly;
using WebApplication1.Functional.Writes.Movie;
using Xunit;

namespace WebApplication1.Functional.Reads.SeatSelection;

public class MovieSelectionTests
{
    [Fact]
    public void Test()
    {
        var movieId = Guid.NewGuid();
        var displayTime = DateTimeOffset.Now;
        object[] events =
        [
            new MovieAdded(movieId, "Inception", 100, displayTime, 15.0),
        ];

        var state = MovieProjection.Apply(events);
        state.Title.ShouldBe("Inception");
        state.DisplayTime.ShouldBe(displayTime);
    }
}