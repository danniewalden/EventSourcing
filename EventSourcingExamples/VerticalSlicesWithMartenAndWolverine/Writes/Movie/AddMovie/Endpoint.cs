using FluentValidation;
using Wolverine.Http;
using Wolverine.Marten;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.AddMovie;

public static class Endpoint
{
    public record Request(string Title, int NumberOfSeats, DateTimeOffset DisplayTime, double TicketPrice);

    public record Response(Guid Id);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Title).MinimumLength(2);
            RuleFor(x => x.NumberOfSeats).GreaterThan(0);
            RuleFor(x => x.DisplayTime).BeInTheFuture();
            RuleFor(x => x.TicketPrice).MustBeValueObject(TicketPrice.Create);
        }
    }

    [WolverinePost("/movies")]
    public static (Response, IStartStream) Handle(Request request)
    {
        var id = Guid.NewGuid();
        var command = new Command(id, request.Title, request.NumberOfSeats, request.DisplayTime, request.TicketPrice);
        return Decider.Decide(command)
            .Match(
                @event => (new Response(id), MartenOps.StartStream<MovieState>(id, @event.Event)),
                error => throw new InvalidOperationException($"Cannot add movie: {error.Error.Message}")
            );
    }
}