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
using Open.CommandAndConquer.Sdl3.CustomMarshalling;

namespace Open.CommandAndConquer.Sdl3;

public static partial class SDL3
{
    public const byte SDL_ALPHA_OPAQUE = 255;
    public const float SDL_ALPHA_OPAQUE_FLOAT = 1F;
    public const byte SDL_ALPHA_TRANSPARENT = 0;
    public const float SDL_ALPHA_TRANSPARENT_FLOAT = 0F;

    public enum SDL_PixelType : uint
    {
        SDL_PIXELTYPE_UNKNOWN,
        SDL_PIXELTYPE_INDEX1,
        SDL_PIXELTYPE_INDEX4,
        SDL_PIXELTYPE_INDEX8,
        SDL_PIXELTYPE_PACKED8,
        SDL_PIXELTYPE_PACKED16,
        SDL_PIXELTYPE_PACKED32,
        SDL_PIXELTYPE_ARRAYU8,
        SDL_PIXELTYPE_ARRAYU16,
        SDL_PIXELTYPE_ARRAYU32,
        SDL_PIXELTYPE_ARRAYF16,
        SDL_PIXELTYPE_ARRAYF32,
        SDL_PIXELTYPE_INDEX2,
    }

    public enum SDL_BitmapOrder : uint
    {
        SDL_BITMAPORDER_NONE,
        SDL_BITMAPORDER_4321,
        SDL_BITMAPORDER_1234,
    }

    public enum SDL_PackedOrder : uint
    {
        SDL_PACKEDORDER_NONE,
        SDL_PACKEDORDER_XRGB,
        SDL_PACKEDORDER_RGBX,
        SDL_PACKEDORDER_ARGB,
        SDL_PACKEDORDER_RGBA,
        SDL_PACKEDORDER_XBGR,
        SDL_PACKEDORDER_BGRX,
        SDL_PACKEDORDER_ABGR,
        SDL_PACKEDORDER_BGRA,
    }

    public enum SDL_ArrayOrder : uint
    {
        SDL_ARRAYORDER_NONE,
        SDL_ARRAYORDER_RGB,
        SDL_ARRAYORDER_RGBA,
        SDL_ARRAYORDER_ARGB,
        SDL_ARRAYORDER_BGR,
        SDL_ARRAYORDER_BGRA,
        SDL_ARRAYORDER_ABGR,
    }

    public enum SDL_PackedLayout : uint
    {
        SDL_PACKEDLAYOUT_NONE,
        SDL_PACKEDLAYOUT_332,
        SDL_PACKEDLAYOUT_4444,
        SDL_PACKEDLAYOUT_1555,
        SDL_PACKEDLAYOUT_5551,
        SDL_PACKEDLAYOUT_565,
        SDL_PACKEDLAYOUT_8888,
        SDL_PACKEDLAYOUT_2101010,
        SDL_PACKEDLAYOUT_1010102,
    }

    public static SDL_PixelFormat SDL_DEFINE_PIXELFOURCC(char A, char B, char C, char D) =>
        (SDL_PixelFormat)SDL_FOURCC(A, B, C, D);

    private static SDL_PixelFormat SDL_DEFINE_PIXELFORMAT(
        SDL_PixelType type,
        uint order,
        SDL_PackedLayout layout,
        byte bits,
        byte bytes
    ) =>
        (SDL_PixelFormat)(
            1 << 28
            | (uint)type << 24
            | order << 20
            | (uint)layout << 16
            | (uint)(bits << 8)
            | bytes
        );

    public static SDL_PixelFormat SDL_DEFINE_PIXELFORMAT(
        SDL_PixelType type,
        SDL_BitmapOrder order,
        SDL_PackedLayout layout,
        byte bits,
        byte bytes
    ) => SDL_DEFINE_PIXELFORMAT(type, (uint)order, layout, bits, bytes);

    public static SDL_PixelFormat SDL_DEFINE_PIXELFORMAT(
        SDL_PixelType type,
        SDL_PackedOrder order,
        SDL_PackedLayout layout,
        byte bits,
        byte bytes
    ) => SDL_DEFINE_PIXELFORMAT(type, (uint)order, layout, bits, bytes);

