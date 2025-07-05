using Shouldly;

namespace WebApplication1;

public static class DeciderResultExtensions
{
    public static T ShouldSucceed<T>(this DeciderResult<T> result)
    {
        result.ShouldBeOfType<WebApplication1.DeciderResult<T>.Success>("Expected successful result but got failure");
        return ((WebApplication1.DeciderResult<T>.Success)result).Event;
    }

    public static Error ShouldFail<T>(this WebApplication1.DeciderResult<T> result)
    {
        result.ShouldBeOfType<WebApplication1.DeciderResult<T>.Failure>("Expected failure result but got success");
        return result.UnwrapFailure().Error;
    }
}