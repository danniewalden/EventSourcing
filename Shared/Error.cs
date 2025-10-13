namespace Shared;

public record Error(string Message)
{
    public static implicit operator string(Error error) => error.Message;
    public static implicit operator Error(string error) => new(error);
}