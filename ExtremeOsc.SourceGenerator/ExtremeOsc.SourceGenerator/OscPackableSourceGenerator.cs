using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExtremeOsc.SourceGenerator
{
    using static OscSyntax;
    using static PackableEmitter;

    [Generator(LanguageNames.CSharp)]
    internal partial class OscPackableSourceGenerator : IIncrementalGenerator
    {
        public const string OscPackableAttributeName = "ExtremeOsc.Annotations.OscPackableAttribute";
        public const string OscElementAtAtributeName = "ExtremeOsc.Annotations.OscElementAtAttribute";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var packableTypes = context.GetSourceWithAttribute(OscPackableAttributeName);
            context.RegisterSourceOutput(packableTypes, EmitPackable);
        }
        
        public static void EmitPackable(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            try
            {
                // check syntax, symbol
                var typeSymbol = source.TargetSymbol as INamedTypeSymbol;
                var typeDeclaration = source.TargetNode as TypeDeclarationSyntax;

                if (typeSymbol is null || typeDeclaration is null)
                {
                    return;
                }

                // check partial, abstract, root, and packable
                if (SyntaxCheck.IsPartial(typeDeclaration) == false)
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(DiagnosticConstants.MustBePartial, typeDeclaration.GetLocation(), typeSymbol.Name)
                        );
                    return;
                }

                if (SyntaxCheck.IsAbstract(typeDeclaration))
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(DiagnosticConstants.AbstractNotSuppoted, typeDeclaration.GetLocation(), typeSymbol.Name)
                        );
                    return;
                }

                if (SyntaxCheck.IsNested(typeDeclaration))
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(DiagnosticConstants.MustBeRoot, typeDeclaration.GetLocation(), typeSymbol.Name)
                        );
                    return;
                }

                if (SyntaxCheck.ExistsDefaultConstructor(typeDeclaration) == false && typeSymbol.TypeKind == TypeKind.Class)
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(DiagnosticConstants.DefaultConstructorNotImplemented, typeDeclaration.GetLocation(), typeSymbol.Name)
                        );
                    return;
                }

                // find namespace
                var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

                // find properties and fields
                var members = typeSymbol.FindMembers();
                var elements = new List<(ITypeSymbol, ISymbol, int)>();

                foreach (var member in members)
                {
                    var attributes = member.GetAttributes();

                    // filter OscElementAtAttribute
                    if (attributes.Length < 1)
                    {
                        continue;
                    }

                    // check OscElementAtAttribute
                    var atAttribute = attributes
                        .Where(attribute => attribute.AttributeClass?.ToDisplayString() == OscElementAtAtributeName)
                        .FirstOrDefault();

                    // null check
                    if (atAttribute is null)
                    {
                        continue;
                    }

                    // check Arguments
                    int? elementIndex = atAttribute.ConstructorArguments[0].Value as int?;
                    if (elementIndex is null)
                    {
                        continue;
                    }

                    switch (member)
                    {
                        case IFieldSymbol field:
                            elements.Add((field.Type, field, elementIndex.Value));
                            break;
                        case IPropertySymbol property:
                            elements.Add((property.Type, property, elementIndex.Value));
                            break;
                        default:
                            break;
                    }
                }

                if (elements.Count < 1)
                {
                    return;
                }

                // sort by index
                elements.Sort((a, b) => a.Item3 - b.Item3);

                // check index is sequencial
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].Item3 != i)
                    {
                        context.ReportDiagnostic(
                            Diagnostic.Create(DiagnosticConstants.ElementAtIndexIsNotSequencial, typeDeclaration.GetLocation())
                            );
                        return;
                    }
                }

                string fullTypeName = typeSymbol.Name;
                string typeKindName = typeSymbol.TypeKind switch
                {   
                    TypeKind.Class => "class",
                    TypeKind.Struct => "struct",
                    _ => throw new NotSupportedException($"Invalid type {typeSymbol.TypeKind}")
                };

                var builder = new CodeBuilder();
                builder.AppendLine("// <auto-generated>");
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Buffers;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using ExtremeOsc;");
                builder.AppendLine("");

                using (var @namespace = builder.BeginScope($"namespace {namespaceName}"))
                {
                    using (var @class = builder.BeginScope($"partial {typeKindName} {fullTypeName} : IOscPackable"))
                    {
                        builder.AppendLine($"// {MembersToTagTypes(elements)}");
                        builder.AppendLine($"public static readonly byte[] TagTypes = new byte[] {MembersToArraySyntax(elements)};");
                        builder.AppendLine();

                        using (var @packMethod = builder.BeginScope("public void Pack (byte[] buffer, ref int offset)"))
                        {
                            builder.AppendLine("int offsetTagTypes = offset + 1;");
                            builder.AppendLine("OscWriter.WriteString(buffer, TagTypes, ref offset);");

                            for (int i = 0; i < elements.Count; i++)
                            {
                                try
                                {
                                    WriteMember(builder, elements[i], "offset", "offsetTagTypes");
                                }
                                catch(NotSupportedException e)
                                {
                                    context.ReportDiagnostic(
                                        Diagnostic.Create(DiagnosticConstants.NotSupportedType, typeDeclaration.GetLocation(), elements[i].Item1.ToDisplayString())
                                        );
                                    throw e;
                                }
                            }
                        }
                        builder.AppendLine();

                        using (var @unpackMethod = builder.BeginScope("public void Unpack (byte[] buffer, ref int offset)"))
                        {
                            builder.AppendLine("int offsetTagTypes = offset + 1;");
                            builder.AppendLine("OscReader.ReadString(buffer, ref offset);");

                            for (int i = 0; i < elements.Count; i++)
                            {
                                try
                                {
                                    ReadMember(builder, elements[i], "offset", "offsetTagTypes");
                                }
                                catch (NotSupportedException e)
                                {
                                    context.ReportDiagnostic(
                                        Diagnostic.Create(DiagnosticConstants.NotSupportedType, typeDeclaration.GetLocation(), elements[i].Item1.ToDisplayString())
                                        );
                                    throw e;
                                }
                            }
                        }
                    }
                }

                Console.WriteLine(builder.ToString());
                context.AddSource($"{fullTypeName}.g.cs", builder.ToString());
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(DiagnosticConstants.ExceptionError, Location.None, ex.Message)
                    );
            }
        }
    }
}
