namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.MarkAsScreened;

public static class Decider
{
    public static DeciderResult<MovieEvent> Decide(MovieState.PendingScreening _, Command command) => new ScreeningFinished(command.MovieId);
}