using EventSourcing.DCB.Events;
using Shared;
using Shouldly;

namespace EventSourcing.DCB.Movie;

public class MovieDeciderTests
{
    private static readonly TicketPrice Price = TicketPrice.From(10).GetValueOrThrow();
    private static readonly Guid TestMovieId = Guid.NewGuid();

    [Fact]
    public void Should_Create_Movie_From_Initial_State()
    {
        // Arrange
        var command = new AddMovie(TestMovieId, "Inception", 100, DateTimeOffset.Now, Price);

        // Act
        var result = AddMovie.Decide(command);

        // Assert
        result.ShouldSucceed<MovieAdded>()
            .MovieId.ShouldBe(TestMovieId);
    }

    [Fact]
    public void Should_Fail_When_Adding_Movie_To_Existing_State()
    {
        // this cannot happen, due to explicit design :D
        // Good design made us able to NOT have a test
    }
}