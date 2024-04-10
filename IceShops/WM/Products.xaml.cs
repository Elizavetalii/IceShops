using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;


namespace IceShops.WM
{
    public partial class Products : Page
    {
        ProductsTableAdapter products = new ProductsTableAdapter();
        ProductTypesTableAdapter productTypes = new ProductTypesTableAdapter();
        StoresTableAdapter stores = new StoresTableAdapter();

        public Products()
        {
            InitializeComponent();
            ProductsDataGrid.ItemsSource = products.GetData();
            Type.ItemsSource = productTypes.GetData();
            Type.DisplayMemberPath = "product_type";
            Store.ItemsSource = stores.GetData();
            Store.DisplayMemberPath = "store_address";
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ProductName.Text) && !string.IsNullOrWhiteSpace(Price.Text)
                && !string.IsNullOrWhiteSpace(Quantity.Text) && !string.IsNullOrWhiteSpace(Type.Text)
                && !string.IsNullOrWhiteSpace(Store.Text))
            {
                if (!ContainsSpecialCharacters(ProductName.Text) && decimal.TryParse(Price.Text, out decimal PriceProduct)
                    && int.TryParse(Quantity.Text, out int QuantityProduct) && PriceProduct >= 0 && QuantityProduct >= 0)
                {
                    var selectedType = Type.SelectedItem as DataRowView;
                    int TypeID = Convert.ToInt32(selectedType.Row[0]);

                    var selectedStore = Store.SelectedItem as DataRowView;
                    int StoreID = Convert.ToInt32(selectedStore.Row[0]);

                    products.CreateProducts(ProductName.Text, PriceProduct, QuantityProduct, TypeID, StoreID);
                    ProductsDataGrid.ItemsSource = products.GetData();
                }
                else
                {

                    MessageBox.Show("Цена и количество должны быть числом и быть не меньше 0. Данные не должны содержать специальные символы");
                   
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите данные продукта.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(ProductName.Text) && !string.IsNullOrWhiteSpace(Price.Text)
                && !string.IsNullOrWhiteSpace(Quantity.Text) && !string.IsNullOrWhiteSpace(Type.Text)
                && !string.IsNullOrWhiteSpace(Store.Text))
                {
                    if (!ContainsSpecialCharacters(ProductName.Text) && decimal.TryParse(Price.Text, out decimal PriceProduct)
                    && int.TryParse(Quantity.Text, out int QuantityProduct) && PriceProduct >= 0 && QuantityProduct >= 0)
                    {
                        var selectedType = Type.SelectedItem as DataRowView;
                        int TypeID = Convert.ToInt32(selectedType.Row[0]);

                        var selectedStore = Store.SelectedItem as DataRowView;
                        int StoreID = Convert.ToInt32(selectedStore.Row[0]);


                        object id = (ProductsDataGrid.SelectedItem as DataRowView).Row[0];

                        products.UpdateProducts(ProductName.Text, PriceProduct, QuantityProduct, TypeID, StoreID, Convert.ToInt32(id));
                        ProductsDataGrid.ItemsSource = products.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Цена и количество должны быть числом и быть не меньше 0. Данные не должны содержать специальные символы");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите данные продукта.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите продукт который хотите удалить.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить продукт?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (ProductsDataGrid.SelectedItem as DataRowView).Row[0];
                    products.DeleteProducts(Convert.ToInt32(id));
                    ProductsDataGrid.ItemsSource = products.GetData();
                }
            }
        }
        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }

        private void ProductsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = ProductsDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                ProductName.Text = selectedRow.Row[1].ToString();
                Price.Text = selectedRow.Row[2].ToString();
                Quantity.Text = selectedRow.Row[3].ToString();
                Type.Text = selectedRow.Row[4].ToString();
                Quantity.Text = selectedRow.Row[5].ToString();
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ProductModel> forImport = Converter.DeserializeObject<List<ProductModel>>();
                foreach (var model in forImport)
                {
                    if (!string.IsNullOrWhiteSpace(model.PoductName) && model.Price >= 0 && model.Quantity >= 0 && model.Type >= 0 && model.Store >= 0) // Добавляем проверку на правильный формат для каждого элемента списка
                    {
                        products.CreateProducts(model.PoductName, model.Price, model.Quantity, model.Type, model.Store);
                    }
                    else
                    {
                        MessageBox.Show("Данные импорта не соответствуют формату. Пожалуйста, проверьте данные перед импортом.");
                    }
                }
                ProductsDataGrid.ItemsSource = null;
                ProductsDataGrid.ItemsSource = products.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка импорта данных: " + ex.Message);
            }
        }
    }
}
