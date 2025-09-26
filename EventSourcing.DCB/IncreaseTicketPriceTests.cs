using EventSourcing.DCB.Events;
using Shouldly;
using WebApplication1;

namespace EventSourcing.DCB;

public class IncreaseTicketPriceTests
{
    private static readonly double Price = 10;
    private static readonly Guid TestMovieId = Guid.NewGuid();

    [Fact]
    public void Should_Increase_Price_Correctly()
    {
        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, 15.0)
        ];

        var command = new IncreaseTicketPrice(TestMovieId, 15.0);

        // When
        var currentState = IncreaseTicketPriceState.Apply(events);
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldSucceed()
            .ShouldBeOfType<TicketPriceIncreased>()
            .Amount.ShouldBe(15);
    }

    [Fact]
    public void Should_Fail_When_New_Ticket_Price_Violates_The_Max_Ticket_Price_1()
    {
        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, 300.0)
        ];

        var command = new IncreaseTicketPrice(TestMovieId, 201);

        // When
        var currentState = IncreaseTicketPriceState.Apply(events);
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldFail().Message.ShouldBe("The ticket price policy of max 500 is violated");
    }
    
    [Fact]
    public void Should_Fail_When_New_Ticket_Price_Violates_The_Max_Ticket_Price_2()
    {
        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, 100.0),
            new TicketPriceIncreased(TestMovieId, 300)
        ];

        var command = new IncreaseTicketPrice(TestMovieId, 200);

        // When
        var currentState = IncreaseTicketPriceState.Apply(events);
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldFail().Message.ShouldBe("The ticket price policy of max 500 is violated");
    }
}