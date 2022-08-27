using MvpMatch.Challenges.VendingMachine.API.Filters;
using MvpMatch.Challenges.VendingMachine.API.Models;
using MvpMatch.Challenges.VendingMachine.API.Models.Accounts;
using MvpMatch.Challenges.VendingMachine.API.Utils;
using MvpMatch.Challenges.VendingMachine.Business;
using MvpMatch.Challenges.VendingMachine.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvpMatch.Challenges.VendingMachine.API.Controllers
{
    public abstract class BaseVendingMachineController : ApiController
    {
        protected SessionToken SessionToken 
            => SessionUtils.GetToken();
    }
}
