using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class Client : BaseNetWork
    {
        TcpClient client;

        public string IpAddress { get; set; }
        public int Port { get; set; }

        public Client()
        {
            
        }

        public Client(string _ipaddress, int _port)
        {
            this.IpAddress = _ipaddress;
            this.Port = _port;
        }

        public bool Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(IpAddress, Port);
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

        protected override void Init()
        {
            if(stream == null)
            {
                stream = client.GetStream();
            }
        }

        
        public void Close()
        {
            if (stream != null & client != null)
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
