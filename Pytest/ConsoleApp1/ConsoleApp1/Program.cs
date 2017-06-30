using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp1
{   
    class UdpHandler
    {
        UdpClient udp;
        int port;

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        bool listening;
        Thread listeningThread;
        
        public UdpHandler(int port)
        {
            this.port = port;
            udp = new UdpClient(port);
            StartListener();
        }

        public void Send(string json) //other than string?
        {
            if (RemoteIpEndPoint.Address != IPAddress.Any && RemoteIpEndPoint.Port != 0)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                udp.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port);
            }
        }

        public void StartListener()
        {
            if (!listening)
            {
                listeningThread = new Thread(Listen);
                this.listening = true;
                listeningThread.Start();
                listeningThread.IsBackground = false; //True?
            }
        }

        public void StopListener()
        {
            listening = false;
        }

        private void Listen()
        {
            while (listening)
            {
                Console.WriteLine("Waiting to receive");
                Byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine(receiveString);
            }
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            UdpHandler testing = new UdpHandler(23023);
            testing.StartListener();
            
            while (true){
                string message = Console.ReadLine();
                testing.Send(message);
            }
            
        }
    }
}
