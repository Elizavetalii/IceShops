using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using IceShops.IceShopDataSetTableAdapters;

namespace IceShops.Admin
{
    public partial class Positions : Page
    {
        PositionsTableAdapter positions = new PositionsTableAdapter();
        public Positions()
        {
            InitializeComponent();
            PositionsDataGrid.ItemsSource = positions.GetData();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PositionName.Text))
            {
                if (!ContainsSpecialCharacters(PositionName.Text))
                {
                    positions.CreatePositions(PositionName.Text);
                    PositionsDataGrid.ItemsSource = positions.GetData();
                }
                else
                {
                    MessageBox.Show("Должность не должна содержать специальные символы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название должности.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (PositionsDataGrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(PositionName.Text))
                {
                    if (!ContainsSpecialCharacters(PositionName.Text))
                    {
                        object id = (PositionsDataGrid.SelectedItem as DataRowView).Row[0];
                        positions.UpdatePositions(PositionName.Text, Convert.ToInt32(id));
                        PositionsDataGrid.ItemsSource = positions.GetData();
                    }
                    else
                    {
                        MessageBox.Show("Должность не должна содержать специальные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите название должности.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите должность для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (PositionsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите должность для удаления.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить должность?", "Подтверждение удаления",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    object id = (PositionsDataGrid.SelectedItem as DataRowView).Row[0];
                    positions.DeletePositions(Convert.ToInt32(id));
                    PositionsDataGrid.ItemsSource = positions.GetData();
                }
            }
        }

        private void PositionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = PositionsDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow.Row.ItemArray.Length > 1)
            {
                PositionName.Text = selectedRow.Row[1].ToString();
            }    
        }

        private bool ContainsSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^\p{L}\p{N}\p{P}\p{Z}]");
        }
    }
}
