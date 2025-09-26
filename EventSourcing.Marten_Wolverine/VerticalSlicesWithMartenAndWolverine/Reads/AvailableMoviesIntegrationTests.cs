// using JasperFx.Events.Projections;
// using Marten;
// using Shouldly;
// using WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie;
// using Xunit;
//
// namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Reads.GetAvailableMovies;
//
// public class AvailableMoviesTests(ProjectionFixture fixture) : IClassFixture<ProjectionFixture>
// {
//     [Fact]
//     public async Task Test_Projection()
//     {
//         var movieId = Guid.NewGuid();
//         var displayTime = DateTimeOffset.Now;
//         const string title = "Inception";
//         const int numberOfSeats = 100;
//
//         MovieEvent[] events =
//         [
//             new MovieAdded(movieId, title, numberOfSeats, displayTime, 15.0),
//             new TicketPriceIncreased(movieId, 5.0), // price should now be 20
//             new TicketPriceDecreased(movieId, 2.0) // price should now be 18
//         ];
//
//         // configure Marten and database
//         await using var store = await fixture.Start(Configuration);
//
//         // insert the events to the event store (triggers the projection) 
//         await using var session = store.LightweightSession();
//         session.Events.StartStream<MovieState>(movieId, events);
//         await session.SaveChangesAsync();
//
//         // load the projection from the database
//         var readModel = await session.LoadAsync<ReadModel>(movieId);
//
//         readModel.ShouldBe(
//             new ReadModel(
//                 Id: movieId,
//                 Title: title,
//                 NumberOfAvailableSeats: numberOfSeats,
//                 DisplayTime: displayTime,
//                 TicketPrice: 18.0)
//         );
//     }
//
//     [Fact]
//     public async Task Should_Be_Deleted_When_Movie_Is_Screened()
//     {
//         var movieId = Guid.NewGuid();
//         var displayTime = DateTimeOffset.Now;
//         const string title = "Inception";
//         const int numberOfSeats = 100;
//
//         MovieEvent[] events =
//         [
//             new MovieAdded(movieId, title, numberOfSeats, displayTime, 15.0),
//             new ScreeningFinished(movieId)
//         ];
//
//         // configure Marten and database
//         await using var store = await fixture.Start(Configuration);
//
//         // insert the events to the event store (triggers the projection) 
//         await using var session = store.LightweightSession();
//         session.Events.StartStream<MovieState>(movieId, events);
//         await session.SaveChangesAsync();
//
//         // load the projection from the database
//         var readModel = await session.LoadAsync<ReadModel>(movieId);
//
//         readModel.ShouldBeNull();
//     }
//
//     private static void Configuration(StoreOptions options) => options.Projections.Add<Projection>(ProjectionLifecycle.Inline);
// }