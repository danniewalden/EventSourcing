using EventSourcing.Functional.Seat;

namespace EventSourcing.Functional.GetSeatSelection;

public record Seat(int SeatNumber, Guid MovieId, bool IsAvailable, double Price)
{
    public static Seat Initial => new(0, Guid.Empty, false, 0);
};

public static class SeatProjection
{
    public static Seat Apply(Seat state, object evt) => (state, evt) switch
    {
        (Seat _, SeatAdded added) => new Seat(added.SeatNumber, added.MovieId, true, added.Price),
        (Seat s, SeatReserved _) => s with { IsAvailable = false },
        _ => throw new Exception("We dont know how to handle this"),
    };

    public static Seat Apply(IEnumerable<object> events) => events.Aggregate(new Seat(0, Guid.Empty, false, 0), Apply);
}