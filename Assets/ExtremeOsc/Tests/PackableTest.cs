using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeOsc.Annotations;
using NUnit.Framework;

namespace ExtremeOsc.Tests
{
    public class PackableTest
    {
        const int BUFFER_SIZE = 4096 * 2;

        [Test]
        public void IntValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new IntValue(
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                IntValue v2 = new IntValue(0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void LongValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new LongValue(
                    Arbitary.GetRandomInt64(),
                    Arbitary.GetRandomInt64(),
                    Arbitary.GetRandomInt64(),
                    Arbitary.GetRandomInt64()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                LongValue v2 = new LongValue(0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void FloatValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new FloatValue(
                    Arbitary.GetRandomFloat(),
                    Arbitary.GetRandomFloat(),
                    Arbitary.GetRandomFloat(),
                    Arbitary.GetRandomFloat()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                FloatValue v2 = new FloatValue(0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void StringValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new StringValue(
                    Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50))
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                StringValue v2 = new StringValue("", "", "", "", "");
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void BooleanValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new BooleanValue(
                    Arbitary.GetRandomBool(),
                    Arbitary.GetRandomBool(),
                    Arbitary.GetRandomBool(),
                    Arbitary.GetRandomBool()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                BooleanValue v2 = new BooleanValue(false, false, false, false);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void BlobValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new BlobValue(
                    Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128))
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                BlobValue v2 = new BlobValue(new byte[0], new byte[0], new byte[0], new byte[0], new byte[0]);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void DoubleValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new DoubleValue(
                    Arbitary.GetRandomDouble(),
                    Arbitary.GetRandomDouble(),
                    Arbitary.GetRandomDouble(),
                    Arbitary.GetRandomDouble(),
                    Arbitary.GetRandomDouble()
                    );
                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                DoubleValue v2 = new DoubleValue(0, 0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void Color32Value()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new Color32Value(
                    Arbitary.GetRandomColor32(),
                    Arbitary.GetRandomColor32(),
                    Arbitary.GetRandomColor32(),
                    Arbitary.GetRandomColor32(),
                    Arbitary.GetRandomColor32()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                Color32Value v2 = new Color32Value(new Color32(), new Color32(), new Color32(), new Color32(), new Color32());
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void CharValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new CharValue(
                    Arbitary.GetRandomChar(),
                    Arbitary.GetRandomChar(),
                    Arbitary.GetRandomChar(),
                    Arbitary.GetRandomChar(),
                    Arbitary.GetRandomChar()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                CharValue v2 = new CharValue('\0', '\0', '\0', '\0', '\0');
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void TimeTagValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new TimeTagValue(
                    Arbitary.GetRandomULong(),
                    Arbitary.GetRandomULong(),
                    Arbitary.GetRandomULong(),
                    Arbitary.GetRandomULong(),
                    Arbitary.GetRandomULong()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                TimeTagValue v2 = new TimeTagValue(0, 0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void NilValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new NilValue(
                    new Nil(),
                    new Nil(),
                    new Nil(),
                    new Nil(),
                    new Nil()
                    );
                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);
                offset = 0;
                NilValue v2 = new NilValue(new Nil(), new Nil(), new Nil(), new Nil(), new Nil());
                v2.Unpack(buffer, ref offset);
                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void InfinitumValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new InfinitumValue(
                    new Infinitum(),
                    new Infinitum(),
                    new Infinitum(),
                    new Infinitum(),
                    new Infinitum()
                    );

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                InfinitumValue v2 = new InfinitumValue(new Infinitum(), new Infinitum(), new Infinitum(), new Infinitum(), new Infinitum());
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void ClassValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new ClassValue()
                {
                    IntValue = Arbitary.GetRandomInt32(),
                    LongValue = Arbitary.GetRandomInt64(),
                    FloatValue = Arbitary.GetRandomFloat(),
                    StringValue = Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    BytesValue = Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    DoubleValue = Arbitary.GetRandomDouble(),
                    Color32Value = Arbitary.GetRandomColor32(),
                    CharValue = Arbitary.GetRandomChar(),
                    TimeTagValue = Arbitary.GetRandomULong(),
                    BooleanValue = Arbitary.GetRandomBool(),
                    NilValue = Nil.Value,
                    InfinitumValue = Infinitum.Value,
                };

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                ClassValue v2 = new ClassValue()
                {

                };
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }

        [Test]
        public void StructValue()
        {
            for (int i = 0; i < 1000; i++)
            {
                var v = new StructValue()
                {
                    IntValue = Arbitary.GetRandomInt32(),
                    LongValue = Arbitary.GetRandomInt64(),
                    FloatValue = Arbitary.GetRandomFloat(),
                    StringValue = Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                    BytesValue = Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                    DoubleValue = Arbitary.GetRandomDouble(),
                    Color32Value = Arbitary.GetRandomColor32(),
                    CharValue = Arbitary.GetRandomChar(),
                    TimeTagValue = Arbitary.GetRandomULong(),
                    BooleanValue = Arbitary.GetRandomBool(),
                    NilValue = Nil.Value,
                    InfinitumValue = Infinitum.Value,
                };

                byte[] buffer = new byte[BUFFER_SIZE];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                StructValue v2 = new StructValue()
                {
                };
                v2.Unpack(buffer, ref offset);

                Assert.AreEqual(v, v2);
            }
        }
    }
}
