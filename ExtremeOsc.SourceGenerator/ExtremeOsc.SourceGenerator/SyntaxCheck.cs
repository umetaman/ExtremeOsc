using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
