using Wolverine.Http;
using Wolverine.Http.Marten;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.MarkAsScreened;

public static class Endpoint
{
    public static (IResult, MovieState.PendingScreening) Validate([Aggregate] MovieState state)
    {
        if (state is MovieState.PendingScreening c) return (WolverineContinue.Result(), c);
        return (Results.ValidationProblem([], "Bad state"), null!);
    }


    [WolverinePost("/movies/{id}/screening-finished")]
    public static ArchiveStream Handle(Guid id, MovieState.PendingScreening state)
    {
        var command = new Command(id);
        return Decider.Decide(state, command)
            .Match(
                @event => new ArchiveStream(id, "Because i said so", @event.Event), // tells marten to archive the stream (no further events can be added to it)
                error => throw new InvalidOperationException($"Cannot mark screening as finished: {error.Error.Message}")
            );
    }
}