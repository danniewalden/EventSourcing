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

    [Fact]
    public void Should_Increase_Ticket_Price()
    {
        // Arrange
        var command = new IncreaseTicketPriceBy(20.0);

        // Act
        var result = MovieDecider.Decide(ExistingMovie, command);

        // Assert
        result.ShouldSucceed().ShouldBeOfType<TicketPriceIncreased>()
            .Amount.ShouldBe(TicketPrice.Create(20.0));
    }  
    
    [Fact]
    public void Should_Not_Increase_Ticket_Price_If_Total_Is_Above_500()
    {
        // Arrange
        var command = new IncreaseTicketPriceBy(491.0);

        // Act
        var result = MovieDecider.Decide(ExistingMovie, command);

        // Assert
        result.ShouldFail().Message.ShouldBe("Ticket price exceeds our policy of maximum 500$ per ticket");
    }

    [Fact]
    public void Should_Decrease_Ticket_Price()
    {
        // Arrange
        var command = new DecreaseTicketPriceBy(5.0);
        var state = new MovieState.PendingScreening(TestMovieId, 10);


        // Act
        var result = MovieDecider.Decide(state, command);

        // Assert
        var @event = result.ShouldSucceed().ShouldBeOfType<TicketPriceDecreased>();
        @event.MovieId.ShouldBe(TestMovieId);
        @event.Amount.ShouldBe(TicketPrice.Create(5.0));
    }


    [Fact]
    public void Should_Fail_When_Decreasing_Price_For_Non_Existing_Movie()
    {
        // Arrange
        var command = new DecreaseTicketPriceBy(10.0);

        // Act
        var result = MovieDecider.Decide(InitialState, command);

        // Assert
        result.ShouldFail()
            .Message.ShouldBe("Invalid command for the current state");
    }
}