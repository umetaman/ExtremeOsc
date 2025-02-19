using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class OscPackable : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class OscElementAt : Attribute
    {
        public int Index { get; private set; }

        public OscElementAt(int index)
        {
            Index = index;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
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
