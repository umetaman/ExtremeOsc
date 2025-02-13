using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace ExtremeOsc
{
    public static class OscEmitter
    {
        public static ulong NtpNow => Utils.UnixToNtp(DateTimeOffset.Now);
        public static ulong NtpImmediate => 1;

        public static void WriteString(NativeArray<byte> buffer, string value, ref int offset)
        {
            var span = value.AsSpan();
            var length = span.Length;
            for (int i = 0; i < length; i++)
            {
                buffer[offset] = (byte)span[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(length + 1);
            for (int i = length; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteInt32(NativeArray<byte> buffer, int value, ref int offset)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);

            offset += 4;
        }

        public static void WriteFloat(NativeArray<byte> buffer, float value, ref int offset)
        {
            unsafe
            {
                byte* ptr = (byte*)&value;
                for (int i = 0; i < 4; i++)
                {
                    buffer[offset + i] = ptr[3 - i];
                }
            }
        }

        public static void WriteBlob(NativeArray<byte> buffer, byte[] value, ref int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, ref offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(byteLength + 4);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteBlob(NativeArray<byte> buffer, NativeArray<byte> value, ref int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, ref offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(byteLength + 4);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteBlob(NativeArray<byte> buffer, Span<byte> value, ref int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, ref offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(byteLength + 4);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteULong(NativeArray<byte> buffer, ulong value, ref int offset)
        {
            unsafe
            {
                byte* ptr = (byte*)&value;

                for (int i = 0; i < 8; i++)
                {
                    buffer[offset + i] = ptr[7 - i];
                }
            }

            offset += 8;
        }

        public static void WriteDouble(NativeArray<byte> buffer, double value, ref int offset)
        {
            unsafe
            {
                byte* ptr = (byte*)&value;
                
                for(int i = 0; i < 8; i++)
                {
                    buffer[offset + i] = ptr[7 - i];
                }
            }

            offset += 8;
        }

        public static void WriteColor32(NativeArray<byte> buffer, Color32 value, ref int offset)
        {
            buffer[offset + 0] = value.r;
            buffer[offset + 1] = value.g;
            buffer[offset + 2] = value.b;
            buffer[offset + 3] = value.a;

            offset += 4;
        }

        public static void WriteChar(NativeArray<byte> buffer, char value, ref int offset)
        {
            buffer[offset + 3] = (byte)value;

            offset += 4;
        }

        public static void WriteTimeTag(NativeArray<byte> buffer, ulong value, ref int offset) => WriteULong(buffer, value, ref offset);

        public static void WriteMidi(NativeArray<byte> buffer, int value, ref int offset) => WriteInt32(buffer, value, ref offset);
    }
}
