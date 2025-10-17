using EventSourcing.Marten_Wolverine.Events;
using EventSourcing.Marten_Wolverine.Plumbing;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine.Http;

namespace EventSourcing.Marten_Wolverine.Features;

public static class AddMovie
{
    public record Request(string Title, int NumberOfSeats, DateTimeOffset DisplayTime, double TicketPrice);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.TicketPrice).MustBeValueObject(TicketPrice.From);
        }
    }

    [WolverinePut("/api/movies")]
    public static (Ok<CreateResponse>, StartStream) Handle(Request request)
    {
        //This is our decider
        var movieId = Guid.CreateVersion7();
        var movieAdded = new MovieAdded(movieId, request.Title, request.NumberOfSeats, request.DisplayTime, TicketPrice.From(request.TicketPrice).GetValueOrThrow());
        return (TypedResults.Ok(new CreateResponse(movieId)), new StartStream(movieId, movieAdded));
    }
}

