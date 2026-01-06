using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeOsc.Annotations;
using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine.Video;

namespace ExtremeOsc.Tests
{
    using static ExtremeOsc.OscWriter;
    using static Arbitary;

    public class BundleTest
    {
        private IOscPackable[] RandomValues()
        {
            var @int = new IntValue(GetRandomInt32(), GetRandomInt32(), GetRandomInt32(), GetRandomInt32());
            var @long = new LongValue(GetRandomInt64(), GetRandomInt64(), GetRandomInt64(), GetRandomInt64());
            var @float = new FloatValue(GetRandomFloat(), GetRandomFloat(), GetRandomFloat(), GetRandomFloat());
            var @string = new StringValue(
                GetRandomStringAscii(UnityEngine.Random.Range(1, 100)),
                GetRandomStringUtf8(UnityEngine.Random.Range(1, 100)),
                GetRandomStringAscii(UnityEngine.Random.Range(1, 100)),
                GetRandomStringUtf8(UnityEngine.Random.Range(1, 100)),
                GetRandomStringUtf8(UnityEngine.Random.Range(1, 100)));
            var @boolean = new BooleanValue(
                GetRandomBool(),
                GetRandomBool(),
                GetRandomBool(),
                GetRandomBool());
            var @blob = new BlobValue(
                GetRandomBlob(UnityEngine.Random.Range(1, 100)),
                GetRandomBlob(UnityEngine.Random.Range(1, 100)),
                GetRandomBlob(UnityEngine.Random.Range(1, 100)),
                GetRandomBlob(UnityEngine.Random.Range(1, 100)),
                GetRandomBlob(UnityEngine.Random.Range(1, 100))
                );
            var @double = new DoubleValue(
                GetRandomDouble(),
                GetRandomDouble(),
                GetRandomDouble(),
                GetRandomDouble(),
                GetRandomDouble());
            var @char = new CharValue(
                GetRandomChar(),
                GetRandomChar(),
                GetRandomChar(),
                GetRandomChar(),
                GetRandomChar());
            var @timetag = new TimeTagValue(
                GetRandomULong(),
                GetRandomULong(),
                GetRandomULong(),
                GetRandomULong(),
                GetRandomULong());
            var @nil = new NilValue();
            var @infinitum = new InfinitumValue();
            var @color = new Color32Value(
                GetRandomColor32(),
                GetRandomColor32(),
                GetRandomColor32(),
                GetRandomColor32(),
                GetRandomColor32()
                );

            var @class = new ClassValue();
            @class.IntValue = GetRandomInt32();
            @class.LongValue = GetRandomInt64();
            @class.FloatValue = GetRandomFloat();
            @class.StringValue = GetRandomStringAscii(10);
            @class.BytesValue = GetRandomBlob(10);
            @class.DoubleValue = GetRandomDouble();
            @class.Color32Value = GetRandomColor32();
            @class.CharValue = GetRandomChar();
            @class.TimeTagValue = GetRandomULong();
            @class.BooleanValue = GetRandomBool();
            @class.NilValue = default;
            @class.InfinitumValue = default;

            var @struct = new StructValue();
            @struct.IntValue = GetRandomInt32();
            @struct.LongValue = GetRandomInt64();
            @struct.FloatValue = GetRandomFloat();
            @struct.StringValue = GetRandomStringAscii(10);
            @struct.BytesValue = GetRandomBlob(10);
            @struct.DoubleValue = GetRandomDouble();
            @struct.Color32Value = GetRandomColor32();
            @struct.CharValue = GetRandomChar();
            @struct.TimeTagValue = GetRandomULong();
            @struct.BooleanValue = GetRandomBool();
            @struct.NilValue = default;
            @struct.InfinitumValue = default;

            var values = new IOscPackable[]
            {
                @int,
                @long,
                @float,
                @string,
                @boolean,
                @blob,
                @double,
                @char,
                @timetag,
                @nil,
                @infinitum,
                @color,
                @class,
                @struct,
            };
            return values;
        }

