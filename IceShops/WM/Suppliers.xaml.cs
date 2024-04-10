using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.WM
{
    public partial class Suppliers : Page
    {
        SuppliersTableAdapter suppliers = new SuppliersTableAdapter();
        ProductsTableAdapter products = new ProductsTableAdapter();
        public Suppliers()
        {
            InitializeComponent();

            SuppliersDataGrid.ItemsSource = suppliers.GetData();
            Product.ItemsSource = products.GetData();
            Product.DisplayMemberPath = "product_name";
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Company.Text)  && !string.IsNullOrWhiteSpace(Product.Text))
            {
                if (!ContainsSpecialCharacters(Company.Text))
                {
                    var selectedProduct = Product.SelectedItem as DataRowView;
                    int ProductID = Convert.ToInt32(selectedProduct.Row[0]);

                    suppliers.CreateSuppliers(Company.Text, ProductID);
                    SuppliersDataGrid.ItemsSource = suppliers.GetData();

                }
                else
                {
                    MessageBox.Show("Данные не должны содержать специальные символы или иметь отличный формат.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите данные.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(Company.Text) && !string.IsNullOrWhiteSpace(Product.Text))
                {
                    if (!ContainsSpecialCharacters(Company.Text))
                    {
                        DataRowView selectedProduct = Product.SelectedItem as DataRowView;
                        int ProductID = Convert.ToInt32(selectedProduct["product_id"]);
                        object id = (SuppliersDataGrid.SelectedItem as DataRowView).Row[0];
                        suppliers.UpdateSuppliers(Company.Text, Convert.ToInt32(id), ProductID);
                        SuppliersDataGrid.ItemsSource = suppliers.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Данные не должны содержать специальные символы или иметь отличный формат.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите данные.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите данные.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите данные которые хотите удалить.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данные?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (SuppliersDataGrid.SelectedItem as DataRowView).Row[0];
                    suppliers.DeleteSuppliers(Convert.ToInt32(id));
                    SuppliersDataGrid.ItemsSource = suppliers.GetData();
                }
            }
        }
        private void SuppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = SuppliersDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                Company.Text = selectedRow.Row[1].ToString();
                Product.Text = selectedRow.Row[2].ToString();
            }
        }
        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }     
    }
}
