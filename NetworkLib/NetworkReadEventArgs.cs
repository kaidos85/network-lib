using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public delegate void NetworkReadHandler(object sender, NetworkReadEventArgs e);
    public class NetworkReadEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public NetworkReadEventArgs(string _text)
        {
            this.Text = _text;
        }
    }
}
