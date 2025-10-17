using EventSourcing.Marten_Wolverine.Plumbing;
using EventSourcing.Marten_Wolverine.Testing;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;

namespace EventSourcing.Marten_Wolverine.Features;

public class AddMovieTests
{
    [Trait("Category", "Integration")]
    [Collection("IntegrationTests")]
    public class IntegrationTests(CustomWebApplicationFactory factory)
    {
        [Fact]
        public async Task Success_should_return_http_200()
        {
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();

            var response = await factory.CreateClient().PutAsJsonAsync("/api/movies", new AddMovie.Request(title, numberOfSeats, displayTime, ticketPriceWhenAdded));

            await response.Verify();
        }
    }

    public class UnitTests
    {
        [Fact]
        public void Success_should_return_OkResult()
        {
            var displayTime = DateTimeOffset.Now;
            const string title = "Inception";
            const int numberOfSeats = 100;
            var ticketPriceWhenAdded = TicketPrice.From(15).GetValueOrThrow();

            var (result, streamAction) = AddMovie.Handle(new AddMovie.Request(title, numberOfSeats, displayTime, ticketPriceWhenAdded));

            result.ShouldBeOfType<Ok<CreateResponse>>().Value.Id.ShouldNotBe(Guid.Empty);
        }
    }
}