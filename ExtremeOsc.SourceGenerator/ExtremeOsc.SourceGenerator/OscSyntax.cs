using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal static class OscSyntax
    {
        // Osc Types
        public const string TypeInt32 = "int";
        public const string TypeInt64 = "long";
        public const string TypeFloat = "float";
        public const string TypeString = "string";
        public const string TypeBlob = "byte[]";
        public const string TypeDouble = "double";
        public const string TypeChar = "char";
        public const string TypeTimeTag = "ulong";
        public const string TypeBoolean = "bool";
        public const string TypeNil = "ExtremeOsc.Nil";
        public const string TypeInfinitum = "ExtremeOsc.Infinitum";
        public const string TypeColor32 = "UnityEngine.Color32";

        public static readonly string[] TypeNames = new string[]
        {
            TypeInt32,
            TypeInt64,
            TypeFloat,
            TypeString,
            TypeBlob,
            TypeDouble,
            TypeChar,
            TypeTimeTag,
            TypeBoolean,
            TypeNil,
            TypeInfinitum,
            TypeColor32
        };

        // OscTags
        public const char TagIntro = ',';
        public const char TagInt32 = 'i';
        public const char TagInt64 = 'h';
        public const char TagFloat = 'f';
        public const char TagDouble = 'd';
        public const char TagString = 's';
        public const char TagBlob = 'b';
        public const char TagChar = 'c';
        public const char TagTimeTag = 't';
        public const char TagBooleanTrue = 'T';
        public const char TagBooleanFalse = 'F';
        public const char TagNil = 'N';
        public const char TagInfinitum = 'I';
        public const char TagColor32 = 'r';
    }
}