    public static SDL_PixelFormat SDL_DEFINE_PIXELFORMAT(
        SDL_PixelType type,
        SDL_ArrayOrder order,
        SDL_PackedLayout layout,
        byte bits,
        byte bytes
    ) => SDL_DEFINE_PIXELFORMAT(type, (uint)order, layout, bits, bytes);

    public static uint SDL_PIXELFLAG(SDL_PixelFormat format) => (uint)format >> 28 & 0x0F;

    public static SDL_PixelType SDL_PIXELTYPE(SDL_PixelFormat format) =>
        (SDL_PixelType)((uint)format >> 24 & 0x0F);

    private static uint SDL_PIXELORDER(SDL_PixelFormat format) => (uint)format >> 20 & 0x0F;

    public static SDL_BitmapOrder SDL_PIXELORDER_BITMAP(SDL_PixelFormat format) =>
        (SDL_BitmapOrder)SDL_PIXELORDER(format);

    public static SDL_PackedOrder SDL_PIXELORDER_PACKED(SDL_PixelFormat format) =>
        (SDL_PackedOrder)SDL_PIXELORDER(format);

    public static SDL_ArrayOrder SDL_PIXELORDER_ARRAY(SDL_PixelFormat format) =>
        (SDL_ArrayOrder)SDL_PIXELORDER(format);

    public static SDL_PackedLayout SDL_PIXELLAYOUT(SDL_PixelFormat format) =>
        (SDL_PackedLayout)((uint)format >> 16 & 0x0F);

    public static byte SDL_BITSPERPIXEL(SDL_PixelFormat format) =>
        (byte)(SDL_ISPIXELFORMAT_FOURCC(format) ? 0 : (uint)format >> 8 & 0xFF);

    public static byte SDL_BYTESPERPIXEL(SDL_PixelFormat format)
    {
        if (SDL_ISPIXELFORMAT_FOURCC(format))
        {
            return (byte)(
                format
                    is SDL_PixelFormat.SDL_PIXELFORMAT_YUY2
                        or SDL_PixelFormat.SDL_PIXELFORMAT_UYVY
                        or SDL_PixelFormat.SDL_PIXELFORMAT_YVYU
                        or SDL_PixelFormat.SDL_PIXELFORMAT_P010
                    ? 2
                    : 1
            );
        }

        return (byte)((uint)format & 0xFF);
    }

    public static bool SDL_ISPIXELFORMAT_INDEXED(SDL_PixelFormat format) =>
        !SDL_ISPIXELFORMAT_FOURCC(format)
        && SDL_PIXELTYPE(format)
            is SDL_PixelType.SDL_PIXELTYPE_INDEX1
                or SDL_PixelType.SDL_PIXELTYPE_INDEX2
                or SDL_PixelType.SDL_PIXELTYPE_INDEX4
                or SDL_PixelType.SDL_PIXELTYPE_INDEX8;

    public static bool SDL_ISPIXELFORMAT_PACKED(SDL_PixelFormat format) =>
        !SDL_ISPIXELFORMAT_FOURCC(format)
        && SDL_PIXELTYPE(format)
            is SDL_PixelType.SDL_PIXELTYPE_PACKED8
                or SDL_PixelType.SDL_PIXELTYPE_PACKED16
                or SDL_PixelType.SDL_PIXELTYPE_PACKED32;

    public static bool SDL_ISPIXELFORMAT_ARRAY(SDL_PixelFormat format) =>
        !SDL_ISPIXELFORMAT_FOURCC(format)
        && SDL_PIXELTYPE(format)
            is SDL_PixelType.SDL_PIXELTYPE_ARRAYU8
                or SDL_PixelType.SDL_PIXELTYPE_ARRAYU16
                or SDL_PixelType.SDL_PIXELTYPE_ARRAYU32
                or SDL_PixelType.SDL_PIXELTYPE_ARRAYF16
                or SDL_PixelType.SDL_PIXELTYPE_ARRAYF32;

