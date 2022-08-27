using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvpMatch.Challenges.VendingMachine.API.Models
{
    public class SessionToken
    {
        public int UserId { get; set; }

        public Guid SessionId { get; set; }

        public int RoleId { get; set; }
    }
}