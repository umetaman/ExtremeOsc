using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeOsc.Annotations;
using NUnit.Framework;
using System;
using System.Linq;

namespace ExtremeOsc.Tests
{
    using static ExtremeOsc.OscWriter;

    public class BundleTest
    {
        [Test]
        public void BundleNoArguments()
        {
            var buffer = new byte[4096];

            var addresses = Enumerable.Range(0, 10)
                .Select(i => "/test")
                .ToArray();

            // write
            int messageCount = 0;
            int offset = 0;
            ulong timestamp = OscWriter.NtpNow;
            WriteBundle(buffer, timestamp, ref offset);
            foreach (var address in addresses)
            {
                WriteBundleElement(buffer, (buffer, offset) =>
                {
                    Write(buffer, address, ref offset);
                    messageCount++;
                    return offset;
                }, ref offset);
            }
            WriteBundleElement(buffer, (buffer, offset) =>
            {
                WriteBundle(buffer, timestamp, ref offset);
                WriteBundleElement(buffer, (buffer, offset) =>
                {
                    Write(buffer, "/nested", ref offset);
                    messageCount++;
                    return offset;
                }, ref offset);
                WriteBundleElement(buffer, (buffer, offset) =>
                {
                    Write(buffer, "/nested", ref offset);
                    messageCount++;
                    return offset;
                }, ref offset);
                WriteBundleElement(buffer, (buffer, offset) =>
                {
                    WriteBundle(buffer, timestamp, ref offset);
                    WriteBundleElement(buffer, (buffer, offset) =>
                    {
                        Write(buffer, "/deeply/nested", ref offset);
                        messageCount++;
                        return offset;
                    }, ref offset);
                    return offset;
                }, ref offset);
                return offset;
            }, ref offset);
            WriteBundleElement(buffer, (buffer, offset) =>
            {
                var intValue = new IntValue(
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32(),
                    Arbitary.GetRandomInt32()
                );
                Write(buffer, "/intValues", intValue, ref offset);
                messageCount++;
                return offset;
            }, ref offset);
            int packetSize = offset;
            Debug.Log(packetSize);

            // read
            offset = 0;
            if (OscReader.IsBundle(buffer, offset))
            {
                offset += TagType.BytesBundle.Length;
                ulong readTimestamp = OscReader.ReadULong(buffer, ref offset);
                Assert.AreEqual(timestamp, readTimestamp);

                foreach (var address in addresses)
                {
                    int elementSize = OscReader.ReadInt32(buffer, ref offset);
                    byte[] data = new byte[elementSize];
                    Buffer.BlockCopy(buffer, offset, data, 0, elementSize);
                    var reader = OscReader.Read(data);
                    Assert.AreEqual(address, reader.Address);
                    offset += elementSize;
                }
            }
            else
            {
                Assert.Fail("Not a bundle");
            }

            // reader
            offset = 0;
            var readers = new List<OscReader>();
            Debug.Log("Start Reading Bundle");
            OscReader.ReadBundle(buffer, packetSize, ref offset, default, readers);
            Assert.AreEqual(messageCount, readers.Count);

            var intValuesReader = readers.FirstOrDefault(r => r.Address == "/intValues");
            Debug.Log(intValuesReader.TagTypes);
        
        }
    }
}