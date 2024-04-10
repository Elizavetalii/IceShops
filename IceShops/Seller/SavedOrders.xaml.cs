using IceShops.IceShopDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IceShops.Seller
{
    public partial class SavedOrders : Page
    {
        ReceiptsTableAdapter receipts = new ReceiptsTableAdapter();
        OrdersTableAdapter order = new OrdersTableAdapter();
        EmployeesTableAdapter employee = new EmployeesTableAdapter();
        ProductsTableAdapter product = new ProductsTableAdapter();
        public List<OrderModel> orders = new List<OrderModel>();
        public SavedOrders()
        {
            InitializeComponent();
            DataGrid.ItemsSource = product.GetData();
            Receipts.ItemsSource = receipts.GetData();
            Receipts.DisplayMemberPath = "date_receipt";           
        }

        private void Receipts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Receipts.SelectedItem != null)
            {
             
                if (Receipts.SelectedItem is OrderModel selectedOrder)
                {
                    if (DateTime.TryParse(selectedOrder.name, out DateTime selectedDate))
                    {
                      
                        var ordersForDate = receipts.OrdersForDate(selectedDate.ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                        DataRowView selectedRow = (DataRowView)DataGridSaved.SelectedItem;

                        if (selectedRow != null)
                        {
                            int id = Convert.ToInt32(selectedRow["product_id"]);
                            string name = selectedRow["product_name"].ToString();
                            decimal price = Convert.ToDecimal(selectedRow["price"]);
                            int type = Convert.ToInt32(selectedRow["types_id"]);
                            int store = Convert.ToInt32(selectedRow["store_id"]);
                            selectedRow["quantity"] = Convert.ToInt32(selectedRow["quantity"]) - 1;

                            OrderModel newOrder = new OrderModel
                            {
                                id = id,
                                name = name,
                                quantity = 1,
                                price = price,
                                type = type,
                                store = store
                            };


                            orders.Add(newOrder);
                            DataGridSaved.ItemsSource = null;
                            DataGridSaved.ItemsSource = orders;

                            var all = receipts.GetData().Rows;
                            var emplo = employee.GetData().Rows;
                            Employee.Text = emplo[1]["FirstName"].ToString() + " " + emplo[2]["LastName"].ToString() + " " + emplo[3]["SurName"].ToString();
                            Date.Text = Receipts.DisplayMemberPath;
                            Price.Text = selectedOrder.price.ToString();
                            DataGridSaved.ItemsSource = ordersForDate;
                        }
                    }
                    else
                    {
                       MessageBox.Show("Неверный формат ");
                    }
                }
            }
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            if (Receipts.SelectedItem != null)
            {
                if (Receipts.SelectedItem is OrderModel selectedOrder)
                {
                    if (DateTime.TryParse(selectedOrder.name, out DateTime selectedDate))
                    {
                        var ordersForDate = receipts.OrdersForDate(selectedDate.ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                        var allReceipt = receipts.GetData().Rows;
                        var lastReceiptId = allReceipt[allReceipt.Count - 1]["receipt_id"];

                        StringBuilder txtContent = new StringBuilder();
                        txtContent.AppendLine("\t Ваш любимый IceShops");
                        txtContent.AppendLine("\t Кассовый чек №" + lastReceiptId);

                        foreach (var order in orders)
                        {
                            txtContent.AppendLine("  " + order.name + " - " + order.price + " x " + order.quantity + " = " + (order.price * order.quantity));
                        }
                        decimal totalPrice = orders.Sum(order => order.price * order.quantity);
                        txtContent.AppendLine("Итого к оплате: " + totalPrice);
                        //txtContent.AppendLine("Внесено: " + amountPaid);
                        //txtContent.AppendLine("Сдача: " + (amountPaid - totalPrice));

                        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\Чек_{lastReceiptId}.txt";

                        File.WriteAllText(filePath, txtContent.ToString());                   
                        MessageBox.Show("Чек сохранен на рабочем столе.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid date format");
            }
        }
    }
}