    public static bool SDL_ISPIXELFORMAT_10BIT(SDL_PixelFormat format) =>
        !SDL_ISPIXELFORMAT_FOURCC(format)
        && SDL_PIXELTYPE(format) is SDL_PixelType.SDL_PIXELTYPE_PACKED32
        && SDL_PIXELLAYOUT(format) is SDL_PackedLayout.SDL_PACKEDLAYOUT_2101010;

    public static bool SDL_ISPIXELFORMAT_FLOAT(SDL_PixelFormat format) =>
        !SDL_ISPIXELFORMAT_FOURCC(format)
        && SDL_PIXELTYPE(format)
            is SDL_PixelType.SDL_PIXELTYPE_ARRAYF16
                or SDL_PixelType.SDL_PIXELTYPE_ARRAYF32;

    public static bool SDL_ISPIXELFORMAT_ALPHA(SDL_PixelFormat format) =>
        (
            SDL_ISPIXELFORMAT_PACKED(format)
            && SDL_PIXELORDER_PACKED(format)
                is SDL_PackedOrder.SDL_PACKEDORDER_ARGB
                    or SDL_PackedOrder.SDL_PACKEDORDER_RGBA
                    or SDL_PackedOrder.SDL_PACKEDORDER_ABGR
                    or SDL_PackedOrder.SDL_PACKEDORDER_BGRA
        )
        || (
            SDL_ISPIXELFORMAT_ARRAY(format)
            && SDL_PIXELORDER_ARRAY(format)
                is SDL_ArrayOrder.SDL_ARRAYORDER_ARGB
                    or SDL_ArrayOrder.SDL_ARRAYORDER_RGBA
                    or SDL_ArrayOrder.SDL_ARRAYORDER_ABGR
                    or SDL_ArrayOrder.SDL_ARRAYORDER_BGRA
        );

    public static bool SDL_ISPIXELFORMAT_FOURCC(SDL_PixelFormat format) =>
        (uint)format != 0 && SDL_PIXELFLAG(format) != 1;

