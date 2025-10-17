using System.Runtime.CompilerServices;

namespace EventSourcing.Marten_Wolverine.Testing;

public static class VerifyInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // All snapshots go into a specific folder under the project root
        DerivePathInfo((sourceFile, projectDirectory, type, method) =>
        {
            var dir = Path.Combine(projectDirectory, "Testing", "Snapshots");

            return new PathInfo(
                directory: dir,
                typeName: type.Name,
                methodName: method.Name);
        });
    }
}