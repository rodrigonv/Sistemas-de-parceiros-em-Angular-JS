using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.Model.Optimus.Web.Parceiros
{
    public class ProductDataAcess
    {
        private List<Product> products = new List<Product>();

        //public ProductDataAcess()
        //{
        //    products.Add(new Product(1, "Item 1", 10, "Um item de exemplo"));
        //    products.Add(new Product(1, "Item 2", 20, "Outro item de exemplo"));
        //}
        public Product[] ProductList
        {
            get { return products.ToArray(); }
        }
    }
}