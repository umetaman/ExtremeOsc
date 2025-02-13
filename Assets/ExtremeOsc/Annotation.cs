using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc.Annotations
{
    public class OscPacket : Attribute
    {

    }

    public class OscElementAt : Attribute
    {
        public int Index { get; private set; }

        public OscElementAt(int index)
        {
            Index = index;
        }
    }

    public class OscCallback : Attribute
    {
        public string Address { get; private set; }

        public OscCallback(string address)
        {
            Address = address;
        }
    }

    public class OscReceiver : Attribute
    {

    }
}
