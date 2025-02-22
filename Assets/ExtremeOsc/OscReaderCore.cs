using System;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    public partial class OscReader
    {
        public static int ReadStringLength(byte[] buffer, int offset)
        {
            int length = 0;

            for(int i = offset; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    break;
                }
                length++;
            }

            return Utils.AlignBytes4(length + 1);
        }

        public static string ReadString(byte[] buffer, ref int offset)
        {
            // find null terminator
            int length = 0;
            int startOffset = offset;

            for (int i = offset; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    break;
                }
                length++;
            }

            string value = Encoding.UTF8.GetString(buffer.AsSpan(startOffset, length));

            offset += Utils.AlignBytes4(length + 1);

            return value;
        }

        public static bool IsBundle(byte[] buffer, ref int offset)
        {
            var value = buffer.AsSpan(offset, 8).SequenceEqual(TagType.BytesBundle);
            offset += 8;

            return value;
        }

        public static int ReadInt32(byte[] buffer, ref int offset)
        {
            // big endian -> little endian
            int value = 0;

            unsafe
            {
                byte* ptr = (byte*)&value;
                for (int i = 0; i < 4; i++)
                {
                    ptr[3 - i] = buffer[offset + i];
                }
            }

            offset += 4;

            return value;
        }

        public static long ReadInt64(byte[] buffer, ref int offset)
        {
            long value = 0;
            for (int i = 0; i < 8; i++)
            {
                value = (value << 8) | buffer[offset + i];
            }

            offset += 8;
            return value;
        }

        public static float ReadFloat(byte[] buffer, ref int offset)
        {
            float value = 0;

            unsafe
            {
                byte* ptr = (byte*)&value;
                ptr[0] = buffer[offset + 3];
                ptr[1] = buffer[offset + 2];
                ptr[2] = buffer[offset + 1];
                ptr[3] = buffer[offset + 0];
            }

            offset += 4;

            return value;
        }

        public static byte[] ReadBlob(byte[] buffer, ref int offset)
        {
            int length = ReadInt32(buffer, ref offset);

            byte[] value = buffer.AsSpan().Slice(offset, length).ToArray();
            offset += Utils.AlignBytes4(length + 1);
            
            return value;
        }

        public static ulong ReadULong(byte[] buffer, ref int offset)
        {
            ulong value = 0;
            for (int i = 0; i < 8; i++)
            {
                value = (value << 8) | buffer[offset + i];
            }

            offset += 8;

            return value;
        }

        public static double ReadDouble(byte[] buffer, ref int offset)
        {
            double value = 0;

            unsafe
            {
                byte* ptr = (byte*)&value;
                for (int i = 0; i < 8; i++)
                {
                    ptr[i] = buffer[offset + (7 - i)];
                }
            }

            offset += 8;

            return value;
        }

        public static Color32 ReadColor32(byte[] buffer, ref int offset)
        {
            byte r = buffer[offset + 0];
            byte g = buffer[offset + 1];
            byte b = buffer[offset + 2];
            byte a = buffer[offset + 3];

            offset += 4;

            return new Color32(r, g, b, a);
        }

        public static char ReadChar(byte[] buffer, ref int offset)
        {
            // 32bit -> 8bit
            char value = (char)buffer[offset + 3];
            offset += 4;
            return value;
        }

        public static bool ReadBoolean(byte[] buffer, int offset)
        {
            if (buffer[offset] == TagType.True)
            {
                return true;
            }
            else if (buffer[offset] == TagType.False)
            {
                return false;
            }
            else
            {
                throw new Exception("Invalid boolean tag");
            }
        }

        public static Infinitum ReadInfinitum(byte[] buffer, int offset)
        {
            if(buffer[offset] == TagType.Infinitum)
            {
                return Infinitum.Value;
            }
            else
            {
                throw new Exception("Invalid infinitum tag");
            }
        }

        public static Nil ReadNil(byte[] buffer, int offset)
        {
            if (buffer[offset] == TagType.Nil)
            {
                return Nil.Value;
            }
            else
            {
                throw new Exception("Invalid nil tag");
            }
        }

        public static ulong ReadTimeTagAsULong(byte[] buffer, ref int offset) => ReadULong(buffer, ref offset);
    
        public static DateTime ReadTimeTagAsDateTime(byte[] buffer, ref int offset)
        {
            var ntpTime = ReadULong(buffer, ref offset);
            return TimeTag.NtpToDateTime(ntpTime);
        }

        public static int ReadMidi(byte[] buffer, ref int offset) => ReadInt32(buffer, ref offset);
    }
}
