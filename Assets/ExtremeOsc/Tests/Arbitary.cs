using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc.Tests
{
    using Random = UnityEngine.Random;

    public static class Arbitary
    {
        public static readonly char[] TagTypes = new char[]
        {
            TagType.Int32,
            TagType.Int64,
            TagType.Float,
            TagType.String,
            // TagType.Symbol,
            TagType.Blob,
            TagType.Double,
            TagType.Char,
            TagType.True,
            TagType.False,
            TagType.Infinitum,
            TagType.Nil,
            TagType.Color32,
            TagType.TimeTag,
        };
        public const string Ascii = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const string Japanase = "つれづれなるまゝに、日暮らし、硯にむかひて、心にうつりゆくよしなし事を、そこはかとなく書きつくれば、あやしうこそものぐるほしけれ。";

        public static string GetRandomAddress(int minLength = 1, int maxLength = 10, int slashCount = 1)
        {
            string address = "/";

            for(int i = 0; i < slashCount; i++)
            {
                address += GetRandomStringAscii(UnityEngine.Random.Range(minLength, maxLength));

                if (i < slashCount - 1)
                {
                    address += "/";
                }
            }
            return address;
        }

        public static object[] GetRandomObjects(int count)
        {
            var tagTypes = GetRandomTagTypes(count);
            var values = new object[count];

            for (int i = 0; i < count + 1; i++)
            {
                int index = i - 1;
                switch (tagTypes[i])
                {
                    case TagType.Int32:
                        values[index] = GetRandomInt32();
                        break;
                    case TagType.Int64:
                        values[index] = GetRandomInt64();
                        break;
                    case TagType.Float:
                        values[index] = GetRandomFloat();
                        break;
                    case TagType.String:
                        values[index] = GetRandomStringUtf8(UnityEngine.Random.Range(0, 256));
                        break;
                    case TagType.Blob:
                        values[index] = GetRandomBlob(UnityEngine.Random.Range(0, 1024));
                        break;
                    case TagType.Double:
                        values[index] = GetRandomDouble();
                        break;
                    case TagType.Char:
                        values[index] = GetRandomChar();
                        break;
                    case TagType.True:
                        values[index] = true;
                        break;
                    case TagType.False:
                        values[index] = false;
                        break;
                    case TagType.Infinitum:
                        values[index] = Infinitum.Value;
                        break;
                    case TagType.Nil:
                        values[index] = Nil.Value;
                        break;
                    case TagType.Color32:
                        values[index] = GetRandomColor32();
                        break;
                    case TagType.TimeTag:
                        values[index] = GetRandomULong();
                        break;
                }
            }

            return values;
        }

        public static string GetTagTypes(object[] values)
        {
            string tagTypes = ",";

            for (int i = 0; i < values.Length; i++)
            {
                switch (values[i])
                {
                    case int _:
                        tagTypes += "i";
                        break;
                    case long _:
                        tagTypes += "h";
                        break;
                    case float _:
                        tagTypes += "f";
                        break;
                    case string _:
                        tagTypes += "s";
                        break;
                    case byte[] _:
                        tagTypes += "b";
                        break;
                    case double _:
                        tagTypes += "d";
                        break;
                    case char _:
                        tagTypes += "c";
                        break;
                    case bool @bool:
                        tagTypes += @bool ? "T" : "F";
                        break;
                    case Infinitum _:
                        tagTypes += "I";
                        break;
                    case Nil _:
                        tagTypes += "N";
                        break;
                    case Color32 _:
                        tagTypes += "r";
                        break;
                    case ulong _:
                        tagTypes += "t";
                        break;
                }
            }

            return tagTypes;
        }

        public static string GetRandomTagTypes(int count)
        {
            string tagTypes = ",";

            for (int i = 0; i < count; i++)
            {
                tagTypes += TagTypes[Random.Range(0, TagTypes.Length)];
            }

            return tagTypes;
        }

        public static int GetRandomInt32()
        {
            return Random.Range(int.MinValue, int.MaxValue);
        }

        public static long GetRandomInt64()
        {
            return Random.Range(int.MinValue, int.MaxValue);
        }

        public static float GetRandomFloat()
        {
            return Random.Range(float.MinValue, float.MaxValue);
        }

        public static string GetRandomStringAscii(int length)
        {
            // ASCII
            string str = "";

            for (int i = 0; i < length; i++)
            {
                str += Ascii[Random.Range(0, Ascii.Length)];
            }

            return str;
        }

        public static string GetRandomStringUtf8(int length)
        {
            string str = "";
            for (int i = 0; i < length; i++)
            {
                if (Random.value > 0.5)
                {
                    str += Japanase[Random.Range(0, Japanase.Length)];
                }
                else
                {
                    str += Ascii[Random.Range(0, Ascii.Length)];
                }
            }
            return str;
        }

        public static byte[] GetRandomBlob(int length)
        {
            byte[] blob = new byte[length];
            for (int i = 0; i < length; i++)
            {
                blob[i] = (byte)Random.Range(0, 255);
            }
            return blob;
        }

        public static double GetRandomDouble()
        {
            return Random.Range(float.MinValue, float.MaxValue);
        }

        public static char GetRandomChar()
        {
            return (char)Random.Range(0, 255);
        }

        public static bool GetRandomBool()
        {
            return Random.Range(0, 2) == 0;
        }

        public static Color32 GetRandomColor32()
        {
            return new Color32(
                (byte)Random.Range(0, 255),
                (byte)Random.Range(0, 255),
                (byte)Random.Range(0, 255),
                (byte)Random.Range(0, 255));
        }

        public static ulong GetRandomULong()
        {
            ulong value = 0;

            var datetime = GetRandomDateTime();
            value = (ulong)datetime.Ticks;

            return value;
        }

        public static DateTime GetRandomDateTime()
        {
            var randomTime = DateTime.Now.AddMilliseconds(Random.Range(-10000000, 10000000));
            return randomTime;
        }
    }
}
