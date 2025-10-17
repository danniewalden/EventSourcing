// using Dunet;
// using EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes.Movie;
// using Wolverine.Http;
// using Wolverine.Marten;
//
// namespace EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes;
//
// public static class MarkAsScreened
// {
//     public static (IResult, MarkAsScreenedState.PendingScreening) Validate([WriteAggregate] MarkAsScreenedState state)
//     {
//         // if (state is State.PendingScreening c) return (WolverineContinue.Result(), c);
//         return (Results.ValidationProblem([], "Bad state"), null!);
//     }
//
//
//     [WolverinePost("/movies/{id:guid}/screening-finished")]
//     public static (IResult, ArchiveStream) Handle(Guid id, MarkAsScreenedState.PendingScreening state)
//     {
//         var evt = new ScreeningFinished(id);
//         return (Results.NoContent(), new ArchiveStream(id, "Because i said so", evt));
//     }
//
// }
//
// [Union]
// public partial record MarkAsScreenedState
// {
//     public partial record Initial;
//
//     public partial record Other;
//
//     public partial record PendingScreening;
//
//     // public static State Create(MovieAdded @event) => new PendingScreening();
// }
