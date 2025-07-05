using Dunet;

namespace WebApplication1;

[Union]
public partial record DeciderResult<T>
{
    public partial record Success(T Event);

    public partial record Failure(Error Error);
}
[Union]
public partial record TypeResult<T>
{
    public partial record Success(T Type);

    public partial record Failure(Error Error);
}

public record Error(string Message)
{
    public static implicit operator string(Error error) => error.Message;
    public static implicit operator Error(string error) => new(error);
}