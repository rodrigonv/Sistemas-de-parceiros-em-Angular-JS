using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.Model.Optimus.Web.Parceiros
{
    public class Product
    {
        private int id;
        private string name;
        private string description;
        private double price;
        private double quantidade;
        public Product() : this(0, "", 0, "",0) { }

        public Product(int id, string name, double price, double pquantidade)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantidade = pquantidade;
        }

        public Product(int id, string name, double price, string description, double pquantidade)
            : this(id, name, price, pquantidade)
        {
            Description = description;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public double Quantidade 
        {
            get { return price; }
            set { price = value; }
        }
    }
}