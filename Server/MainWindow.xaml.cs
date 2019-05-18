using Server.Entities;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private EFContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new EFContext();

            List<ImageModel> l = new List<ImageModel>(_context.Images.Select(i => new ImageModel()
            {
                Id = i.Id,
                Name = i.Name,
                Base64 = i.Base64
            }).ToList());
            dg.ItemsSource = l;
        }
    }
}
