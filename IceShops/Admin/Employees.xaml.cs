using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.Admin
{
    public partial class Employees : Page
    {
        EmployeesTableAdapter employees = new EmployeesTableAdapter();
        PositionsTableAdapter positions = new PositionsTableAdapter();
        StoresTableAdapter stores = new StoresTableAdapter();
        public Employees()
        {
            InitializeComponent();
            EmployeesDataGrid.ItemsSource = employees.GetData();
            Position.ItemsSource = positions.GetData();
            Position.DisplayMemberPath = "position_name";

            Store.ItemsSource = stores.GetData();
            Store.DisplayMemberPath = "store_address";
        }

 
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FirstName.Text) && !string.IsNullOrWhiteSpace(LastName.Text) 
                && !string.IsNullOrWhiteSpace(SurName.Text) && !string.IsNullOrWhiteSpace(Position.Text) 
                && !string.IsNullOrWhiteSpace(Store.Text))
            {
                if (!ContainsSpecialCharacters(FirstName.Text + SurName.Text + LastName.Text))
                {
                    var selectedPosition = Position.SelectedItem as DataRowView;
                    int PositionID = Convert.ToInt32(selectedPosition.Row[0]);

                    var selectedStore = Store.SelectedItem as DataRowView;
                    int StoreID = Convert.ToInt32(selectedStore.Row[0]);

                    employees.CreateEmployees(FirstName.Text, LastName.Text, SurName.Text, PositionID, StoreID);
                    EmployeesDataGrid.ItemsSource = employees.GetData();
                }
                else
                {
                    MessageBox.Show("Данные сотрудника не должны содержать специальные символы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите данные сотрудника.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(FirstName.Text) && !string.IsNullOrWhiteSpace(LastName.Text)
                    && !string.IsNullOrWhiteSpace(SurName.Text) && !string.IsNullOrWhiteSpace(Position.Text)
                    && !string.IsNullOrWhiteSpace(Store.Text))
                {
                    if (!ContainsSpecialCharacters(FirstName.Text + SurName.Text + LastName.Text))
                    {
                        var selectedPosition = Position.SelectedItem as DataRowView;
                        int PositionID = Convert.ToInt32(selectedPosition.Row[0]);

                        var selectedStore = Store.SelectedItem as DataRowView;
                        int StoreID = Convert.ToInt32(selectedStore.Row[0]);

                        object id = (EmployeesDataGrid.SelectedItem as DataRowView).Row[0];

                        employees.UpdateEmployees(FirstName.Text, LastName.Text, SurName.Text, PositionID, StoreID, Convert.ToInt32(id));
                        EmployeesDataGrid.ItemsSource = employees.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Данные сотрудника не должны содержать специальные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите данные сотрудника.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника которого хотите удалить.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить сотрудника?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (EmployeesDataGrid.SelectedItem as DataRowView).Row[0];
                    employees.DeleteEmployees(Convert.ToInt32(id));
                    EmployeesDataGrid.ItemsSource = employees.GetData();
                }
            }
        }
        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = EmployeesDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                FirstName.Text = selectedRow.Row[1].ToString();
                LastName.Text = selectedRow.Row[2].ToString();
                SurName.Text = selectedRow.Row[3].ToString();
                Position.Text = selectedRow.Row[4].ToString();
                Store.Text = selectedRow.Row[5].ToString();
            }
        }
    }
}

