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

namespace Open.CommandAndConquer.Sdl3.Imports;

internal static partial class SDL3
{
    public record struct SDL_BlendMode(uint Value)
    {
        public static implicit operator SDL_BlendMode(uint value) => new(value);

        public static implicit operator uint(SDL_BlendMode value) => value.Value;
    }

    public static SDL_BlendMode SDL_BLENDMODE_NONE => new(0x00000000U);
    public static SDL_BlendMode SDL_BLENDMODE_BLEND => new(0x00000001U);
    public static SDL_BlendMode SDL_BLENDMODE_BLEND_PREMULTIPLIED => new(0x00000010U);
    public static SDL_BlendMode SDL_BLENDMODE_ADD => new(0x00000002U);
    public static SDL_BlendMode SDL_BLENDMODE_ADD_PREMULTIPLIED => new(0x00000020U);
    public static SDL_BlendMode SDL_BLENDMODE_MOD => new(0x00000004U);
    public static SDL_BlendMode SDL_BLENDMODE_MUL => new(0x00000008U);
    public static SDL_BlendMode SDL_BLENDMODE_INVALID => new(0x7FFFFFFFU);

    public enum SDL_BlendOperation
    {
        SDL_BLENDOPERATION_ADD = 0x1,
        SDL_BLENDOPERATION_SUBTRACT = 0x2,
        SDL_BLENDOPERATION_REV_SUBTRACT = 0x3,
        SDL_BLENDOPERATION_MINIMUM = 0x4,
        SDL_BLENDOPERATION_MAXIMUM = 0x5,
    }

    public enum SDL_BlendFactor
    {
        SDL_BLENDFACTOR_ZERO = 0x1,
        SDL_BLENDFACTOR_ONE = 0x2,
        SDL_BLENDFACTOR_SRC_COLOR = 0x3,
        SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR = 0x4,
        SDL_BLENDFACTOR_SRC_ALPHA = 0x5,
        SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA = 0x6,
        SDL_BLENDFACTOR_DST_COLOR = 0x7,
        SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR = 0x8,
        SDL_BLENDFACTOR_DST_ALPHA = 0x9,
        SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA = 0xA,
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ComposeCustomBlendMode))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_BlendMode SDL_ComposeCustomBlendMode(
        SDL_BlendFactor srcColorFactor,
        SDL_BlendFactor dstColorFactor,
        SDL_BlendOperation colorOperation,
        SDL_BlendFactor srcAlphaFactor,
        SDL_BlendFactor dstAlphaFactor,
        SDL_BlendOperation alphaOperation
    );
}
