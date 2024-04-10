using IceShops.Admin;
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

namespace IceShops.NDM
{
    /// <summary>
    /// Логика взаимодействия для WindowNDM.xaml
    /// </summary>
    
    public partial class WindowNDM : Window
    {
        public WindowNDM()
        {
            InitializeComponent();
        }

        private void Regions_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Regions();
        }

        private void Stores_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Stores();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
