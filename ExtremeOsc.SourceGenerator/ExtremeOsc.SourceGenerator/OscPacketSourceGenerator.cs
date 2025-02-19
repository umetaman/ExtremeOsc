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
        public const string TypeDouble = "double";
        public const string TypeString = "string";
        public const string TypeBoolean = "bool";
        public const string TypeBlob = "byte[]";
        public const string TypeChar = "char";
        public const string TypeByte = "byte";

        // OscTags
        public static readonly string TagInt32 = ((byte)('i')).ToString("X");
        public static readonly string TagInt64 = ((byte)('h')).ToString("X");
        public static readonly string TagFloat = ((byte)('f')).ToString("X");
        public static readonly string TagDouble = ((byte)('d')).ToString("X");
        public static readonly string TagString = ((byte)('s')).ToString("X");
        public static readonly string TagBooleanTrue = ((byte)('T')).ToString("X");
        public static readonly string TagBooleanFalse = ((byte)('F')).ToString("X");
        public static readonly string TagBlob = ((byte)('b')).ToString("X");
        public static readonly string TagChar = ((byte)('c')).ToString("X");
        public static readonly string TagByte = ((byte)('B')).ToString("X");

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput((context) =>
            {
                context.AddSource("OscPackableAttribute.g.cs", AttributeGeneration.OscPackable);
                context.AddSource("OscElementAtAttribute.g.cs", AttributeGeneration.OscElementAt);
                context.AddSource("OscCallbackAttribute.g.cs", AttributeGeneration.OscCallback);
                context.AddSource("OscReceiverAttribute.g.cs", AttributeGeneration.OscReceiver);
                context.AddSource("IOscPackable.g.cs", InterfaceGeneration.IOscPackable);
            });

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
                            TypeDouble => TagDouble,
                            TypeString => TagString,
                            TypeBoolean => TagBooleanFalse, // default false
                            TypeBlob => TagBlob,
                            TypeChar => TagChar,
                            TypeByte => TagByte,
                            _ => throw new NotSupportedTypeException($"Invalid type {typeSymbol.ToDisplayString()}")
                        };
                    }
                ));

            return $"{{ {arrayExpression} }}";
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

            Console.WriteLine("Memer");

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

            string fullTypeName = typeSymbol.ToDisplayString();
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

            using (var @class = builder.BeginScope($"partial {typeKindName} {fullTypeName} : IOscPackable"))
            {
                builder.AppendLine($"public static readonly byte[] TagTypes = new byte[] { MembersToArraySyntax(elements) };");
                builder.AppendLine();

                using (var @packMethod = builder.BeginScope("public void Pack (byte[] buffer, ref int offset)"))
                {
                    builder.AppendLine("// pack");
                }
                builder.AppendLine();

                using (var @unpackMethod = builder.BeginScope("public void Unpack (byte[] buffer, ref int offset)"))
                {
                    builder.AppendLine("// unpack");
                }
            }

            Console.WriteLine(builder.ToString());
            context.AddSource($"{fullTypeName}.g.cs", builder.ToString());
        }
    }
}
