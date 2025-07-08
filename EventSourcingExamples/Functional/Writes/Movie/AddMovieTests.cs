using Shouldly;
using Xunit;

namespace WebApplication1.Functional.Writes.Movie;

public class MovieDeciderTests
{
    private static readonly TicketPrice Price = 10;
    private static readonly Guid TestMovieId = Guid.NewGuid();
    private static readonly MovieState InitialState = new MovieState.Initial();
    private static readonly MovieState ExistingMovie = new MovieState.PendingScreening(TestMovieId, Price);

    [Fact]
    public void Should_Create_Movie_From_Initial_State()
    {
        // Arrange
        var command = new AddMovie(TestMovieId, "Inception", 100, DateTimeOffset.Now, 15.0);

        // Act
        var result = MovieDecider.Decide(InitialState, command);

        // Assert
        result.ShouldSucceed()
            .ShouldBeOfType<MovieAdded>()
            .MovieId.ShouldBe(TestMovieId);
    }

    [Fact]
    public void Should_Fail_When_Adding_Movie_To_Existing_State()
    {
        // Arrange
        var command = new AddMovie(TestMovieId, "Inception", 100, DateTimeOffset.Now, 15.0);

        // Act
        var result = MovieDecider.Decide(ExistingMovie, command);

        // Assert
        result.ShouldFail().Message.ShouldBe("Invalid command for the current state");
    }
}