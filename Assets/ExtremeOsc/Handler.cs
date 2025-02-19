using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    public interface IOscPackable
    {
        void Pack(byte[] buffer, ref int offset);
        void Unpack(byte[] buffer, ref int offset);
    }

    public interface IOscReceivable
    {
        void ReceiveOscData(byte[] buffer);
    }
}
