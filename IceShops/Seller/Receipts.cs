using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceShops.Seller
{
    internal class Receipts
    {
        public DateTime date_receipts { get; set; }
        public int id_receipt { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public int id_employees { get; set; }
    }
}
