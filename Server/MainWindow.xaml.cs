using ImgSend.Helpers;
using Newtonsoft.Json;
using Server.Entities;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string EPoint { get; set; }
        public string StrJson { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            Task SrvStart = new Task(ServerStart);
            SrvStart.Start();

            Thread.Sleep(1000);
            this.Title = EPoint;
        }

        public void ServerStart()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 1098);
            EPoint = ep.ToString();
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            s.Bind(ep);
            s.Listen(10);
            try
            {
                while (true)
                {
                    Socket ns = s.Accept();
                    string data = null;
                    byte[] bytes = new byte[100000];
                    int bytesRec = ns.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    StrJson = data;
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Socket error: " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowSend send = new WindowSend();
            send.ShowDialog();

            if (!string.IsNullOrEmpty(StrJson))
            {
                var sendRes = JsonConvert.DeserializeObject<ImageModel>(StrJson);

                imgAddImg.Source = ImageHelper.BitmapToImageSource(ImageHelper.Base64ToImg(sendRes.Base64));
                txtNameImg.Text = sendRes.Name;
            }
        }
    }
}
