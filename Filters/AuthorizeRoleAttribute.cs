using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniManage.Filters
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserId"] == null || httpContext.Session["Role"] == null)
            {
                return false;
            }

            string role = httpContext.Session["Role"].ToString();
            return _roles.Contains(role);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Account/Login");
        }
    }
}
