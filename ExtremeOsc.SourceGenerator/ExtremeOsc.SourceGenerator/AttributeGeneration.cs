using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal static class AttributeGeneration
    {
        public const string Namespace = "namespace ExtremeOsc.Annotations";

        public static string OscElementAt
        {
            get
            {
                var builder = new CodeBuilder();
                using (var @namespace = builder.BeginScope(Namespace))
                {
                    builder.AppendLine("[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]");
                    using (var @class = builder.BeginScope("public class OscElementAtAttribute : System.Attribute"))
                    {
                        builder.AppendLine("public int Index { private set; get; }");

                        using (var @constructor = builder.BeginScope("public OscElementAtAttribute(int index)"))
                        {
                            builder.AppendLine("this.Index = index;");
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static string OscCallback
        {
            get
            {
                var builder = new CodeBuilder();
                using (var @namespace = builder.BeginScope(Namespace))
                {
                    builder.AppendLine("[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]");
                    using (var @class = builder.BeginScope("public class OscCallbackAttribute : System.Attribute"))
                    {
                        builder.AppendLine("public string Address { private set; get; }");

                        using (var @constructor = builder.BeginScope("public OscCallbackAttribute(string address)"))
                        {
                            builder.AppendLine("this.Address = address;");
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public static string OscPackable
        {
            get
            {
                var builder = new CodeBuilder();
                using (var @namespace = builder.BeginScope(Namespace))
                {
                    builder.AppendLine("[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]");
                    using (var @class = builder.BeginScope("public class OscPackableAttribute : System.Attribute"))
                    {
                        // :)
                    }
                }

                return builder.ToString();
            }
        }

        public static string OscReceiver
        {
            get
            {
                var builder = new CodeBuilder();
                using (var @namespace = builder.BeginScope(Namespace))
                {
                    builder.AppendLine("[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]");
                    using (var @class = builder.BeginScope("public class OscReceiverAttribute : System.Attribute"))
                    {
                        // :)
                    }
                }

                return builder.ToString();
            }
        }
    }
}
