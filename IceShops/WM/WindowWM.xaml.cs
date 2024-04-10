using IceShops.NDM;
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
using System.Windows.Shapes;

namespace IceShops.WM
{
    /// <summary>
    /// Логика взаимодействия для WindowWM.xaml
    /// </summary>
    public partial class WindowWM : Window
    {
        public WindowWM()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Types_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Types();
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Products();
        }

        private void Suppliers_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Suppliers();
        }
    }
}
