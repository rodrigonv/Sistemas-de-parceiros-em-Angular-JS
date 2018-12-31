using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.Model.Optimus.Web.Parceiros
{
    public class Cart
    {
        private static Cart instance;
        private Dictionary<Product, int> items;

        public Cart()
        {
            items = new Dictionary<Product, int>();
            //selfPopulate();
        }

        //private void selfPopulate()
        //{
        //    int i = 1;
        //    ProductDataAcess products = new ProductDataAcess();
        //    foreach (Product product in products.ProductList)
        //    {
        //        Add(product, i++);
        //    }
        //}
        public static Cart Instance()
        {
            if (instance == null)
            {
                instance = new Cart();
            }
            return instance;
        }
        public void Add(Product product, int quantity)
        {
            items.Add(product, quantity);
        }
        public Dictionary<Product, int> Items
        {
            get { return items; }
        }
    }
}