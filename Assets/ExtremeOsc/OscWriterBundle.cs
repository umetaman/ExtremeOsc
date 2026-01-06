using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ExtremeOsc
{
    public struct BundleWriter : IDisposable
    {
        public void Dispose()
        {
        }

        public BundleWriter(byte[] buffer, ulong timestamp, ref int offset)
        {
            OscWriter.WriteBundle(buffer, timestamp, ref offset);
        }
    }

    public unsafe struct BundleElementWriter : System.IDisposable
    {
        private byte[] buffer;
        private int startOffset;
        private int* offset;

        public BundleElementWriter(byte[] buffer, ref int offset)
        {
            this.buffer = buffer;
            this.startOffset = offset;
            offset += 4;
            fixed (int* p = &offset)
            {
                this.offset = p;
            }
        }

        public void Dispose()
        {
            int endOffset = *offset;
            int elementSize = endOffset - (startOffset + 4);
            OscWriter.WriteInt32(buffer, elementSize, ref startOffset);
        }
    }

    public unsafe struct BundleBuilder
    {
        private byte[] buffer;
        private int* offset;

        public static BundleBuilder Bundle(byte[] buffer, ulong timestamp, ref int offset)
        {
            OscWriter.WriteBundle(buffer, timestamp, ref offset);
            var builder = new BundleBuilder();
            builder.buffer = buffer;
            fixed (int* p = &offset)
            {
                builder.offset = p;
            }
            return builder;
        }

        public BundleBuilder Element(OscWriter.ElementWriter writer)
        {
            int tempOffset = *offset;
            // reserve element size
            tempOffset += 4;
            int endOffset = writer(buffer, tempOffset);
            int elementSize = endOffset - (tempOffset);
            OscWriter.WriteInt32(buffer, elementSize, ref *offset);
            *offset += elementSize;
            return this;
        }
    }

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

        public static BundleWriter BeginBundle(byte[] buffer, ulong timestamp, ref int offset)
        {
            return new BundleWriter(buffer, timestamp, ref offset);
        }

        public static BundleElementWriter BeginElement(byte[] buffer, ref int offset)
        {
            return new BundleElementWriter(buffer, ref offset);
        }
    }
}
