using IceShops.Admin;
using System.Windows;
using System.Windows.Controls;

namespace IceShops.Seller
{
    public partial class WindowSeller : Window
    {
        private int employeeId;
        private int employeeStores;
        public WindowSeller(object employeeID, object employeeStores)
        {
            this.employeeId = (int)employeeId;
            this.employeeStores = (int)employeeStores;
            InitializeComponent();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new Order( employeeId, employeeStores);
        }

        private void SavedOrders_Click(object sender, RoutedEventArgs e)
        {
            Hey.Visibility = Visibility.Hidden;
            Frame.Content = new SavedOrders();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
