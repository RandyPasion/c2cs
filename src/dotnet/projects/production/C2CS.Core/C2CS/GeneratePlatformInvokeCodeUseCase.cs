// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory (https://github.com/lithiumtoast/c2cs) for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using ClangSharp;
using ClangSharp.Interop;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace C2CS
{
	public class GeneratePlatformInvokeCodeUseCase
	{
		private readonly CodeCSharpGenerator _codeGenerator;
		private readonly List<EnumDeclarationSyntax> _enums = new();
		private readonly CodeCExplorer _explorer = new();

		private readonly List<FieldDeclarationSyntax> _fields = new();
		private readonly List<MemberDeclarationSyntax> _functionPointers = new();
		private readonly CodeCStructLayoutCalculator _layoutCalculator = new();
		private readonly List<MethodDeclarationSyntax> _methods = new();
		private readonly List<StructDeclarationSyntax> _structs = new();

		public GeneratePlatformInvokeCodeUseCase(string libraryName)
		{
			_codeGenerator = new CodeCSharpGenerator(libraryName);
			_explorer.EnumFound += TranspileEnum;
			_explorer.FunctionFound += TranspileFunction;
			_explorer.RecordFound += TranspileRecord;
			_explorer.TypeAliasFound += TranspileTypeAlias;
			_explorer.FunctionProtoFound += TranspileFunctionProto;
		}

		public string GenerateCode(TranslationUnit translationUnit, string libraryName, IEnumerable<string>? includeDirectories)
		{
			_explorer.Explore(translationUnit, includeDirectories);

			const string comment = @"
//-------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the following tool:
//        https://github.com/lithiumtoast/c2cs
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ReSharper disable All
//-------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;";

			var commentFormatted = comment.TrimStart() + "\r\n";

			var className = Path.GetFileNameWithoutExtension(libraryName);

			var membersBuilder = ImmutableArray.CreateBuilder<MemberDeclarationSyntax>();
			membersBuilder.AddRange(_fields);
			membersBuilder.AddRange(_enums);
			membersBuilder.AddRange(_structs);
			membersBuilder.AddRange(_methods);
			membersBuilder.AddRange(_functionPointers);
			var members = membersBuilder.ToImmutableArray();

			var @class = _codeGenerator.CreatePInvokeClass(className, members)
				.WithLeadingTrivia(SyntaxFactory.Comment(commentFormatted));

			return @class
				.Format()
				.ToFullString();
		}

		private void TranspileRecord(RecordDecl recordC)
		{
			var isTypeForward = recordC != recordC.Definition;
			if (isTypeForward)
			{
				return;
			}

			var name = recordC.Name;
			if (string.IsNullOrEmpty(recordC.Name))
			{
				name = recordC.TypeForDecl.AsString;
			}

			var @struct = _codeGenerator.CreateStruct(name, recordC, _layoutCalculator);
			_structs.Add(@struct);
		}

		private void TranspileFunction(FunctionDecl functionC)
		{
			var method = _codeGenerator.CreateExternMethod(functionC);
			_methods.Add(method);
		}

		private void TranspileConstant(EnumConstantDecl constantC)
		{
			var constant = _codeGenerator.CreateConstant(constantC);
			_fields.Add(constant);
		}

		private void TranspileEnum(EnumDecl enumC)
		{
			var name = enumC.Name;
			if (string.IsNullOrEmpty(name))
			{
				name = enumC.TypeForDecl.AsString;
			}

			if (string.IsNullOrEmpty(name))
			{
				foreach (var constantC in enumC.Enumerators)
				{
					TranspileConstant(constantC);
				}
			}
			else
			{
				var @enum = _codeGenerator.CreateEnum(enumC);
				_enums.Add(@enum);
			}
		}

		private void TranspileTypeAlias(TypedefDecl typeAlias)
		{
			if (typeAlias.UnderlyingType is PointerType pointerType && pointerType.PointeeType.Kind == CXTypeKind.CXType_Void)
			{
				var @struct = _codeGenerator.CreateOpaqueStruct(typeAlias);
				_structs.Add(@struct);
			}
			else
			{
				_codeGenerator.AddAlias(typeAlias);
			}
		}

		private void TranspileFunctionProto(FunctionProtoType functionProto)
		{
			// TODO: Create delegate / function pointer; https://github.com/lithiumtoast/c2cs/issues/2
		}
	}
}
