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

using System.Runtime.InteropServices.Marshalling;
using Open.CommandAndConquer.Sdl3.CustomMarshalling;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3.Geometry;

[NativeMarshalling(typeof(RectMarshaller))]
public record Rect(int X, int Y, int W, int H)
{
    public bool IsEmpty => W <= 0 || H <= 0;

    public bool HasIntersection(Rect other) => SDL_HasRectIntersection(this, other);

    public bool TryGetIntersection(Rect other, out Rect result) =>
        SDL_GetRectIntersection(this, other, out result);

    public bool TryGetUnion(Rect other, out Rect result) =>
        SDL_GetRectUnion(this, other, out result);

    public bool TryGetLineIntersection(
        (Point Point1, Point Point2) line,
        out (Point Point1, Point Point2) result
    )
    {
        var x1 = line.Point1.X;
        var y1 = line.Point1.Y;
        var x2 = line.Point2.X;
        var y2 = line.Point2.Y;
        var achieved = SDL_GetRectAndLineIntersection(this, ref x1, ref y1, ref x2, ref y2);
        result = (Point1: new Point(x1, y1), Point2: new Point(x2, y2));
        return achieved;
    }

    public FRect ToFRect() => new(X, Y, W, H);
}
