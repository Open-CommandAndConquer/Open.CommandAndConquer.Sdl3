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
using Open.CommandAndConquer.Sdl3.Blending;

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
{
    public record struct SDL_SurfaceFlags(uint Value)
    {
        public static SDL_SurfaceFlags None => new(0U);

        public static SDL_SurfaceFlags operator |(SDL_SurfaceFlags lhs, SDL_SurfaceFlags rhs) =>
            new(lhs.Value | rhs.Value);

        public static SDL_SurfaceFlags operator &(SDL_SurfaceFlags lhs, SDL_SurfaceFlags rhs) =>
            new(lhs.Value & rhs.Value);

        public static SDL_SurfaceFlags operator ^(SDL_SurfaceFlags lhs, SDL_SurfaceFlags rhs) =>
            new(lhs.Value ^ rhs.Value);

        public static SDL_SurfaceFlags operator ~(SDL_SurfaceFlags flags) => new(~flags.Value);

        public static implicit operator uint(SDL_SurfaceFlags flags) => flags.Value;

        public static implicit operator SDL_SurfaceFlags(uint flags) => new(flags);
    }

    public static SDL_SurfaceFlags SDL_SURFACE_PREALLOCATED => new(00000001U);
    public static SDL_SurfaceFlags SDL_SURFACE_LOCK_NEEDED => new(00000002U);
    public static SDL_SurfaceFlags SDL_SURFACE_LOCKED => new(00000004U);
    public static SDL_SurfaceFlags SDL_SURFACE_SIMD_ALIGNED => new(00000008U);

    public static bool SDL_MUSTLOCK(SDL_Surface S) =>
        (S.Flags & SDL_SURFACE_LOCK_NEEDED) == SDL_SURFACE_LOCK_NEEDED;

    public enum SDL_ScaleMode
    {
        SDL_SCALEMODE_NEAREST,
        SDL_SCALEMODE_LINEAR,
    }

    public enum SDL_FlipMode
    {
        SDL_FLIP_NONE,
        SDL_FLIP_HORIZONTAL,
        SDL_FLIP_VERTICAL,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INTERNAL_SDL_Surface
    {
        public SDL_SurfaceFlags flags;
        public SDL_PixelFormat format;
        public int w;
        public int h;
        public int pitch;
        public unsafe void* pixels;
        public int refcount;
        public unsafe void* reserved;
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<SDL_Surface>))]
    public sealed class SDL_Surface : SafeHandle
    {
        internal unsafe SDL_SurfaceFlags Flags => ((INTERNAL_SDL_Surface*)handle)->flags;

        public override bool IsInvalid => handle == IntPtr.Zero;

        internal SDL_Surface(IntPtr preexistingHandle)
            : base(invalidHandleValue: IntPtr.Zero, ownsHandle: false) =>
            SetHandle(preexistingHandle);

        public SDL_Surface()
            : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true) => SetHandle(IntPtr.Zero);

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            SDL_DestroySurface(handle);
            SetHandle(IntPtr.Zero);
            return true;
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_CreateSurface(
        int width,
        int height,
        SDL_PixelFormat format
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurfaceFrom))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial SDL_Surface INTERNAL_SDL_CreateSurfaceFrom(
        int width,
        int height,
        SDL_PixelFormat format,
        void* pixels,
        int pitch
    );

    public static SDL_Surface SDL_CreateSurfaceFrom(
        int width,
        int height,
        SDL_PixelFormat format,
        ReadOnlySpan<byte> pixels,
        int pitch
    )
    {
        unsafe
        {
            fixed (byte* pixelsPtr = pixels)
            {
                return INTERNAL_SDL_CreateSurfaceFrom(width, height, format, pixelsPtr, pitch);
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroySurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroySurface(IntPtr surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_GetSurfaceProperties(SDL_Surface surface);

    public const string SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT = "SDL.surface.SDR_white_point";
    public const string SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT = "SDL.surface.HDR_headroom";
    public const string SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING = "SDL.surface.tonemap";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceColorspace(
        SDL_Surface surface,
        SDL_Colorspace colorspace
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Colorspace SDL_GetSurfaceColorspace(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr INTERNAL_SDL_CreateSurfacePalette(SDL_Surface surface);

    public static SDL_Palette SDL_CreateSurfacePalette(SDL_Surface surface) =>
        new(INTERNAL_SDL_CreateSurfacePalette(surface));

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfacePalette(SDL_Surface surface, SDL_Palette palette);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr INTERNAL_SDL_GetSurfacePalette(SDL_Surface surface);

    public static SDL_Palette SDL_GetSurfacePalette(SDL_Surface surface) =>
        new(INTERNAL_SDL_GetSurfacePalette(surface));

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_AddSurfaceAlternateImage))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_AddSurfaceAlternateImage(SDL_Surface surface, SDL_Surface image);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasAlternateImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SurfaceHasAlternateImages(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial IntPtr* INTERNAL_SDL_GetSurfaceImages(
        SDL_Surface surface,
        out int count
    );

    public static ICollection<SDL_Surface>? SDL_GetSurfaceImages(SDL_Surface surface)
    {
        unsafe
        {
            var result = INTERNAL_SDL_GetSurfaceImages(surface, out var count);
            if (result is null)
            {
                return null;
            }

            var array = ArrayPool<SDL_Surface>.Shared.Rent(count);
            try
            {
                for (var i = 0; i < count; i++)
                {
                    array[i] = new SDL_Surface(result[i]);
                }

                return array;
            }
            finally
            {
                ArrayPool<SDL_Surface>.Shared.Return(array);
                SDL_free(result);
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_RemoveSurfaceAlternateImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_RemoveSurfaceAlternateImages(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LockSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_LockSurface(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_UnlockSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_UnlockSurface(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LoadBMP_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_LoadBMP_IO(
        SDL_IOStream src,
        [MarshalAs(UnmanagedType.I1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LoadBMP),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_LoadBMP(string file);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SaveBMP_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SaveBMP_IO(
        SDL_Surface surface,
        SDL_IOStream dst,
        [MarshalAs(UnmanagedType.I1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SaveBMP),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SaveBMP(SDL_Surface surface, string file);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceRLE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceRLE(
        SDL_Surface surface,
        [MarshalAs(UnmanagedType.I1)] bool enabled
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasRLE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SurfaceHasRLE(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceColorKey(
        SDL_Surface surface,
        [MarshalAs(UnmanagedType.I1)] bool enabled,
        uint key
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SurfaceHasColorKey(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetSurfaceColorKey(SDL_Surface surface, out uint key);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceColorMod(SDL_Surface surface, byte r, byte g, byte b);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetSurfaceColorMod(
        SDL_Surface surface,
        out byte r,
        out byte g,
        out byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceAlphaMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceAlphaMod(SDL_Surface surface, byte alpha);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceAlphaMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetSurfaceAlphaMod(SDL_Surface surface, out byte alpha);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceBlendMode))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetSurfaceBlendMode(SDL_Surface surface, BlendMode blendMode);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceBlendMode))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetSurfaceBlendMode(
        SDL_Surface surface,
        out BlendMode blendMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceClipRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_SetSurfaceClipRect(
        SDL_Surface surface,
        SDL_Rect* rect
    );

    public static bool SDL_SetSurfaceClipRect(SDL_Surface surface)
    {
        unsafe
        {
            return INTERNAL_SDL_SetSurfaceClipRect(surface, null);
        }
    }

    public static bool SDL_SetSurfaceClipRect(SDL_Surface surface, SDL_Rect rect)
    {
        unsafe
        {
            return INTERNAL_SDL_SetSurfaceClipRect(surface, &rect);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceClipRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetSurfaceClipRect(SDL_Surface surface, out SDL_Rect rect);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FlipSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_FlipSurface(SDL_Surface surface, SDL_FlipMode flip);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DuplicateSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_DuplicateSurface(SDL_Surface surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ScaleSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_ScaleSurface(
        SDL_Surface surface,
        int width,
        int height,
        SDL_ScaleMode scaleMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_ConvertSurface(
        SDL_Surface surface,
        SDL_PixelFormat format
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertSurfaceAndColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_Surface SDL_ConvertSurfaceAndColorspace(
        SDL_Surface surface,
        SDL_PixelFormat format,
        SDL_Palette palette,
        SDL_Colorspace colorspace,
        SDL_PropertiesID properties
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertPixels))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_ConvertPixels(
        int width,
        int height,
        SDL_PixelFormat src_format,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        void* dst,
        int dst_pitch
    );

    public static bool SDL_ConvertPixels(
        int width,
        int height,
        SDL_PixelFormat src_format,
        ReadOnlySpan<byte> src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        out Span<byte> dst,
        int dst_pitch
    )
    {
        unsafe
        {
            dst = new Span<byte>(null, src.Length);
            fixed (byte* srcPtr = src)
            fixed (byte* dstPtr = dst)
            {
                var result = INTERNAL_SDL_ConvertPixels(
                    width,
                    height,
                    src_format,
                    srcPtr,
                    src_pitch,
                    dst_format,
                    dstPtr,
                    dst_pitch
                );
                dst = new Span<byte>(dstPtr, dst.Length);
                return result;
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertPixelsAndColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_ConvertPixelsAndColorspace(
        int width,
        int height,
        SDL_PixelFormat src_format,
        SDL_Colorspace src_colorspace,
        SDL_PropertiesID src_properties,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        SDL_Colorspace dst_colorspace,
        SDL_PropertiesID dst_properties,
        void* dst,
        int dst_pitch
    );

    public static bool SDL_ConvertPixelsAndColorspace(
        int width,
        int height,
        SDL_PixelFormat src_format,
        SDL_Colorspace src_colorspace,
        SDL_PropertiesID src_properties,
        ReadOnlySpan<byte> src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        SDL_Colorspace dst_colorspace,
        SDL_PropertiesID dst_properties,
        out Span<byte> dst,
        int dst_pitch
    )
    {
        unsafe
        {
            dst = new Span<byte>(null, src.Length);
            fixed (byte* srcPtr = src)
            fixed (byte* dstPtr = dst)
            {
                var result = INTERNAL_SDL_ConvertPixelsAndColorspace(
                    width,
                    height,
                    src_format,
                    src_colorspace,
                    src_properties,
                    srcPtr,
                    src_pitch,
                    dst_format,
                    dst_colorspace,
                    dst_properties,
                    dstPtr,
                    dst_pitch
                );

                dst = new Span<byte>(dstPtr, dst.Length);
                return result;
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_PremultiplyAlpha))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_PremultiplyAlpha(
        int width,
        int height,
        SDL_PixelFormat src_format,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        void* dst,
        int dst_pitch,
        [MarshalAs(UnmanagedType.I1)] bool linear
    );

    public static bool SDL_PremultiplyAlpha(
        int width,
        int height,
        SDL_PixelFormat src_format,
        ReadOnlySpan<byte> src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        out Span<byte> dst,
        int dst_pitch,
        bool linear
    )
    {
        unsafe
        {
            dst = new Span<byte>(null, src.Length);
            fixed (byte* srcPtr = src)
            fixed (byte* dstPtr = dst)
            {
                var result = INTERNAL_SDL_PremultiplyAlpha(
                    width,
                    height,
                    src_format,
                    srcPtr,
                    src_pitch,
                    dst_format,
                    dstPtr,
                    dst_pitch,
                    linear
                );

                dst = new Span<byte>(dstPtr, dst.Length);
                return result;
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_PremultiplySurfaceAlpha))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_PremultiplySurfaceAlpha(
        SDL_Surface surface,
        [MarshalAs(UnmanagedType.I1)] bool linear
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ClearSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ClearSurface(
        SDL_Surface surface,
        float r,
        float g,
        float b,
        float a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FillSurfaceRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_FillSurfaceRect(
        SDL_Surface dst,
        SDL_Rect* rect,
        uint color
    );

    public static bool SDL_FillSurfaceRect(SDL_Surface dst, uint color)
    {
        unsafe
        {
            return INTERNAL_SDL_FillSurfaceRect(dst, null, color);
        }
    }

    public static bool SDL_FillSurfaceRect(SDL_Surface dst, SDL_Rect rect, uint color)
    {
        unsafe
        {
            return INTERNAL_SDL_FillSurfaceRect(dst, &rect, color);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FillSurfaceRects))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool INTERNAL_SDL_FillSurfaceRects(
        SDL_Surface dst,
        [In]
        [MarshalUsing(
            typeof(ArrayMarshaller<SDL_Rect, SDL_Rect>),
            CountElementName = nameof(count)
        )]
            SDL_Rect[] rects,
        int count,
        uint color
    );

    public static bool SDL_FillSurfaceRects(
        SDL_Surface dst,
        ReadOnlySpan<SDL_Rect> rects,
        uint color
    ) => INTERNAL_SDL_FillSurfaceRects(dst, rects.ToArray(), rects.Length, color);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurface(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect
    );

    public static bool SDL_BlitSurface(SDL_Surface src, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface(src, null, dst, null);
        }
    }

    public static bool SDL_BlitSurface(SDL_Surface src, SDL_Rect srcrect, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface(src, &srcrect, dst, null);
        }
    }

    public static bool SDL_BlitSurface(SDL_Surface src, SDL_Surface dst, SDL_Rect dstrect)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface(src, null, dst, &dstrect);
        }
    }

    public static bool SDL_BlitSurface(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface(src, &srcrect, dst, &dstrect);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceUnchecked))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurfaceUnchecked(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect
    );

    public static bool SDL_BlitSurfaceUnchecked(SDL_Surface src, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUnchecked(src, null, dst, null);
        }
    }

    public static bool SDL_BlitSurfaceUnchecked(SDL_Surface src, SDL_Rect srcrect, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUnchecked(src, &srcrect, dst, null);
        }
    }

    public static bool SDL_BlitSurfaceUnchecked(SDL_Surface src, SDL_Surface dst, SDL_Rect dstrect)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUnchecked(src, null, dst, &dstrect);
        }
    }

    public static bool SDL_BlitSurfaceUnchecked(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUnchecked(src, &srcrect, dst, &dstrect);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceScaled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurfaceScaled(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect,
        SDL_ScaleMode scaleMode
    );

    public static bool SDL_BlitSurfaceScaled(
        SDL_Surface src,
        SDL_Surface dst,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceScaled(src, null, dst, null, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceScaled(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceScaled(src, &srcrect, dst, null, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceScaled(
        SDL_Surface src,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceScaled(src, null, dst, &dstrect, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceScaled(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceScaled(src, &srcrect, dst, &dstrect, scaleMode);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceUncheckedScaled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect,
        SDL_ScaleMode scaleMode
    );

    public static bool SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface src,
        SDL_Surface dst,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUncheckedScaled(src, null, dst, null, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUncheckedScaled(src, &srcrect, dst, null, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface src,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUncheckedScaled(src, null, dst, &dstrect, scaleMode);
        }
    }

    public static bool SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceUncheckedScaled(src, &srcrect, dst, &dstrect, scaleMode);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_StretchSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_StretchSurface(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect,
        SDL_ScaleMode scaleMode
    );

    public static bool SDL_StretchSurface(SDL_Surface src, SDL_Surface dst, SDL_ScaleMode scaleMode)
    {
        unsafe
        {
            return INTERNAL_SDL_StretchSurface(src, null, dst, null, scaleMode);
        }
    }

    public static bool SDL_StretchSurface(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_StretchSurface(src, &srcrect, dst, null, scaleMode);
        }
    }

    public static bool SDL_StretchSurface(
        SDL_Surface src,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_StretchSurface(src, null, dst, &dstrect, scaleMode);
        }
    }

    public static bool SDL_StretchSurface(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect,
        SDL_ScaleMode scaleMode
    )
    {
        unsafe
        {
            return INTERNAL_SDL_StretchSurface(src, &srcrect, dst, &dstrect, scaleMode);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceTiled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurfaceTiled(
        SDL_Surface src,
        SDL_Rect* srcrect,
        SDL_Surface dst,
        SDL_Rect* dstrect
    );

    public static bool SDL_BlitSurfaceTiled(SDL_Surface src, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiled(src, null, dst, null);
        }
    }

    public static bool SDL_BlitSurfaceTiled(SDL_Surface src, SDL_Rect srcrect, SDL_Surface dst)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiled(src, &srcrect, dst, null);
        }
    }

    public static bool SDL_BlitSurfaceTiled(SDL_Surface src, SDL_Surface dst, SDL_Rect dstrect)
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiled(src, null, dst, &dstrect);
        }
    }

    public static bool SDL_BlitSurfaceTiled(
        SDL_Surface src,
        SDL_Rect srcrect,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiled(src, &srcrect, dst, &dstrect);
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceTiledWithScale))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurfaceTiledWithScale(
        SDL_Surface src,
        SDL_Rect* srcrect,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst,
        SDL_Rect* dstrect
    );

    public static bool SDL_BlitSurfaceTiledWithScale(
        SDL_Surface src,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiledWithScale(src, null, scale, scaleMode, dst, null);
        }
    }

    public static bool SDL_BlitSurfaceTiledWithScale(
        SDL_Surface src,
        SDL_Rect srcrect,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiledWithScale(
                src,
                &srcrect,
                scale,
                scaleMode,
                dst,
                null
            );
        }
    }

    public static bool SDL_BlitSurfaceTiledWithScale(
        SDL_Surface src,
        SDL_Surface dst,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiledWithScale(
                src,
                null,
                scale,
                scaleMode,
                dst,
                &dstrect
            );
        }
    }

    public static bool SDL_BlitSurfaceTiledWithScale(
        SDL_Surface src,
        SDL_Rect srcrect,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurfaceTiledWithScale(
                src,
                &srcrect,
                scale,
                scaleMode,
                dst,
                &dstrect
            );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurface9Grid))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_BlitSurface9Grid(
        SDL_Surface src,
        SDL_Rect* srcrect,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst,
        SDL_Rect* dstrect
    );

    public static bool SDL_BlitSurface9Grid(
        SDL_Surface src,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface9Grid(
                src,
                null,
                left_width,
                right_width,
                top_height,
                bottom_height,
                scale,
                scaleMode,
                dst,
                null
            );
        }
    }

    public static bool SDL_BlitSurface9Grid(
        SDL_Surface src,
        SDL_Rect srcrect,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface9Grid(
                src,
                &srcrect,
                left_width,
                right_width,
                top_height,
                bottom_height,
                scale,
                scaleMode,
                dst,
                null
            );
        }
    }

    public static bool SDL_BlitSurface9Grid(
        SDL_Surface src,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface9Grid(
                src,
                null,
                left_width,
                right_width,
                top_height,
                bottom_height,
                scale,
                scaleMode,
                dst,
                &dstrect
            );
        }
    }

    public static bool SDL_BlitSurface9Grid(
        SDL_Surface src,
        SDL_Rect srcrect,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        SDL_ScaleMode scaleMode,
        SDL_Surface dst,
        SDL_Rect dstrect
    )
    {
        unsafe
        {
            return INTERNAL_SDL_BlitSurface9Grid(
                src,
                &srcrect,
                left_width,
                right_width,
                top_height,
                bottom_height,
                scale,
                scaleMode,
                dst,
                &dstrect
            );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapSurfaceRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint SDL_MapSurfaceRGB(SDL_Surface surface, byte r, byte g, byte b);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapSurfaceRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint SDL_MapSurfaceRGBA(
        SDL_Surface surface,
        byte r,
        byte g,
        byte b,
        byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadSurfacePixel))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadSurfacePixel(
        SDL_Surface surface,
        int x,
        int y,
        out byte r,
        out byte g,
        out byte b,
        out byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadSurfacePixelFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ReadSurfacePixelFloat(
        SDL_Surface surface,
        int x,
        int y,
        out float r,
        out float g,
        out float b,
        out float a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteSurfacePixel))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteSurfacePixel(
        SDL_Surface surface,
        int x,
        int y,
        byte r,
        byte g,
        byte b,
        byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteSurfacePixelFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_WriteSurfacePixelFloat(
        SDL_Surface surface,
        int x,
        int y,
        float r,
        float g,
        float b,
        float a
    );
}
