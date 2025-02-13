using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    public class OscClient : System.IDisposable
    {
        private UdpClient udpClient = null;
        private NativeArray<byte> buffer;

        public OscClient(string ipaddress, int port, int bufferSize = 4096)
        {
            this.udpClient = new UdpClient(ipaddress, port);
            this.buffer = new NativeArray<byte>(bufferSize, Allocator.Persistent);
        }

        public void Send<T>(string address, T value) where T : IOscPackable
        {
            int offset = 0;
            int length = 0;

            // Clear
            buffer.AsSpan().Fill(0);

            // Write address
            OscEmitter.WriteString(buffer, address, ref offset);
            length += offset;

            // Write Data
            value.Pack(buffer, ref offset);
            length += (offset - length);

            udpClient.Client.Send(buffer.AsReadOnlySpan().Slice(0, length));
        }

        public void Dispose()
        {
            this.udpClient.Close();
            this.udpClient = null;
            this.buffer.Dispose();
        }
    }
}
