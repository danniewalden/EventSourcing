using FluentValidation;
using Wolverine.Http;
using Wolverine.Http.Marten;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.IncreateTicketPrice;

public static class Endpoint
{
    public record Request(double Amount);


    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Amount).MustBeValueObject(TicketPrice.Create);
        }
    }

    public static (IResult, MovieState.PendingScreening) Validate(Guid id, [Aggregate] MovieState state)
    {
        if (state is MovieState.PendingScreening c) return (WolverineContinue.Result(), c);
        return (Results.ValidationProblem([], "Bad state"), null!);
    }

    [WolverinePost("/movies/{id}/increase-price")]
    public static (IResult, AppendToStream) Handle(Request request, MovieState.PendingScreening state)
    {
        var command = new Command(request.Amount);
        return Decider.Decide(state, command)
            .Match(
                success => (Results.NoContent(), new AppendToStream(state.Id, success.Event)),
                failure => (Results.BadRequest(failure.Error.Message), AppendToStream.NoOp));
    }
}