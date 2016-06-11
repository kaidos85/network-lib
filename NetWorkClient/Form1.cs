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

namespace NetWorkClient
{
    
    public partial class Form1 : Form
    {
        Client client;
        bool connected;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ipaddress = Properties.Settings.Default.ipaddress;
            int port = Properties.Settings.Default.port;
            client = new Client(ipaddress, port);

            connected = client.Connect();

            if (connected)
            {
                client.handler += ChangedText;
                client.Read();
                client.Write("Client connected.");
            }
            else
            {
                MessageBox.Show("Ошибка", "Нет соедининия с сервером", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void ChangedText(object sender, NetworkReadEventArgs e)
        {
            textBox2.BeginInvoke(
                        new Action(() =>
                        {
                            textBox2.Text += e.Text + "\r\n";
                        }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = String.Format("Client: {0}", textBox1.Text);
            client.Write(text);
            textBox2.Text += text + "\r\n";
            textBox1.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(connected)
                client.Write("Client disconnected.");
            client.Close();
        }
    }
}
