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
    public class CreateProductTest : BaseTest
    {
        private static SessionToken SessionToken { get; set; }
        private static Product CreatedProduct { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SessionToken = CreateUser(Entities.Enums.Roles.Seller);
        }

        [TestMethod]
        public void CreateReturnsProduct()
        {
            var createRequest = new ProductCreateRequest()
            {
               Name = "Bolo de Teste",
               Cost = 105,
               AmountAvailable = 5
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<Product>(out var product);
            CreatedProduct = product;

            Assert.IsTrue(success);
            Assert.IsNotNull(product);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(product.AmountAvailable, 5);
            Assert.AreEqual(product.Cost, 105);
            Assert.AreEqual(product.Name, "Bolo de Teste");
        }

        [TestMethod]
        public void CostIsMandatory()
        {
            var createRequest = new ProductCreateRequest()
            {
                Name = "Bolo de Teste",
                AmountAvailable = 5
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Product's cost must be a positive multiple of 5.");
        }

        [TestMethod]
        public void CostMustBeMultipleOf5()
        {
            var createRequest = new ProductCreateRequest()
            {
                Name = "Bolo de Teste",
                Cost = 6,
                AmountAvailable = 5
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Product's cost must be a positive multiple of 5.");
        }

        [TestMethod]
        public void NameIsMandatory()
        {
            var createRequest = new ProductCreateRequest()
            {
                Cost = 105,
                AmountAvailable = 5
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Product must have a name.");
        }

        [TestMethod]
        public void AmountIsMandatory()
        {
            var createRequest = new ProductCreateRequest()
            {
                Name = "Bolo de Teste",
                Cost = 105
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Product's amount must be a positive integer.");
        }

        [TestMethod]
        public void AmountMustBePositive()
        {
            var createRequest = new ProductCreateRequest()
            {
                Name = "Bolo de Teste",
                Cost = 105,
                AmountAvailable = -5
            };

            var createResult = CreateController<ProductController>(SessionToken).Create(createRequest);
            var success = createResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(createResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Product's amount must be a positive integer.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if(CreatedProduct != null)
            {
                var productcontroller = CreateController<ProductController>(SessionToken);
                productcontroller.Delete(CreatedProduct.Id);
                CreatedProduct = null;
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var accountController = CreateController<AccountController>(SessionToken);
            accountController.Delete();
        }

    }
}
