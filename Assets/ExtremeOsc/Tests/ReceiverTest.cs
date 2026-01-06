using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using ExtremeOsc.Annotations;
using System;

namespace ExtremeOsc.Tests
{
    [OscReceiver]
    public partial class TestReceiver
    {
        public static bool IsStaticCalled = false;
        public bool IsCalled = false;

        public static Action<ClassValue> OnTestStaticClassValue;
        public Action<IntValue> OnTestIntValue;
        public Action<ClassValue> OnTestClassValue;
        public Action<StructValue> OnTestStructValue;

        [OscCallback("/test/intvalue")]
        public void TestIntValue(string address, IntValue intValue)
        {
            IsCalled = true;
            OnTestIntValue?.Invoke(intValue);
        }

        [OscCallback("/test/classvalue")]
        public void TestClassValue(string address, ClassValue classValue)
        {
            IsCalled = true;
            OnTestClassValue?.Invoke(classValue);
        }

        [OscCallback("/test/structvalue")]
        public void TestStructValue(string address, StructValue structValue)
        {
            IsCalled = true;
            OnTestStructValue?.Invoke(structValue);
        }

        [OscCallback("/test/noargument/multiple")]
        [OscCallback("/test/noargument")]
        public void TestNoArgument(string address)
        {
            IsCalled = true;
        }

        [OscCallback("/test/static/classvalue")]
        public static void TestStaticClassValue(string address, ClassValue classValue)
        {
            IsStaticCalled = true;
            OnTestStaticClassValue?.Invoke(classValue);
        }

        [OscCallback("/test/primitive/classvalue")]
        public void TestPrimitiveClassValue(string address,
            int v0, long v1, float v2, string v3, byte[] v4, double v5, Color32 v6, char v7, ulong v8, bool v9, Nil v10, Infinitum v11)
        {
            IsCalled = true;
            var v = new ClassValue()
            {
                IntValue = v0,
                LongValue = v1,
                FloatValue = v2,
                StringValue = v3,
                BytesValue = v4,
                DoubleValue = v5,
                Color32Value = v6,
                CharValue = v7,
                TimeTagValue = v8,
                BooleanValue = v9,
                NilValue = v10,
                InfinitumValue = v11,
            };

            OnTestClassValue?.Invoke(v);
        }

        [OscCallback("/test/objects/classvalue")]
        public void TestObjectsClassValue(string address, object[] objects)
        {
            IsCalled = true;

            var v = new ClassValue()
            {
                IntValue = (int)objects[0],
                LongValue = (long)objects[1],
                FloatValue = (float)objects[2],
                StringValue = (string)objects[3],
                BytesValue = (byte[])objects[4],
                DoubleValue = (double)objects[5],
                Color32Value = (Color32)objects[6],
                CharValue = (char)objects[7],
                TimeTagValue = (ulong)objects[8],
                BooleanValue = (bool)objects[9],
                NilValue = (Nil)objects[10],
                InfinitumValue = (Infinitum)objects[11],
            };

            OnTestClassValue?.Invoke(v);
        }

        [OscCallback("/test/reader/classvalue")]
        public void TestReaderClassValue(string address, OscReader reader)
        {
            IsCalled = true;
            var v = new ClassValue()
            {
                IntValue = reader.GetAsInt32(0),
                LongValue = reader.GetAsInt64(1),
                FloatValue = reader.GetAsFloat(2),
                StringValue = reader.GetAsString(3),
                BytesValue = reader.GetAsBlob(4),
                DoubleValue = reader.GetAsDouble(5),
                Color32Value = reader.GetAsColor32(6),
                CharValue = reader.GetAsChar(7),
                TimeTagValue = reader.GetAsTimetagAsULong(8),
                BooleanValue = reader.GetAsBoolean(9),
                NilValue = reader.GetAsNil(10),
                InfinitumValue = reader.GetAsInfinitum(11),
            };
            OnTestClassValue?.Invoke(v);
        }

        [OscCallback("/test/reader/classvalue/timestamp")]
        public void TestReaderClassValueTimestamp(string address, OscReader reader, ulong timestamp)
        {
            IsCalled = true;
            var v = new ClassValue()
            {
                IntValue = reader.GetAsInt32(0),
                LongValue = reader.GetAsInt64(1),
                FloatValue = reader.GetAsFloat(2),
                StringValue = reader.GetAsString(3),
                BytesValue = reader.GetAsBlob(4),
                DoubleValue = reader.GetAsDouble(5),
                Color32Value = reader.GetAsColor32(6),
                CharValue = reader.GetAsChar(7),
                TimeTagValue = timestamp,
                BooleanValue = reader.GetAsBoolean(8),
                NilValue = reader.GetAsNil(9),
                InfinitumValue = reader.GetAsInfinitum(10),
            };
            Assert.AreEqual(timestamp, 1UL);
            OnTestClassValue?.Invoke(v);
        }

        [OscCallback("/test/ref/classvalue")]
        public void TestRefClassValue(string address, ref ClassValue classValue)
        {
            IsCalled = true;
            OnTestClassValue?.Invoke(classValue);
        }

        [OscCallback("/test/in/classvalue")]
        public void TestInClassValue(string address, in ClassValue classValue)
        {
            IsCalled = true;
            OnTestClassValue?.Invoke(classValue);
        }
    }

    public class ReceiverTest
    {
        [Test]
        public void ReceiverIntValue()
        {
            var buffer = new byte[4096];
            var intValue = new IntValue(
                Arbitary.GetRandomInt32(),
                Arbitary.GetRandomInt32(),
                Arbitary.GetRandomInt32(),
                Arbitary.GetRandomInt32()
                );
            OscWriter.Write(buffer, "/test/intvalue", intValue);

            var receiver = new TestReceiver();
            IntValue result = new IntValue();
            receiver.OnTestIntValue = (value) =>
            {
                result = value;
            };

            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(intValue, result);
        }

