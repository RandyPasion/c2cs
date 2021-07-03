﻿// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

using System;
using System.Runtime.InteropServices;

namespace C2CS
{
    public static partial class Runtime
    {
        /// <summary>
        ///     Gets the current <see cref="RuntimePlatform" />.
        /// </summary>
        public static RuntimePlatform Platform
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return RuntimePlatform.Windows;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return RuntimePlatform.macOS;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return RuntimePlatform.Linux;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                {
                    return RuntimePlatform.Linux;
                }

                return RuntimePlatform.Unknown;
            }
        }

        /// <summary>
        ///     Gets the library file name extension given a <see cref="RuntimePlatform" />.
        /// </summary>
        /// <param name="runtimePlatform">The runtime platform.</param>
        /// <returns>A <see cref="string" /> containing the library file name extension for the <paramref name="runtimePlatform" />.</returns>
        /// <exception cref="NotImplementedException"><paramref name="runtimePlatform" /> is not available yet with .NET 5.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="runtimePlatform"/> is not a known valid value.</exception>
        public static string LibraryFileNameExtension(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Windows:
                    return ".dll";
                case RuntimePlatform.macOS:
                case RuntimePlatform.iOS:
                    return ".dylib";
                case RuntimePlatform.Linux:
                case RuntimePlatform.FreeBSD:
                case RuntimePlatform.Android:
                    return ".so";
                case RuntimePlatform.Web:
                    throw new NotImplementedException();
                case RuntimePlatform.Unknown:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(runtimePlatform), runtimePlatform, null);
            }
        }

        /// <summary>
        ///     Gets the library file name prefix for a <see cref="RuntimePlatform" />.
        /// </summary>
        /// <param name="runtimePlatform">The runtime platform.</param>
        /// <returns>A <see cref="string" /> containing the library file name prefix for the <paramref name="runtimePlatform" />.</returns>
        /// <exception cref="NotImplementedException"><paramref name="runtimePlatform" /> is not available yet with .NET 5.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="runtimePlatform"/> is not a known valid value.</exception>
        public static string LibraryFileNamePrefix(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Windows:
                    return string.Empty;
                case RuntimePlatform.macOS:
                case RuntimePlatform.iOS:
                case RuntimePlatform.Linux:
                case RuntimePlatform.FreeBSD:
                case RuntimePlatform.Android:
                    return "lib";
                case RuntimePlatform.Web:
                    throw new NotImplementedException();
                case RuntimePlatform.Unknown:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(runtimePlatform), runtimePlatform, null);
            }
        }
    }
}