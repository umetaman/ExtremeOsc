using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExtremeOsc.SourceGenerator
{
    using static OscSyntax;

    internal static class PackableEmitter
    {
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
                            TypeInt32 => ((byte)TagInt32).ToString("D"),
                            TypeInt64 => ((byte)TagInt64).ToString("D"),
                            TypeFloat => ((byte)TagFloat).ToString("D"),
                            TypeString => ((byte)TagString).ToString("D"),
                            TypeBlob => ((byte)TagBlob).ToString("D"),
                            TypeDouble => ((byte)TagDouble).ToString("D"),
                            TypeChar => ((byte)TagChar).ToString("D"),
                            TypeTimeTag => ((byte)TagTimeTag).ToString("D"),
                            TypeBoolean => ((byte)TagBooleanFalse).ToString("D"),
                            TypeNil => ((byte)TagNil).ToString("D"),
                            TypeInfinitum => ((byte)TagInfinitum).ToString("D"),
                            TypeColor32 => ((byte)TagColor32).ToString("D"),
                            _ => throw new NotSupportedTypeException($"Invalid type {typeSymbol.ToDisplayString()}")
                        };
                    }
                ));

            return $"{{ {((byte)TagIntro).ToString("D")}, {arrayExpression} }}";
        }

        public static string MembersToTagTypes(IEnumerable<(ITypeSymbol, ISymbol, int)> members)
        {
            var tagTypes = string.Join(
                "",
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
                            TypeTimeTag => TagTimeTag,
                            TypeBoolean => TagBooleanFalse,
                            TypeNil => TagNil,
                            TypeInfinitum => TagInfinitum,
                            TypeColor32 => TagColor32,
                            _ => throw new NotSupportedTypeException($"Invalid type {typeSymbol.ToDisplayString()}")
                        };
                    }
                ));

            return $",{tagTypes}";
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
                TypeTimeTag => $"OscWriter.WriteTimeTag(buffer, {@name}, ref {@offset});",
                TypeBoolean => $"OscWriter.WriteBoolean(buffer, {@name}, {@offsetTagType});",
                TypeNil => $"OscWriter.WriteNil(buffer, {@offsetTagType});",
                TypeInfinitum => $"OscWriter.WriteInfinitum(buffer, {@offsetTagType});",
                TypeColor32 => $"OscWriter.WriteColor32(buffer, {@name}, ref {@offset});",
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
                TypeTimeTag => $"this.{@name} = OscReader.ReadTimeTagAsULong(buffer, ref {@offset});",
                TypeBoolean => $"this.{@name} = OscReader.ReadBoolean(buffer, {@offsetTagType});",
                TypeNil => $"this.{@name} = OscReader.ReadNil(buffer, {@offsetTagType});",
                TypeInfinitum => $"this.{@name} = OscReader.ReadInfinitum(buffer, {@offsetTagType});",
                TypeColor32 => $"this.{@name} = OscReader.ReadColor32(buffer, ref {@offset});",
                _ => throw new NotSupportedTypeException($"Invalid type {type}")
            };
            builder.AppendLine(line);
            builder.AppendLine($"{@offsetTagType}++;");
        }

        public static void ReadWithDeclaration(CodeBuilder builder, (ITypeSymbol, ISymbol, int) member, string @offset, string @offsetTagType)
        {
            var (typeSymbol, symbol, _) = member;
            var type = typeSymbol.ToDisplayString();
            var @name = symbol.Name;
            string line = type switch
            {
                TypeInt32 => $"int {@name} = OscReader.ReadInt32(buffer, ref {@offset});",
                TypeInt64 => $"long {@name} = OscReader.ReadInt64(buffer, ref {@offset});",
                TypeFloat => $"float {@name} = OscReader.ReadFloat(buffer, ref {@offset});",
                TypeString => $"string {@name} = OscReader.ReadString(buffer, ref {@offset});",
                TypeBlob => $"byte[] {@name} = OscReader.ReadBlob(buffer, ref {@offset});",
                TypeDouble => $"double {@name} = OscReader.ReadDouble(buffer, ref {@offset});",
                TypeChar => $"char {@name} = OscReader.ReadChar(buffer, ref {@offset});",
                TypeTimeTag => $"ulong {@name} = OscReader.ReadTimeTagAsULong(buffer, ref {@offset});",
                TypeBoolean => $"bool {@name} = OscReader.ReadBoolean(buffer, {@offsetTagType});",
                // any namespace :)
                TypeNil => $"var {@name} = OscReader.ReadNil(buffer, {@offsetTagType});",
                TypeInfinitum => $"var {@name} = OscReader.ReadInfinitum(buffer, {@offsetTagType});",
                TypeColor32 => $"var {@name} = OscReader.ReadColor32(buffer, ref {@offset});",
                _ => throw new NotSupportedTypeException($"Invalid type {type}")
            };
            builder.AppendLine(line);
            builder.AppendLine($"{@offsetTagType}++;");
        }

        public static string AddressToVariableName(string address)
        {
            var sb = new StringBuilder();
            bool isNextUpper = false;
            foreach (var c in address)
            {
                if (c == '/')
                {
                    isNextUpper = true;
                    continue;
                }
                if (isNextUpper)
                {
                    sb.Append(char.ToUpper(c));
                    isNextUpper = false;
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
