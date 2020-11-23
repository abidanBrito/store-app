using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string manufacturer { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }
    }

    internal class Products
    {
    public List<Product> products { get; set; }
    }
}
