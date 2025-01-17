// Copyright (c) Bottlenose Labs Inc. (https://github.com/bottlenoselabs). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

using System.Collections.Immutable;

namespace C2CS.ReadCodeC.Explore;

public class ExploreOptions
{
    public ImmutableHashSet<string> HeaderFilesBlocked { get; init; } = ImmutableHashSet<string>.Empty;

    public ImmutableHashSet<string> EnumConstantNamesAllowed { get; init; } = ImmutableHashSet<string>.Empty;

    public bool IsEnabledLocationFullPaths { get; init; }

    public bool IsEnabledEnumConstants { get; set; }

    public bool IsEnabledEnumsDangling { get; init; }

    public bool IsEnabledAllowNamesWithPrefixedUnderscore { get; init; }

    public bool IsEnabledSystemDeclarations { get; init; }

    public ImmutableHashSet<string> PassThroughTypeNames { get; init; } = ImmutableHashSet<string>.Empty;
}
