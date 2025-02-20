using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace ExtremeOsc.Tests
{
    public class OscMessageTest
    {
        [Test]
        public void TagTypesTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                var randomObjects = Arbitary.GetRandomObjects(randomCount);
                var address = Arbitary.GetRandomAddress();
                var buffer = new byte[4096];

                ReadWriteTest.Write(buffer, address, randomObjects);

                var tagTypes = Arbitary.GetTagTypes(randomObjects);
                var oscMessage = OscReader.Read(buffer);

                Assert.AreEqual(address, oscMessage.Address);
                Assert.AreEqual(tagTypes, oscMessage.TagTypes);
            }
        }

        [Test]
        public void ObjectsTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                var randomObjects = Arbitary.GetRandomObjects(randomCount);
                var address = Arbitary.GetRandomAddress();
                var buffer = new byte[4096];

                ReadWriteTest.Write(buffer, address, randomObjects);

                var tagTypes = Arbitary.GetTagTypes(randomObjects);
                var oscMessage = OscReader.Read(buffer);
                var objects = oscMessage.GetAsObjects();

                for(int j = 0; j < randomCount; j++)
                {
                    Debug.Log($"{randomObjects[j]}, {objects[j]}");
                }

                Assert.AreEqual(address, oscMessage.Address);
                Assert.AreEqual(tagTypes, oscMessage.TagTypes);
                Assert.AreEqual(randomObjects, objects);
            }
        }

        [Test]
        public void GetAsTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                int randomCount = UnityEngine.Random.Range(1, 10);
                var randomObjects = Arbitary.GetRandomObjects(randomCount);
                var address = Arbitary.GetRandomAddress();
                var buffer = new byte[4096];

                ReadWriteTest.Write(buffer, address, randomObjects);

                var tagTypes = Arbitary.GetTagTypes(randomObjects);
                var oscMessage = OscReader.Read(buffer);
                var objects = new object[oscMessage.Count];

                for(int j = 0; j < oscMessage.Count; j++)
                {
                    var tagType = oscMessage.TagTypes[j + 1];

                    switch(tagType)
                    {
                        case TagType.Int32:
                            objects[j] = oscMessage.GetAsInt32(j);
                            break;
                        case TagType.Int64:
                            objects[j] = oscMessage.GetAsInt64(j);
                            break;
                        case TagType.Float:
                            objects[j] = oscMessage.GetAsFloat(j);
                            break;
                        case TagType.String:
                            objects[j] = oscMessage.GetAsString(j);
                            break;
                        case TagType.Blob:
                            objects[j] = oscMessage.GetAsBlob(j);
                            break;
                        case TagType.Double:
                            objects[j] = oscMessage.GetAsDouble(j);
                            break;
                        case TagType.Char:
                            objects[j] = oscMessage.GetAsChar(j);
                            break;
                        case TagType.True:
                            objects[j] = oscMessage.GetAsBoolean(j);
                            break;
                        case TagType.False:
                            objects[j] = oscMessage.GetAsBoolean(j);
                            break;
                        case TagType.Infinitum:
                            objects[j] = oscMessage.GetAsInfinitum(j);
                            break;
                        case TagType.Nil:
                            objects[j] = oscMessage.GetAsNil(j);
                            break;
                        case TagType.Color32:
                            objects[j] = oscMessage.GetAsColor32(j);
                            break;
                        case TagType.TimeTag:
                            objects[j] = oscMessage.GetAsTimetagAsULong(j);
                            break;
                    }
                }

                Assert.AreEqual(address, oscMessage.Address);
                Assert.AreEqual(tagTypes, oscMessage.TagTypes);
                Assert.AreEqual(randomObjects, objects);
            }
        }
    }
}
