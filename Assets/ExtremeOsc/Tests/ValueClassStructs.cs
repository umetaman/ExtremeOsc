using ExtremeOsc.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public partial class LongValue
    {
        [OscElementAt(0)]
        public long V0 { get; set; }
        [OscElementAt(1)]
        public long V1 { get; set; }
        [OscElementAt(2)]
        public long V2;
        [OscElementAt(3)]
        public long V3 { private set; get; }
        public LongValue(long v0, long v1, long v2, long v3)
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
            LongValue v = (LongValue)obj;
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

    [OscPackable]
    public partial class StringValue
    {
        [OscElementAt(0)]
        public string V0 { get; set; }
        [OscElementAt(1)]
        public string V1 { get; set; }
        [OscElementAt(2)]
        public string V2;
        [OscElementAt(3)]
        public string V3 { private set; get; }
        [OscElementAt(4)]
        public string V4 { get; set; }

        public StringValue(string v0, string v1, string v2, string v3, string v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            StringValue v = (StringValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3 && V4 == v.V4;
        }
    }

    [OscPackable]
    public partial class BooleanValue
    {
        [OscElementAt(0)]
        public bool V0 { get; set; }
        [OscElementAt(1)]
        public bool V1 { get; set; }
        [OscElementAt(2)]
        public bool V2;
        [OscElementAt(3)]
        public bool V3 { private set; get; }

        public BooleanValue(bool v0, bool v1, bool v2, bool v3)
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
            BooleanValue v = (BooleanValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3;
        }
    }

    [OscPackable]
    public partial class BlobValue
    {
        [OscElementAt(0)]
        public byte[] V0 { get; set; }
        [OscElementAt(1)]
        public byte[] V1 { get; set; }
        [OscElementAt(2)]
        public byte[] V2;
        [OscElementAt(3)]
        public byte[] V3 { private set; get; }
        [OscElementAt(4)]
        public byte[] V4 { get; set; }

        public BlobValue(byte[] v0, byte[] v1, byte[] v2, byte[] v3, byte[] v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            BlobValue v = (BlobValue)obj;
            return
                V0.SequenceEqual(v.V0) &&
                V1.SequenceEqual(v.V1) &&
                V2.SequenceEqual(v.V2) &&
                V3.SequenceEqual(v.V3) &&
                V4.SequenceEqual(v.V4);
        }
    }

    [OscPackable]
    public partial class DoubleValue
    {
        [OscElementAt(0)]
        public double V0 { get; set; }
        [OscElementAt(1)]
        public double V1 { get; set; }
        [OscElementAt(2)]
        public double V2;
        [OscElementAt(3)]
        public double V3 { private set; get; }
        [OscElementAt(4)]
        public double V4 { get; set; }

        public DoubleValue(double v0, double v1, double v2, double v3, double v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            DoubleValue v = (DoubleValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3 && V4 == v.V4;
        }
    }

    [OscPackable]
    public partial class Color32Value
    {
        [OscElementAt(0)]
        public Color32 V0 { get; set; }
        [OscElementAt(1)]
        public Color32 V1 { get; set; }
        [OscElementAt(2)]
        public Color32 V2;
        [OscElementAt(3)]
        public Color32 V3 { private set; get; }
        [OscElementAt(4)]
        public Color32 V4 { get; set; }
        public Color32Value(Color32 v0, Color32 v1, Color32 v2, Color32 v3, Color32 v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Color32Value v = (Color32Value)obj;
            return V0.Equals(v.V0) && V1.Equals(v.V1) && V2.Equals(v.V2) && V3.Equals(v.V3) && V4.Equals(v.V4);
        }
    }

    [OscPackable]
    public partial class CharValue
    {
        [OscElementAt(0)]
        public char V0 { get; set; }
        [OscElementAt(1)]
        public char V1 { get; set; }
        [OscElementAt(2)]
        public char V2;
        [OscElementAt(3)]
        public char V3 { private set; get; }
        [OscElementAt(4)]
        public char V4 { get; set; }
        public CharValue(char v0, char v1, char v2, char v3, char v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CharValue v = (CharValue)obj;
            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3 && V4 == v.V4;
        }
    }

    [OscPackable]
    public partial class TimeTagValue
    {
        [OscElementAt(0)]
        public ulong V0 { get; set; }
        [OscElementAt(1)]
        public ulong V1 { get; set; }
        [OscElementAt(2)]
        public ulong V2;
        [OscElementAt(3)]
        public ulong V3 { private set; get; }
        [OscElementAt(4)]
        public ulong V4 { get; set; }

        public TimeTagValue(ulong v0, ulong v1, ulong v2, ulong v3, ulong v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TimeTagValue v = (TimeTagValue)obj;

            return V0 == v.V0 && V1 == v.V1 && V2 == v.V2 && V3 == v.V3 && V4 == v.V4;
        }
    }

    [OscPackable]
    public partial struct NilValue
    {
        [OscElementAt(0)]
        public Nil V0 { get; set; }
        [OscElementAt(1)]
        public Nil V1 { get; set; }
        [OscElementAt(2)]
        public Nil V2;
        [OscElementAt(3)]
        public Nil V3 { private set; get; }
        [OscElementAt(4)]
        public Nil V4 { get; set; }
        public NilValue(Nil v0, Nil v1, Nil v2, Nil v3, Nil v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            NilValue v = (NilValue)obj;
            return V0.Equals(v.V0) && V1.Equals(v.V1) && V2.Equals(v.V2) && V3.Equals(v.V3) && V4.Equals(v.V4);
        }
    }

    [OscPackable]
    public partial struct InfinitumValue
    {
        [OscElementAt(0)]
        public Infinitum V0 { get; set; }
        [OscElementAt(1)]
        public Infinitum V1 { get; set; }
        [OscElementAt(2)]
        public Infinitum V2;
        [OscElementAt(3)]
        public Infinitum V3 { private set; get; }
        [OscElementAt(4)]
        public Infinitum V4 { get; set; }

        public InfinitumValue(Infinitum v0, Infinitum v1, Infinitum v2, Infinitum v3, Infinitum v4)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            InfinitumValue v = (InfinitumValue)obj;
            return V0.Equals(v.V0) && V1.Equals(v.V1) && V2.Equals(v.V2) && V3.Equals(v.V3) && V4.Equals(v.V4);
        }
    }

    [OscPackable]
    public partial class ClassValue
    {
        [OscElementAt(0)]
        public int IntValue;
        [OscElementAt(1)]
        public float LongValue;
        [OscElementAt(2)]
        public float FloatValue;
        [OscElementAt(3)]
        public string StringValue;
        [OscElementAt(4)]
        public byte[] BytesValue;
        [OscElementAt(5)]
        public double DoubleValue;
        [OscElementAt(6)]
        public Color32 Color32Value;
        [OscElementAt(7)]
        public char CharValue;
        [OscElementAt(8)]
        public ulong TimeTagValue;
        [OscElementAt(9)]
        public bool BooleanValue;
        [OscElementAt(10)]
        public Nil NilValue;
        [OscElementAt(11)]
        public Infinitum InfinitumValue;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ClassValue v = (ClassValue)obj;
            return IntValue == v.IntValue &&
                LongValue == v.LongValue &&
                FloatValue == v.FloatValue &&
                StringValue == v.StringValue &&
                BytesValue.SequenceEqual(v.BytesValue) &&
                DoubleValue == v.DoubleValue &&
                Color32Value.Equals(v.Color32Value) &&
                CharValue == v.CharValue &&
                TimeTagValue == v.TimeTagValue &&
                BooleanValue == v.BooleanValue &&
                NilValue.Equals(v.NilValue) &&
                InfinitumValue.Equals(v.InfinitumValue);
        }
    }

    [OscPackable]
    public partial struct StructValue
    {
        [OscElementAt(0)]
        public int IntValue;
        [OscElementAt(1)]
        public float LongValue;
        [OscElementAt(2)]
        public float FloatValue;
        [OscElementAt(3)]
        public string StringValue;
        [OscElementAt(4)]
        public byte[] BytesValue;
        [OscElementAt(5)]
        public double DoubleValue;
        [OscElementAt(6)]
        public Color32 Color32Value;
        [OscElementAt(7)]
        public char CharValue;
        [OscElementAt(8)]
        public ulong TimeTagValue;
        [OscElementAt(9)]
        public bool BooleanValue;
        [OscElementAt(10)]
        public Nil NilValue;
        [OscElementAt(11)]
        public Infinitum InfinitumValue;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            StructValue v = (StructValue)obj;
            return IntValue == v.IntValue &&
                LongValue == v.LongValue &&
                FloatValue == v.FloatValue &&
                StringValue == v.StringValue &&
                BytesValue.SequenceEqual(v.BytesValue) &&
                DoubleValue == v.DoubleValue &&
                Color32Value.Equals(v.Color32Value) &&
                CharValue == v.CharValue &&
                TimeTagValue == v.TimeTagValue &&
                BooleanValue == v.BooleanValue &&
                NilValue.Equals(v.NilValue) &&
                InfinitumValue.Equals(v.InfinitumValue);
        }
    }
}
