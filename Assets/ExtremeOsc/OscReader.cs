using System;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    using TimeTag = UInt64;

    public static class OscReader
    {
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

            return Encoding.UTF8.GetString(buffer.AsReadOnlySpan().Slice(offset, length));
        }

        public static bool IsBundle(NativeArray<byte> buffer, int offset)
        {
            return buffer.AsReadOnlySpan().Slice(offset, 8).SequenceEqual(TagType.BundleBytes);
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
            return buffer.AsSpan().Slice(offset + 4, length).ToArray();
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

        public static TimeTag ReadTimeTag(NativeArray<byte> buffer, int offset) => ReadULong(buffer, offset);
    
        public static int ReadMidi(NativeArray<byte> buffer, int offset) => ReadInt32(buffer, offset);
    }
}
