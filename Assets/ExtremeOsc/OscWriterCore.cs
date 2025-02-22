using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace ExtremeOsc
{
    public partial class OscWriter
    {
        public static ulong NtpNow => TimeTag.DateTimeToNtp(DateTime.UtcNow);
        public static ulong NtpImmediate => 1;

        public static void WriteString(byte[] buffer, string value, ref int offset)
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

        public static void WriteString(byte[] buffer, byte[] value, ref int offset)
        {
            var length = value.Length;
            for(int i = 0; i < length; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(length + 1);
            for (int i = length; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteStringUtf8(byte[] buffer, string value, ref int offset)
        {
            unsafe
            {
                int byteCount = Encoding.UTF8.GetByteCount(value);
                var span = stackalloc byte[byteCount];

                fixed (char* ptr = value)
                {
                    if (value.Length > 0)
                    {
                        Encoding.UTF8.GetBytes(ptr, value.Length, span, byteCount);
                    }
                }

                for (int i = 0; i < byteCount; i++)
                {
                    buffer[offset] = span[i];
                    offset++;
                }

                int aligned = Utils.AlignBytes4(byteCount + 1);
                for (int i = byteCount; i < aligned; i++)
                {
                    buffer[offset] = 0;
                    offset++;
                }
            }
        }

        public static void WriteInt32(byte[] buffer, int value, ref int offset)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);

            offset += 4;
        }

        public static void WriteInt64(byte[] buffer, long value, ref int offset)
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

        public static void WriteFloat(byte[] buffer, float value, ref int offset)
        {
            unsafe
            {
                byte* ptr = (byte*)&value;

                buffer[offset + 0] = ptr[3];
                buffer[offset + 1] = ptr[2];
                buffer[offset + 2] = ptr[1];
                buffer[offset + 3] = ptr[0];

                offset += 4;
            }
        }

        public static void WriteBlob(byte[] buffer, byte[] value, ref int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, ref offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(byteLength + 1);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteBlob(byte[] buffer, Span<byte> value, ref int offset)
        {
            int byteLength = value.Length;
            WriteInt32(buffer, byteLength, ref offset);

            for (int i = 0; i < byteLength; i++)
            {
                buffer[offset] = value[i];
                offset++;
            }

            int aligned = Utils.AlignBytes4(byteLength + 1);
            for (int i = byteLength; i < aligned; i++)
            {
                buffer[offset] = 0;
                offset++;
            }
        }

        public static void WriteULong(byte[] buffer, ulong value, ref int offset)
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

        public static void WriteDouble(byte[] buffer, double value, ref int offset)
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

        public static void WriteColor32(byte[] buffer, Color32 value, ref int offset)
        {
            buffer[offset + 0] = value.r;
            buffer[offset + 1] = value.g;
            buffer[offset + 2] = value.b;
            buffer[offset + 3] = value.a;

            offset += 4;
        }

        public static void WriteChar(byte[] buffer, char value, ref int offset)
        {
            buffer[offset + 3] = (byte)value;

            offset += 4;
        }

        public static void WriteBoolean(byte[] buffer, bool value, int offset)
        {
            buffer[offset] = Boolean(value);
        }

        public static void WriteNil(byte[] buffer, int offset) => buffer[offset] = (byte)'N';

        public static void WriteNil(byte[] buffer, Nil value, int offset) => WriteNil(buffer, offset);

        public static void WriteInfinitum(byte[] buffer, int offset) => buffer[offset] = (byte)'I';

        public static void WriteInfinitum(byte[] buffer, Infinitum value, int offset) => WriteInfinitum(buffer, offset);

        public static void WriteTimeTag(byte[] buffer, ulong value, ref int offset) => WriteULong(buffer, value, ref offset);

        public static void WriteMidi(byte[] buffer, int value, ref int offset) => WriteInt32(buffer, value, ref offset);

        public static byte Boolean(bool value) => value ? (byte)'T' : (byte)'F';
    }
}
