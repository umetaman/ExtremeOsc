using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class TagType
    {
        public const byte ByteIntro = (byte)',';
        public const byte ByteInt32 = (byte)'i';
        public const byte ByteInt64 = (byte)'h';
        public const byte ByteFloat = (byte)'f';
        public const byte ByteString = (byte)'s';
        public const byte ByteSymbol = (byte)'S';
        public const byte ByteBlob = (byte)'b';
        public const byte ByteDouble = (byte)'d';
        public const byte ByteColor32 = (byte)'r';
        public const byte ByteChar = (byte)'c';
        public const byte ByteTimeTag = (byte)'t';
        public const byte ByteTrue = (byte)'T';
        public const byte ByteFalse = (byte)'F';
        public const byte ByteNil = (byte)'N';
        public const byte ByteInfinitum = (byte)'I';
        public const byte ByteMidi = (byte)'m';

        public const char Int32 = 'i';
        public const char Int64 = 'h';
        public const char Float = 'f';
        public const char String = 's';
        public const char Symbol = 'S';
        public const char Blob = 'b';
        public const char Double = 'd';
        public const char Color32 = 'r';
        public const char Char = 'c';
        public const char TimeTag = 't';
        public const char True = 'T';
        public const char False = 'F';
        public const char Nil = 'N';
        public const char Infinitum = 'I';
        public const char Midi = 'm';

        public static byte[] BytesBundle => new byte[] { 0x23, 0x62, 0x75, 0x6e, 0x64, 0x6c, 0x65, 0x00 };
    }
}
