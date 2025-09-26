using Shared;

namespace EventSourcing.Functional.Movie;

public record TicketPrice
{
    public double Amount { get; }

    private TicketPrice(double amount) => Amount = amount;

    public static TypeResult<TicketPrice> Create(double amount) =>
        amount switch
        {
            // should probably return a result instead of throwing exception... But i am in a rush
            < 0 => "Ticket price cannot be negative",
            > 500 => "Ticket price exceeds our policy of maximum 500$ per ticket",
            _ => new TicketPrice(amount)
        };
    
    public static implicit operator double(TicketPrice ticketPrice) => ticketPrice.Amount;
    public static TypeResult<TicketPrice> operator +(TicketPrice left, TicketPrice right) => Create(left.Amount + right.Amount);
    public static TypeResult<TicketPrice> operator -(TicketPrice left, TicketPrice right) => Create(left.Amount - right.Amount);
    public override string ToString() => $"${Amount:F2}";
}