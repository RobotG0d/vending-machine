using MvpMatch.Challenges.VendingMachine.API.Filters;
using MvpMatch.Challenges.VendingMachine.API.Models.Accounts;
using MvpMatch.Challenges.VendingMachine.API.Models.Clients;
using MvpMatch.Challenges.VendingMachine.API.Utils;
using MvpMatch.Challenges.VendingMachine.Business;
using MvpMatch.Challenges.VendingMachine.Entities.Enums;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvpMatch.Challenges.VendingMachine.API.Controllers
{
    [RoutePrefix("")]
    [RoleFilter(Roles.Buyer)]
    public class ClientController : BaseVendingMachineController
    {
        [HttpPost]
        [Route("deposit/{coin}")]
        public HttpResponseMessage Deposit(int coin)
        {
            var clientManager = new ClientManager();
            if (!clientManager.ValidateCoin(coin, out var errors))
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);

            var user = clientManager.AddCoin(SessionToken.UserId, coin);

            var response = new BalanceResponse(){ Balance = user.Deposit };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [SessionFilter]
        [Route("balance")]
        public HttpResponseMessage Balance()
        {
            var user = new AccountManager().Get(SessionToken.UserId);

            var response = new BalanceResponse() { Balance = user.Deposit };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [SessionFilter]
        [Route("reset")]
        public HttpResponseMessage Reset()
        {
            var coins = new ClientManager().ResetBalance(SessionToken.UserId);

            return Request.CreateResponse(HttpStatusCode.OK, coins);
        }
    }
}
