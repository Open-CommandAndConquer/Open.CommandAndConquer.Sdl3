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
using Open.CommandAndConquer.Sdl3.Geometry;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3.CustomMarshalling;

[CustomMarshaller(typeof(FRect), MarshalMode.Default, typeof(FRectMarshaller))]
[CustomMarshaller(typeof(FRect), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
internal static class FRectMarshaller
{
    public static SDL_FRect ConvertToUnmanaged(FRect managed) =>
        new()
        {
            x = managed.X,
            y = managed.Y,
            w = managed.W,
            h = managed.H,
        };

    public static FRect ConvertToManaged(SDL_FRect unmanaged) =>
        new(unmanaged.x, unmanaged.y, unmanaged.w, unmanaged.y);

    public unsafe ref struct ManagedToUnmanagedIn
    {
        private GCHandle _handle;

        public SDL_FRect* ToUnmanaged() => (SDL_FRect*)GCHandle.ToIntPtr(_handle);

        public void FromManaged(FRect? managed)
        {
            if (managed is null)
            {
                return;
            }

            SDL_FRect unmanaged = new()
            {
                x = managed.X,
                y = managed.Y,
                w = managed.W,
                h = managed.H,
            };

            _handle = GCHandle.Alloc(unmanaged, GCHandleType.Pinned);
        }

        public void Free()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
        }
    }
}
