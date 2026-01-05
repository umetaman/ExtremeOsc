using System.Runtime.CompilerServices;
using UnityEngine;

namespace ExtremeOsc
{
    public partial class OscWriter
    {
        public delegate int ElementWriter(byte[] buffer, int offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBundle(byte[] buffer, ulong timestamp, ref int offset)
        {
            WriteString(buffer, "#bundle", ref offset);
            WriteULong(buffer, timestamp, ref offset);
        }

        public static void WriteBundleElement(byte[] buffer, ElementWriter writer, ref int offset)
        {
            int startOffset = offset;
            // reserve element size
            offset += 4;
            int endOffset = writer(buffer, offset);
            int elementSize = endOffset - (startOffset + 4);
            WriteInt32(buffer, elementSize, ref startOffset);
            offset += elementSize;
        }
    }
}
