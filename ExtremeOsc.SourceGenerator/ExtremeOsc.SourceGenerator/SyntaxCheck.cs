using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExtremeOsc.SourceGenerator
{
    internal static class SyntaxCheck
    {
        public static bool IsPartial(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);
        }

        public static bool IsAbstract(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword);
        }

        public static bool IsNested(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax.Parent is TypeDeclarationSyntax;
        }

        public static bool IsPackable(ITypeSymbol? typeSymbol)
        {
            if (typeSymbol is null)
            {
                return false;
            }

            return typeSymbol.GetAttributes().Any(attr => attr.AttributeClass?.ToDisplayString() == OscPackableSourceGenerator.OscPackableAttributeName);
        }

        public static bool ExistsDefaultConstructor(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax.Members
                .OfType<ConstructorDeclarationSyntax>()
                .Any(ctor => ctor.Modifiers.Any(SyntaxKind.PublicKeyword) && ctor.ParameterList.Parameters.Count == 0);
        }

        public static bool IsPrimitive(ISymbol? typeSymbol)
        {
            if (typeSymbol is null)
            {
                return false;
            }

            return typeSymbol.ToDisplayString() switch
            {
                OscSyntax.TypeInt32 => true,
                OscSyntax.TypeInt64 => true,
                OscSyntax.TypeFloat => true,
                OscSyntax.TypeString => true,
                OscSyntax.TypeBlob => true,
                OscSyntax.TypeDouble => true,
                OscSyntax.TypeChar => true,
                OscSyntax.TypeTimeTag => true,
                OscSyntax.TypeBoolean => true,
                OscSyntax.TypeNil => true,
                OscSyntax.TypeInfinitum => true,
                OscSyntax.TypeColor32 => true,
                _ => false
            };
        }

        public static bool HasTimestamp(IMethodSymbol methodSymbol)
        {
            var secondParameter = methodSymbol.Parameters.ElementAtOrDefault(1);
            if (secondParameter is null)
            {
                return false;
            }
            
            return secondParameter.Type.ToDisplayString() == OscSyntax.TypeTimeTag
                && secondParameter.Name == "timestamp";
        }

        public static bool IsPrimitiveOnly(IMethodSymbol methodSymbol, bool hasTimestamp)
        {
            bool isPrimitive = methodSymbol.Parameters
                .Skip(hasTimestamp ? 2 : 1)
                .All(p => IsPrimitive(p.Type));

            return isPrimitive && IsObjectArrayOnly(methodSymbol) == false;
        }

        public static bool IsObjectArrayOnly(IMethodSymbol methodSymbol)
        {
            var lastParameter = methodSymbol.Parameters.LastOrDefault();
            if (lastParameter is null)
            {
                return false;
            }

            return lastParameter.Type.ToDisplayString() == "object[]";
        }

        public static bool IsReaderOnly(IMethodSymbol methodSymbol)
        {
            var lastParameter = methodSymbol.Parameters.LastOrDefault();
            if (lastParameter is null)
            {
                return false;
            }

            return lastParameter.Type.ToDisplayString() == "ExtremeOsc.OscReader";
        }

        public static bool IsNoArgument(IMethodSymbol methodSymbol, bool hasTimestamp)
        {
            if (hasTimestamp)
            {
                var secondParameter = methodSymbol.Parameters.ElementAtOrDefault(1);
                if (secondParameter is null)
                {
                    return false;
                }
                
                if (secondParameter.Type.ToDisplayString() == "ulong" && secondParameter.Name == "timestamp")
                {
                    return methodSymbol.Parameters.Length == 2;
                }
                
                return false;
            }
            else
            {
                return methodSymbol.Parameters.Length == 1;
            }
        }

        public static bool IsSingleArgument(IMethodSymbol methodSymbol, bool hasTimestamp)
        {
            return hasTimestamp
                ? methodSymbol.Parameters.Length == 3
                : methodSymbol.Parameters.Length == 2;
        }
    }
}
