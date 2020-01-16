using System.Web;

namespace Cogworks.SiteLock.Web.Authentication
{
    /// <summary>
    /// Checks Authentication, returns true if authenticated
    /// </summary>
    public interface IAuthenticationChecker
    {
        bool IsAuthenticated(HttpContextBase httpContext);
    }
}