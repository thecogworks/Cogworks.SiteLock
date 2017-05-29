using System.Web;
using Umbraco.Core.Security;

namespace Cogworks.SiteLock.Web.Authentication
{
    public interface IAuthenticationChecker
    {
        bool IsAuthenticated(HttpContextBase httpContext);
    }

    public class AuthenticationChecker : IAuthenticationChecker
    {
        public bool IsAuthenticated(HttpContextBase httpContext)
        {
            var ticket = httpContext.GetUmbracoAuthTicket();
            return ticket != null;
        }
    }
}
