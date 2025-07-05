using Shouldly;
using Xunit;

namespace WebApplication1.Oop.Writes.Movie;

public class MovieAggregateTests
{
    private static readonly Guid TestMovieId = Guid.NewGuid();
    private static readonly Aggregate Existing = Aggregate.Create(TestMovieId, "Inception", 100, DateTimeOffset.Now, 15.0);

    [Fact]
    public void Should_Create_Movie_From_Initial_State()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        
        // Act
        var aggregate = Aggregate.Create(TestMovieId, "Inception", 100, now, 15.0);

        // Assert
        aggregate.EventsToBeStored.ShouldHaveSingleItem().ShouldBe(new MovieAdded(TestMovieId, "Inception", 100, now, 15.0));
    }
    
    [Fact]
    public void Should_Increase_Ticket_Price()
    {
        // Act
        Existing.IncreaseTicketPrice(20);

        // Assert
        Existing.EventsToBeStored.Last().ShouldBeOfType<TicketPriceIncreased>().Amount.ShouldBe(20);
    }

    [Fact]
    public void Should_Fail_When_Increasing_Price_Violates_Max_price_of_500() => Should.Throw<InvalidOperationException>(() => Existing.IncreaseTicketPrice(600));

    [Fact]
    public void Should_Decrease_Ticket_Price()
    {
        // Arrange
        Existing.DecreaseTicketPrice(15);

        // Assert
        Existing.EventsToBeStored.Last().ShouldBeOfType<TicketPriceDecreased>().Amount.ShouldBe(15);
    }
}