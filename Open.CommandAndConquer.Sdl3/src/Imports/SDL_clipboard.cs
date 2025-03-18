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
    [NativeMarshalling(typeof(SafeHandleMarshaller<SDL_ClipboardData>))]
    public sealed class SDL_ClipboardData : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SDL_ClipboardData()
            : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true) => SetHandle(nint.Zero);

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            SDL_free(handle);
            SetHandle(IntPtr.Zero);
            return true;
        }
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetClipboardText),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetClipboardText(string text);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetClipboardText),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string SDL_GetClipboardText();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasClipboardText))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasClipboardText();

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetPrimarySelectionText),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetPrimarySelectionText(string text);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPrimarySelectionText),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string SDL_GetPrimarySelectionText();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasPrimarySelectionText))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasPrimarySelectionText();
}
