using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class TagType
    {
        public const byte Intro = (byte)',';
        public const byte Int32 = (byte)'i';
        public const byte Int64 = (byte)'h';
        public const byte Float = (byte)'f';
        public const byte String = (byte)'s';
        public const byte Symbol = (byte)'S';
        public const byte Blob = (byte)'b';
        public const byte Double = (byte)'d';
        public const byte Color32 = (byte)'r';
        public const byte Char = (byte)'c';
        public const byte TimeTag = (byte)'t';
        public const byte True = (byte)'T';
        public const byte False = (byte)'F';
        public const byte Nil = (byte)'N';
        public const byte Infinitum = (byte)'I';
        public const byte Midi = (byte)'m';
        public static byte[] BundleBytes => new byte[] { 0x23, 0x62, 0x75, 0x6e, 0x64, 0x6c, 0x65, 0x00 };
    }
}
