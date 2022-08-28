using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvpMatch.Challenges.VendingMachine.API.Models.Clients
{
    public class BuyResponse
    {
        public Product Product { get; set; }
        
        public int ProductAmount { get; set; }

        public int TotalSpent { get; set; }

        public int[] Change { get; set; }

        public BuyResponse() { }

        public BuyResponse(ProductBuyResult input)
        {
            ProductAmount = input.ProductAmount;
            TotalSpent = input.TotalSpent;
            Change = input.Change;
            Product = new Product()
            {
                Id = input.Id,
                Name = input.Name,
                Cost = input.Cost,
                AmountAvailable = input.AmountAvailable,
                SellerId = input.SellerId
            };
        }
    }
}