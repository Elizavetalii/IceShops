
using System.Windows;
using System.Windows.Controls;

namespace IceShops.Admin
{
    public partial class WindowAdmin : Window
    {
        public WindowAdmin()
        {
            InitializeComponent();
        }

        private void Positions_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Positions();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Employees();
        }

        private void Authorizations_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Authorizations();
        }
    }
}
