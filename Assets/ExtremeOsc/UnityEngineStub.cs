#if !UNITY_5_3_OR_NEWER && !UNITY
namespace UnityEngine
{
    public struct Color32
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Color32(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Color32(Color c)
        {
            return new Color32((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255), (byte)(c.a * 255));
        }
    }

    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    public static class Debug
    {
        public static void LogWarning(object message)
        {
            System.Diagnostics.Debug.WriteLine($"[Warning] {message}");
        }

        public static void LogError(object message)
        {
            System.Diagnostics.Debug.WriteLine($"[Error] {message}");
        }
    }
}
#endif
