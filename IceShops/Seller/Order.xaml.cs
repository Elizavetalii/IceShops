using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using IceShops.IceShopDataSetTableAdapters;
using IceShops.Admin;
using IceShops.WM;

namespace IceShops.Seller
{
    public partial class Order : Page
    {
        public OrdersTableAdapter order = new OrdersTableAdapter();
        public ReceiptsTableAdapter receipt = new ReceiptsTableAdapter();
        public ProductsTableAdapter product = new ProductsTableAdapter();
        public EmployeesTableAdapter employee = new EmployeesTableAdapter();
        public List<OrderModel> orders = new List<OrderModel>();
        static decimal totalPrice;
        private int employeeId;
        private int storeId;
        public Order(int employeeId, int storeId)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            this.storeId = storeId;
            DataGridProducts.ItemsSource = product.SelectStore(storeId);
            DataGridOrder.ItemsSource = orders;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DataGridProducts.SelectedItem;

            if (selectedRow != null)
            {
                int id = Convert.ToInt32(selectedRow["product_id"]);
                string name = selectedRow["product_name"].ToString();
                decimal price = Convert.ToDecimal(selectedRow["price"]);
                int type = Convert.ToInt32(selectedRow["types_id"]);
                int store = Convert.ToInt32(selectedRow["store_id"]);
                selectedRow["quantity"] = Convert.ToInt32(selectedRow["quantity"]) - 1;
                
                OrderModel existingProduct = orders.FirstOrDefault(product => product.id == id);

                if (existingProduct != null)
                {
                    existingProduct.quantity += 1;  
                }
                else
                {
                    OrderModel newProduct = new OrderModel
                    {
                        id = id,
                        name = name,
                        quantity = 1,
                        price = price,
                        type = type,
                        store = store
                    };

                    orders.Add(newProduct);
                }

                DataGridOrder.ItemsSource = null;
                DataGridOrder.ItemsSource = orders;
                product.UpdateForOrders(id, Convert.ToInt32(selectedRow["quantity"]));
                UpdateTotalPrice();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (OrderModel)DataGridOrder.SelectedItem;
            DataRowView selectedRow = (DataRowView)DataGridProducts.SelectedItem;
            if (selectedOrder != null)
            {
                if (selectedOrder.quantity > 1)
                {
                    selectedOrder.quantity -= 1;
                    selectedRow["quantity"] = Convert.ToInt32(selectedRow["quantity"]) + 1;
                }
                else
                {
                    orders.Remove(selectedOrder);
                    selectedRow["quantity"] = Convert.ToInt32(selectedRow["quantity"]) + 1;
                }

                DataGridOrder.ItemsSource = null;
                DataGridOrder.ItemsSource = orders;
                int id = selectedOrder.id;
                product.UpdateForOrders(id, Convert.ToInt32(selectedRow["quantity"]));
                UpdateTotalPrice();
            }
        }
        private void UpdateTotalPrice()
        {
            totalPrice = orders.Sum(order => order.price * order.quantity);
            TotalPrice.Text = totalPrice.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(AmountBuyer.Text) && decimal.TryParse(AmountBuyer.Text, out decimal amountPaid))
                {
                    if (amountPaid >= totalPrice)
                    {
                        int receiptId = receipt.InsertReceipt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), employeeId, orders.Count);

                        foreach (var or in orders)
                        {
                            int Order = order.CreateOrders(or.id, receiptId);
                        }

                        var allReceipt = receipt.GetData().Rows;
                        var lastReceiptId = allReceipt[allReceipt.Count - 1]["receipt_id"];

                        StringBuilder txtContent = new StringBuilder();
                        txtContent.AppendLine("\t Ваш любимый IceShops");
                        txtContent.AppendLine("\t Кассовый чек №" + lastReceiptId);

                        foreach (var order in orders)
                        {
                            txtContent.AppendLine("  " + order.name + " - " + order.price + " x " + order.quantity + " = " + (order.price * order.quantity));
                        }

                        txtContent.AppendLine("Итого к оплате: " + totalPrice);
                        txtContent.AppendLine("Внесено: " + amountPaid);
                        txtContent.AppendLine("Сдача: " + (amountPaid - totalPrice));

                        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\Чек_{lastReceiptId}.txt";

                        File.WriteAllText(filePath, txtContent.ToString());
                      
                        DataGridOrder.ItemsSource = null;
                        orders.Clear();
                        AmountBuyer.Text = "Amount paid by the buyer";
                        TotalPrice.Text = "0";

                        MessageBox.Show("Чек сохранен на рабочем столе.");
                    }
                    else
                    {
                        MessageBox.Show("Сумма, внесённая покупателем, должна быть больше или равна общей стоимости.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректную сумму, оплаченную покупателем.");
                }
            }
            catch
            {
                MessageBox.Show("Пожалуйста, внесите в заказ продукты для покупки.");
            }
        }
        

        private void AmountBuyer_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Amount paid by the buyer")
            {
                textBox.Text = "";
            }
        }
    }
}
