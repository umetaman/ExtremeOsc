using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class Utils
    {
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
            if(ntpTime == 0)
            {
                return DateTime.UtcNow;
            }

            uint seconds = (uint)(ntpTime >> 32);
            uint fraction = (uint)(ntpTime & 0xFFFFFFFF);

            double fractionSeconds = fraction / (double)NtpFractionalUnit;

            var time = Epoch;
            return time.AddSeconds(seconds).AddSeconds(fractionSeconds);
        }
    }
}
