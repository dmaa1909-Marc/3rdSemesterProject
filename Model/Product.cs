using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum ProductType {
        TableSkin,
        CardSkin,
        PremiumAccount
    }

    public enum Currency {
        DKK,
        EUR,
        USD
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }

        public Currency Currency { get; set; }

        public ProductType ProductType { get; set; }

        public int AmountAvailable { get; set; }

        public int AmountTotal { get; set; }

        public bool Active { get; set; }

        public Product(string name, string description, double price, Currency currency, ProductType productType, int amountAvailable, int amountTotal, bool active) :
            this(0, name, description, price, currency, productType, amountAvailable, amountTotal, active) {}

        public Product(int id, string name, string description, double price, Currency currency, ProductType productType, int amountAvailable, int amountTotal, bool active)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            ProductType = productType;
            AmountAvailable = amountAvailable;
            AmountTotal = amountTotal;
            Active = active;
        }

    }
}
