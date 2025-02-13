using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    public interface IOscPackable
    {
        void Pack(NativeArray<byte> buffer, ref int offset);
        void Unpack(NativeArray<byte> buffer, ref int offset);
    }

    public interface IOscReceivable
    {
        void ReceiveOscData(NativeArray<byte> buffer);
    }
}
