using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class TypeSize
    {
        public const int Int32 = sizeof(int);
        public const int Int64 = sizeof(long);
        public const int Float = sizeof(float);
        public const int Double = sizeof(double);
        public const int Char = sizeof(char);
        public const int Color32 = sizeof(byte) * 4;
        public const int TimeTag = sizeof(ulong);
        public const int Midi = sizeof(int) * 4;
    }
}
