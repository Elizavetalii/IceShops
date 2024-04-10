using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.Admin
{
    public partial class Authorizations : Page
    {
        AuthorizationsTableAdapter authorizations = new AuthorizationsTableAdapter();
        EmployeesTableAdapter employees = new EmployeesTableAdapter();
        public Authorizations()
        {
            InitializeComponent();
            AuthorizationDataGrid.ItemsSource = authorizations.GetData();
            
            LastName.ItemsSource = employees.GetData();
            LastName.DisplayMemberPath = "LastName";
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Login.Text) && !string.IsNullOrWhiteSpace(Password.Password)
                && !string.IsNullOrWhiteSpace(LastName.Text))
            {
                if (!ContainsSpecialCharacters(Login.Text + Password.Password) )
                {
                    var selectedEmployee = LastName.SelectedItem as DataRowView;
                    int EmployeeID = Convert.ToInt32(selectedEmployee.Row[0]);

                    bool userAlreadyHasAuthorization = false;

                    var allAuthorizations = authorizations.GetData().Rows;
                    foreach (DataRow authorization in allAuthorizations)
                    {
                        int EId = (int)authorization["employee_id"];

                        if (EId == EmployeeID)
                        {
                            userAlreadyHasAuthorization = true;
                            break;
                        }
                    }

                    if (userAlreadyHasAuthorization)
                    {
                        MessageBox.Show("Пользователь уже имеет логин и пароль. Пожалуйста, выберите другого пользователя.");
                    }
                    else
                    {
                        authorizations.CreateAuthorization(Login.Text, Password.Password, EmployeeID);
                        AuthorizationDataGrid.ItemsSource = authorizations.GetData();
                    }
                }
                else
                {
                    MessageBox.Show("Параметры для авторизации не должны содержать специальные символы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите параметры для авторизации.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorizationDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(Login.Text) && !string.IsNullOrWhiteSpace(Password.Password) && !string.IsNullOrWhiteSpace(LastName.Text))
                {
                    if (!ContainsSpecialCharacters(Login.Text + Password.Password))
                    {
                        DataRowView selectedEmployee = LastName.SelectedItem as DataRowView;
                        int EmployeeID = Convert.ToInt32(selectedEmployee["employee_id"]);
                        object id = (AuthorizationDataGrid.SelectedItem as DataRowView).Row[0];

                        bool userAlreadyHasAuthorization = false;

                        var allAuthorizations = authorizations.GetData().Rows;
                        foreach (DataRow authorization in allAuthorizations)
                        {
                            int EId = (int)authorization["employee_id"];

                            if (EId == EmployeeID)
                            {
                                userAlreadyHasAuthorization = true;
                                break;
                            }
                        }

                        if (userAlreadyHasAuthorization)
                        {
                            MessageBox.Show("Пользователь уже имеет логин и пароль. Пожалуйста, выберите другого пользователя.");
                        }
                        else
                        {
                            authorizations.UpdateAuthorizations(Login.Text, Password.Password, Convert.ToInt32(id), EmployeeID);
                            AuthorizationDataGrid.ItemsSource = authorizations.GetData();
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("Параметры для авторизации не должны содержать специальные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите параметры для авторизации.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите параметры для авторизации.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorizationDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите параметры для авторизации которые хотите удалить.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить параметры для авторизации?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (AuthorizationDataGrid.SelectedItem as DataRowView).Row[0];
                    authorizations.DeleteAuthorizations(Convert.ToInt32(id));
                    AuthorizationDataGrid.ItemsSource = authorizations.GetData();
                }
            }
        }
        private void AuthorizationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = AuthorizationDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                Login.Text = selectedRow.Row[1].ToString();
                Password.Password = selectedRow.Row[2].ToString();
                LastName.Text = selectedRow.Row[3].ToString();
            }
        }

        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }
    }
}
