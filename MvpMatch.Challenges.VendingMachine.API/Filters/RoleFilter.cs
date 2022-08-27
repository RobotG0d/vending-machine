using MvpMatch.Challenges.VendingMachine.API.Exceptions;
using MvpMatch.Challenges.VendingMachine.API.Utils;
using MvpMatch.Challenges.VendingMachine.Entities.Enums;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MvpMatch.Challenges.VendingMachine.API.Filters
{
    public class RoleFilter : SessionFilter
    {
        private Roles ValidRole { get; set; }

        public RoleFilter(Roles validRole)
            => ValidRole = validRole;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var token = SessionUtils.GetToken();
            if (token.RoleId != (int)ValidRole)
                throw new ForbiddenException($@"Only users with role '{ValidRole}' can perform this action.");
        }
    }
}