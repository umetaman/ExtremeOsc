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
        public void Write(byte[] buffer, string address, object[] values)
        {
            var tagTypes = Arbitary.GetTagTypes(values);
            Debug.Log($"ReadWriteTest::Write: {address} {tagTypes}");

            int offsetTagTypes = 0;
            int offset = 0;

            OscWriter.WriteString(buffer, address, ref offset);

            offsetTagTypes += offset + 1;

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
                    case Infinitum _:
                        OscWriter.WriteInfinitum(buffer, offsetTagTypes);
                        break;
                    case Nil _:
                        OscWriter.WriteNil(buffer, offsetTagTypes);
                        break;
                    case Color32 color32:
                        OscWriter.WriteColor32(buffer, color32, ref offset);
                        break;
                    case ulong @ulong:
                        OscWriter.WriteTimeTag(buffer, @ulong, ref offset);
                        break;
                }

                offsetTagTypes++;
            }
        }

        public (string, object[]) Read(byte[] buffer)
        {
            int offsetTagTypes = 0;
            int offset = 0;

            string address = OscReader.ReadString(buffer, ref offset);
            offsetTagTypes += offset;

            string tagTypes = OscReader.ReadString(buffer, ref offset);
            Debug.Log($"ReadWriteTest::Read: {address} {tagTypes}");

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
                    case TagType.Infinitum:
                        values.Add(OscReader.ReadInfinitum(buffer, offsetTagTypes));
                        break;
                    case TagType.Nil:
                        values.Add(OscReader.ReadNil(buffer, offsetTagTypes));
                        break;
                    case TagType.Color32:
                        values.Add(OscReader.ReadColor32(buffer, ref offset));
                        break;
                    case TagType.TimeTag:
                        //values.Add(OscReader.ReadTimeTag(buffer, ref offset));
                        values.Add(OscReader.ReadTimeTagAsULong(buffer, ref offset));
                        break;
                }
                offsetTagTypes++;
            }

            return (address, values.ToArray());
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
                string address = Arbitary.GetRandomAddress();

                var ints = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    ints[j] = Arbitary.GetRandomInt32();
                }
                Write(buffer, address, ints);
                var (readAddress, readInts) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();
                
                var longs = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    longs[j] = Arbitary.GetRandomInt64();
                }

                Write(buffer, address, longs);
                var (readAddress, readLongs) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var floats = new object[randomCount];

                for (int j = 0; j < randomCount; j++)
                {
                    floats[j] = Arbitary.GetRandomFloat();
                }

                Write(buffer, address, floats);
                var (readAddress, readFloats) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var blobs = new object[randomCount];

                for (int j = 0; j < randomCount; j++)
                {
                    blobs[j] = Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 100));
                }

                Write(buffer, address, blobs);

                var (readAddress, readBlobs) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var doubles = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    doubles[j] = Arbitary.GetRandomDouble();
                }

                Write(buffer, address, doubles);
                
                var (readAddress, readDoubles) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var chars = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    chars[j] = Arbitary.GetRandomChar();
                }
                Write(buffer, address, chars);

                var (readAddress, readChars) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var bools = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    bools[j] = Arbitary.GetRandomBool();
                }

                Write(buffer, address, bools);
                var (readAddress, readBools) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var strings = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    strings[j] = Arbitary.GetRandomStringAscii(UnityEngine.Random.Range(1, 256));
                }

                Write(buffer, address, strings);
                var (readAddress, readStrings) = Read(buffer);

                Assert.AreEqual(readAddress, address);
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
                string address = Arbitary.GetRandomAddress();

                var strings = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    strings[j] = Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(1, 256));
                }

                Write(buffer, address, strings);
                var (readAddress, readStrings) = Read(buffer);

                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(strings, readStrings);
            }
        }

        [Test]
        public void ReadWriteInfinitum()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                string address = Arbitary.GetRandomAddress();

                var infinitums = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    infinitums[j] = Infinitum.Value;
                }

                Write(buffer, address, infinitums);
                var (readAddress, readInfinitums) = Read(buffer);

                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(infinitums, readInfinitums);
            }
        }

        [Test]
        public void ReadWriteNil()
        {
            var buffer = new byte[4096];

            for(int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                string address = Arbitary.GetRandomAddress();

                var nils = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    nils[j] = Nil.Value;
                }
                
                Write(buffer, address, nils);
                var (readAddress, readNils) = Read(buffer);
                
                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(nils, readNils);
            }
        }

        [Test]
        public void ReadWriteColor32()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                string address = Arbitary.GetRandomAddress();

                var colors = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    colors[j] = Arbitary.GetRandomColor32();
                }

                Write(buffer, address, colors);
                var (readAddress, readColors) = Read(buffer);
                
                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(colors, readColors);
            }
        }

        [Test]
        public void ReadWriteTimetag()
        {
            var buffer = new byte[4096];

            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                string address = Arbitary.GetRandomAddress();
                
                var timeTags = new object[randomCount];
                for (int j = 0; j < randomCount; j++)
                {
                    timeTags[j] = Arbitary.GetRandomULong();
                }
                
                Write(buffer, address, timeTags);
                var (readAddress, readTimeTags) = Read(buffer);
                
                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(timeTags, readTimeTags);
            }
        }

        [Test]
        public void UlongNtpConvertion()
        {
            for(int i = 0; i < 1000; i++)
            {
                ulong a = Arbitary.GetRandomULong();
                var unix_a = Utils.NtpToDateTime(a);
                var ntp_a = Utils.DateTimeToNtp(unix_a);
                var unix_b = Utils.NtpToDateTime(ntp_a);

                Assert.AreEqual(unix_a, unix_b);
            }
        }

        [Test]
        public void ReadWrite()
        {
            var buffer = new byte[4096*8];

            for (int i = 0; i < 10000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 20);
                string address = Arbitary.GetRandomAddress();

                string tagTypes = Arbitary.GetRandomTagTypes(randomCount);

                var values = Arbitary.GetRandomObjects(randomCount);
                Write(buffer, address, values);

                var (readAddress, readValues) = Read(buffer);

                Assert.AreEqual(readAddress, address);
                Assert.AreEqual(values, readValues);
            }

            buffer = null;
        }
    }
}
