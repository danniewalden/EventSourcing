using EventSourcing.Marten_Wolverine.Events;
using EventSourcing.Marten_Wolverine.Testing;
using JasperFx.Events.Projections;
using Marten;
using Shouldly;

namespace EventSourcing.Marten_Wolverine.Features;

public class GetAvailableMoviesTests
{
    [Trait("Category", "Integration")]
    public class IntegrationTests(MartenFixture fixture) : IClassFixture<MartenFixture>
    {
        [Fact]
        public async Task Test_Projection()
        {
            var movieId = Guid.NewGuid();
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();
            var increasedTicketPrice = TicketPrice.From(5.0).GetValueOrThrow();
            var decreasedTicketPrice = TicketPrice.From(2.0).GetValueOrThrow();

            MovieEvent[] events =
            [
                new MovieAdded(movieId, title, numberOfSeats, displayTime, ticketPriceWhenAdded),
                new TicketPriceIncreased(movieId, increasedTicketPrice), // price should now be 20
                new TicketPriceDecreased(movieId, decreasedTicketPrice) // price should now be 18
            ];

            // configure Marten and database
            await using var scope = await fixture.Start(Configuration);

            // insert the events to the event store (triggers the projection) 
            await scope.Given(movieId, events);

            // load the projection from the database
            await scope.Then(async session =>
            {
                var readModel = await session.LoadAsync<GetAvailableMovies.Response>(movieId);

                readModel.ShouldBe(
                    new GetAvailableMovies.Response(
                        movieId,
                        Title: title,
                        NumberOfAvailableSeats: numberOfSeats,
                        TicketPrice: 18.0)
                );
            });
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
            await using var scope = await fixture.Start(Configuration);

            // insert the events to the event store (triggers the projection) 
            await scope.Given(movieId, events);

            // load the projection from the database
            await scope.Then(async session =>
            {
                var readModel = await session.LoadAsync<GetAvailableMovies.Response>(movieId);

                readModel.ShouldBeNull();
            });
        }

        private static void Configuration(StoreOptions options)
        {
            options.Projections.Add<GetAvailableMoviesProjection>(ProjectionLifecycle.Inline);
            options.Schema.For<GetAvailableMovies.Response>().DocumentAlias("read_available_movies").Identity(p => p.MovieId);
        }
    }
}