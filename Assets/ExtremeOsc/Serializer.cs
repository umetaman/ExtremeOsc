using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace ExtremeOsc
{
    internal static class Writer
    {
        public static void WriteString(NativeArray<byte> buffer, string value, int offset)
        {
            int strLength = value.Length;
            for (int i = 0; i < strLength; i++)
            {
                buffer[offset + i] = (byte)value[i];
            }

            int aligned = Utils.AlignBytes4(strLength + 1);
            for (int i = strLength; i < aligned; i++)
            {
                buffer[offset + i] = 0;
            }
        }

        public static void WriteInt32(NativeArray<byte> buffer, int value, int offset)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        public static void WriteFloat(NativeArray<byte> buffer, float value, int offset)
        {
            var union = new FloatIntUnion { f = value };
            WriteInt32(buffer, union.i, offset);
        }

        public static void WriteBlob(NativeArray<byte> buffer, byte[] value, int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset + 4 + i] = value[i];
            }

            int aligned = Utils.AlignBytes4(byteLength + 4);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset + 4 + i] = 0;
            }
        }

        public static void WriteULong(NativeArray<byte> buffer, ulong value, int offset)
        {
            for (int i = 0; i < 8; i++)
            {
                buffer[offset + i] = (byte)(value >> (56 - i * 8));
            }
        }

        public static void WriteDouble(NativeArray<byte> buffer, double value, int offset)
        {
            unsafe
            {
                byte* ptr = (byte*)&value;
                
                for(int i = 0; i < 8; i++)
                {
                    buffer[offset + i] = ptr[i];
                }
            }
        }

        public static void WriteColor32(NativeArray<byte> buffer, Color32 value, int offset)
        {
            buffer[offset + 0] = value.r;
            buffer[offset + 1] = value.g;
            buffer[offset + 2] = value.b;
            buffer[offset + 3] = value.a;
        }

        public static void WriteChar(NativeArray<byte> buffer, char value, int offset)
        {
            buffer[offset] = (byte)value;
        }

        public static void WriteDateTime()
        {

        }
    }
}
