using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal static class InterfaceGeneration
    {
        public const string Namespace = "namespace ExtremeOsc";

        public static string IOscPackable
        {
            get
            {
                var builder = new CodeBuilder();
                using (var @namespace = builder.BeginScope(Namespace))
                {
                    using (var @interface = builder.BeginScope("public interface IOscPackable"))
                    {
                        builder.AppendLine("void Pack(byte[] buffer, ref int offset);");
                        builder.AppendLine("void Unpack(byte[] buffer, ref int offset);");
                    }
                }

                return builder.ToString();
            }
        }

        public static string IOscReceivable
        {
            get
            {
                var builder = new CodeBuilder();
                using(var @namespace = builder.BeginScope(Namespace))
                {
                    using (var @interface = builder.BeginScope("public interface IOscReceivable"))
                    {
                        builder.AppendLine("void ReceiveOscPacket(byte[] buffer);");
                    }
                }

                return builder.ToString();
            }
        }
    }
}
