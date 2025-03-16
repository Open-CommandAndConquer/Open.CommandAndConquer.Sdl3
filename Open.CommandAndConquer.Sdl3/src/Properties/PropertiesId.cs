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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Open.CommandAndConquer.Sdl3.Exceptions;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3.Properties;

public record PropertiesId(uint Value)
{
    public static PropertiesId Invalid => new(0U);

    public static PropertiesId CreateGlobal()
    {
        var result = SDL_GetGlobalProperties().Value;
        if (result == 0U)
        {
            throw new PropertiesException(SDL_GetError());
        }

        return new PropertiesId(result);
    }

    public static PropertiesId Create()
    {
        var result = SDL_CreateProperties().Value;
        if (result == 0U)
        {
            throw new PropertiesException(SDL_GetError());
        }

        return new PropertiesId(result);
    }

    private SDL_PropertiesID ToNative() => new(Value);

    public bool HasProperty(string name) => SDL_HasProperty(ToNative(), name);

    public PropertyType GetPropertyType(string name) => SDL_GetPropertyType(ToNative(), name);

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, IntPtr value, Action<IntPtr> cleanup)
    {
        unsafe
        {
            if (
                !SDL_SetPointerPropertyWithCleanup(
                    ToNative(),
                    name,
                    value,
                    (delegate* unmanaged[Cdecl]<void*, void*, void>)
                        Marshal.GetFunctionPointerForDelegate(
                            void (void* _, void* value1) => cleanup(new IntPtr(value1))
                        ),
                    IntPtr.Zero
                )
            )
            {
                throw new PropertiesException(SDL_GetError());
            }
        }
    }

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, IntPtr value)
    {
        if (!SDL_SetPointerProperty(ToNative(), name, value))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, string value)
    {
        if (!SDL_SetStringProperty(ToNative(), name, value))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, long value)
    {
        if (!SDL_SetNumberProperty(ToNative(), name, value))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, float value)
    {
        if (!SDL_SetFloatProperty(ToNative(), name, value))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Part of the public API"
    )]
    public void Set(string name, bool value)
    {
        if (!SDL_SetBooleanProperty(ToNative(), name, value))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    public void Set(
        ICollection<Dictionary<string, KeyValuePair<IntPtr, Action<IntPtr>>>> properties
    )
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            foreach ((string name, (IntPtr value, Action<IntPtr> cleanup)) in property)
            {
                Set(name, value, cleanup);
            }
        }

        SDL_UnlockProperties(native);
    }

    public void Set(ICollection<Dictionary<string, IntPtr>> properties)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            foreach ((string name, IntPtr value) in property)
            {
                Set(name, value);
            }
        }

        SDL_UnlockProperties(native);
    }

    public void Set(ICollection<KeyValuePair<string, string>> properties)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            Set(property.Key, property.Value);
        }

        SDL_UnlockProperties(native);
    }

    public void Set(ICollection<KeyValuePair<string, long>> properties)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            Set(property.Key, property.Value);
        }

        SDL_UnlockProperties(native);
    }

    public void Set(ICollection<KeyValuePair<string, float>> properties)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            Set(property.Key, property.Value);
        }

        SDL_UnlockProperties(native);
    }

    public void Set(ICollection<KeyValuePair<string, bool>> properties)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        foreach (var property in properties)
        {
            Set(property.Key, property.Value);
        }

        SDL_UnlockProperties(native);
    }

    public IntPtr Get(string name, IntPtr defaultValue = 0)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        var result = SDL_GetPointerProperty(ToNative(), name, defaultValue);
        SDL_UnlockProperties(native);
        return result;
    }

    public string? Get(string name, string? defaultValue = null)
    {
        var native = ToNative();
        if (!SDL_LockProperties(native))
        {
            throw new PropertiesException(SDL_GetError());
        }

        var result = SDL_GetStringProperty(ToNative(), name, defaultValue);
        SDL_UnlockProperties(native);
        return result;
    }

    public long Get(string name, long defaultValue = 0L) =>
        SDL_GetNumberProperty(ToNative(), name, defaultValue);

    public float Get(string name, float defaultValue = 0F) =>
        SDL_GetFloatProperty(ToNative(), name, defaultValue);

    public bool Get(string name, bool defaultValue = false) =>
        SDL_GetBooleanProperty(ToNative(), name, defaultValue);

    public void Enumerate(Action<PropertiesId, string> callback)
    {
        unsafe
        {
            if (
                !SDL_EnumerateProperties(
                    ToNative(),
                    (delegate* unmanaged[Cdecl]<IntPtr, SDL_PropertiesID, byte*, void>)
                        Marshal.GetFunctionPointerForDelegate(
                            void (void* _, SDL_PropertiesID props, byte* name) =>
                                callback(
                                    new PropertiesId(props.Value),
                                    Utf8StringMarshaller.ConvertToManaged(name) ?? string.Empty
                                )
                        ),
                    IntPtr.Zero
                )
            )
            {
                throw new PropertiesException(SDL_GetError());
            }
        }
    }

    public void Clear(string name)
    {
        if (!SDL_ClearProperty(ToNative(), name))
        {
            throw new PropertiesException(SDL_GetError());
        }
    }

    public void Destroy()
    {
        var native = ToNative();
        SDL_UnlockProperties(native);
        SDL_DestroyProperties(native);
    }

    public PropertiesId Copy(PropertiesId destination)
    {
        SDL_PropertiesID src = new(Value);
        SDL_PropertiesID dst = new(destination.Value);
        if (!SDL_CopyProperties(src, dst))
        {
            throw new PropertiesException(SDL_GetError());
        }

        return new PropertiesId(dst.Value);
    }
}
