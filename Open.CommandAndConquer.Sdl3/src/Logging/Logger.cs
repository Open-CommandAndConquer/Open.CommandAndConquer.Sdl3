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

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Open.CommandAndConquer.Sdl3.Exceptions;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3.Logging;

public static class Logger
{
    public static void SetPriorities(LogPriority priority) => SDL_SetLogPriorities(priority);

    public static void SetPriority<TEnum>(TEnum category, LogPriority priority)
        where TEnum : Enum => SDL_SetLogPriority(Convert.ToInt32(category), priority);

    public static LogPriority GetPriority(LogCategory category) =>
        SDL_GetLogPriority(Convert.ToInt32(category));

    public static void ResetPriorities() => SDL_ResetLogPriorities();

    public static void SetPrefix(LogPriority priority, string prefix)
    {
        if (!SDL_SetLogPriorityPrefix(priority, prefix))
        {
            throw new LogException(SDL_GetError());
        }
    }

    public static void Log(string message) => SDL_Log(message);

    public static void LogTrace<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogTrace(Convert.ToInt32(category), message);

    public static void LogVerbose<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogVerbose(Convert.ToInt32(category), message);

    public static void LogDebug<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogDebug(Convert.ToInt32(category), message);

    public static void LogInfo<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogInfo(Convert.ToInt32(category), message);

    public static void LogWarn<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogWarn(Convert.ToInt32(category), message);

    public static void LogError<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogError(Convert.ToInt32(category), message);

    public static void LogCritical<TEnum>(TEnum category, string message)
        where TEnum : Enum => SDL_LogCritical(Convert.ToInt32(category), message);

    public static void LogMessage<TEnum>(TEnum category, LogPriority priority, string message)
        where TEnum : Enum => SDL_LogMessage(Convert.ToInt32(category), priority, message);

    public static Action<TEnum, LogPriority, string> GetDefaultOutputFunction<TEnum>()
        where TEnum : Enum
    {
        unsafe
        {
            var result = SDL_GetDefaultLogOutputFunction();
            return (category, priority, message) =>
                result(
                    IntPtr.Zero,
                    Convert.ToInt32(category),
                    priority,
                    Utf8StringMarshaller.ConvertToUnmanaged(message)
                );
        }
    }

    public static Action<TEnum, LogPriority, string> GetOutputFunction<TEnum>()
        where TEnum : Enum
    {
        unsafe
        {
            SDL_GetLogOutputFunction(out var result, out IntPtr _);
            return (category, priority, message) =>
                result(
                    IntPtr.Zero,
                    Convert.ToInt32(category),
                    priority,
                    Utf8StringMarshaller.ConvertToUnmanaged(message)
                );
        }
    }

    public static void SetOutputFunction<TEnum>(Action<TEnum, LogPriority, string> function)
        where TEnum : Enum
    {
        unsafe
        {
            SDL_SetLogOutputFunction(
                (delegate* unmanaged[Cdecl]<IntPtr, int, LogPriority, byte*, void>)
                    Marshal.GetFunctionPointerForDelegate(
                        void (IntPtr _, int category, LogPriority priority, byte* message) =>
                            function(
                                (TEnum)Enum.ToObject(typeof(TEnum), category),
                                priority,
                                Utf8StringMarshaller.ConvertToManaged(message) ?? string.Empty
                            )
                    ),
                IntPtr.Zero
            );
        }
    }
}
