using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public struct Infinitum
    {
        public static readonly Infinitum Value = new Infinitum();

        public override bool Equals(object obj)
        {
            return obj is Infinitum;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Infinitum other)
        {
            return true;
        }
    }
}
