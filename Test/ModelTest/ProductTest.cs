using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Model;

namespace Test
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void CheckIfProductIsCreatedWithTheCorrectParameters()
        {
            
            // Arrange
            int id = 1;
            string name = "Test Product";
            string description = "Product for testing";
            double price = 15;
            Currency currency = Currency.DKK;
            ProductType productType = ProductType.CardSkin;
            int amountAvailable = 20;
            int amountTotal = 20;
            bool active = true;


            // Act
            Product product = new Product(id, name, description, price, currency, productType, amountAvailable, amountTotal, active);


            // Assert
            Assert.AreEqual(product.Name, name);
            Assert.AreEqual(product.Description, description);
            Assert.AreEqual(product.Price, price);
            Assert.AreEqual(product.Currency, currency);
            Assert.AreEqual(product.ProductType, productType);
            Assert.AreEqual(product.AmountAvailable, amountAvailable);
            Assert.AreEqual(product.AmountTotal, amountTotal);
            Assert.AreEqual(product.Active, active);
        }
    }
}
