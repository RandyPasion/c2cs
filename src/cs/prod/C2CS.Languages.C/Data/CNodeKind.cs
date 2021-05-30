// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

namespace C2CS.Languages.C
{
    public enum CNodeKind
    {
        Unknown = 0,
        Function,
        FunctionResult,
        FunctionParameter,
        PointerFunction,
        PointerFunctionResult,
        PointerFunctionParameter,
        Record,
        RecordField,
        Enum,
        EnumValue,
        OpaqueType,
        Typedef,
        Variable,
    }
}