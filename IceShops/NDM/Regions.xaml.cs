using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.NDM
{
    public partial class Regions : Page
    {
        RegionsTableAdapter regions = new RegionsTableAdapter();
        public Regions()
        {
            InitializeComponent();
            RegionsDataGrid.ItemsSource = regions.GetData();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Region.Text))
            {
                if (!ContainsSpecialCharacters(Region.Text))
                {
                    regions.CreateRegions(Region.Text);
                    RegionsDataGrid.ItemsSource = regions.GetData();
                }
                else
                {
                    MessageBox.Show("Название региона не должно содержать специальные символы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название региона.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (RegionsDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(Region.Text))
                {
                    if (!ContainsSpecialCharacters(Region.Text))
                    {
                        object id = (RegionsDataGrid.SelectedItem as DataRowView).Row[0];
                        regions.UpdateRegions(Region.Text, Convert.ToInt32(id));
                        RegionsDataGrid.ItemsSource = regions.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Название региона не должно содержать специальные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите название региона.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите регион для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RegionsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите регион для удаления.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить регион?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (RegionsDataGrid.SelectedItem as DataRowView).Row[0];
                    regions.DeleteRegions(Convert.ToInt32(id));
                    RegionsDataGrid.ItemsSource = regions.GetData();
                }
            }
        }

        private void RegionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = RegionsDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                Region.Text = selectedRow.Row[1].ToString();
            }
        }
        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }
    }
}
