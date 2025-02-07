using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    internal static class Utils
    {
        public static int AlignBytes4(int length) => (length + 3) & ~0b0011;
    }
}