        [Test]
        public void BundleNest()
        {
            const int bufferSize = 4096 * 2 * 2 * 2;

            (byte[] buffer, int offset, int packetSize) chaining = (new byte[bufferSize], 0, 0);
            (byte[] buffer, int offset, int packetSize) usingApi = (new byte[bufferSize], 0, 0);

            for(int i = 0; i < 100; i++)
            {
                int randomCount = UnityEngine.Random.Range(5, 20);
                var addresses = Enumerable.Range(0, randomCount)
                    .Select(_ => Arbitary.GetRandomAddress())
                    .ToList();
                ulong timestamp = Arbitary.GetRandomULong();

                var values = RandomValues();

                {
                    var bundle = BundleBuilder.Bundle(chaining.buffer, timestamp, ref chaining.offset);
                    foreach (var address in addresses)
                    {
                        bundle.Element((buffer, offset) =>
                        {
                            Write(buffer, address, ref offset);
                            return offset;
                        });
                    }
                    // nest bundle
                    bundle.Element((buffer, offset) =>
                    {
                        var nestBundle = BundleBuilder.Bundle(buffer, timestamp + 1000, ref offset);
                        int valueCount = 0;
                        foreach (var value in values)
                        {
                            nestBundle.Element((b, o) =>
                            {
                                Write(b, $"/values/{valueCount}", value, ref o);
                                return o;
                            });
                            valueCount++;
                        }

                        nestBundle.Element((b, o) =>
                        {
                            var deepBundle = BundleBuilder.Bundle(b, timestamp + 2000, ref o);
                            deepBundle.Element((bb, oo) =>
                            {
                                Write(bb, "/deep/nest", new[] { "hello" }, ref oo);
                                return oo;
                            });
                            return o;
                        });
                        return offset;
                    });

                    chaining.packetSize = chaining.offset;
                }

                {
                    using (var bundle = BeginBundle(usingApi.buffer, timestamp, ref usingApi.offset))
                    {
                        foreach (var address in addresses)
                        {
                            using (var element = BeginElement(usingApi.buffer, ref usingApi.offset))
                            {
                                Write(usingApi.buffer, address, ref usingApi.offset);
                            }
                        }
                        // nest bundle
                        using (var element = BeginElement(usingApi.buffer, ref usingApi.offset))
                        {
                            using (var nestBundle = BeginBundle(usingApi.buffer, timestamp + 1000, ref usingApi.offset))
                            {
                                int valueCount = 0;
                                foreach (var value in values)
                                {
                                    using (var nestElement = BeginElement(usingApi.buffer, ref usingApi.offset))
                                    {
                                        string tempAddress = $"/values/{valueCount}";
                                        Write(usingApi.buffer, tempAddress, value, ref usingApi.offset);
                                        addresses.Add(tempAddress);
                                    }
                                    valueCount++;
                                }

                                using (var nestElement = BeginElement(usingApi.buffer, ref usingApi.offset))
                                {
                                    using (var deepBundle = BeginBundle(usingApi.buffer, timestamp + 2000, ref usingApi.offset))
                                    {
                                        using (var deepElement = BeginElement(usingApi.buffer, ref usingApi.offset))
                                        {
                                            Write(usingApi.buffer, "/deep/nest", new[] { "hello" }, ref usingApi.offset);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    usingApi.packetSize = usingApi.offset;
                }

                Assert.AreEqual(chaining.packetSize, usingApi.packetSize);
                for (int j = 0; j < chaining.packetSize; j++)
                {
                    Assert.AreEqual(chaining.buffer[j], usingApi.buffer[j]);
                }

                int readOffset = 0;
                (List<OscReader> chaining, List<OscReader> usingApi) readers = (new List<OscReader>(), new List<OscReader>());
                OscReader.ReadBundle(chaining.buffer, chaining.packetSize, ref readOffset, default, readers.chaining);
                readOffset = 0;
                OscReader.ReadBundle(usingApi.buffer, usingApi.packetSize, ref readOffset, default, readers.usingApi);

                for (int j = 0; j < readers.chaining.Count; j++)
                {
                    var readerChaining = readers.chaining[j];
                    var readerUsingApi = readers.usingApi[j];
                    Assert.AreEqual(readerChaining.Address, readerUsingApi.Address);
                    Assert.AreEqual(readerChaining.TagTypes, readerUsingApi.TagTypes);
                    Assert.AreEqual(readerChaining.Count, readerUsingApi.Count);
                    for (int k = 0; k < readerChaining.Count; k++)
                    {
                        Assert.AreEqual(readerChaining.TagTypes[k + 1], readerUsingApi.TagTypes[k + 1]); // skip ,
                        Assert.AreEqual(readerChaining.GetAsObjects()[k], readerUsingApi.GetAsObjects()[k]);
                    }
                }

                // reset
                chaining.buffer.AsSpan().Fill(0);
                chaining.offset = 0;
                chaining.packetSize = 0;
                usingApi.buffer.AsSpan().Fill(0);
                usingApi.offset = 0;
                usingApi.packetSize = 0;
            }
        }

        [Test]
        public void BundleSingle()
        {
            const int bufferSize = 4096 * 2 * 2;

            (byte[] buffer, int offset, int packetSize) chaining = (new byte[bufferSize], 0, 0);
            (byte[] buffer, int offset, int packetSize) usingApi = (new byte[bufferSize], 0, 0);

            for (int i = 0; i < 100; i++)
            {
                int randomCount = UnityEngine.Random.Range(5, 20);
                var addresses = Enumerable.Range(0, randomCount)
                    .Select(_ => Arbitary.GetRandomAddress())
                    .ToList();
                ulong timestamp = Arbitary.GetRandomULong();

                var values = RandomValues();

                // single nest
                // chaining
                {
                    var bundle = BundleBuilder.Bundle(chaining.buffer, timestamp, ref chaining.offset);
                    foreach (var address in addresses)
                    {
                        bundle.Element((buffer, offset) =>
                        {
                            Write(buffer, address, ref offset);
                            return offset;
                        });
                    }
                    int valueCount = 0;
                    foreach (var value in values)
                    {
                        bundle.Element((buffer, offset) =>
                        {
                            Write(buffer, $"/values/{valueCount}", value, ref offset);
                            return offset;
                        });
                        valueCount++;
                    }

                    chaining.packetSize = chaining.offset;
                }
                // using api
                {
                    using (var bundle = BeginBundle(usingApi.buffer, timestamp, ref usingApi.offset))
                    {
                        foreach (var address in addresses)
                        {
                            using (var element = BeginElement(usingApi.buffer, ref usingApi.offset))
                            {
                                Write(usingApi.buffer, address, ref usingApi.offset);
                            }
                        }

                        int valueCount = 0;
                        foreach (var value in values)
                        {
                            using (var element = BeginElement(usingApi.buffer, ref usingApi.offset))
                            {
                                string tempAddress = $"/values/{valueCount}";
                                Write(usingApi.buffer, tempAddress, value, ref usingApi.offset);
                                addresses.Add(tempAddress);
                            }
                            valueCount++;
                        }
                    }

                    usingApi.packetSize = usingApi.offset;
                }

                Assert.AreEqual(chaining.packetSize, usingApi.packetSize);
                for (int j = 0; j < chaining.packetSize; j++)
                {
                    Assert.AreEqual(chaining.buffer[j], usingApi.buffer[j]);
                }

                int readOffset = 0;
                (List<OscReader> chaining, List<OscReader> usingApi) readers = (new List<OscReader>(), new List<OscReader>());
                OscReader.ReadBundle(chaining.buffer, chaining.packetSize, ref readOffset, NtpImmediate, readers.chaining);
                readOffset = 0;
                OscReader.ReadBundle(usingApi.buffer, usingApi.packetSize, ref readOffset, NtpImmediate, readers.usingApi);

                for (int j = 0; j < readers.chaining.Count; j++)
                {
                    var readerChaining = readers.chaining[j];
                    var readerUsingApi = readers.usingApi[j];
                    Assert.AreEqual(readerChaining.Address, readerUsingApi.Address);
                    Assert.AreEqual(readerChaining.TagTypes, readerUsingApi.TagTypes);
                    Assert.AreEqual(readerChaining.Count, readerUsingApi.Count);
                    for (int k = 0; k < readerChaining.Count; k++)
                    {
                        Assert.AreEqual(readerChaining.TagTypes[k + 1], readerUsingApi.TagTypes[k + 1]); // skip ,
                        Assert.AreEqual(readerChaining.GetAsObjects()[k], readerUsingApi.GetAsObjects()[k]);
                    }
                }

                // reset
                chaining.buffer.AsSpan().Fill(0);
                chaining.offset = 0;
                chaining.packetSize = 0;
                usingApi.buffer.AsSpan().Fill(0);
                usingApi.offset = 0;
                usingApi.packetSize = 0;
            }
        }
    }
}