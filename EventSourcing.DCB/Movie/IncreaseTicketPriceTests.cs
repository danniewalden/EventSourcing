using EventSourcing.DCB.Events;
using Shared;
using Shouldly;

namespace EventSourcing.DCB.Movie;

public class IncreaseTicketPriceTests
{
    private static readonly TicketPrice Price = TicketPrice.From(10).GetValueOrThrow();
    private static readonly Guid TestMovieId = Guid.NewGuid();

    [Fact]
    public void Should_Increase_Price_Correctly()
    {
        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, Price)
        ];

        var currentState = IncreaseTicketPriceState.Apply(events);
        var command = new IncreaseTicketPrice(TestMovieId, Price);

        // When
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldSucceed<TicketPriceIncreased>()
            .Amount.ShouldBe(Price);
    }

    [Fact]
    public void Should_Fail_When_New_Ticket_Price_Violates_The_Max_Ticket_Price_1()
    {
        var currentPrice = TicketPrice.From(300).GetValueOrThrow();
        var priceToIncrease = TicketPrice.From(201).GetValueOrThrow();

        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, currentPrice)
        ];

        var currentState = IncreaseTicketPriceState.Apply(events);
        var command = new IncreaseTicketPrice(TestMovieId, priceToIncrease);

        // When
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldFail().Message.ShouldBe("Ticket price exceeds our policy of maximum 500 per ticket");
    }

    [Fact]
    public void Should_Fail_When_New_Ticket_Price_Violates_The_Max_Ticket_Price_2()
    {
        var priceWhenAdded = TicketPrice.From(100).GetValueOrThrow();
        var priceIncrease = TicketPrice.From(300).GetValueOrThrow();
        var priceToIncrease = TicketPrice.From(201).GetValueOrThrow();

        // Given
        MovieEvent[] events =
        [
            new MovieAdded(TestMovieId, "Inception", 100, DateTimeOffset.Now, priceWhenAdded),
            new TicketPriceIncreased(TestMovieId, priceIncrease)
        ];

        var currentState = IncreaseTicketPriceState.Apply(events);
        var command = new IncreaseTicketPrice(TestMovieId, priceToIncrease);

        // When
        var result = IncreaseTicketPrice.Decide(currentState, command);

        // Then
        result.ShouldFail().Message.ShouldBe("Ticket price exceeds our policy of maximum 500 per ticket");
    }
}