using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            return symbol.GetMembers().Where(member => member is IPropertySymbol || member is IFieldSymbol).ToImmutableArray();
        }

        public static ImmutableArray<ISymbol> FindMethods(this INamedTypeSymbol symbol)
        {
            return symbol.GetMembers().Where(member => member is IMethodSymbol).ToImmutableArray();
        }
    }
}
