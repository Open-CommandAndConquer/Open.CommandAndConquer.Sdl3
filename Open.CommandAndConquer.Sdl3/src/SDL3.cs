using System.Runtime.InteropServices;

namespace Open.CommandAndConquer.Sdl3;

public static partial class SDL3
{
    private static KeyValuePair<string, bool>[] LibraryNames =>
        [
            new("libSDL3.dll", OperatingSystem.IsWindows()),
            new("libSDL3.so.0", OperatingSystem.IsLinux()),
        ];

    static SDL3() =>
        NativeLibrary.SetDllImportResolver(
            typeof(SDL3).Assembly,
            (name, assembly, path) =>
                NativeLibrary.Load(
                    name switch
                    {
                        nameof(SDL3) => LibraryNames.FirstOrDefault(pair => pair.Value).Key ?? name,
                        _ => name,
                    },
                    assembly,
                    path
                )
        );
}
