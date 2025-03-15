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
using System.Runtime.InteropServices.Marshalling;

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
{
    public enum SDL_LogCategory
    {
        SDL_LOG_CATEGORY_APPLICATION,
        SDL_LOG_CATEGORY_ERROR,
        SDL_LOG_CATEGORY_ASSERT,
        SDL_LOG_CATEGORY_SYSTEM,
        SDL_LOG_CATEGORY_AUDIO,
        SDL_LOG_CATEGORY_VIDEO,
        SDL_LOG_CATEGORY_RENDER,
        SDL_LOG_CATEGORY_INPUT,
        SDL_LOG_CATEGORY_TEST,
        SDL_LOG_CATEGORY_GPU,
        SDL_LOG_CATEGORY_RESERVED2,
        SDL_LOG_CATEGORY_RESERVED3,
        SDL_LOG_CATEGORY_RESERVED4,
        SDL_LOG_CATEGORY_RESERVED5,
        SDL_LOG_CATEGORY_RESERVED6,
        SDL_LOG_CATEGORY_RESERVED7,
        SDL_LOG_CATEGORY_RESERVED8,
        SDL_LOG_CATEGORY_RESERVED9,
        SDL_LOG_CATEGORY_RESERVED10,
        SDL_LOG_CATEGORY_CUSTOM,
    }

    public enum SDL_LogPriority
    {
        SDL_LOG_PRIORITY_INVALID,
        SDL_LOG_PRIORITY_TRACE,
        SDL_LOG_PRIORITY_VERBOSE,
        SDL_LOG_PRIORITY_DEBUG,
        SDL_LOG_PRIORITY_INFO,
        SDL_LOG_PRIORITY_WARN,
        SDL_LOG_PRIORITY_ERROR,
        SDL_LOG_PRIORITY_CRITICAL,
        SDL_LOG_PRIORITY_COUNT,
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogPriorities))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_SetLogPriorities(SDL_LogPriority priority);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogPriority))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_SetLogPriority(int category, SDL_LogPriority priority);

    public static void SDL_SetLogPriority<TEnum>(TEnum category, SDL_LogPriority priority)
        where TEnum : Enum => INTERNAL_SDL_SetLogPriority(Convert.ToInt32(category), priority);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetLogPriority))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SDL_LogPriority INTERNAL_SDL_GetLogPriority(int category);

    public static SDL_LogPriority SDL_GetLogPriority<TEnum>(TEnum category)
        where TEnum : Enum => INTERNAL_SDL_GetLogPriority(Convert.ToInt32(category));

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
    public static partial bool SDL_SetLogPriorityPrefix(SDL_LogPriority priority, string prefix);

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
    private static partial void INTERNAL_SDL_LogTrace(int category, string message);

    public static void SDL_LogTrace<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogTrace(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogVerbose),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogVerbose(int category, string message);

    public static void SDL_LogVerbose<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogVerbose(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogDebug),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogDebug(int category, string message);

    public static void SDL_LogDebug<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogDebug(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogInfo),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogInfo(int category, string message);

    public static void SDL_LogInfo<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogInfo(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogWarn),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogWarn(int category, string message);

    public static void SDL_LogWarn<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogWarn(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogError),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogError(int category, string message);

    public static void SDL_LogError<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogError(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogCritical),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogCritical(int category, string message);

    public static void SDL_LogCritical<TEnum>(TEnum category, string message)
        where TEnum : Enum => INTERNAL_SDL_LogCritical(Convert.ToInt32(category), message);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LogMessage),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void INTERNAL_SDL_LogMessage(
        int category,
        SDL_LogPriority priority,
        string message
    );

    public static void SDL_LogMessage<TEnum>(
        TEnum category,
        SDL_LogPriority priority,
        string message
    )
        where TEnum : Enum => INTERNAL_SDL_LogMessage(Convert.ToInt32(category), priority, message);

    public delegate void SDL_LogOutputFunction<in TEnum>(
        IntPtr userdata,
        TEnum category,
        SDL_LogPriority priority,
        string message
    )
        where TEnum : Enum;

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetDefaultLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial delegate* unmanaged[Cdecl]<
        IntPtr,
        int,
        SDL_LogPriority,
        byte*,
        void> INTERNAL_SDL_GetDefaultLogOutputFunction();

    public static SDL_LogOutputFunction<TEnum>? SDL_GetDefaultLogOutputFunction<TEnum>()
        where TEnum : Enum
    {
        unsafe
        {
            var result = INTERNAL_SDL_GetDefaultLogOutputFunction();
            return result is null
                ? null
                : (userdata, category, priority, message) =>
                    result(
                        userdata,
                        Convert.ToInt32(category),
                        priority,
                        Utf8StringMarshaller.ConvertToUnmanaged(message)
                    );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial void INTERNAL_SDL_GetLogOutputFunction(
        out delegate* unmanaged[Cdecl]<IntPtr, int, SDL_LogPriority, byte*, void> callback,
        out IntPtr userdata
    );

    public static void SDL_GetLogOutputFunction<TEnum>(
        out SDL_LogOutputFunction<TEnum>? callback,
        out IntPtr userdata
    )
        where TEnum : Enum
    {
        unsafe
        {
            INTERNAL_SDL_GetLogOutputFunction(out var callbackOut, out var userdataOut);
            callback = callbackOut is null
                ? null
                : (userdataPtr, category, priority, message) =>
                    callbackOut(
                        userdataPtr,
                        Convert.ToInt32(category),
                        priority,
                        Utf8StringMarshaller.ConvertToUnmanaged(message)
                    );
            userdata = userdataOut;
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetLogOutputFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial void INTERNAL_SDL_SetLogOutputFunction(
        delegate* unmanaged[Cdecl]<IntPtr, int, SDL_LogPriority, byte*, void> callback,
        IntPtr userdata
    );

    public static void SDL_SetLogOutputFunction<TEnum>(
        SDL_LogOutputFunction<TEnum>? callback,
        IntPtr userdata
    )
        where TEnum : Enum
    {
        unsafe
        {
            INTERNAL_SDL_SetLogOutputFunction(
                callback is null
                    ? null
                    : (delegate* unmanaged[Cdecl]<IntPtr, int, SDL_LogPriority, byte*, void>)
                        Marshal.GetFunctionPointerForDelegate(
                            void (
                                IntPtr userdataCallback,
                                int categoryCallback,
                                SDL_LogPriority priorityCallback,
                                byte* messageCallback
                            ) =>
                                callback(
                                    userdataCallback,
                                    (TEnum)Enum.ToObject(typeof(TEnum), categoryCallback),
                                    priorityCallback,
                                    Utf8StringMarshaller.ConvertToManaged(messageCallback)
                                        ?? string.Empty
                                )
                        ),
                userdata
            );
        }
    }
}
