using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;
using IceShops.WM;

namespace IceShops.NDM
{
    public partial class Stores : Page
    {
        StoresTableAdapter stores = new StoresTableAdapter();
        RegionsTableAdapter regions = new RegionsTableAdapter();
        public Stores()
        {
            InitializeComponent();
            StoresDataGrid.ItemsSource = stores.GetData();

            Regions.ItemsSource = regions.GetData();
            Regions.DisplayMemberPath = "region_name";
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Address.Text) && !string.IsNullOrWhiteSpace(Rent.Text) && !string.IsNullOrWhiteSpace(Regions.Text))
            {
                if (!ContainsSpecialCharacters(Address.Text) && decimal.TryParse(Rent.Text, out decimal rentAmount) && rentAmount >= 0)
                {
                    var selectedRegions = Regions.SelectedItem as DataRowView;
                    int RegionsID = Convert.ToInt32(selectedRegions.Row[0]);

                    stores.CreateStores(Address.Text, rentAmount, RegionsID);
                    StoresDataGrid.ItemsSource = stores.GetData();

                }
                else
                {
                    MessageBox.Show("Арендная плата должна быть числом и быть не меньше 0. Данные не должны содержать специальные символы");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите данные.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (StoresDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(Address.Text) && !string.IsNullOrWhiteSpace(Rent.Text) && !string.IsNullOrWhiteSpace(Regions.Text))
                {
                    if (!ContainsSpecialCharacters(Address.Text) && decimal.TryParse(Rent.Text, out decimal rentAmount) && rentAmount >= 0)
                    {
                        DataRowView selectedRegions = Regions.SelectedItem as DataRowView;
                        int RegionsID = Convert.ToInt32(selectedRegions["region_id"]);
                        object id = (StoresDataGrid.SelectedItem as DataRowView).Row[0];      
                        stores.UpdateStores(Address.Text, rentAmount, Convert.ToInt32(id), RegionsID);
                        StoresDataGrid.ItemsSource = stores.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Арендная плата должна быть числом и быть не меньше 0. Данные не должны содержать специальные символы");
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
            if (StoresDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите данные которые хотите удалить.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить магазин?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (StoresDataGrid.SelectedItem as DataRowView).Row[0];
                    stores.DeleteStores(Convert.ToInt32(id));
                    StoresDataGrid.ItemsSource = stores.GetData();
                }
            }
        }
        private void StoresDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = StoresDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                Address.Text = selectedRow.Row[1].ToString();
                Rent.Text = selectedRow.Row[2].ToString();
                Regions.Text = selectedRow.Row[3].ToString();
            }
        }
        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, "[`|><,._+@$&^'={}/#~!\"№;%:?*()]");
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<StoresModel> forImport = Converter.DeserializeObject<List<StoresModel>>();
                foreach (var model in forImport)
                {
                    if (!string.IsNullOrWhiteSpace(model.StoreAddress) && model.Rent >= 0) // Добавляем проверку на правильный формат для каждого элемента списка
                    {
                        stores.CreateStores(model.StoreAddress, model.Rent, model.Region);
                    }
                    else
                    {
                        MessageBox.Show("Данные импорта не соответствуют формату. Пожалуйста, проверьте данные перед импортом.");
                    }
                }
                StoresDataGrid.ItemsSource = null;
                StoresDataGrid.ItemsSource = stores.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка импорта данных: " + ex.Message);
            }
        }
    }
}
