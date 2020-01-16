using System.Web;
using Umbraco.Web.Security;

namespace Cogworks.SiteLock.Web.Authentication
{
    public class AuthenticationChecker : IAuthenticationChecker
    {
        /// <summary>
        /// Checks authentication from HttpContextBase.GetUmbracoAuthTicket()
        /// </summary>
        /// <param name="httpContext">HttpContextBase</param>
        /// <returns>true if authenticated</returns>
        public bool IsAuthenticated(HttpContextBase httpContext)
        {
            Microsoft.Owin.Security.AuthenticationTicket ticket = httpContext.GetUmbracoAuthTicket();
            return ticket != null;
        }
    }
}
