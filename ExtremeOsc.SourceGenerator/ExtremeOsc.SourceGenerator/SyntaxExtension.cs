﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal static class SyntaxExtension
    {
        public static IncrementalValuesProvider<GeneratorAttributeSyntaxContext> GetSourceWithAttribute(this IncrementalGeneratorInitializationContext context, string attributeName)
        {
            return context.SyntaxProvider.ForAttributeWithMetadataName(
                attributeName,
                (_, _) => true,
                (context, _) => context
                );
        }

        public static ImmutableArray<ISymbol> FindMembers(this INamedTypeSymbol symbol)
        {
            // find assignable
            return symbol.GetMembers()
                .Where(member => member is IPropertySymbol || member is IFieldSymbol)
                .Where(member => member.IsAbstract == false)
                .Where(member => member.IsOverride == false)
                .Where(member => member.IsStatic == false)
                .ToImmutableArray();
        }

        public static ImmutableArray<IMethodSymbol> FindMethods(this INamedTypeSymbol symbol)
        {
            // find callable
            return symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(member => member.IsAbstract == false)
                .Where(member => member.MethodKind == MethodKind.Ordinary)
                .ToImmutableArray();
        }
    }
}
