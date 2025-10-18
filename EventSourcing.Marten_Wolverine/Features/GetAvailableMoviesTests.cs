using EventSourcing.Marten_Wolverine.Events;
using EventSourcing.Marten_Wolverine.Testing;
using Shouldly;

namespace EventSourcing.Marten_Wolverine.Features;

public class GetAvailableMoviesTests
{
    [Trait("Category", "Integration")]
    [Collection("IntegrationTests")]
    public class IntegrationTests(CustomWebApplicationFactory factory)
    {
        [Fact]
        public async Task Test_Projection()
        {
            var movieId1 = Guid.NewGuid();
            var movieId2 = Guid.NewGuid();
            DateTimeOffset displayTime = DateTimeOffset.Now;

            MovieEvent[] movie1Events =
            [
                new MovieAdded(movieId1, "Lord of the Rings", 50, displayTime, TicketPrice.From(15).GetValueOrThrow()),
                new TicketPriceIncreased(movieId1, TicketPrice.From(5.0).GetValueOrThrow()), // price should now be 20
                new TicketPriceDecreased(movieId1, TicketPrice.From(2.0).GetValueOrThrow()), // price should now be 18
            ];

            MovieEvent[] movie2Events =
            [
                new MovieAdded(movieId2, "Inception", 100, displayTime, TicketPrice.From(100).GetValueOrThrow()),
            ];

            await using var scope = await factory.Start();

            // Given
            await scope.Given(movieId1, movie1Events);
            await scope.Given(movieId2, movie2Events);

            // When
            var response = await factory.CreateClient().GetAsync($"/api/movies/");

            // Then
            await response.Verify();
        }

        [Fact]
        public async Task Should_Be_Deleted_When_Movie_Is_Screened()
        {
            var movieId = Guid.NewGuid();
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();

            MovieEvent[] events =
            [
                new MovieAdded(movieId, title, numberOfSeats, displayTime, ticketPriceWhenAdded),
                new ScreeningFinished(movieId)
            ];

            // configure Marten and database
            await using var scope = await factory.Start();

            // insert the events to the event store (triggers the projection) 
            await scope.Given(movieId, events);


            var response = await factory.CreateClient().GetAsync($"/api/movies/");
            
            await response.Verify();
        }
    }
}