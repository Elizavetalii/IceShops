using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceShops.Seller
{
    public class OrderModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public int quantity { set; get; }
        public decimal price { set; get; }
        public int type { set; get; }
        public int store { set; get; } 
    }
}
