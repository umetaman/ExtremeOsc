using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace ExtremeOsc.Tests
{
    public class ReaderWriterTest
    {
        [Test]
        public void TemporaryTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                var address = Arbitary.GetRandomAddress();
                var objects = Arbitary.GetRandomObjects(UnityEngine.Random.Range(1, 20));
                var tagTypes = Arbitary.GetTagTypes(objects);

                var buffer = new byte[4096 * 4];

                OscWriter.Write(buffer, address, objects);

                var reader = OscReader.Read(buffer);

                Assert.AreEqual(address, reader.Address);
                Assert.AreEqual(tagTypes, reader.TagTypes);

                var readerObjects = reader.GetAsObjects();
                Assert.AreEqual(objects, readerObjects);

                buffer = null;
            }
        }

        [Test]
        public void AppendTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                var address = Arbitary.GetRandomAddress();
                var objects = Arbitary.GetRandomObjects(UnityEngine.Random.Range(1, 20));
                var tagTypes = Arbitary.GetTagTypes(objects);

                var buffer = new byte[4096 * 4];

                OscWriter.Write(buffer, address, objects);

                var reader = OscReader.Read(buffer);

                Assert.AreEqual(address, reader.Address);
                Assert.AreEqual(tagTypes, reader.TagTypes);

                var readObjects = new object[objects.Length];

                for (int j = 0; j < objects.Length; j++)
                {
                    switch (objects[j])
                    {
                        case int _:
                            readObjects[j] = reader.GetAsInt32(j);
                            break;
                        case long _:
                            readObjects[j] = reader.GetAsInt64(j);
                            break;
                        case float _:
                            readObjects[j] = reader.GetAsFloat(j);
                            break;
                        case string _:
                            readObjects[j] = reader.GetAsString(j);
                            break;
                        case byte[] _:
                            readObjects[j] = reader.GetAsBlob(j);
                            break;
                        case double _:
                            readObjects[j] = reader.GetAsDouble(j);
                            break;
                        case Color32 _:
                            readObjects[j] = reader.GetAsColor32(j);
                            break;
                        case char _:
                            readObjects[j] = reader.GetAsChar(j);
                            break;
                        case ulong _:
                            readObjects[j] = reader.GetAsTimetagAsULong(j);
                            break;
                        case bool _:
                            readObjects[j] = reader.GetAsBoolean(j);
                            break;
                        case Nil _:
                            readObjects[j] = reader.GetAsNil(j);
                            break;
                        case Infinitum _:
                            readObjects[j] = reader.GetAsInfinitum(j);
                            break;
                    }
                }

                Assert.AreEqual(objects, readObjects);

                buffer = null;
            }
        }
    }
}
