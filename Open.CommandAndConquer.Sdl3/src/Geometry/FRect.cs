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

[NativeMarshalling(typeof(FRectMarshaller))]
public record FRect(float X, float Y, float W, float H)
{
    public bool IsEmpty => W <= 0 || H <= 0;

    public bool HasIntersection(FRect other) => SDL_HasRectIntersectionFloat(this, other);

    public bool TryGetIntersection(FRect other, out FRect result) =>
        SDL_GetRectIntersectionFloat(this, other, out result);

    public bool TryGetUnion(FRect other, out FRect result) =>
        SDL_GetRectUnionFloat(this, other, out result);

    public bool TryGetLineIntersection(
        (FPoint Point1, FPoint Point2) line,
        out (FPoint Point1, FPoint Point2) result
    )
    {
        var x1 = line.Point1.X;
        var y1 = line.Point1.Y;
        var x2 = line.Point2.X;
        var y2 = line.Point2.Y;
        var achieved = SDL_GetRectAndLineIntersectionFloat(this, ref x1, ref y1, ref x2, ref y2);
        result = (Point1: new FPoint(x1, y1), Point2: new FPoint(x2, y2));
        return achieved;
    }
}
