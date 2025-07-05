using Dunet;

namespace WebApplication1.Functional.Writes.Movie;

// State used to make decisions. (This is actually a projection)


[Union]
public partial record MovieState
{
    public partial record Initial;

    public partial record Screened;

    public partial record PendingScreening(Guid Id, TicketPrice TicketPrice);
}