    public enum SDL_PixelFormat : uint
    {
        SDL_PIXELFORMAT_UNKNOWN = 0U,
        SDL_PIXELFORMAT_INDEX1LSB = 0x11100100U,
        SDL_PIXELFORMAT_INDEX1MSB = 0x11200100U,
        SDL_PIXELFORMAT_INDEX2LSB = 0x1C100200U,
        SDL_PIXELFORMAT_INDEX2MSB = 0x1C200200U,
        SDL_PIXELFORMAT_INDEX4LSB = 0x12100400U,
        SDL_PIXELFORMAT_INDEX4MSB = 0x12200400U,
        SDL_PIXELFORMAT_INDEX8 = 0x13000801U,
        SDL_PIXELFORMAT_RGB332 = 0x14110801U,
        SDL_PIXELFORMAT_XRGB4444 = 0x15120C02U,
        SDL_PIXELFORMAT_XBGR4444 = 0x15520C02U,
        SDL_PIXELFORMAT_XRGB1555 = 0x15130F02U,
        SDL_PIXELFORMAT_XBGR1555 = 0x15530F02U,
        SDL_PIXELFORMAT_ARGB4444 = 0x15321002U,
        SDL_PIXELFORMAT_RGBA4444 = 0x15421002U,
        SDL_PIXELFORMAT_ABGR4444 = 0x15721002U,
        SDL_PIXELFORMAT_BGRA4444 = 0x15821002U,
        SDL_PIXELFORMAT_ARGB1555 = 0x15331002U,
        SDL_PIXELFORMAT_RGBA5551 = 0x15441002U,
        SDL_PIXELFORMAT_ABGR1555 = 0x15731002U,
        SDL_PIXELFORMAT_BGRA5551 = 0x15841002U,
        SDL_PIXELFORMAT_RGB565 = 0x15151002U,
        SDL_PIXELFORMAT_BGR565 = 0x15551002U,
        SDL_PIXELFORMAT_RGB24 = 0x17101803U,
        SDL_PIXELFORMAT_BGR24 = 0x17401803U,
        SDL_PIXELFORMAT_XRGB8888 = 0x16161804U,
        SDL_PIXELFORMAT_RGBX8888 = 0x16261804U,
        SDL_PIXELFORMAT_XBGR8888 = 0x16561804U,
        SDL_PIXELFORMAT_BGRX8888 = 0x16661804U,
        SDL_PIXELFORMAT_ARGB8888 = 0x16362004U,
        SDL_PIXELFORMAT_RGBA8888 = 0x16462004U,
        SDL_PIXELFORMAT_ABGR8888 = 0x16762004U,
        SDL_PIXELFORMAT_BGRA8888 = 0x16862004U,
        SDL_PIXELFORMAT_XRGB2101010 = 0x16172004U,
        SDL_PIXELFORMAT_XBGR2101010 = 0x16572004U,
        SDL_PIXELFORMAT_ARGB2101010 = 0x16372004U,
        SDL_PIXELFORMAT_ABGR2101010 = 0x16772004U,
        SDL_PIXELFORMAT_RGB48 = 0x18103006U,
        SDL_PIXELFORMAT_BGR48 = 0x18403006U,
        SDL_PIXELFORMAT_RGBA64 = 0x18204008U,
        SDL_PIXELFORMAT_ARGB64 = 0x18304008U,
        SDL_PIXELFORMAT_BGRA64 = 0x18504008U,
        SDL_PIXELFORMAT_ABGR64 = 0x18604008U,
        SDL_PIXELFORMAT_RGB48_FLOAT = 0x1A103006U,
        SDL_PIXELFORMAT_BGR48_FLOAT = 0x1A403006U,
        SDL_PIXELFORMAT_RGBA64_FLOAT = 0x1A204008U,
        SDL_PIXELFORMAT_ARGB64_FLOAT = 0x1A304008U,
        SDL_PIXELFORMAT_BGRA64_FLOAT = 0x1A504008U,
        SDL_PIXELFORMAT_ABGR64_FLOAT = 0x1A604008U,
        SDL_PIXELFORMAT_RGB96_FLOAT = 0x1B10600CU,
        SDL_PIXELFORMAT_BGR96_FLOAT = 0x1B40600CU,
        SDL_PIXELFORMAT_RGBA128_FLOAT = 0x1B208010U,
        SDL_PIXELFORMAT_ARGB128_FLOAT = 0x1B308010U,
        SDL_PIXELFORMAT_BGRA128_FLOAT = 0x1B508010U,
        SDL_PIXELFORMAT_ABGR128_FLOAT = 0x1B608010U,
        SDL_PIXELFORMAT_YV12 = 0x32315659U,
        SDL_PIXELFORMAT_IYUV = 0x56555949U,
        SDL_PIXELFORMAT_YUY2 = 0x32595559U,
        SDL_PIXELFORMAT_UYVY = 0x59565955U,
        SDL_PIXELFORMAT_YVYU = 0x55595659U,
        SDL_PIXELFORMAT_NV12 = 0x3231564EU,
        SDL_PIXELFORMAT_NV21 = 0x3132564EU,
        SDL_PIXELFORMAT_P010 = 0x30313050U,
        SDL_PIXELFORMAT_EXTERNAL_OES = 0x2053454FU,
    }

    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_RGBA8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_BGRA8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_ARGB8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_ARGB8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_BGRA8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_RGBA8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBX32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_XBGR8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_RGBX8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_BGRX8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_XRGB8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRX32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_XRGB8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_BGRX8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR32 =>
        BitConverter.IsLittleEndian
            ? SDL_PixelFormat.SDL_PIXELFORMAT_RGBX8888
            : SDL_PixelFormat.SDL_PIXELFORMAT_XBGR8888;

    public enum SDL_ColorType : uint
    {
        SDL_COLOR_TYPE_UNKNOWN = 0,
        SDL_COLOR_TYPE_RGB = 1,
        SDL_COLOR_TYPE_YCBCR = 2,
    }

    public enum SDL_ColorRange : uint
    {
        SDL_COLOR_RANGE_UNKNOWN = 0,
        SDL_COLOR_RANGE_LIMITED = 1,
        SDL_COLOR_RANGE_FULL = 2,
    }

