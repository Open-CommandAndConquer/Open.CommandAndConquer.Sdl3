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

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
{
    public enum SDL_IOStatus
    {
        SDL_IO_STATUS_READY,
        SDL_IO_STATUS_ERROR,
        SDL_IO_STATUS_EOF,
        SDL_IO_STATUS_NOT_READY,
        SDL_IO_STATUS_READONLY,
        SDL_IO_STATUS_WRITEONLY,
    }

    public enum SDL_IOWhence
    {
        SDL_IO_SEEK_SET,
        SDL_IO_SEEK_CUR,
        SDL_IO_SEEK_END,
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<SDL_IOStream>))]
    public sealed class SDL_IOStream : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SDL_IOStream()
            : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true) => SetHandle(IntPtr.Zero);

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            SDL_CloseIO(handle);
            SetHandle(IntPtr.Zero);
            return true;
        }
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_IOFromFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_IOStream SDL_IOFromFile(string file, string mode);

    public const string SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER = "SDL.iostream.windows.handle";
    public const string SDL_PROP_IOSTREAM_STDIO_FILE_POINTER = "SDL.iostream.stdio.file";
    public const string SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER = "SDL.iostream.file_descriptor";
    public const string SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER = "SDL.iostream.android.aasset";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SDL_IOStream INTERNAL_SDL_IOFromMem(
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))]
            byte[] mem,
        CULong size
    );

    public static SDL_IOStream SDL_IOFromMem(Memory<byte> mem) =>
        INTERNAL_SDL_IOFromMem(mem.Span.ToArray(), new CULong((nuint)mem.Length));

    public const string SDL_PROP_IOSTREAM_MEMORY_POINTER = "SDL.iostream.memory.base";
    public const string SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER = "SDL.iostream.memory.size";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromConstMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SDL_IOStream INTERNAL_SDL_IOFromConstMem(
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))]
            byte[] mem,
        CULong size
    );

    public static SDL_IOStream SDL_IOFromConstMem(ReadOnlyMemory<byte> mem) =>
        INTERNAL_SDL_IOFromConstMem(mem.Span.ToArray(), new CULong((nuint)mem.Length));

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromDynamicMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_IOStream SDL_IOFromDynamicMem();

    public const string SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER = "SDL.iostream.dynamic.memory";
    public const string SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER =
        "SDL.iostream.dynamic.chunksize";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CloseIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_CloseIO(IntPtr context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_GetIOProperties(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOStatus))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_IOStatus SDL_GetIOStatus(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOSize))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SDL_GetIOSize(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SeekIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SDL_SeekIO(SDL_IOStream context, long offset, SDL_IOWhence whence);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_TellIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong SDL_TellIO(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial CULong INTERNAL_SDL_ReadIO(
        SDL_IOStream context,
        [Out]
        [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))]
            byte[] buffer,
        CULong size
    );

    public static CULong SDL_ReadIO(SDL_IOStream context, out Span<byte> buffer, CULong size)
    {
        var array = ArrayPool<byte>.Shared.Rent((int)size.Value);
        try
        {
            var result = INTERNAL_SDL_ReadIO(context, array, size);
            buffer = array;
            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial CULong INTERNAL_SDL_WriteIO(
        SDL_IOStream context,
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))]
            byte[] buffer,
        CULong size
    );

    public static CULong SDL_WriteIO(SDL_IOStream context, ReadOnlySpan<byte> buffer) =>
        INTERNAL_SDL_WriteIO(context, buffer.ToArray(), new CULong((nuint)buffer.Length));

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_IOprintf),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong SDL_IOprintf(SDL_IOStream context, string data);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FlushIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_FlushIO(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LoadFile_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial void* INTERNAL_SDL_LoadFile_IO(
        SDL_IOStream src,
        out CULong datasize,
        [MarshalAs(UnmanagedType.I1)] bool closeio
    );

    public static ICollection<byte>? SDL_LoadFile_IO(SDL_IOStream src, bool closeio)
    {
        unsafe
        {
            var result = INTERNAL_SDL_LoadFile_IO(src, out var dataSize, closeio);
            return result is null
                ? null
                : new Span<byte>((byte*)result, (int)dataSize.Value).ToArray();
        }
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LoadFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial void* INTERNAL_SDL_LoadFile(string file, out CULong datasize);

    public static ICollection<byte>? SDL_LoadFile(string file)
    {
        unsafe
        {
            var result = INTERNAL_SDL_LoadFile(file, out var dataSize);
            return result is null
                ? null
                : new Span<byte>((byte*)result, (int)dataSize.Value).ToArray();
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SaveFile_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool INTERNAL_SDL_SaveFile_IO(
        SDL_IOStream src,
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>))] byte[] data,
        CULong datasize,
        [MarshalAs(UnmanagedType.I1)] bool closeio
    );

    public static bool SDL_SaveFile_IO(SDL_IOStream src, ReadOnlySpan<byte> data, bool closeio) =>
        INTERNAL_SDL_SaveFile_IO(src, data.ToArray(), new CULong((nuint)data.Length), closeio);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SaveFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool INTERNAL_SDL_SaveFile(
        string file,
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>))] byte[] data,
        CULong datasize
    );

    public static bool SDL_SaveFile(string file, ReadOnlySpan<byte> data) =>
        INTERNAL_SDL_SaveFile(file, data.ToArray(), new CULong((nuint)data.Length));

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU8(SDL_IOStream context, out byte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS8(SDL_IOStream context, out sbyte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU16LE(SDL_IOStream context, out ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS16LE(SDL_IOStream context, out short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU16BE(SDL_IOStream context, out ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS16BE(SDL_IOStream context, out short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU32LE(SDL_IOStream context, out uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS32LE(SDL_IOStream context, out int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU32BE(SDL_IOStream context, out uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS32BE(SDL_IOStream context, out int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU64LE(SDL_IOStream context, out ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS64LE(SDL_IOStream context, out long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadU64BE(SDL_IOStream context, out ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadS64BE(SDL_IOStream context, out long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU8(SDL_IOStream context, byte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS8(SDL_IOStream context, sbyte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU16LE(SDL_IOStream context, ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS16LE(SDL_IOStream context, short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU16BE(SDL_IOStream context, ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS16BE(SDL_IOStream context, short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU32LE(SDL_IOStream context, uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS32LE(SDL_IOStream context, int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU32BE(SDL_IOStream context, uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS32BE(SDL_IOStream context, int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU64LE(SDL_IOStream context, ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS64LE(SDL_IOStream context, long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteU64BE(SDL_IOStream context, ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteS64BE(SDL_IOStream context, long value);
}
