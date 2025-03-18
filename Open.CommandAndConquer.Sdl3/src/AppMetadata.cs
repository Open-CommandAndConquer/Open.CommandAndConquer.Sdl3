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

using Open.CommandAndConquer.Sdl3.Exceptions;
using static Open.CommandAndConquer.Sdl3.Imports.SDL3;

namespace Open.CommandAndConquer.Sdl3;

public static class AppMetadata
{
    public static void Set(string? appName, string? appVersion, string? appIdentifier)
    {
        if (!SDL_SetAppMetadata(appName, appVersion, appIdentifier))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static void SetName(string value)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_NAME_STRING, value))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetName() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_NAME_STRING);

    public static void SetVersion(Version version)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_VERSION_STRING, version.ToString()))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static void SetVersion(string version)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_VERSION_STRING, version))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetVersion() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_VERSION_STRING);

    public static void SetIdentifier(string identifier)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_IDENTIFIER_STRING, identifier))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetIdentifier() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_IDENTIFIER_STRING);

    public static void SetCreator(string creator)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_CREATOR_STRING, creator))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetCreator() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_CREATOR_STRING);

    public static void SetCopyright(string copyright)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_COPYRIGHT_STRING, copyright))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetCopyright() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_COPYRIGHT_STRING);

    public static void SetUrl(string url)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_URL_STRING, url))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetUrl() => SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_URL_STRING);

    public static void SetAppType(string type)
    {
        if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_TYPE_STRING, type))
        {
            throw new AppMetadataException(SDL_GetError());
        }
    }

    public static string? GetAppType() =>
        SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_TYPE_STRING);
}
