using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc.Tests
{
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
        };

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
                        values[index] = GetRandomString(UnityEngine.Random.Range(0, 256));
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

        public static string GetRandomString(int length)
        {
            //string str = "ajshakjshdlakjhsdlajhflajhdfljahsldfkjhasldkjhflasjfhdlajh";
            ////for (int i = 0; i < length; i++)
            ////{
            ////    str += (char)Random.Range(1, 255);
            ////}
            //return str;

            // ASCII
            string str = "日本語";

            //for (int i = 0; i < length; i++)
            //{
            //    str += (char)Random.Range(1, 255);
            //}

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
    }
}
