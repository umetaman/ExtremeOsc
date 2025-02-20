using ExtremeOsc.Annotations;
using System;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

namespace ExtremeOsc.Test
{
    [OscPackable]
    public partial class PackableTest
    {
        [OscElementAt(0)]
        public int IntValue;
        [OscElementAt(1)]
        public float FloatValue;
        [OscElementAt(2)]
        public string StringValue { private set; get; } = "";
        [OscElementAt(3)]
        public long LongValue;
        [OscElementAt(4)]
        public double DoubleValue;
        [OscElementAt(5)]
        public bool BoolValue;
        [OscElementAt(6)]
        public Byte[] BytesValue = new byte[0];
        [OscElementAt(7)]
        public Char CharValue;
    }
}
