using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvpMatch.Challenges.VendingMachine.API.Controllers;
using MvpMatch.Challenges.VendingMachine.API.Models;
using MvpMatch.Challenges.VendingMachine.API.Models.Accounts;
using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace MvpMatch.Challenges.VendingMachine.API.Test
{
    [TestClass]
    public abstract class BaseTest
    {
        #region Protected Utils

        protected static SessionToken CreateUser(Roles roleId)
        {
            var username = Guid.NewGuid().ToString("n");
            var registerBuyerRequest = new RegisterRequest()
            {
                RoleId = (int)roleId,
                Username = username,
                Password = username
            };

            var buyerRegisterResult = CreateController<AccountController>().Register(registerBuyerRequest);
            if (buyerRegisterResult.StatusCode != HttpStatusCode.Created)
                throw new Exception("Could not create user to start testing with.");

            var success = buyerRegisterResult.TryGetContentValue<User>(out var user);
            if (!success)
                throw new Exception("Could not create user to start testing with.");

            return new SessionToken()
            {
                UserId = user.Id,
                RoleId = user.RoleId,
                SessionId = Guid.NewGuid()
            };
        }

        protected static T CreateController<T>(SessionToken token = null) where T : BaseVendingMachineController, new()
        {
            var controller = new T();
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            controller.SessionToken = token;

            return controller;
        }

        #endregion

    }
}
