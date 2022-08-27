using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvpMatch.Challenges.VendingMachine.API.Models.Accounts
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}