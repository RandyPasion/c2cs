// Copyright (c) Bottlenose Labs Inc. (https://github.com/bottlenoselabs). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

using C2CS.Data.C.Model;
using Microsoft.Extensions.Logging;
using static bottlenoselabs.clang;

namespace C2CS.ReadCodeC.Explore;

public abstract class ExploreHandler<TNode> : ExploreHandler
    where TNode : CNode
{
    protected ExploreHandler(ILogger<ExploreHandler<TNode>> logger, bool logAlreadyExplored = true)
        : base(logger, logAlreadyExplored)
    {
    }
}

public abstract partial class ExploreHandler
{
    private readonly bool _logAlreadyExplored;
    private readonly ILogger<ExploreHandler> _logger;
    private readonly Dictionary<string, CLocation> _visitedNames = new();

    protected ExploreHandler(ILogger<ExploreHandler> logger, bool logAlreadyExplored = true)
    {
        _logger = logger;
        _logAlreadyExplored = logAlreadyExplored;
    }

    protected abstract ExploreKindCursors ExpectedCursors { get; }

    protected abstract ExploreKindTypes ExpectedTypes { get; }

    internal CNode ExploreInternal(ExploreContext context, ExploreInfoNode info)
    {
        LogExploring(info.Kind, info.Name, info.Location);
        var result = Explore(context, info);
        LogSuccess(info.Kind, info.Name, info.Location);
        return result;
    }

    internal bool CanVisitInternal(ExploreContext context, ExploreInfoNode info)
    {
        if (string.IsNullOrEmpty(info.Name))
        {
            throw new NotImplementedException();
        }

        if (!IsExpectedCursor(info))
        {
            LogFailureUnexpectedCursor(info.Cursor.kind);
            return false;
        }

        if (!IsExpectedType(info))
        {
            LogFailureUnexpectedType(info.Type.kind);
            return false;
        }

        if (!context.IsAllowed(info.Kind, info.Name, info.Cursor, info.Type))
        {
            return false;
        }

        if (IsAlreadyVisited(info.Name, out var firstLocation))
        {
            if (_logAlreadyExplored)
            {
                LogAlreadyVisited(info.Kind, info.Name, firstLocation);
            }

            return false;
        }

        if (!CanVisit(context, info.Name, info.Parent))
        {
            return false;
        }

        MarkAsVisited(info);
        return true;
    }

    internal bool IsBlocked(
        ExploreContext context,
        CKind kind,
        string name,
        CXCursor cursor,
        CXType type,
        ExploreInfoNode? parentInfo)
    {
        if (!context.IsAllowed(kind, name, cursor, type))
        {
            return true;
        }

        if (!CanVisit(context, name, parentInfo))
        {
            return true;
        }

        return false;
    }

    protected virtual bool CanVisit(ExploreContext context, string name, ExploreInfoNode? parentInfo)
    {
        return true;
    }

    private bool IsExpectedCursor(ExploreInfoNode info)
    {
        if (ExpectedCursors.Matches(info.Cursor.kind))
        {
            return true;
        }

        return false;
    }

    private bool IsExpectedType(ExploreInfoNode info)
    {
        var typeKind = info.Type.kind;
        if (ExpectedTypes.Matches(typeKind))
        {
            return true;
        }

        return false;
    }

    private bool IsAlreadyVisited(string name, out CLocation firstLocation)
    {
        var result = _visitedNames.TryGetValue(name, out firstLocation);
        return result;
    }

    private void MarkAsVisited(ExploreInfoNode info)
    {
        _visitedNames.Add(info.Name, info.Location);
    }

    public abstract CNode Explore(ExploreContext context, ExploreInfoNode info);

    [LoggerMessage(0, LogLevel.Error, "- Unexpected cursor kind '{Kind}'")]
    private partial void LogFailureUnexpectedCursor(CXCursorKind kind);

    [LoggerMessage(1, LogLevel.Error, "- Unexpected type kind '{Kind}'")]
    private partial void LogFailureUnexpectedType(CXTypeKind kind);

    [LoggerMessage(2, LogLevel.Debug, "- Exploring {Kind} '{Name}' ({Location})'")]
    public partial void LogExploring(CKind kind, string name, CLocation location);

    [LoggerMessage(3, LogLevel.Error, "- Already visited {Kind} '{Name}' ({Location})")]
    public partial void LogAlreadyVisited(CKind kind, string name, CLocation location);

    [LoggerMessage(4, LogLevel.Debug, "- Explored {Kind} '{Name}' ({Location})'")]
    public partial void LogSuccess(CKind kind, string name, CLocation location);
}