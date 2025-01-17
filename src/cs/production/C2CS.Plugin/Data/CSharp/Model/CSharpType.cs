// Copyright (c) Bottlenose Labs Inc. (https://github.com/bottlenoselabs). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

namespace C2CS.Data.CSharp.Model;

public sealed class CSharpType : IEquatable<CSharpType>
{
    public string Name { get; init; } = string.Empty;

    public string? OriginalName { get; init; }

    public int SizeOf { get; init; }

    public int? AlignOf { get; init; }

    public int? ArraySizeOf { get; init; }

    public bool IsArray => ArraySizeOf > 0;

    public bool Equals(CSharpType? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        var isEqual = Name == other.Name &&
                      SizeOf == other.SizeOf &&
                      AlignOf == other.AlignOf &&
                      ArraySizeOf == other.ArraySizeOf;
        return isEqual;
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? string.Empty : Name;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as CSharpType);
    }

    public static bool operator ==(CSharpType? a, CSharpType? b)
    {
        if (a is null)
        {
            if (b is null)
            {
                return true;
            }

            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(CSharpType? a, CSharpType? b) => !(a == b);

    public override int GetHashCode()
    {
        var hashCode = HashCode.Combine(Name, SizeOf, AlignOf, ArraySizeOf);
        return hashCode;
    }
}
