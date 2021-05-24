// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

/// <summary>
///     Unix time; the number of seconds that have elapsed since the epoch, minus leap seconds. The Unix epoch is 00:00:00 UTC on 1 January 1970.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "C style.")]
[SuppressMessage("ReSharper", "IdentifierTypo", Justification = "C style.")]
public struct time_t
{
    /// <summary>
    ///     The number of seconds that have elapsed since 00:00:00 UTC on 1 January 1970.
    /// </summary>
    public long Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(time_t value) => value.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator time_t(long value) => new() {Value = value};
}