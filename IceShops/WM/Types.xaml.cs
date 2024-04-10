using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.WM
{
    public partial class Types : Page
    {
        ProductTypesTableAdapter types = new ProductTypesTableAdapter();
        public Types()
        {
            InitializeComponent();
            TypesDataGrid.ItemsSource = types.GetData();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Type.Text))
            {
                if (!ContainsSpecialCharacters(Type.Text))
                {
                    types.CreateTypes(Type.Text);
                    TypesDataGrid.ItemsSource = types.GetData();
                }
                else
                {
                    MessageBox.Show("Название типа не должно содержать специальные символы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название типа.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (TypesDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(Type.Text))
                {
                    if (!ContainsSpecialCharacters(Type.Text))
                    {
                        object id = (TypesDataGrid.SelectedItem as DataRowView).Row[0];
                        types.UpdateTypes(Type.Text, Convert.ToInt32(id));
                        TypesDataGrid.ItemsSource = types.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Название типа не должно содержать специальные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите название типа.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тип для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TypesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите тип для удаления.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить тип?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (TypesDataGrid.SelectedItem as DataRowView).Row[0];
                    types.DeleteTypes(Convert.ToInt32(id));
                    TypesDataGrid.ItemsSource = types.GetData();
                }
            }
        }
        private void TypesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = TypesDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                Type.Text = selectedRow.Row[1].ToString();
            }
        }

        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }      
    }
}
