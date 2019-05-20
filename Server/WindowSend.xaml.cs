using Microsoft.Win32;
using Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для WindowSend.xaml
    /// </summary>
    public partial class WindowSend : Window
    {
        private EFContext _context;
        public string NameImg { get; set; }
        public System.Drawing.Image AddImg { get; set; }
        public WindowSend()
        {
            InitializeComponent();
            _context = new EFContext();

        }

        private void BtnAddImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    NameImg = ofd.SafeFileName;
                    AddImg = System.Drawing.Image.FromFile(ofd.FileName);
                    imgAddImg.Source = new BitmapImage(new Uri(ofd.FileName));
                    txtNameImg.Text = NameImg;
                }
            }
            catch
            {
                throw new Exception("errors!");
            }
        }

        private void BtnSendImg_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 1098);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                s.Connect(ep);
                if (s.Connected)
                {
                    s.Send(Encoding.UTF8.GetBytes(txtNameImg.Text));
                    byte[] buffer = new byte[1024];
                    int l;
                    do
                    {
                        l = s.Receive(buffer);
                    } while (l > 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            this.Close();
        }
    }
}
