using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace ExtremeOsc.Tests
{
    public class ReadWriteTest
    {
        public void Write(byte[] buffer, object[] values)
        {
            var tagTypes = Arbitary.GetTagTypes(values);
            Debug.Log($"ReadWriteTest::Write: {tagTypes}");

            int offsetTagTypes = 1;
            int offset = 0;

            OscWriter.WriteString(buffer, tagTypes, ref offset);

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                switch(value)
                {
                    case int @int:
                        OscWriter.WriteInt32(buffer, @int, ref offset);
                        break;
                    case long @long:
                        OscWriter.WriteInt64(buffer, @long, ref offset);
                        break;
                    case float @float:
                        OscWriter.WriteFloat(buffer, @float, ref offset);
                        break;
                    case string @string:
                        OscWriter.WriteStringUtf8(buffer, @string, ref offset);
                        break;
                    case byte[] @byte:
                        OscWriter.WriteBlob(buffer, @byte, ref offset);
                        break;
                    case double @double:
                        OscWriter.WriteDouble(buffer, @double, ref offset);
                        break;
                    case char @char:
                        OscWriter.WriteChar(buffer, @char, ref offset);
                        break;
                    case bool @bool:
                        OscWriter.WriteBoolean(buffer, @bool, offsetTagTypes);
                        break;
                }

                offsetTagTypes++;
            }
        }

        public object[] Read(byte[] buffer)
        {
            int offsetTagTypes = 0;
            int offset = 0;

            string tagTypes = OscReader.ReadString(buffer, ref offset);
            Debug.Log($"ReadWriteTest::Write: {tagTypes}");

            var values = new List<object>();

            for (int i = 0; i < tagTypes.Length; i++)
            {
                switch (tagTypes[i])
                {
                    case TagType.Int32:
                        values.Add(OscReader.ReadInt32(buffer, ref offset));
                        break;
                    case TagType.Int64:
                        values.Add(OscReader.ReadInt64(buffer, ref offset));
                        break;
                    case TagType.Float:
                        values.Add(OscReader.ReadFloat(buffer, ref offset));
                        break;
                    case TagType.String:
                        values.Add(OscReader.ReadString(buffer, ref offset));
                        break;
                    case TagType.Blob:
                        values.Add(OscReader.ReadBlob(buffer, ref offset));
                        break;
                    case TagType.Double:
                        values.Add(OscReader.ReadDouble(buffer, ref offset));
                        break;
                    case TagType.Char:
                        values.Add(OscReader.ReadChar(buffer, ref offset));
                        break;
                    case TagType.True:
                        values.Add(OscReader.ReadBoolean(buffer, offsetTagTypes));
                        break;
                    case TagType.False:
                        values.Add(OscReader.ReadBoolean(buffer, offsetTagTypes));
                        break;
                }
                offsetTagTypes++;
            }

            return values.ToArray();
        }

        [Test]
        public void TagTypeTest()
        {
            var buffer = new byte[4096];
            
            for(int i = 0; i < 100; i++)
            {
                var span = buffer.AsSpan();
                span.Fill(0);

                int offset = 0;

                int count = UnityEngine.Random.Range(1, 20);
                string a = Arbitary.GetRandomTagTypes(count);
                OscWriter.WriteString(buffer, a, ref offset);

                offset = 0;
                string b = OscReader.ReadString(buffer, ref offset);

                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void ReadWriteInt32()
        {
            var buffer = new byte[4096];
            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);

                var ints = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    ints[j] = Arbitary.GetRandomInt32();
                }
                Write(buffer, ints);
                var readInts = Read(buffer);
                Assert.AreEqual(ints, readInts);
            }
        }

        [Test]
        public void ReadWriteInt64()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);
                var longs = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    longs[j] = Arbitary.GetRandomInt64();
                }
                Write(buffer, longs);
                var readLongs = Read(buffer);
                Assert.AreEqual(longs, readLongs);
            }
        }

        [Test]
        public void ReadWriteFloat()
        {
            var buffer = new byte[4096];

            for(int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);
                
                var floats = new object[randomCount];

                for (int j = 0; j < randomCount; j++)
                {
                    floats[j] = Arbitary.GetRandomFloat();
                }

                Write(buffer, floats);

                var readFloats = Read(buffer);

                Assert.AreEqual(floats, readFloats);
            }
        }

        [Test]
        public void ReadWriteBlob()
        {
            var buffer = new byte[4096];

            for(int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);

                var blobs = new object[randomCount];

                for (int j = 0; j < randomCount; j++)
                {
                    blobs[j] = Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 100));
                }

                Write(buffer, blobs);

                var readBlobs = Read(buffer);

                Assert.IsTrue(blobs.Length == readBlobs.Length);

                for(int j = 0; j < blobs.Length; j++)
                {
                    if(blobs[j] is byte[] a && readBlobs[j] is byte[] b)
                    {
                        for(int k = 0; k < a.Length; k++)
                        {
                            Assert.AreEqual(a[k], b[k], $"Not Equal at: [{k}]");
                        }
                    }
                }
            }
        }

        [Test]
        public void ReadWriteDouble()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);
                var doubles = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    doubles[j] = Arbitary.GetRandomDouble();
                }
                Write(buffer, doubles);
                var readDoubles = Read(buffer);
                Assert.AreEqual(doubles, readDoubles);
            }
        }

        [Test]
        public void ReadWriteChar()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);
                var chars = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    chars[j] = Arbitary.GetRandomChar();
                }
                Write(buffer, chars);
                var readChars = Read(buffer);
                Assert.AreEqual(chars, readChars);
            }
        }

        [Test]
        public void ReadWriteTrueOrFalse()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 100);
                var bools = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    bools[j] = Arbitary.GetRandomBool();
                }
                Write(buffer, bools);
                var readBools = Read(buffer);
                Assert.AreEqual(bools, readBools);
            }
        }

        [Test]
        public void ReadWriteString()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                var strings = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    strings[j] = Arbitary.GetRandomStringAscii(UnityEngine.Random.Range(1, 256));
                }
                Write(buffer, strings);
                var readStrings = Read(buffer);
                Assert.AreEqual(strings, readStrings);
            }
        }

        [Test]
        public void ReadWriteStringUtf8()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                var strings = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    strings[j] = Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(1, 256));
                }
                Write(buffer, strings);
                var readStrings = Read(buffer);
                Assert.AreEqual(strings, readStrings);
            }
        }

        [Test]
        public void ReadWrite()
        {
            var buffer = new byte[4096*2];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                string tagTypes = Arbitary.GetRandomTagTypes(randomCount);

                var values = Arbitary.GetRandomObjects(randomCount);

                Write(buffer, values);

                var readValues = Read(buffer);

                Assert.AreEqual(values, readValues);
            }
        }
    }
}
