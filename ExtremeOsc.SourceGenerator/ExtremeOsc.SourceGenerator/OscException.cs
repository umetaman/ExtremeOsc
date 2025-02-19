using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException(string message) : base(message)
        {
        }
    }
}