        [Test]
        public void ReceiverClassValue()
        {
            var buffer = new byte[4096];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/classvalue", classValue);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };

            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(classValue, result);
        }

        [Test]
        public void ReceiverStructValue()
        {
            var buffer = new byte[4096];
            var structValue = new StructValue()
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
            OscWriter.Write(buffer, "/test/structvalue", structValue);
            var receiver = new TestReceiver();

            StructValue result = new StructValue();
            receiver.OnTestStructValue = (value) =>
            {
                result = value;
            };
            
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(structValue, result);
        }

        [Test]
        public void ReceiverNoArgument()
        {
            var buffer = new byte[4096];
            OscWriter.Write(buffer, "/test/noargument");
            
            var receiver = new TestReceiver();
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
        }

        [Test]
        public void ReceiverNoArgumentMultiple()
        {
            var buffer = new byte[4096];
            OscWriter.Write(buffer, "/test/noargument/multiple");

            var receiver = new TestReceiver();
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
        }

        [Test]
        public void ReceiverStaticClassValue()
        {
            var buffer = new byte[4096];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/static/classvalue", classValue);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            TestReceiver.OnTestStaticClassValue = (value) =>
            {
                result = value;
            };
            
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(TestReceiver.IsStaticCalled);
            Assert.AreEqual(classValue, result);
        }

        [Test]
        public void ReceiverPrimitiveClassValue()
        {
            var buffer = new byte[4096*2];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/primitive/classvalue", classValue);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };

            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(classValue, result);
        }

        [Test]
        public void ReceiverObjectArrayToArguments()
        {
            var buffer = new byte[4096 * 2];
            var objects = new object[]
            {
                Arbitary.GetRandomInt32(),
                Arbitary.GetRandomInt64(),
                Arbitary.GetRandomFloat(),
                Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                Arbitary.GetRandomDouble(),
                Arbitary.GetRandomColor32(),
                Arbitary.GetRandomChar(),
                Arbitary.GetRandomULong(),
                Arbitary.GetRandomBool(),
                Nil.Value,
                Infinitum.Value,
            };
            OscWriter.Write(buffer, "/test/primitive/classvalue", objects);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(objects[0], result.IntValue);
            Assert.AreEqual(objects[1], result.LongValue);
            Assert.AreEqual(objects[2], result.FloatValue);
            Assert.AreEqual(objects[3], result.StringValue);
            Assert.AreEqual(objects[4], result.BytesValue);
            Assert.AreEqual(objects[5], result.DoubleValue);
            Assert.AreEqual(objects[6], result.Color32Value);
            Assert.AreEqual(objects[7], result.CharValue);
            Assert.AreEqual(objects[8], result.TimeTagValue);
            Assert.AreEqual(objects[9], result.BooleanValue);
            Assert.AreEqual(objects[10], result.NilValue);
            Assert.AreEqual(objects[11], result.InfinitumValue);
        }

        [Test]
        public void ReceiverObjectArrayArguments()
        {
            var buffer = new byte[4096 * 2];
            var objects = new object[]
            {
                Arbitary.GetRandomInt32(),
                Arbitary.GetRandomInt64(),
                Arbitary.GetRandomFloat(),
                Arbitary.GetRandomStringUtf8(UnityEngine.Random.Range(0, 50)),
                Arbitary.GetRandomBlob(UnityEngine.Random.Range(0, 128)),
                Arbitary.GetRandomDouble(),
                Arbitary.GetRandomColor32(),
                Arbitary.GetRandomChar(),
                Arbitary.GetRandomULong(),
                Arbitary.GetRandomBool(),
                Nil.Value,
                Infinitum.Value,
            };
            OscWriter.Write(buffer, "/test/objects/classvalue", objects);
            
            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };
            
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(objects[0], result.IntValue);
            Assert.AreEqual(objects[1], result.LongValue);
            Assert.AreEqual(objects[2], result.FloatValue);
            Assert.AreEqual(objects[3], result.StringValue);
            Assert.AreEqual(objects[4], result.BytesValue);
            Assert.AreEqual(objects[5], result.DoubleValue);
            Assert.AreEqual(objects[6], result.Color32Value);
            Assert.AreEqual(objects[7], result.CharValue);
            Assert.AreEqual(objects[8], result.TimeTagValue);
            Assert.AreEqual(objects[9], result.BooleanValue);
            Assert.AreEqual(objects[10], result.NilValue);
            Assert.AreEqual(objects[11], result.InfinitumValue);
        }

        [Test]
        public void ReceiverReaderArgument()
        {
            var buffer = new byte[4096 * 2];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/reader/classvalue", classValue);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };

            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(classValue, result);
        }

        [Test]
        public void ReceiverRefArgument()
        {
            var buffer = new byte[4096];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/ref/classvalue", classValue);
            
            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };
            
            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(classValue, result);
        }

        [Test]
        public void ReceiverInArgument()
        {
            var buffer = new byte[4096];
            var classValue = new ClassValue()
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
            OscWriter.Write(buffer, "/test/in/classvalue", classValue);

            var receiver = new TestReceiver();
            ClassValue result = new ClassValue();
            receiver.OnTestClassValue = (value) =>
            {
                result = value;
            };

            receiver.ReceiveOscPacket(buffer);
            Assert.IsTrue(receiver.IsCalled);
            Assert.AreEqual(classValue, result);
        }
    }
}
