using MvpMatch.Challenges.VendingMachine.API.Exceptions;
using MvpMatch.Challenges.VendingMachine.API.Utils;
using MvpMatch.Challenges.VendingMachine.Business;
using MvpMatch.Challenges.VendingMachine.Common.Settings;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MvpMatch.Challenges.VendingMachine.API.Filters
{
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            const string NoAuthenticationMessage = "User must be authenticated to perform this action.";

            var token = SessionUtils.GetToken();
            if (token == null)
                throw new UnauthorizedException(NoAuthenticationMessage);

            var userManager = new AccountManager();
            var session = userManager.GetSession(token.UserId, token.SessionId);
            if (session == null)
                throw new UnauthorizedException(NoAuthenticationMessage); 
        }
    }
}