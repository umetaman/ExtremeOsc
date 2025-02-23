using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtremeOsc
{
    public partial class OscWriter : System.IDisposable
    {
        private byte[] buffer = null;
        private int offset = 0;
        private int offsetTagTypes = 0;
        private List<char> tagTypes = new List<char>();
        private List<int> offsets = new List<int>();

        private static string GetTagTypes(object[] objects)
        {
            var sb = new StringBuilder();
            sb.Append(',');

            for (int i = 0; i < objects.Length; i++)
            {
                switch (objects[i])
                {
                    case int _:
                        sb.Append('i');
                        break;
                    case long _:
                        sb.Append('h');
                        break;
                    case float _:
                        sb.Append('f');
                        break;
                    case string _:
                        sb.Append('s');
                        break;
                    case byte[] _:
                        sb.Append('b');
                        break;
                    case double _:
                        sb.Append('d');
                        break;
                    case Color32 _:
                        sb.Append('r');
                        break;
                    case char _:
                        sb.Append('c');
                        break;
                    case DateTime _:
                        sb.Append('t');
                        break;
                    case bool @bool:
                        sb.Append(@bool ? 'T' : 'F');
                        break;
                    case Nil _:
                        sb.Append('N');
                        break;
                    case Infinitum _:
                        sb.Append('I');
                        break;
                    case ulong _:
                        sb.Append('t');
                        break;
                    default:
                        sb.Append('N');
                        break;
                }
            }

            return sb.ToString();
        }

        public static void Write(byte[] buffer, string address)
        {
            int offset = 0;
            WriteString(buffer, address, ref offset);
            WriteString(buffer, ",", ref offset);
        }

        public static void Write<T>(byte[] buffer, string address, T value) where T : IOscPackable
        {
            int offset = 0;
            WriteString(buffer, address, ref offset);
            value.Pack(buffer, ref offset);
        }

        public static void Write(byte[] buffer, string address, params object[] objects)
        {
            int offset = 0;
            int offsetTagTypes = 0;

            WriteString(buffer, address, ref offset);

            // address + ","
            offsetTagTypes = offset + 1;

            WriteString(buffer, GetTagTypes(objects), ref offset);

            for (int i = 0; i < objects.Length; i++)
            {
                switch (objects[i])
                {
                    case int @int:
                        WriteInt32(buffer, @int, ref offset);
                        break;
                    case long @long:
                        WriteInt64(buffer, @long, ref offset);
                        break;
                    case float @float:
                        WriteFloat(buffer, @float, ref offset);
                        break;
                    case string @string:
                        WriteStringUtf8(buffer, @string, ref offset);
                        break;
                    case byte[] @bytes:
                        WriteBlob(buffer, @bytes, ref offset);
                        break;
                    case double @double:
                        WriteDouble(buffer, @double, ref offset);
                        break;
                    case Color32 @color32:
                        WriteColor32(buffer, @color32, ref offset);
                        break;
                    case char @char:
                        WriteChar(buffer, @char, ref offset);
                        break;
                    case ulong timetag:
                        WriteTimeTag(buffer, timetag, ref offset);
                        break;
                    case bool @bool:
                        WriteBoolean(buffer, @bool, offsetTagTypes);
                        break;
                    case Nil _:
                        WriteNil(buffer, offsetTagTypes);
                        break;
                    case Infinitum _:
                        WriteInfinitum(buffer, offsetTagTypes);
                        break;
                    default:
                        throw new Exception("Invalid type");
                }

                offsetTagTypes++;
            }
        }

        public void Dispose()
        {
            buffer = null;
            offset = 0;
            offsetTagTypes = 0;
        }
    }
}
