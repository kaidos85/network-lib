using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class Server : BaseNetWork
    {
        TcpListener server;
        TcpClient client;


        public IPAddress IpAddress { get; private set; }
        public int Port { get; set; }

        public Server()
        {

        }

        public Server(string _ipaddress, int _port)
        {
            this.IpAddress = IPAddress.Parse(_ipaddress);
            this.Port = _port;
        }

        public bool Connect()
        {
            try
            {
                server = new TcpListener(IpAddress, Port);
                server.Start();
            }
            catch (SocketException e)
            {
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        protected override async void Init()
        {
            if(client == null)
            {
                client = await server.AcceptTcpClientAsync();
                stream = client.GetStream();
                Read();
            }
        }

        public void ClientClose()
        {
            if (stream != null & client != null)
            {
                stream.Close();
                client.Close();
            }
            client = null;
            stream = null;
        }

        
        public void Close()
        {
            server.Stop();
        }
    }
}
