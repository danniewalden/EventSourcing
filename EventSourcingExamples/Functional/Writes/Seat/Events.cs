namespace WebApplication1.Functional.Writes.Seat;

public record SeatEvent;
public record SeatReserved(int SeatNumber, Guid MovieId, double Price, Guid ReservationId): SeatEvent;
public record SeatAdded(int SeatNumber, Guid MovieId, double Price): SeatEvent;
