// The MIT License (MIT)
//
// Copyright (c) 2025 Open.CommandAndConquer, Victor Matia <vmatir@outlook.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Open.CommandAndConquer.Sdl3.Logging;

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
{
    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogPriorities))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_SetLogPriorities(LogPriority priority);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogPriority))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_SetLogPriority(int category, LogPriority priority);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetLogPriority))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LogPriority SDL_GetLogPriority(int category);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ResetLogPriorities))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_ResetLogPriorities();

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetLogPriorityPrefix),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetLogPriorityPrefix(LogPriority priority, string prefix);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_Log),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_Log(string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogTrace),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogTrace(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogVerbose),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogVerbose(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogDebug),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogDebug(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogInfo),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogInfo(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogWarn),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogWarn(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogError),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogError(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogCritical),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogCritical(int category, string message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogMessage),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LogMessage(int category, LogPriority priority, string message);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetDefaultLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial delegate* unmanaged[Cdecl]<
        IntPtr,
        int,
        LogPriority,
        byte*,
        void> SDL_GetDefaultLogOutputFunction();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_GetLogOutputFunction(
        out delegate* unmanaged[Cdecl]<IntPtr, int, LogPriority, byte*, void> callback,
        out IntPtr userdata
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_SetLogOutputFunction(
        delegate* unmanaged[Cdecl]<IntPtr, int, LogPriority, byte*, void> callback,
        IntPtr userdata
    );
}
