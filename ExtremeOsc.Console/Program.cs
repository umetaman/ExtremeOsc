using System;
using ExtremeOsc;

namespace ExtremeOsc.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                ShowUsage();
                return;
            }

            string command = args[0].ToLower();

            if (command == "receive" || command == "recv")
            {
                if (args.Length < 2 || !int.TryParse(args[1], out int port))
                {
                    Console.WriteLine("Error: Please specify a valid port number.");
                    Console.WriteLine("Usage: receive <port>");
                    return;
                }

                StartReceiver(port);
            }
            else if (command == "send")
            {
                if (args.Length < 4)
                {
                    Console.WriteLine("Error: Missing parameters.");
                    Console.WriteLine("Usage: send <ip> <port> <address> [arg1] [arg2] ...");
                    Console.WriteLine("Prefix arguments to specify types: i:int, f:float, d:double, b:bool, s:string (default)");
                    Console.WriteLine("Example: send 127.0.0.1 5555 /my/address i:123 f:3.14 b:true \"hello\"");
                    return;
                }

                string ip = args[1];
                if (!int.TryParse(args[2], out int port))
                {
                    Console.WriteLine("Error: Invalid port number.");
                    return;
                }

                string address = args[3];

                object[] values = new object[args.Length - 4];
                for (int i = 4; i < args.Length; i++)
                {
                    values[i - 4] = ParseArgument(args[i]);
                }

                SendMessage(ip, port, address, values);
            }
            else
            {
                ShowUsage();
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("=== ExtremeOsc Universal CLI Tool ===");
            Console.WriteLine("Commands:");
            Console.WriteLine("  receive <port>                   Start OSC receiver on specified port.");
            Console.WriteLine("  send <ip> <port> <address> [args] Send OSC message with arbitrary arguments.");
            Console.WriteLine();
            Console.WriteLine("Argument format for sending:");
            Console.WriteLine("  i:VALUE   integer (e.g. i:42)");
            Console.WriteLine("  f:VALUE   float   (e.g. f:3.14)");
            Console.WriteLine("  d:VALUE   double  (e.g. d:1.2345678)");
            Console.WriteLine("  b:VALUE   boolean (e.g. b:true or b:false)");
            Console.WriteLine("  s:VALUE   string  (e.g. s:hello)");
            Console.WriteLine("  VALUE     treated as string by default (e.g. hello)");
        }

        private static object ParseArgument(string raw)
        {
            if (raw.StartsWith("i:"))
            {
                if (int.TryParse(raw.Substring(2), out int val)) return val;
            }
            else if (raw.StartsWith("f:"))
            {
                if (float.TryParse(raw.Substring(2), out float val)) return val;
            }
            else if (raw.StartsWith("d:"))
            {
                if (double.TryParse(raw.Substring(2), out double val)) return val;
            }
            else if (raw.StartsWith("b:"))
            {
                if (bool.TryParse(raw.Substring(2), out bool val)) return val;
            }
            else if (raw.StartsWith("s:"))
            {
                return raw.Substring(2);
            }

            return raw;
        }

        private static void StartReceiver(int port)
        {
            Console.WriteLine($"Starting OSC Server on port {port}...");
            using var server = new OscServer(port);
            var receiver = new GenericReceiver();
            server.Register(receiver);
            server.Open();

            Console.WriteLine("Server is listening.");
            if (Console.IsInputRedirected)
            {
                // Wait indefinitely when input is redirected (e.g. background run)
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }

            server.Unregister(receiver);
            Console.WriteLine("Server stopped.");
        }

        private static void SendMessage(string ip, int port, string address, object[] values)
        {
            Console.WriteLine($"Sending OSC message to {ip}:{port} with address '{address}'...");
            using var client = new OscClient(ip, port);
            client.Send(address, values);
            Console.WriteLine("Message sent successfully!");
        }
    }

    public class GenericReceiver : IOscReceivable
    {
        public void ReceiveOscPacket(byte[] buffer)
        {
            try
            {
                var reader = OscReader.Read(buffer);
                Console.WriteLine($"Received OSC Message:");
                Console.WriteLine($"  Address:  {reader.Address}");
                Console.WriteLine($"  TagTypes: {reader.TagTypes}");

                var objects = reader.GetAsObjects();
                for (int i = 0; i < objects.Length; i++)
                {
                    var val = objects[i];
                    Console.WriteLine($"  Argument [{i}]: ({val?.GetType().Name ?? "null"}) {val}");
                }
                Console.WriteLine();
                reader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing packet: {ex.Message}");
            }
        }

        public void ReceiveOscPacket(byte[] buffer, ulong timestamp, ref int offset)
        {
            ReceiveOscPacket(buffer);
        }
    }
}
