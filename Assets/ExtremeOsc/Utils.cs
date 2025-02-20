using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class Utils
    {
        //// 60 * 60 * 24 * 365 * 70
        //public const uint Seconds70Years = 2207520000;
        // 2^32
        public const long NtpFractionalUnit = 4294967296;

        public static int AlignBytes4(int length) => (length + 3) & ~0b0011;

        public static readonly DateTime Epoch = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static UInt64 DateTimeToNtp(DateTime dateTime)
        {
            var span = dateTime - Epoch;

            uint seconds = (uint)span.TotalSeconds;
            uint fraction = (uint)((span.TotalSeconds - seconds) * NtpFractionalUnit);

            return ((ulong)seconds << 32) | fraction;
        }

        public static DateTime NtpToDateTime(UInt64 ntpTime)
        {
            uint seconds = (uint)(ntpTime >> 32);
            uint fraction = (uint)(ntpTime & 0xFFFFFFFF);

            double fractionSeconds = fraction / (double)NtpFractionalUnit;

            var time = Epoch;
            return time.AddSeconds(seconds).AddSeconds(fractionSeconds);
        }

        public static double NtpToFraction(UInt64 ntpTime)
        {
            UInt32 seconds = (UInt32)(ntpTime & 0x00000000FFFFFFFF);
            return (double)seconds / (UInt32)0xFFFFFFFF;
        }
    }
}
