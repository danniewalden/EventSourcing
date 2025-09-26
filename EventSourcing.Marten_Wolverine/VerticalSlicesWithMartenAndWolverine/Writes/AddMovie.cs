using EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes.Movie;
using FluentValidation;
using Wolverine.Http;

namespace EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes;

public static class AddMovie
{
    public record Request(string Title, int NumberOfSeats, DateTimeOffset DisplayTime, double TicketPrice);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.TicketPrice).MustBeValueObject(TicketPrice.Create);
        }
    }

    [WolverinePut("/api/movies")]
    public static (IResult, StartStream) Handle(Request request)
    {
        //This is our decider
        var movieId = Guid.CreateVersion7();
        var movieAdded = new MovieAdded(movieId, request.Title, request.NumberOfSeats, request.DisplayTime, TicketPrice.Create(request.TicketPrice).GetValueOrThrow());
        return (Results.Ok(movieId), new StartStream(movieId, movieAdded));
    }
}