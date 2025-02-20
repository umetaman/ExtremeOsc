using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Unity.Collections;
using UnityEngine;

namespace ExtremeOsc
{
    public class OscServer : System.IDisposable
    {
        private bool isDisposed = false;
        private int port = -1;
        private int bufferSize = -1;
        private UdpClient udpClient = null;
        private CancellationToken cancellationToken;
        private byte[] buffer;
        private readonly HashSet<IOscReceivable> receivers = new HashSet<IOscReceivable>();

        public OscServer(int port, int bufferSize = 4096)
        {
            this.port = port;
            this.bufferSize = bufferSize;
            this.udpClient = new UdpClient(port);
            this.buffer = new byte[bufferSize];
        }

        public void Open(CancellationToken cancellationToken = default)
        {
            this.cancellationToken = cancellationToken;
            Thread thread = new Thread(ThreadReceive);
            thread.Start();
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;

            this.udpClient.Close();
            this.udpClient = null;
            this.buffer = null;
            this.receivers.Clear();
        }

        public void Register(IOscReceivable receiver)
        {
            this.receivers.Add(receiver);
        }

        public void Unregister(IOscReceivable receiver)
        {
            this.receivers.Remove(receiver);
        }

        private void ThreadReceive()
        {
            var socket = udpClient.Client;
            
            while(this.isDisposed || this.cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Clear Buffer
                    var span = buffer.AsSpan();
                    span.Fill(0);

                    int receivedSize = socket.Receive(span);

                    if (receivedSize > 0)
                    {
                        foreach(var receiver in receivers)
                        {
                            receiver.ReceiveOscPacket(buffer);
                        }
                    }
                }
                catch(Exception e)
                {
                    if(e is SocketException || e is ThreadAbortException)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
