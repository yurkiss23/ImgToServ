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
        //private EFContext _context;
        public string EPoint { get; set; }
        public string NameImg { get; set; }
        public static string RecMessage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //_context = new EFContext();

            Task SrvStart = new Task(ServerStart);
            SrvStart.Start();

            //List<ImageModel> l = new List<ImageModel>(_context.Images.Select(i => new ImageModel()
            //{
            //    Id = i.Id,
            //    Name = i.Name,
            //    Base64 = i.Base64
            //}).ToList());
            //dg.ItemsSource = l;

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
                    byte[] bytes = new byte[1024];
                    int bytesRec = ns.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    NameImg = data;
                    ns.Send(Encoding.UTF8.GetBytes($" sending {DateTime.Now}"));
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

            if (!string.IsNullOrEmpty(NameImg))
            {
                txtNameImg.Text = NameImg;
            }
        }
    }
}
