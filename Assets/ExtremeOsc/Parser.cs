using System;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    internal static class Reader
    {
        public const char TagIntro = ',';
        public const char TagInt32 = 'i';
        public const char TagFloat = 'f';
        public const char TagString = 's';
        public const char TagBlob = 'b';
        public const char TagDouble = 'd';
        public const char TagColor32 = 'c';
        public const char TagChar = 'h';
        public const char TagDateTime = 't';
        public const char TagTrue = 'T';
        public const char TagFalse = 'F';
        public const char TagNil = 'N';
        public const char TagInfinitum = 'I';

        public static readonly byte[] TagIntroBytes = new byte[] { (byte)TagIntro };
        public static readonly byte[] TagBundleBytes = new byte[] { (byte)'#', (byte)'b', (byte)'u', (byte)'n', (byte)'d', (byte)'l', (byte)'e' };
        public static readonly byte[] TagInt32Bytes = new byte[] { (byte)TagInt32 };
        public static readonly byte[] TagFloatBytes = new byte[] { (byte)TagFloat };
        public static readonly byte[] TagStringBytes = new byte[] { (byte)TagString };
        public static readonly byte[] TagBlobBytes = new byte[] { (byte)TagBlob };
        public static readonly byte[] TagDoubleBytes = new byte[] { (byte)TagDouble };
        public static readonly byte[] TagColor32Bytes = new byte[] { (byte)TagColor32 };
        public static readonly byte[] TagCharBytes = new byte[] { (byte)TagChar };
        public static readonly byte[] TagDateTimeBytes = new byte[] { (byte)TagDateTime };
        public static readonly byte[] TagTrueBytes = new byte[] { (byte)TagTrue };
        public static readonly byte[] TagFalseBytes = new byte[] { (byte)TagFalse };
        public static readonly byte[] TagNilBytes = new byte[] { (byte)TagNil };
        public static readonly byte[] TagInfinitumBytes = new byte[] { (byte)TagInfinitum };

        public static string ReadString(NativeArray<byte> buffer, int offset)
        {
            // find null terminator
            int length = 0;

            for (int i = offset; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    break;
                }
                length++;
            }

            return Encoding.UTF8.GetString(buffer.GetSubArray(offset, length));
        }

        public static int ReadInt32(NativeArray<byte> buffer, int offset)
        {
            // big endian -> little endian
            return (buffer[offset + 0] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
        }

        public static float ReadFloat(NativeArray<byte> buffer, int offset)
        {
            int i = ReadInt32(buffer, offset);
            var union = new FloatIntUnion { i = i };
            return union.f;
        }

        public static byte[] ReadBlob(NativeArray<byte> buffer, int offset)
        {
            int length = ReadInt32(buffer, offset);
            return buffer.GetSubArray(offset + 4, length).ToArray();
        }

        public static ulong ReadULong(NativeArray<byte> buffer, int offset)
        {
            ulong value = 0;
            for (int i = 0; i < 8; i++)
            {
                value = (value << 8) | buffer[offset + i];
            }
            return value;
        }

        public static double ReadDouble(NativeArray<byte> buffer, int offset)
        {
            return (double)(
                buffer[offset + 0] << 56 |
                buffer[offset + 1] << 48 |
                buffer[offset + 2] << 40 |
                buffer[offset + 3] << 32 |
                buffer[offset + 4] << 24 |
                buffer[offset + 5] << 16 |
                buffer[offset + 6] << 8 |
                buffer[offset + 7] << 0
                );
        }

        public static Color32 ReadColor32(NativeArray<byte> buffer, int offset)
        {
            byte r = buffer[offset + 0];
            byte g = buffer[offset + 1];
            byte b = buffer[offset + 2];
            byte a = buffer[offset + 3];
            return new Color32(r, g, b, a);
        }

        public static char ReadChar(NativeArray<byte> buffer, int offset)
        {
            // 32bit -> 8bit
            return (char)buffer[offset + 3];
        }

        public static DateTime ReadReadDateTime(NativeArray<byte> buffer, int offset)
        {
            ulong value = ReadULong(buffer, offset);

            if(value == 1)
            {
                // immediately
                return DateTime.UtcNow;
            }

            // [32bits seconds] [32bits fraction]
            uint seconds = (uint)(value >> 32);
            double fraction = (uint)(value & 0xFFFFFFFF) / (double)int.MaxValue;

            // 1900/1/1 00:00:00
            DateTime epoch = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddSeconds(seconds + fraction);
            return epoch;
        }
    }
}
