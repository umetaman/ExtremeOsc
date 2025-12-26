namespace ExtremeOsc
{
    public interface IOscReceivable
    {
        void ReceiveOscPacket(byte[] buffer);
        void ReceiveOscPacket(byte[] buffer, ref int offset, ulong timestamp = 1UL);
    }
}
