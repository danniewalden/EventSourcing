using Marten;
using Wolverine.Http;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Reads.GetAvailableMovies;

public static class Endpoint
{
    [WolverineGet("/movies/available")]
    public static async Task<IReadOnlyCollection<ReadModel>> Handle(IQuerySession session)
    {
        return await session.Query<ReadModel>().ToListAsync();
    }
}