    public enum SDL_ColorPrimaries : uint
    {
        SDL_COLOR_PRIMARIES_UNKNOWN = 0,
        SDL_COLOR_PRIMARIES_BT709 = 1,
        SDL_COLOR_PRIMARIES_UNSPECIFIED = 2,
        SDL_COLOR_PRIMARIES_BT470M = 4,
        SDL_COLOR_PRIMARIES_BT470BG = 5,
        SDL_COLOR_PRIMARIES_BT601 = 6,
        SDL_COLOR_PRIMARIES_SMPTE240 = 7,
        SDL_COLOR_PRIMARIES_GENERIC_FILM = 8,
        SDL_COLOR_PRIMARIES_BT2020 = 9,
        SDL_COLOR_PRIMARIES_XYZ = 10,
        SDL_COLOR_PRIMARIES_SMPTE431 = 11,
        SDL_COLOR_PRIMARIES_SMPTE432 = 12,
        SDL_COLOR_PRIMARIES_EBU3213 = 22,
        SDL_COLOR_PRIMARIES_CUSTOM = 31,
    }

    public enum SDL_TransferCharacteristics : uint
    {
        SDL_TRANSFER_CHARACTERISTICS_UNKNOWN = 0,
        SDL_TRANSFER_CHARACTERISTICS_BT709 = 1,
        SDL_TRANSFER_CHARACTERISTICS_UNSPECIFIED = 2,
        SDL_TRANSFER_CHARACTERISTICS_GAMMA22 = 4,
        SDL_TRANSFER_CHARACTERISTICS_GAMMA28 = 5,
        SDL_TRANSFER_CHARACTERISTICS_BT601 = 6,
        SDL_TRANSFER_CHARACTERISTICS_SMPTE240 = 7,
        SDL_TRANSFER_CHARACTERISTICS_LINEAR = 8,
        SDL_TRANSFER_CHARACTERISTICS_LOG100 = 9,
        SDL_TRANSFER_CHARACTERISTICS_LOG100_SQRT10 = 10,
        SDL_TRANSFER_CHARACTERISTICS_IEC61966 = 11,
        SDL_TRANSFER_CHARACTERISTICS_BT1361 = 12,
        SDL_TRANSFER_CHARACTERISTICS_SRGB = 13,
        SDL_TRANSFER_CHARACTERISTICS_BT2020_10BIT = 14,
        SDL_TRANSFER_CHARACTERISTICS_BT2020_12BIT = 15,
        SDL_TRANSFER_CHARACTERISTICS_PQ = 16,
        SDL_TRANSFER_CHARACTERISTICS_SMPTE428 = 17,
        SDL_TRANSFER_CHARACTERISTICS_HLG = 18,
        SDL_TRANSFER_CHARACTERISTICS_CUSTOM = 31,
    }

    public enum SDL_MatrixCoefficients : uint
    {
        SDL_MATRIX_COEFFICIENTS_IDENTITY = 0,
        SDL_MATRIX_COEFFICIENTS_BT709 = 1,
        SDL_MATRIX_COEFFICIENTS_UNSPECIFIED = 2,
        SDL_MATRIX_COEFFICIENTS_FCC = 4,
        SDL_MATRIX_COEFFICIENTS_BT470BG = 5,
        SDL_MATRIX_COEFFICIENTS_BT601 = 6,
        SDL_MATRIX_COEFFICIENTS_SMPTE240 = 7,
        SDL_MATRIX_COEFFICIENTS_YCGCO = 8,
        SDL_MATRIX_COEFFICIENTS_BT2020_NCL = 9,
        SDL_MATRIX_COEFFICIENTS_BT2020_CL = 10,
        SDL_MATRIX_COEFFICIENTS_SMPTE2085 = 11,
        SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_NCL = 12,
        SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_CL = 13,
        SDL_MATRIX_COEFFICIENTS_ICTCP = 14,
        SDL_MATRIX_COEFFICIENTS_CUSTOM = 31,
    }

