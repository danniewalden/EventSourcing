using Dunet;

namespace Shared;

[Union]
public partial record DeciderResult<T>
{
    public partial record Success(T Event);

    public partial record Failure(Error Error);
    
    public static implicit operator DeciderResult<T>(string error) => new Failure(error);
}