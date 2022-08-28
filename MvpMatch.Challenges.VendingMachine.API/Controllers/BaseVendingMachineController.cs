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
        private SessionToken _sessionToken;
        public SessionToken SessionToken
        {
            get => _sessionToken == null ? 
                _sessionToken = SessionUtils.GetToken() 
                : _sessionToken;
            set => _sessionToken = value;
        }
    }
}
