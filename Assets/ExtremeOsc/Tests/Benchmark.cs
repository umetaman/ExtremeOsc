using NUnit.Framework;
using System;
using Unity.PerformanceTesting;
using UnityEngine;

namespace ExtremeOsc.Tests
{
    public class Benchmark
    {
        [Test, Performance]
        public void Ref()
        {
            var allocated = new SampleGroup("Allocated", SampleUnit.Megabyte);

            Measure.Method(() =>
            {
                var before = GC.GetTotalMemory(false);

                var packable = new ReceiverTest();
                packable.ReceiverRefArgument();

                var after = GC.GetTotalMemory(false);
                Measure.Custom(allocated, (after - before) / (1024f * 1024f));

            })
                .WarmupCount(10)
                .GC()
                .MeasurementCount(10)
                .IterationsPerMeasurement(10)
                .Run();
                
        }

        [Test, Performance]
        public void New()
        {
            var allocated = new SampleGroup("Allocated", SampleUnit.Megabyte);

            Measure.Method(() =>
            {
                var before = GC.GetTotalMemory(false);

                var packable = new ReceiverTest();
                packable.ReceiverClassValue();

                var after = GC.GetTotalMemory(false);
                Measure.Custom(allocated, (after - before) / (1024f * 1024f));

            })
                .WarmupCount(10)
                .GC()
                .MeasurementCount(10)
                .IterationsPerMeasurement(10)
                .Run();

        }

        [Test, Performance]
        public void ObjectArray()
        {
            var allocated = new SampleGroup("Allocated", SampleUnit.Megabyte);

            Measure.Method(() =>
            {
                var before = GC.GetTotalMemory(false);

                var packable = new ReceiverTest();
                packable.ReceiverObjectArrayArguments();

                var after = GC.GetTotalMemory(false);
                Measure.Custom(allocated, (after - before) / (1024f * 1024f));

            })
                .WarmupCount(10)
                .GC()
                .MeasurementCount(10)
                .IterationsPerMeasurement(10)
                .Run();

        }
    }
}
