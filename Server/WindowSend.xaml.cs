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
using ImgSend.Helpers;
using System.Transactions;
using Newtonsoft.Json;
using Server.Models;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для WindowSend.xaml
    /// </summary>
    public partial class WindowSend : Window
    {
        private EFContext _context;
        public string NameImg { get; set; }
        public string ImgBase64string { get; set; }
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
                    //AddImg = System.Drawing.Image.FromFile(ofd.FileName);
                    ImgBase64string = ImageHelper.ImgToBase64(System.Drawing.Image.FromFile(ofd.FileName));
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
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    _context.Images.Add(new Images()
                    {
                        Name = NameImg,
                        Base64 = ImgBase64string
                    });
                    _context.SaveChanges();
                    MessageBox.Show("addind to db complite");
                }
                catch
                {
                    throw new Exception("error db");
                }

                var img = new ImageModel
                {
                    Name = NameImg,
                    Base64 = ImgBase64string
                };
                
                var strJson = JsonConvert.SerializeObject(img);
                MessageBox.Show(strJson.Length.ToString());

                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint ep = new IPEndPoint(ip, 1098);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                try
                {
                    s.Connect(ep);
                    if (s.Connected)
                    {
                        s.Send(Encoding.UTF8.GetBytes(strJson));
                        //byte[] buffer = new byte[1024];
                        //int l;
                        //do
                        //{
                        //    l = s.Receive(buffer);
                        //} while (l > 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
                MessageBox.Show("sending to server complite");
                //scope.Complete();
            }
            MessageBox.Show("transaction complite");
            this.Close();
        }
    }
}
