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

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Open.CommandAndConquer.Sdl3;

public static partial class SDL3
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

    public delegate IntPtr SDL_ClipboardDataCallback(
        IntPtr userdata,
        string mime_type,
        out CULong size
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SDL_ClipboardCleanupCallback(IntPtr userdata);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetClipboardData),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_SetClipboardData(
        delegate* unmanaged[Cdecl]<IntPtr, byte*, nuint*, IntPtr> callback,
        SDL_ClipboardCleanupCallback cleanup,
        IntPtr userdata,
        byte** mime_types,
        CULong num_mime_types
    );

    public static bool SDL_SetClipboardData(
        SDL_ClipboardDataCallback callback,
        SDL_ClipboardCleanupCallback cleanup,
        IntPtr userdata,
        ICollection<string> mime_types
    )
    {
        unsafe
        {
            var mimeTypesArray = new byte*[mime_types.Count];
            for (var i = 0; i < mime_types.Count; i++)
            {
                mimeTypesArray[i] = Utf8StringMarshaller.ConvertToUnmanaged(
                    mime_types.ElementAt(i)
                );
            }

            fixed (byte** mimeTypesArrayPtr = mimeTypesArray)
            {
                return INTERNAL_SDL_SetClipboardData(
                    (delegate* unmanaged[Cdecl]<IntPtr, byte*, nuint*, IntPtr>)
                        Marshal.GetFunctionPointerForDelegate(
                            IntPtr (
                                IntPtr userdataCallback,
                                byte* mimeTypeCallback,
                                nuint* sizeCallback
                            ) =>
                                callback(
                                    userdataCallback,
                                    Utf8StringMarshaller.ConvertToManaged(mimeTypeCallback)
                                        ?? string.Empty,
                                    out var _
                                )
                        ),
                    cleanup,
                    userdata,
                    mimeTypesArrayPtr,
                    new CULong((nuint)mime_types.Count)
                );
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ClearClipboardData))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ClearClipboardData();

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetClipboardData),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_ClipboardData SDL_GetClipboardData(string mime_type, out CULong size);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_HasClipboardData),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasClipboardData(string mime_type);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetClipboardMimeTypes))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial byte** INTERNAL_SDL_GetClipboardMimeTypes(
        out CULong num_mime_types
    );

    public static ICollection<string>? SDL_GetClipboardMimeTypes()
    {
        unsafe
        {
            var result = INTERNAL_SDL_GetClipboardMimeTypes(out var numMimeTypes);
            if (result is null)
            {
                return null;
            }

            var array = ArrayPool<string>.Shared.Rent((int)numMimeTypes.Value);
            try
            {
                for (var i = 0UL; i < numMimeTypes.Value; i++)
                {
                    array[i] = Utf8StringMarshaller.ConvertToManaged(result[i]) ?? string.Empty;
                }

                return array;
            }
            finally
            {
                ArrayPool<string>.Shared.Return(array);
                SDL_free(result);
            }
        }
    }
}
