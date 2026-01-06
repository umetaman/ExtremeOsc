using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public partial class OscReader : System.IDisposable
    {
        public static readonly byte[] ZeroBytes = new byte[0];

        public ulong Timestamp { get; private set; }
        public string Address { get; private set; }
        public string TagTypes => tagTypes;
        public int Count => tagTypes.Length - 1;

        private string tagTypes = null;
        private int[] offsets = null;
        private byte[] buffer = null;

        public static void ReadBundle(byte[] buffer, int length, ref int offset, ulong defaultTimestamp, List<OscReader> readers)
        {
            int tempOffset = offset;
            string address = ReadString(buffer, ref tempOffset);

            if (address == "#bundle")
            {
                offset = tempOffset;
                ulong bundleTimestamp = ReadULong(buffer, ref offset);

                while (offset < length)
                {
                    int elementSize = ReadInt32(buffer, ref offset);
                    ReadBundle(buffer, elementSize, ref offset, bundleTimestamp, readers);
                }
            }
            else
            {
                readers.Add(Read(buffer, ref offset, defaultTimestamp));
            }
        }

        public static OscReader Read(byte[] buffer, ulong timestamp = default)
        {
            int offset = 0;
            return Read(buffer, ref offset, timestamp);
        }

        public static OscReader Read(byte[] buffer, ref int offset, ulong timestamp = default)
        {
            var oscMessage = new OscReader();
            oscMessage.Timestamp = timestamp;

            int offsetTagTypes = 0;

            oscMessage.Address = ReadString(buffer, ref offset);

            // address + ","
            offsetTagTypes = offset + 1;

            string tagTypes = ReadString(buffer, ref offset);
            oscMessage.tagTypes = tagTypes;
            oscMessage.offsets = new int[oscMessage.Count];
            oscMessage.buffer = buffer;

            for (int i = 0; i < oscMessage.Count; i++)
            {
                oscMessage.offsets[i] = offset;
                char tag = tagTypes[i + 1];

                switch (tag)
                {
                    case TagType.Int32:
                        offset += 4;
                        break;
                    case TagType.Int64:
                        offset += 8;
                        break;
                    case TagType.Float:
                        offset += 4;
                        break;
                    case TagType.String:
                        offset += ReadStringLength(buffer, offset);
                        break;
                    case TagType.Symbol:
                        offset += ReadStringLength(buffer, offset);
                        break;
                    case TagType.Blob:
                        int byteLength = ReadInt32(buffer, ref offset);
                        offset += Utils.AlignBytes4(byteLength + 1);
                        break;
                    case TagType.Double:
                        offset += 8;
                        break;
                    case TagType.Color32:
                        offset += 4;
                        break;
                    case TagType.Char:
                        offset += 4;
                        break;
                    case TagType.TimeTag:
                        offset += 8;
                        break;
                    case TagType.True:
                    case TagType.False:
                    case TagType.Nil:
                    case TagType.Infinitum:
                        oscMessage.offsets[i] = offsetTagTypes;
                        break;
                    case TagType.Midi:
                        offset += 4;
                        break;
                }

                offsetTagTypes++;
            }

            return oscMessage;
        }

        public int GetAsInt32(int index, int defaultValue = 0)
        {
            if (tagTypes[index + 1] != TagType.Int32)
            {
                Debug.LogWarning("Not Int32");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadInt32(this.buffer, ref offset);
        }

        public long GetAsInt64(int index, long defaultValue = 0)
        {
            if (tagTypes[index + 1] != TagType.Int64)
            {
                Debug.LogWarning("Not Int64");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadInt64(this.buffer, ref offset);
        }

        public float GetAsFloat(int index, float defaultValue = 0)
        {
            if (tagTypes[index + 1] != TagType.Float)
            {
                Debug.LogWarning("Not Float");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadFloat(this.buffer, ref offset);
        }

        public string GetAsString(int index, string defaultValue = "")
        {
            if (tagTypes[index + 1] != TagType.String)
            {
                Debug.LogWarning("Not String");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadString(this.buffer, ref offset);
        }

        public string GetAsSymbol(int index, string defaultValue = "")
        {
            if(tagTypes[index + 1] != TagType.Symbol)
            {
                Debug.LogWarning("Not Symbol");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadString(this.buffer, ref offset);
        }

        public byte[] GetAsBlob(int index, byte[] defaultValue = null)
        {
            if(tagTypes[index + 1] != TagType.Blob)
            {
                Debug.LogWarning("Not Blob");
                return ZeroBytes;
            }

            int offset = offsets[index];
            return ReadBlob(this.buffer, ref offset);
        }

        public double GetAsDouble(int index, double defaultValue = 0)
        {
            if(tagTypes[index + 1] != TagType.Double)
            {
                Debug.LogWarning("Not Double");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadDouble(this.buffer, ref offset);
        }

        public Color32 GetAsColor32(int index, Color defaultValue = default)
        {
            if (tagTypes[index + 1] != TagType.Color32)
            {
                Debug.LogWarning("Not Color32");
                return defaultValue;
            }
            int offset = offsets[index];
            return ReadColor32(this.buffer, ref offset);
        }

        public char GetAsChar(int index, char defaultValue = default)
        {
            if (tagTypes[index + 1] != TagType.Char)
            {
                Debug.LogWarning("Not Char");
                return defaultValue;
            }
            int offset = offsets[index];
            return ReadChar(this.buffer, ref offset);
        }

        public DateTime GetAsTimeTag(int index, DateTime defaultValue = default)
        {
            if (tagTypes[index + 1] != TagType.TimeTag)
            {
                Debug.LogWarning("Not TimeTag");
                return defaultValue;
            }
            int offset = offsets[index];
            return ReadTimeTagAsDateTime(this.buffer, ref offset);
        }

        public ulong GetAsTimetagAsULong(int index, ulong defaultValue = default)
        {
            if (tagTypes[index + 1] != TagType.TimeTag)
            {
                Debug.LogWarning("Not TimeTag");
                return defaultValue;
            }
            int offset = offsets[index];
            return ReadTimeTagAsULong(this.buffer, ref offset);
        }

        public bool GetAsBoolean(int index, bool defaultValue = default)
        {
            var tagType = tagTypes[index + 1];

            if (tagType == TagType.True)
            {
                return true;
            }
            else if (tagType == TagType.False)
            {
                return false;
            }
            else
            {
                Debug.LogWarning("Not Boolean");
                return defaultValue;
            }
        }

        public Infinitum GetAsInfinitum(int index, Infinitum defaultValue = default)
        {
            if(tagTypes[index + 1] != TagType.Infinitum)
            {
                Debug.LogWarning("Not Infinitum");
                return defaultValue;
            }

            return Infinitum.Value;
        }

        public Nil GetAsNil(int index)
        {
            if(tagTypes[index + 1] != TagType.Nil)
            {
                Debug.LogWarning("Not Nil");
                return Nil.Value;
            }

            return Nil.Value;
        }

        public int GetAsMidi(int index, int defaultValue = 0)
        {
            if(tagTypes[index + 1] != TagType.Midi)
            {
                Debug.LogWarning("Not Midi");
                return defaultValue;
            }

            int offset = offsets[index];
            return ReadMidi(this.buffer, ref offset);
        }

        public object[] GetAsObjects()
        {
            var objects = new object[tagTypes.Length - 1];

            for (int i = 0; i < offsets.Length; i++)
            {
                var tagType = tagTypes[i + 1];
                int offset = offsets[i];

                switch (tagType)
                {
                    case TagType.Int32:
                        objects[i] = ReadInt32(this.buffer, ref offset);
                        break;
                    case TagType.Int64:
                        objects[i] = ReadInt64(this.buffer, ref offset);
                        break;
                    case TagType.Float:
                        objects[i] = ReadFloat(this.buffer, ref offset);
                        break;
                    case TagType.String:
                        objects[i] = ReadString(this.buffer, ref offset);
                        break;
                    case TagType.Symbol:
                        objects[i] = ReadString(this.buffer, ref offset);
                        break;
                    case TagType.Blob:
                        objects[i] = ReadBlob(this.buffer, ref offset);
                        break;
                    case TagType.Double:
                        objects[i] = ReadDouble(this.buffer, ref offset);
                        break;
                    case TagType.Color32:
                        objects[i] = ReadColor32(this.buffer, ref offset);
                        break;
                    case TagType.Char:
                        objects[i] = ReadChar(this.buffer, ref offset);
                        break;
                    case TagType.TimeTag:
                        //objects[i] = OscReader.ReadTimeTag(this.buffer, ref offset);
                        objects[i] = ReadTimeTagAsULong(this.buffer, ref offset);
                        break;
                    case TagType.True:
                        objects[i] = true;
                        break;
                    case TagType.False:
                        objects[i] = false;
                        break;
                    case TagType.Nil:
                        objects[i] = Nil.Value;
                        break;
                    case TagType.Infinitum:
                        objects[i] = Infinitum.Value;
                        break;
                    case TagType.Midi:
                        objects[i] = ReadMidi(this.buffer, ref offset);
                        break;
                }
            }

            return objects;
        }

        public void Dispose()
        {
            Address = null;
            tagTypes = null;
            offsets = null;
            buffer = null;
        }
    }
}
