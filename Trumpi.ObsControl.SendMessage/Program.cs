using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Trumpi.ObsControl.SendMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint endPoint = new IPEndPoint(serverAddress, 5555);

                for (string line = Console.ReadLine(); line != null && line != "quit"; line = Console.ReadLine())
                {
                    byte[] sendBuffer = Encoding.UTF8.GetBytes(line);
                    sock.SendTo(sendBuffer, endPoint);
                }
            }
        }
    }
}
