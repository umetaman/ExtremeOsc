namespace ExtremeOsc
{
    public interface IOscReceivable
    {
        void ReceiveOscPacket(byte[] buffer);
    }
}
