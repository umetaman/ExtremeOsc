namespace ExtremeOsc
{
    public interface IOscReceivable
    {
        void ReceiveOscPacket(byte[] buffer);
        void ReceiveOscPacket(byte[] buffer, ulong timestamp, ref int offset);
    }
}
