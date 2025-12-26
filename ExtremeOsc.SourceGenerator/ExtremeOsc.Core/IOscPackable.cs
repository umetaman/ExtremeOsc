namespace ExtremeOsc
{
    public interface IOscPackable
    {
        void Pack(byte[] buffer, ref int offset);
        void Unpack(byte[] buffer, ref int offset);
    }
}
