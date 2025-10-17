using EventSourcing.Marten_Wolverine.Events;
using EventSourcing.Marten_Wolverine.Testing;
using Shouldly;

namespace EventSourcing.Marten_Wolverine.Features;

public static class IncreaseTicketPriceTests
{
    [Trait("Category", "Integration")]
    [Collection("IntegrationTests")]
    public class IntegrationTests(CustomWebApplicationFactory factory)
    {
        [Fact]
        public async Task Should_return_204_when_increased_price()
        {
            var movieId = Guid.NewGuid();
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();
            var increasedTicketPrice = TicketPrice.From(5.0).GetValueOrThrow();

            MovieEvent[] events =
            [
                new MovieAdded(movieId, title, numberOfSeats, displayTime, ticketPriceWhenAdded),
            ];

            await using var scope = factory.Start();

            // Given
            await scope.Given(movieId, events);

            // When
            await scope.When(async () => { _ = await factory.CreateClient().PostAsJsonAsync($"/api/movies/{movieId}/increase-price", new IncreaseTicketPrice.Request(increasedTicketPrice)); });

            // Then
            await scope.Then(async session =>
            {
                var eventsInStream = await session.Events.FetchStreamAsync(movieId);
                eventsInStream.ShouldContain(@event => @event.Data is TicketPriceIncreased);
            });
        }

        [Fact]
        public async Task Should_return_problem_details_when_increased_price_violates_max_price()
        {
            var movieId = Guid.NewGuid();
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();
            var increasedTicketPrice = TicketPrice.From(500).GetValueOrThrow();

            MovieEvent[] events =
            [
                new MovieAdded(movieId, title, numberOfSeats, displayTime, ticketPriceWhenAdded),
            ];

            await using var scope = factory.Start();

            // Given
            await scope.Given(movieId, events);

            // When
            var response = await factory.CreateClient().PostAsJsonAsync($"/api/movies/{movieId}/increase-price", new IncreaseTicketPrice.Request(increasedTicketPrice));

            // Then
            await VerifyJson(response.Content.ReadAsStreamAsync());
        }
    }
}