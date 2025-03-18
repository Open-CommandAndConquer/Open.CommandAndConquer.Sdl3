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

using Open.CommandAndConquer.Sdl3.Exceptions.SubsystemExceptions.VideoSubsystemExceptions;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3.Subsystems.VideoSubsystems;

public class Clipboard
{
    private string? _text;

    internal Clipboard() { }

    public void SetText(string text)
    {
        if (!SDL_SetClipboardText(text))
        {
            throw new ClipboardException(SDL_GetError());
        }

        _text = text;
    }

    public string? GetText()
    {
        if (!SDL_HasClipboardText())
        {
            return null;
        }

        _text = SDL_GetClipboardText();
        if (string.IsNullOrEmpty(_text))
        {
            throw new ClipboardException(SDL_GetError());
        }

        return _text;
    }
}
