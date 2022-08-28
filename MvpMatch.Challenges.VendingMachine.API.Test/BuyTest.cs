using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvpMatch.Challenges.VendingMachine.API.Controllers;
using MvpMatch.Challenges.VendingMachine.API.Models;
using MvpMatch.Challenges.VendingMachine.API.Models.Accounts;
using MvpMatch.Challenges.VendingMachine.API.Models.Clients;
using MvpMatch.Challenges.VendingMachine.API.Models.Products;
using MvpMatch.Challenges.VendingMachine.Entities;

namespace MvpMatch.Challenges.VendingMachine.API.Test
{
    [TestClass]
    public class BuyTest : BaseTest
    {
        private static SessionToken BuyerToken { get; set; }
        private static SessionToken SellerToken { get; set; }

        private static Product CreatedProduct { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            BuyerToken = CreateUser(Entities.Enums.Roles.Buyer);
            SellerToken = CreateUser(Entities.Enums.Roles.Seller);

            var createRequest = new ProductCreateRequest()
            {
                Name = "Bolo de Teste",
                Cost = 15,
                AmountAvailable = 2
            };

            var createResult = CreateController<ProductController>(SellerToken).Create(createRequest);
            createResult.TryGetContentValue<Product>(out var product);
            CreatedProduct = product;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var depositRequest = new DepositRequest() { Coin = 50 };
            CreateController<ClientController>(BuyerToken).Deposit(depositRequest);
        }

        [TestMethod]
        public void BuyGivesCorrectProduct()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.Product.Id, CreatedProduct.Id);
        }

        [TestMethod]
        public void BuyGivesCorrectAmount()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.ProductAmount, 1);
        }

        [TestMethod]
        public void BuyGivesCorrectChange()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.Change[0], 1);
            Assert.AreEqual(purchaseData.Change[1], 1);
            Assert.AreEqual(purchaseData.Change[2], 1);
            Assert.AreEqual(purchaseData.Change[3], 0);
            Assert.AreEqual(purchaseData.Change[4], 0);
        }

        [TestMethod]
        public void BuyReportsCorrectCost()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 2
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.TotalSpent, 2 * CreatedProduct.Cost);
        }

        [TestMethod]
        public void BuyOneWorks()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.Change[0], 1);
            Assert.AreEqual(purchaseData.Change[1], 1);
            Assert.AreEqual(purchaseData.Change[2], 1);
            Assert.AreEqual(purchaseData.Change[3], 0);
            Assert.AreEqual(purchaseData.Change[4], 0);
            Assert.AreEqual(purchaseData.ProductAmount, 1);
            Assert.AreEqual(purchaseData.Product.Id, CreatedProduct.Id);
            Assert.AreEqual(purchaseData.TotalSpent, CreatedProduct.Cost);
        }

        [TestMethod]
        public void BuyMultipleWorks()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 2
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<BuyResponse>(out var purchaseData);

            Assert.IsTrue(success);
            Assert.IsNotNull(purchaseData);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(purchaseData.Change[0], 0);
            Assert.AreEqual(purchaseData.Change[1], 0);
            Assert.AreEqual(purchaseData.Change[2], 1);
            Assert.AreEqual(purchaseData.Change[3], 0);
            Assert.AreEqual(purchaseData.Change[4], 0);
            Assert.AreEqual(purchaseData.ProductAmount, 2);
            Assert.AreEqual(purchaseData.Product.Id, CreatedProduct.Id);
            Assert.AreEqual(purchaseData.TotalSpent, 2 * CreatedProduct.Cost);
        }

        [TestMethod]
        public void CantBuyWithoutMoney()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 5
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<string>(out var errorMessage);

            Assert.IsTrue(success);
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage, "User does not have enough money to fulfill the request.");
        }

        [TestMethod]
        public void CantBuyProductWithNoStock()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 3
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<string>(out var errorMessage);

            Assert.IsTrue(success);
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage, "There aren't enough products to satisfy the request.");
        }

        [TestMethod]
        public void CantBuyProductThatDoesntExist()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = int.MaxValue - 1,
                Amount = 1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<string>(out var errorMessage);

            Assert.IsTrue(success);
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage, "Requested product does not exist.");
        }

        [TestMethod]
        public void CantBuyNegativeAmount()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = -1
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<string>(out var errorMessage);

            Assert.IsTrue(success);
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage, "Amount must be a positive integer.");
        }

        [TestMethod]
        public void CantBuyZeroAmount()
        {
            var buyRequest = new BuyRequest()
            {
                ProductId = CreatedProduct.Id,
                Amount = 0
            };

            var buyResult = CreateController<ClientController>(BuyerToken).Buy(buyRequest);
            var success = buyResult.TryGetContentValue<string>(out var errorMessage);

            Assert.IsTrue(success);
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(buyResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage, "Amount must be a positive integer.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var productController = CreateController<ProductController>(SellerToken);
            productController.Update(CreatedProduct.Id, new ProductUpdateRequest() { AmountAvailable = 2, Cost = 15 });

            var clientController = CreateController<ClientController>(BuyerToken);
            clientController.Reset();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (CreatedProduct != null)
            {
                var productcontroller = CreateController<ProductController>(SellerToken);
                productcontroller.Delete(CreatedProduct.Id);
                CreatedProduct = null;
            }

            var accountController = CreateController<AccountController>();
            accountController.SessionToken = BuyerToken;
            accountController.Delete();
            accountController.SessionToken = SellerToken;
            accountController.Delete();
        }
    }
}
