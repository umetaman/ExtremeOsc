using System;

namespace ExtremeOsc.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OscCallbackAttribute : System.Attribute
    {
        public string Address { private set; get; }
        public OscCallbackAttribute(string address)
        {
            this.Address = address;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class OscElementAtAttribute : System.Attribute
    {
        public int Index { private set; get; }
        public OscElementAtAttribute(int index)
        {
            this.Index = index;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class OscPackableAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class OscReceiverAttribute : System.Attribute
    {
    }
}
