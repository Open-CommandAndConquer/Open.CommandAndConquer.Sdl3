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
    public record struct SDL_PropertiesID(uint Value)
    {
        public static SDL_PropertiesID Invalid => new(0U);

        public static implicit operator uint(SDL_PropertiesID id) => id.Value;

        public static implicit operator SDL_PropertiesID(uint value) => new(value);
    }

    public enum SDL_PropertyType
    {
        SDL_PROPERTY_TYPE_INVALID,
        SDL_PROPERTY_TYPE_POINTER,
        SDL_PROPERTY_TYPE_STRING,
        SDL_PROPERTY_TYPE_NUMBER,
        SDL_PROPERTY_TYPE_FLOAT,
        SDL_PROPERTY_TYPE_BOOLEAN,
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetGlobalProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_GetGlobalProperties();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_CreateProperties();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CopyProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_CopyProperties(SDL_PropertiesID src, SDL_PropertiesID dst);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LockProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_LockProperties(SDL_PropertiesID props);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_UnlockProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_UnlockProperties(SDL_PropertiesID props);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SDL_CleanupPropertyCallback(IntPtr userdata, IntPtr value);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetPointerPropertyWithCleanup),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetPointerPropertyWithCleanup(
        SDL_PropertiesID props,
        string name,
        IntPtr value,
        SDL_CleanupPropertyCallback cleanup,
        IntPtr userdata
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetPointerProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetPointerProperty(
        SDL_PropertiesID props,
        string name,
        IntPtr value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetStringProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetStringProperty(
        SDL_PropertiesID props,
        string name,
        string value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetNumberProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetNumberProperty(
        SDL_PropertiesID props,
        string name,
        long value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetFloatProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetFloatProperty(
        SDL_PropertiesID props,
        string name,
        float value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetBooleanProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_SetBooleanProperty(
        SDL_PropertiesID props,
        string name,
        [MarshalAs(UnmanagedType.I1)] bool value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_HasProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_HasProperty(SDL_PropertiesID props, string name);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPropertyType),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertyType SDL_GetPropertyType(SDL_PropertiesID props, string name);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPointerProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr SDL_GetPointerProperty(
        SDL_PropertiesID props,
        string name,
        IntPtr default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetStringProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(UnownedUtf8StringMarshaller))]
    public static partial string? SDL_GetStringProperty(
        SDL_PropertiesID props,
        string name,
        string? default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetNumberProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SDL_GetNumberProperty(
        SDL_PropertiesID props,
        string name,
        long default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetFloatProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float SDL_GetFloatProperty(
        SDL_PropertiesID props,
        string name,
        float default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetBooleanProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_GetBooleanProperty(
        SDL_PropertiesID props,
        string name,
        [MarshalAs(UnmanagedType.I1)] bool default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_ClearProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SDL_ClearProperty(SDL_PropertiesID props, string name);

    public delegate void SDL_EnumeratePropertiesCallback(
        IntPtr userdata,
        SDL_PropertiesID props,
        string name
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_EnumerateProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static unsafe partial bool INTERNAL_SDL_EnumerateProperties(
        SDL_PropertiesID props,
        delegate* unmanaged[Cdecl]<IntPtr, SDL_PropertiesID, byte*, void> callback,
        IntPtr userdata
    );

    public static bool SDL_EnumerateProperties(
        SDL_PropertiesID props,
        SDL_EnumeratePropertiesCallback callback,
        IntPtr userdata
    )
    {
        unsafe
        {
            return INTERNAL_SDL_EnumerateProperties(
                props,
                (delegate* unmanaged[Cdecl]<IntPtr, SDL_PropertiesID, byte*, void>)
                    Marshal.GetFunctionPointerForDelegate(
                        void (
                            void* userdataCallback,
                            SDL_PropertiesID propsCallback,
                            byte* nameCallback
                        ) =>
                            callback(
                                new IntPtr(userdataCallback),
                                propsCallback,
                                Utf8StringMarshaller.ConvertToManaged(nameCallback) ?? string.Empty
                            )
                    ),
                userdata
            );
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroyProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_DestroyProperties(SDL_PropertiesID props);
}
