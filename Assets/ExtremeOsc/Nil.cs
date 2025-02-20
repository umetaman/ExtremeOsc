using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public struct Nil
    {
        public static readonly Nil Value = new Nil();
        
        public override bool Equals(object obj)
        {
            return obj is Nil;
        }
        
        public override int GetHashCode()
        {
            return 0;
        }
        
        public bool Equals(Nil other)
        {
            return true;
        }
    }
}
