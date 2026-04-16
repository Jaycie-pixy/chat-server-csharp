using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        private string message;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Enter username first!");
                return;
            }

            client = new TcpClient("127.0.0.1", 5000);
            stream = client.GetStream();

            Thread t = new Thread(ReceiveMessages);
            t.Start();

            txtChat.AppendText(txtUsername.Text + " (Me): " + message + "\r\n"); ;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string message = txtMessage.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(message))
                return;

            string fullMessage = username + ": " + message;

            byte[] data = Encoding.UTF8.GetBytes(fullMessage);
            stream.Write(data, 0, data.Length);

            txtChat.AppendText(txtUsername.Text + " (Me): " + message + "\r\n");
            txtMessage.Clear();
        }
        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);

                if (bytes > 0)
                {
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                    Invoke(new Action(() =>
                    {
                        txtChat.AppendText(msg + "\r\n");
                    }));

                    }
                        
                    }
                }
            
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSend.PerformClick();
        }
    }
}
