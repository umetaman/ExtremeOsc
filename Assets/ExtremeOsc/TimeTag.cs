using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc
{
    public static class TimeTag
    {
        public static DateTime Epoch { get => new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc); }

        public static UInt64 DateTimeToNtp(DateTime dateTime)
        {
            UInt64 seconds = (UInt32)(dateTime - Epoch).TotalSeconds;
            UInt64 fraction = (UInt32)(0xFFFFFFFF * ((double)dateTime.Millisecond / 1000));

            return (seconds << 32) + fraction;
        }

        public static DateTime NtpToDateTime(UInt64 ntpTime)
        {
            if (ntpTime == 1)
                return DateTime.Now;

            UInt32 seconds = (UInt32)(ntpTime >> 32);
            var fraction = TimetagToFraction(ntpTime);

            var dateTime = Epoch;
            dateTime = dateTime.AddSeconds(seconds);
            dateTime = dateTime.AddSeconds(fraction);
            return dateTime;
        }

        public static double TimetagToFraction(UInt64 timetag)
        {
            if (timetag == 1)
                return 0.0;

            UInt32 seconds = (UInt32)(timetag & 0x00000000FFFFFFFF);
            double fraction = (double)seconds / (UInt32)(0xFFFFFFFF);
            return fraction;
        }

        public static bool EqualWithMiliseconds(DateTime a, DateTime b)
        {
            var span = a - b;
            return span.TotalMilliseconds < 1;
        }
    }
}
