using Shouldly;

namespace EventSourcing.Aggregates.Movie;

public class MovieAggregateTests
{
    private static readonly Guid TestMovieId = Guid.NewGuid();
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;
    private static readonly MovieAdded MovieAdded = new(TestMovieId, "Inception", 100, Now, 15.0);

    [Fact]
    public void Should_Create_Movie_From_Initial_State()
    {
        // Act
        var aggregate = MovieAggregate.Add(TestMovieId, "Inception", 100, Now, 15.0);

        // Assert
        aggregate.EventsToBeStored.ShouldHaveSingleItem().ShouldBe(new MovieAdded(TestMovieId, "Inception", 100, Now, 15.0));
    }

    [Fact]
    public void Should_Increase_Ticket_Price()
    {
        // Arrange
        var aggregate = Events.Apply<MovieAggregate>(MovieAdded);

        // Act
        aggregate.IncreaseTicketPrice(20);

        // Assert
        aggregate.EventsToBeStored.Last().ShouldBeOfType<TicketPriceIncreased>().Amount.ShouldBe(20);
    }

    [Fact]
    public void Should_Fail_When_Increasing_Price_Violates_Max_price_of_500() => Should.Throw<InvalidOperationException>(() =>
    {
        var aggregate = Events.Apply<MovieAggregate>(MovieAdded);
        aggregate.IncreaseTicketPrice(600);
    });

    [Fact]
    public void Should_Decrease_Ticket_Price()
    {
        // Arrange
        var aggregate = Events.Apply<MovieAggregate>(MovieAdded);

        // Act
        aggregate.DecreaseTicketPrice(15);

        // Assert
        aggregate.EventsToBeStored.Last().ShouldBeOfType<TicketPriceDecreased>().Amount.ShouldBe(15);
    }

    [Fact]
    public void Should_Mark_Movie_As_Screened()
    {
        // Arrange
        var aggregate = Events.Apply<MovieAggregate>(MovieAdded);

        // Act
        aggregate.MarkAsScreened();

        // Assert
        aggregate.EventsToBeStored.Last().ShouldBeOfType<MovieScreened>();
    }


    [Fact]
    public void Should_Fail_When_Increasing_Price_On_an_already_Screened_Movie() => Should.Throw<InvalidOperationException>(() =>
    {
        var aggregate = Events.Apply<MovieAggregate>(
            MovieAdded,
            new MovieScreened(TestMovieId)
        );
        aggregate.IncreaseTicketPrice(600);
    });
}