using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExtremeOsc.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    internal partial class OscPacketSourceGenerator : IIncrementalGenerator
    {
        public const string OscPackableAttributeName = "ExtremeOsc.Annotations.OscPackableAttribute";
        public const string OscElementAtAtributeName = "ExtremeOsc.Annotations.OscElementAtAttribute";
        public const string OscCallbackAttributeName = "ExtremeOsc.Annotations.OscCallbackAttribute";
        public const string OscReceiverAttributeName = "ExtremeOsc.Annotations.OscReceiverAttribute";

        // Osc Types
        public const string TypeInt32 = "int";
        public const string TypeInt64 = "long";
        public const string TypeFloat = "float";
        public const string TypeString = "string";
        public const string TypeBlob = "byte[]";
        public const string TypeDouble = "double";
        public const string TypeChar = "char";
        public const string TypeTimetag = "DateTime";
        public const string TypeBoolean = "bool";
        public const string TypeNil = "Nil";
        public const string TypeInfinitum = "Infinitum";

        // OscTags
        public static readonly string TagIntro = ((byte)(',')).ToString("D");
        public static readonly string TagInt32 = ((byte)('i')).ToString("D");
        public static readonly string TagInt64 = ((byte)('h')).ToString("D");
        public static readonly string TagFloat = ((byte)('f')).ToString("D");
        public static readonly string TagDouble = ((byte)('d')).ToString("D");
        public static readonly string TagString = ((byte)('s')).ToString("D");
        public static readonly string TagBlob = ((byte)('b')).ToString("D");
        public static readonly string TagChar = ((byte)('c')).ToString("D");
        public static readonly string TagTimetag = ((byte)('t')).ToString("D");
        public static readonly string TagBooleanTrue = ((byte)('T')).ToString("D");
        public static readonly string TagBooleanFalse = ((byte)('F')).ToString("D");
        public static readonly string TagNil = ((byte)('N')).ToString("D");
        public static readonly string TagInfinitum = ((byte)('I')).ToString("D");

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var packableTypes = context.GetSourceWithAttribute(OscPackableAttributeName);
            context.RegisterSourceOutput(packableTypes, EmitPackable);
        }

        public static string MembersToArraySyntax(IEnumerable<(ITypeSymbol, ISymbol, int)> members)
        {
            var arrayExpression = string.Join(
                ", ",
                members
                    .Select(m =>
                    {
                        var (typeSymbol, symbol, _) = m;
                        return typeSymbol.ToDisplayString() switch
                        {
                            TypeInt32 => TagInt32,
                            TypeInt64 => TagInt64,
                            TypeFloat => TagFloat,
                            TypeString => TagString,
                            TypeBlob => TagBlob,
                            TypeDouble => TagDouble,
                            TypeChar => TagChar,
                            TypeTimetag => TagTimetag,
                            TypeBoolean => TagBooleanFalse,
                            TypeNil => TagNil,
                            TypeInfinitum => TagInfinitum,
                            _ => throw new NotSupportedTypeException($"Invalid type {typeSymbol.ToDisplayString()}")
                        };
                    }
                ));

            return $"{{ {TagIntro}, {arrayExpression} }}";
        }

        public static void WriteMember(CodeBuilder builder, (ITypeSymbol, ISymbol, int) member, string @offset, string @offsetTagType)
        {
            var (typeSymbol, symbol, _) = member;
            var type = typeSymbol.ToDisplayString();
            var @name = symbol.Name;

            string line = type switch
            {
                TypeInt32 => $"OscWriter.WriteInt32(buffer, {@name}, ref {@offset});",
                TypeInt64 => $"OscWriter.WriteInt64(buffer, {@name}, ref {@offset});",
                TypeFloat => $"OscWriter.WriteFloat(buffer, {@name}, ref {@offset});",
                TypeString => $"OscWriter.WriteStringUtf8(buffer, {@name}, ref {@offset});",
                TypeBlob => $"OscWriter.WriteBlob(buffer, {@name}, ref {@offset});",
                TypeDouble => $"OscWriter.WriteDouble(buffer, {@name}, ref {@offset});",
                TypeChar => $"OscWriter.WriteChar(buffer, {@name}, ref {@offset});",
                TypeTimetag => $"OscWriter.WriteTimetag(buffer, {@name}, ref {@offset});",
                TypeBoolean => $"OscWriter.WriteBoolean(buffer, {@name}, {@offsetTagType});",
                TypeNil => $"OscWriter.WriteNil(buffer, {@offsetTagType});",
                TypeInfinitum => $"OscWriter.WriteInfinitum(buffer, {@offsetTagType});",
                _ => throw new NotSupportedTypeException($"Invalid type {type}")
            };

            builder.AppendLine(line);
            builder.AppendLine($"{@offsetTagType}++;");
        }

        public static void ReadMember(CodeBuilder builder, (ITypeSymbol, ISymbol, int) member, string @offset, string @offsetTagType)
        {
            var (typeSymbol, symbol, _) = member;
            var type = typeSymbol.ToDisplayString();
            var @name = symbol.Name;
            string line = type switch
            {
                TypeInt32 => $"this.{@name} = OscReader.ReadInt32(buffer, ref {@offset});",
                TypeInt64 => $"this.{@name} = OscReader.ReadInt64(buffer, ref {@offset});",
                TypeFloat => $"this.{@name} = OscReader.ReadFloat(buffer, ref {@offset});",
                TypeString => $"this.{@name} = OscReader.ReadString(buffer, ref {@offset});",
                TypeBlob => $"this.{@name} = OscReader.ReadBlob(buffer, ref {@offset});",
                TypeDouble => $"this.{@name} = OscReader.ReadDouble(buffer, ref {@offset});",
                TypeChar => $"this.{@name} = OscReader.ReadChar(buffer, ref {@offset});",
                TypeTimetag => $"this.{@name} = OscReader.ReadTimetag(buffer, ref {@offset});",
                TypeBoolean => $"this.{@name} = OscReader.ReadBoolean(buffer, {@offsetTagType});",
                TypeNil => $"this.{@name} = OscReader.ReadNil(buffer, {@offsetTagType});",
                TypeInfinitum => $"this.{@name} = OscReader.ReadInfinitum(buffer, {@offsetTagType});",
                _ => throw new NotSupportedTypeException($"Invalid type {type}")
            };
            builder.AppendLine(line);
            builder.AppendLine($"{@offsetTagType}++;");
        }

        public static void EmitPackable(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
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

            if(SyntaxCheck.IsAbstract(typeDeclaration))
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
                if(elementIndex is null)
                {
                    continue;
                }

                switch(member)
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

            if(elements.Count < 1)
            {
                return;
            }

            // sort by index
            elements.Sort((a, b) => a.Item3 - b.Item3);

            // check index is sequencial
            for(int i = 0; i < elements.Count; i++)
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
                    builder.AppendLine($"public static readonly byte[] TagTypes = new byte[] {MembersToArraySyntax(elements)};");
                    builder.AppendLine();

                    using (var @packMethod = builder.BeginScope("public void Pack (byte[] buffer, ref int offset)"))
                    {
                        builder.AppendLine("int offsetTagTypes = offset + 1;");
                        builder.AppendLine("OscWriter.WriteString(buffer, TagTypes, ref offset);");

                        for (int i = 0; i < elements.Count; i++)
                        {
                            WriteMember(builder, elements[i], "offset", "offsetTagTypes");
                        }
                    }
                    builder.AppendLine();

                    using (var @unpackMethod = builder.BeginScope("public void Unpack (byte[] buffer, ref int offset)"))
                    {
                        builder.AppendLine("int offsetTagTypes = offset + 1;");
                        builder.AppendLine("OscReader.ReadString(buffer, ref offset);");

                        for (int i = 0; i < elements.Count; i++)
                        {
                            ReadMember(builder, elements[i], "offset", "offsetTagTypes");
                        }
                    }
                }
            }

            Console.WriteLine(builder.ToString());
            context.AddSource($"{fullTypeName}.g.cs", builder.ToString());
        }
    }
}