    public enum SDL_ChromaLocation : uint
    {
        SDL_CHROMA_LOCATION_NONE = 0,
        SDL_CHROMA_LOCATION_LEFT = 1,
        SDL_CHROMA_LOCATION_CENTER = 2,
        SDL_CHROMA_LOCATION_TOPLEFT = 3,
    }

    public static SDL_Colorspace SDL_DEFINE_COLORSPACE(
        SDL_ColorType type,
        SDL_ColorRange range,
        SDL_ColorPrimaries primaries,
        SDL_TransferCharacteristics transfer,
        SDL_MatrixCoefficients matrix,
        SDL_ChromaLocation chroma
    ) =>
        (SDL_Colorspace)(
            (uint)type << 28
            | (uint)range << 24
            | (uint)chroma << 20
            | (uint)primaries << 10
            | (uint)transfer << 5
            | (uint)matrix
        );

    public static SDL_ColorType SDL_COLORSPACETYPE(SDL_Colorspace cspace) =>
        (SDL_ColorType)((uint)cspace >> 28 & 0x0F);

    public static SDL_ColorRange SDL_COLORSPACERANGE(SDL_Colorspace cspace) =>
        (SDL_ColorRange)((uint)cspace >> 24 & 0x0F);

    public static SDL_ChromaLocation SDL_COLORSPACECHROMA(SDL_Colorspace cspace) =>
        (SDL_ChromaLocation)((uint)cspace >> 20 & 0x0F);

    public static SDL_ColorPrimaries SDL_COLORSPACEPRIMARIES(SDL_Colorspace cspace) =>
        (SDL_ColorPrimaries)((uint)cspace >> 10 & 0x1F);

    public static SDL_TransferCharacteristics SDL_COLORSPACETRANSFER(SDL_Colorspace cspace) =>
        (SDL_TransferCharacteristics)((uint)cspace >> 5 & 0x1F);

    public static SDL_MatrixCoefficients SDL_COLORSPACEMATRIX(SDL_Colorspace cspace) =>
        (SDL_MatrixCoefficients)((uint)cspace & 0x1F);

    public static bool SDL_ISCOLORSPACE_MATRIX_BT601(SDL_Colorspace cspace) =>
        SDL_COLORSPACEMATRIX(cspace)
            is SDL_MatrixCoefficients.SDL_MATRIX_COEFFICIENTS_BT601
                or SDL_MatrixCoefficients.SDL_MATRIX_COEFFICIENTS_BT470BG;

    public static bool SDL_ISCOLORSPACE_MATRIX_BT709(SDL_Colorspace cspace) =>
        SDL_COLORSPACEMATRIX(cspace) is SDL_MatrixCoefficients.SDL_MATRIX_COEFFICIENTS_BT709;

    public static bool SDL_ISCOLORSPACE_MATRIX_BT2020_NCL(SDL_Colorspace cspace) =>
        SDL_COLORSPACEMATRIX(cspace) is SDL_MatrixCoefficients.SDL_MATRIX_COEFFICIENTS_BT2020_NCL;

    public static bool SDL_ISCOLORSPACE_LIMITED_RANGE(SDL_Colorspace cspace) =>
        SDL_COLORSPACERANGE(cspace) is not SDL_ColorRange.SDL_COLOR_RANGE_FULL;

    public static bool SDL_ISCOLORSPACE_FULL_RANGE(SDL_Colorspace cspace) =>
        SDL_COLORSPACERANGE(cspace) is SDL_ColorRange.SDL_COLOR_RANGE_FULL;

