using WebApplication1.Functional.Writes.Seat;

namespace WebApplication1.Functional.Reads.SeatSelection;

public record Reservation(Guid ReservationId, int SeatNumber, Guid MovieId);

public static class ReservationProjection
{
    public static Reservation Apply(Reservation state, object evt) => (state, evt) switch
    {
        (Reservation _, SeatReserved reserved) => new Reservation(reserved.ReservationId, reserved.SeatNumber, reserved.MovieId),
        _ => state,
    };
}