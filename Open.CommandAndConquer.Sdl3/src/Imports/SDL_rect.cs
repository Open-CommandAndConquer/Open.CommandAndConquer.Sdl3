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
using Open.CommandAndConquer.Sdl3.Geometry;

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
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

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasRectIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasRectIntersection(Rect A, Rect B);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectIntersection(Rect A, Rect B, out Rect result);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectUnion))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectUnion(Rect A, Rect B, out Rect result);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectEnclosingPoints))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static unsafe partial bool SDL_GetRectEnclosingPoints(
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<Point, SDL_Point>), CountElementName = nameof(count))]
            Point[] points,
        int count,
        Rect? clip,
        out Rect result
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectAndLineIntersection))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectAndLineIntersection(
        Rect rect,
        ref int X1,
        ref int Y1,
        ref int X2,
        ref int Y2
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_HasRectIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasRectIntersectionFloat(FRect A, FRect B);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectIntersectionFloat(FRect A, FRect B, out FRect result);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectUnionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectUnionFloat(FRect A, FRect B, out FRect result);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectEnclosingPointsFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static unsafe partial bool SDL_GetRectEnclosingPointsFloat(
        [In] [MarshalUsing(typeof(ArrayMarshaller<FPoint, SDL_FPoint>))] FPoint[] points,
        int count,
        FRect? clip,
        out FRect result
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRectAndLineIntersectionFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetRectAndLineIntersectionFloat(
        FRect rect,
        ref float X1,
        ref float Y1,
        ref float X2,
        ref float Y2
    );
}
