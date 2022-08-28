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
using MvpMatch.Challenges.VendingMachine.Entities;

namespace MvpMatch.Challenges.VendingMachine.API.Test
{
    [TestClass]
    public class DepositTest : BaseTest
    {
        private static int TotalBalance { get; set; }
        private static SessionToken SessionToken { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            TotalBalance = 0;
            SessionToken = CreateUser(Entities.Enums.Roles.Buyer);
        }

        [TestMethod]
        public void Deposit5Adds5ToBalance()
            => DepositCorrectlyAddsToBalance(5);

        [TestMethod]
        public void Deposit10Adds10ToBalance()
            => DepositCorrectlyAddsToBalance(10);

        [TestMethod]
        public void Deposit20Adds20ToBalance()
            => DepositCorrectlyAddsToBalance(20);

        [TestMethod]
        public void Deposit50Adds50ToBalance()
            => DepositCorrectlyAddsToBalance(50);
        
        [TestMethod]
        public void Deposit100Adds100ToBalance()
            => DepositCorrectlyAddsToBalance(100);

        [TestMethod]
        public void DepositInvalidCoinThrowsError()
        {
            var depositRequest = new DepositRequest() { Coin = 51 };

            var depositResult = CreateController<ClientController>(SessionToken).Deposit(depositRequest);
            var success = depositResult.TryGetContentValue<List<string>>(out var messages);

            Assert.IsTrue(success);
            Assert.IsNotNull(messages);
            Assert.AreEqual(depositResult.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(messages.First(), "Invalid coin. Allowed coins are: 5; 10; 20; 50; 100; 200");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var accountController = CreateController<AccountController>(SessionToken);
            accountController.Delete();
        }

        #region Private Utils

        private void DepositCorrectlyAddsToBalance(int coin)
        {
            var depositRequest = new DepositRequest() { Coin = coin };

            var depositResult = CreateController<ClientController>(SessionToken).Deposit(depositRequest);
            var success = depositResult.TryGetContentValue<BalanceResponse>(out var balance);

            Assert.IsTrue(success);
            Assert.IsNotNull(balance);
            Assert.AreEqual(depositResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(balance.Balance, TotalBalance + coin);

            TotalBalance = balance.Balance;
        }

        #endregion
    }
}
