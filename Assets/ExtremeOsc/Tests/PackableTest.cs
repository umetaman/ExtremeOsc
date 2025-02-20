using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeOsc;
using ExtremeOsc.Annotations;
using NUnit.Framework;

namespace ExtremeOsc.Tests
{
    [OscPackable]
    public partial class IntValue
    {
        [OscElementAt(0)]
        public int V0 { get; set; }
        [OscElementAt(1)]
        public int V1 { get; set; }
        [OscElementAt(2)]
        public int V2;
        [OscElementAt(3)]
        public int V3 { private set; get; }

        public IntValue(int v0, int v1, int v2, int v3)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            IntValue v = (IntValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3;
        }
    }

    [OscPackable]
    public partial struct FloatValue
    {
        [OscElementAt(0)]
        public float V0 { get; set; }
        [OscElementAt(1)]
        public float V1 { get; set; }
        [OscElementAt(2)]
        public float V2;
        [OscElementAt(3)]
        public float V3 { private set; get; }

        public FloatValue(float v0, float v1, float v2, float v3)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            FloatValue v = (FloatValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3;
        }
    }

    public class PackableTest
    {
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

                byte[] buffer = new byte[1024];
                int offset = 0;
                v.Pack(buffer, ref offset);

                offset = 0;
                IntValue v2 = new IntValue(0, 0, 0, 0);
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
                byte[] buffer = new byte[1024];
                int offset = 0;
                v.Pack(buffer, ref offset);
                
                offset = 0;
                FloatValue v2 = new FloatValue(0, 0, 0, 0);
                v2.Unpack(buffer, ref offset);
                
                Assert.AreEqual(v, v2);
            }
        }
    }
}
