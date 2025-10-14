using Shouldly;
using WebApplication1;

namespace EventSourcing.Functional.Movie;

public class MovieDeciderTests
{
    private static readonly TicketPrice Price = TicketPrice.Create(10).UnwrapSuccess().Type;
    private static readonly Guid TestMovieId = Guid.NewGuid();

    [Fact]
    public void Should_Create_Movie_From_Initial_State()
    {
        // Arrange
        var initState = Projection.Apply();
        var command = new AddMovie(TestMovieId, "Inception", 100, DateTimeOffset.Now, Price);

        // Act
        var result = MovieDecider.Decide(initState, command);

        // Assert
        result.ShouldSucceed<MovieAdded>()
            .MovieId.ShouldBe(TestMovieId);
    }

    [Fact]
    public void Should_Fail_When_Adding_Movie_To_Existing_State()
    {
        // Arrange
        var currentState = Projection.Apply(new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, Price));
        var command = new AddMovie(TestMovieId, "Inception", 100, DateTimeOffset.Now, Price);

        // Act
        var result = MovieDecider.Decide(currentState, command);

        // Assert
        result.ShouldFail().Message.ShouldBe("Invalid command for the current state");
    }
}