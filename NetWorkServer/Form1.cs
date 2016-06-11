using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkLib;

namespace NetWorkApp
{
    public partial class Form1 : Form
    {
        
        Server server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ipaddress = Properties.Settings.Default.ipaddress;
            int port = Properties.Settings.Default.port;
            server = new Server(ipaddress, port);

            server.Connect();
            server.Read();
            server.handler += ChangedText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = String.Format("Server: {0}", textBox1.Text);
            server.Write(text);
            textBox2.Text += text + "\r\n";
            textBox1.Clear();
        }

        private void ChangedText(object sender, NetworkReadEventArgs e)
        {
            textBox2.BeginInvoke(
                        new Action(() =>
                        {
                            textBox2.Text += e.Text + "\r\n";
                        }));
            if (e.Text == "Client disconnected.")
            {
                server.ClientClose();
                server.Read();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Close();
            server.ClientClose();
            
        }
    }
}
