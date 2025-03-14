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

namespace Open.CommandAndConquer.Sdl3;

public static partial class SDL3
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Point
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_FPoint
    {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Rect
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_FRect
    {
        public float x;
        public float y;
        public float w;
        public float h;
    }

    public static void SDL_RectToFRect(in SDL_Rect rect, out SDL_FRect frect)
    {
        frect.x = rect.x;
        frect.y = rect.y;
        frect.w = rect.w;
        frect.h = rect.h;
    }

    public static bool SDL_PointInRect(in SDL_Point p, in SDL_Rect r) =>
        p.x >= r.x && p.x < r.x + r.w && p.y >= r.y && p.y < r.y + r.h;

    public static bool SDL_RectEmpty(in SDL_Rect r) => r.w <= 0 || r.h <= 0;

    public static bool SDL_RectsEqual(in SDL_Rect a, in SDL_Rect b) =>
        a.x == b.x && a.y == b.y && a.w == b.w && a.h == b.h;

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasRectIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasRectIntersection(in SDL_Rect A, in SDL_Rect B);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectIntersection(
        in SDL_Rect A,
        in SDL_Rect B,
        out SDL_Rect result
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectUnion))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectUnion(in SDL_Rect A, in SDL_Rect B, out SDL_Rect result);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectEnclosingPoints))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_GetRectEnclosingPoints(
        [In]
        [MarshalUsing(
            typeof(ArrayMarshaller<SDL_Point, SDL_Point>),
            CountElementName = nameof(count)
        )]
            SDL_Point[] points,
        int count,
        SDL_Rect* clip,
        out SDL_Rect result
    );

    public static bool SDL_GetRectEnclosingPoints(Span<SDL_Point> points, out SDL_Rect result)
    {
        unsafe
        {
            return INTERNAL_SDL_GetRectEnclosingPoints(
                points.ToArray(),
                points.Length,
                null,
                out result
            );
        }
    }

    public static bool SDL_GetRectEnclosingPoints(
        Span<SDL_Point> points,
        SDL_Rect clip,
        out SDL_Rect result
    )
    {
        unsafe
        {
            return INTERNAL_SDL_GetRectEnclosingPoints(
                points.ToArray(),
                points.Length,
                &clip,
                out result
            );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectAndLineIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectAndLineIntersection(
        in SDL_Rect rect,
        ref int X1,
        ref int Y1,
        ref int X2,
        ref int Y2
    );

    public static bool SDL_PointInRectFloat(in SDL_FPoint p, in SDL_FRect r) =>
        p.x >= r.x && p.x < r.x + r.w && p.y >= r.y && p.y < r.y + r.h;

    public static bool SDL_RectEmptyFloat(in SDL_FRect r) => r.w <= 0 || r.h <= 0;

    public static bool SDL_RectsEqualEpsilon(in SDL_FRect a, in SDL_FRect b, float epsilon) =>
        SDL_fabsf(a.x - b.x) <= epsilon
        && SDL_fabsf(a.y - b.y) <= epsilon
        && SDL_fabsf(a.w - b.w) <= epsilon
        && SDL_fabsf(a.h - b.h) <= epsilon;

    public static bool SDL_RectsEqualFloat(in SDL_FRect a, in SDL_FRect b) =>
        SDL_RectsEqualEpsilon(a, b, SDL_FLT_EPSILON);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasRectIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasRectIntersectionFloat(in SDL_FRect A, in SDL_FRect B);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectIntersectionFloat(
        in SDL_FRect A,
        in SDL_FRect B,
        out SDL_FRect result
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectUnionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectUnionFloat(
        in SDL_FRect A,
        in SDL_FRect B,
        out SDL_FRect result
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectEnclosingPointsFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_GetRectEnclosingPointsFloat(
        [In] [MarshalUsing(typeof(ArrayMarshaller<SDL_FPoint, SDL_FPoint>))] SDL_FPoint[] points,
        int count,
        SDL_FRect* clip,
        out SDL_FRect result
    );

    public static bool SDL_GetRectEnclosingPointsFloat(
        Span<SDL_FPoint> points,
        out SDL_FRect result
    )
    {
        unsafe
        {
            return INTERNAL_SDL_GetRectEnclosingPointsFloat(
                points.ToArray(),
                points.Length,
                null,
                out result
            );
        }
    }

    public static bool SDL_GetRectEnclosingPointsFloat(
        Span<SDL_FPoint> points,
        SDL_FRect clip,
        out SDL_FRect result
    )
    {
        unsafe
        {
            return INTERNAL_SDL_GetRectEnclosingPointsFloat(
                points.ToArray(),
                points.Length,
                &clip,
                out result
            );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectAndLineIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectAndLineIntersectionFloat(
        in SDL_FRect rect,
        ref int X1,
        ref int Y1,
        ref int X2,
        ref int Y2
    );
}
