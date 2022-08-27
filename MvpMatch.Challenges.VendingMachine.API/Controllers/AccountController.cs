using MvpMatch.Challenges.VendingMachine.API.Filters;
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
    [RoutePrefix("")]
    public class AccountController : BaseVendingMachineController
    {
        [HttpPost]
        [Route("user")]
        public HttpResponseMessage Register(RegisterRequest request)
        {
            var user = new User()
            {
                Username = request.Username,
                Password = request.Password,
                RoleId = request.RoleId
            };

            var userManager = new AccountManager();
            if (!userManager.Validate(user, out var errors))
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);

            user = userManager.Create(user);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(LoginRequest request)
        {
            var userManager = new AccountManager();

            var user = userManager.Authenticate(request.Username, request.Password);
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            var currentSession = userManager.CreateSession(user.Id);
            SessionUtils.SetToken(user, currentSession);

            var sessions = userManager.ListSessions(user.Id);
            if (sessions.Any(s => s.Id != currentSession.Id))
                return Request.CreateResponse("There is already an active session using your account.");

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [SessionFilter]
        [Route("logout")]
        public HttpResponseMessage Logout()
        {
            new AccountManager().DeleteSession(SessionToken.UserId, SessionToken.SessionId);
            SessionUtils.EndToken();

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [SessionFilter]
        [Route("logout/all")]
        public HttpResponseMessage LogoutEverywhere()
        {
            new AccountManager().DeleteSessions(SessionToken.UserId);
            SessionUtils.EndToken();

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
