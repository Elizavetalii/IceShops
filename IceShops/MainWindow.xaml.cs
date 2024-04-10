using IceShops.NDM;
using IceShops.WM;
using IceShops.Admin;
using System.Windows;
using IceShops.Seller;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops
{
    public partial class MainWindow : Window
    {
        AuthorizationsTableAdapter authorizations = new AuthorizationsTableAdapter();
        PositionsTableAdapter positions = new PositionsTableAdapter();
        EmployeesTableAdapter employees = new EmployeesTableAdapter();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Autorize_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Поля не должны быть пустыми");
                return;
            }

            var allLogins = authorizations.GetData().Rows;
            var allPositions = positions.GetData().Rows;
            var allEmployees = employees.GetData().Rows;
            bool loginMatched = false;

            for (int i = 0; i < allLogins.Count && i < allPositions.Count; i++)
            {
                if (Login.Text == allLogins[i]["logins"].ToString())
                {
                    loginMatched = true;

                    if (Password.Password == allLogins[i]["passwords"].ToString())
                    {
                        string positionId = (string)allPositions[i]["position_name"];

                        switch (positionId)
                        {
                            case "Администратор":
                                WindowAdmin windowAdmin = new WindowAdmin();
                                windowAdmin.Show();
                                this.Close();
                            break;

                            case "Менеджер по развитию сети":
                                WindowNDM windowNetworkManager = new WindowNDM();
                                windowNetworkManager.Show();
                                this.Close();
                            break;

                            case "Склад-менеджер":
                                WindowWM windowWarehouseManager = new WindowWM();
                                windowWarehouseManager.Show();
                                this.Close();
                            break;

                            case "Продавец":
                                var employeeID = allEmployees[i]["employee_id"];
                                var employeeStores = allEmployees[i]["store_id"];

                                WindowSeller windowSeller = new WindowSeller(employeeID, employeeStores);
                                windowSeller.Show();
                                this.Close();
                            break;

                            default:
                                MessageBox.Show("Рабочее пространство в разроботке. Пожалуйста, попробуйте позже.");
                            break;                           
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль.");
                    }
                }
            }
            if (!loginMatched)
            {
                MessageBox.Show("Логин не найден.");
            }
        }
    }
}
