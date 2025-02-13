using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    internal static class Utils
    {
        // 60 * 60 * 24 * 365 * 70
        public const uint Seconds70Years = 2207520000;
        // 2^32
        public const long NtpFractionalUnit = 4294967296;

        public static int AlignBytes4(int length) => (length + 3) & ~0b0011;

        public static UInt64 UnixToNtp(DateTimeOffset unixTime)
        {
            UInt32 seconds = (UInt32)(unixTime.ToUnixTimeSeconds() + Seconds70Years);
            UInt32 fraction = (UInt32)((unixTime.Millisecond * NtpFractionalUnit) / 1000);
            return ((UInt64)seconds << 32) | fraction;
        }
    }
}
