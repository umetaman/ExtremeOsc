using System;
using System.Buffers;
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
        private byte[] buffer;

        public OscClient(string ipaddress, int port, int bufferSize = 4096)
        {
            this.udpClient = new UdpClient(ipaddress, port);
            this.buffer = new byte[bufferSize];
        }

        public void Send<T>(string address, T value) where T : IOscPackable
        {
            int offset = 0;
            int length = 0;

            // Clear
            buffer.AsSpan().Fill(0);

            // Write address
            OscWriter.WriteString(buffer, address, ref offset);
            length += offset;

            // Write Data
            value.Pack(buffer, ref offset);
            length += (offset - length);

            udpClient.Client.Send(buffer, 0, length, SocketFlags.None);
        }

        public void Send(string address)
        {
            int offset = 0;
            // Clear
            buffer.AsSpan().Fill(0);
            // Write address
            OscWriter.WriteString(buffer, address, ref offset);
            // Write TagType ,
            OscWriter.WriteString(buffer, ",", ref offset);
            udpClient.Client.Send(buffer, 0, offset, SocketFlags.None);
        }

        public void Send(string address, object[] values)
        {
            OscWriter.Write(buffer, address, values);
            udpClient.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        public void Send(byte[] buffer, int length)
        {
            udpClient.Client.Send(buffer, 0, length, SocketFlags.None);
        }

        public void Dispose()
        {
            this.udpClient.Close();
            this.udpClient = null;
            this.buffer = null;
        }
    }
}