    public enum SDL_Colorspace : uint
    {
        SDL_COLORSPACE_UNKNOWN = 0U,
        SDL_COLORSPACE_SRGB = 0x120005A0U,
        SDL_COLORSPACE_SRGB_LINEAR = 0x12000500U,
        SDL_COLORSPACE_HDR10 = 0x12002600U,
        SDL_COLORSPACE_JPEG = 0x220004C6U,
        SDL_COLORSPACE_BT601_LIMITED = 0x211018C6U,
        SDL_COLORSPACE_BT601_FULL = 0x221018C6U,
        SDL_COLORSPACE_BT709_LIMITED = 0x21100421U,
        SDL_COLORSPACE_BT709_FULL = 0x22100421U,
        SDL_COLORSPACE_BT2020_LIMITED = 0x21102609U,
        SDL_COLORSPACE_BT2020_FULL = 0x22102609U,
        SDL_COLORSPACE_RGB_DEFAULT = SDL_COLORSPACE_SRGB,
        SDL_COLORSPACE_YUV_DEFAULT = SDL_COLORSPACE_JPEG,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_FColor
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INTERNAL_SDL_Palette
    {
        public int ncolors;
        public unsafe SDL_Color* colors;
        public uint version;
        public int refcount;
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<SDL_Palette>))]
    public sealed class SDL_Palette : SafeHandle
    {
        private unsafe INTERNAL_SDL_Palette* UnsafeHandle => (INTERNAL_SDL_Palette*)handle;

        public unsafe Span<SDL_Color> Colors => new(UnsafeHandle->colors, UnsafeHandle->ncolors);

        public unsafe uint Version => UnsafeHandle->version;

        public unsafe int RefCount => UnsafeHandle->refcount;

        public override bool IsInvalid => handle == IntPtr.Zero;

        public SDL_Palette()
            : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true) => SetHandle(IntPtr.Zero);

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            SDL_DestroyPalette(handle);
            SetHandleAsInvalid();
            return true;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_PixelFormatDetails
    {
        public SDL_PixelFormat format;
        public byte bits_per_pixel;
        public byte bytes_per_pixel;
        private unsafe fixed byte _padding[2];
        public uint Rmask;
        public uint Gmask;
        public uint Bmask;
        public uint Amask;
        public byte Rbits;
        public byte Gbits;
        public byte Bbits;
        public byte Abits;
        public byte Rshift;
        public byte Gshift;
        public byte Bshift;
        public byte Ashift;
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPixelFormatName),
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(UnownedUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string SDL_GetPixelFormatName(SDL_PixelFormat format);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetMasksForPixelFormat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetMasksForPixelFormat(
        SDL_PixelFormat format,
        out uint Rmask,
        out uint Gmask,
        out uint Bmask,
        out uint Amask
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetPixelFormatForMasks))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PixelFormat SDL_GetPixelFormatForMasks(
        int bpp,
        uint Rmask,
        uint Gmask,
        uint Bmask,
        uint Amask
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetPixelFormatDetails))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial SDL_PixelFormatDetails* INTERNAL_SDL_GetPixelFormatDetails(
        SDL_PixelFormat format
    );

    public static SDL_PixelFormatDetails SDL_GetPixelFormatDetails(
        SDL_PixelFormat format,
        out bool hadError
    )
    {
        unsafe
        {
            hadError = false;
            var result = INTERNAL_SDL_GetPixelFormatDetails(format);
            if (result is not null)
            {
                return *result;
            }

            hadError = true;
            return default;
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreatePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Palette SDL_CreatePalette(int ncolors);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetPaletteColors))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool INTERNAL_SDL_SetPaletteColors(
        SDL_Palette palette,
        [In]
        [MarshalUsing(
            typeof(ArrayMarshaller<SDL_Color, SDL_Color>),
            CountElementName = nameof(ncolors)
        )]
            SDL_Color[] colors,
        int firstcolor,
        int ncolors
    );

    public static bool SDL_SetPaletteColors(
        SDL_Palette palette,
        ReadOnlySpan<SDL_Color> colors,
        int firstcolor
    ) => INTERNAL_SDL_SetPaletteColors(palette, colors.ToArray(), firstcolor, colors.Length);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroyPalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroyPalette(IntPtr palette);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint SDL_MapRGB(SDL_PixelFormat format, byte r, byte g, byte b);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint SDL_MapRGBA(SDL_PixelFormat format, byte r, byte g, byte b, byte a);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_GetRGB(
        uint pixel,
        SDL_PixelFormat format,
        out byte r,
        out byte g,
        out byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_GetRGBA(
        uint pixel,
        SDL_PixelFormat format,
        out byte r,
        out byte g,
        out byte b,
        out byte a
    );
}
