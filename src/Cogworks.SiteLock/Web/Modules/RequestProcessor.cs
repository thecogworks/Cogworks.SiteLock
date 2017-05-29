using Cogworks.SiteLock.Web.Authentication;
using Cogworks.SiteLock.Web.Configuration;
using Cogworks.SiteLock.Web.Helpers;
using System.Web;

namespace Cogworks.SiteLock.Web.Modules
{
    public class RequestProcessor
    {
        ISiteLockConfiguration _config;
        IAuthenticationChecker _authChecker;

        public RequestProcessor(ISiteLockConfiguration config, IAuthenticationChecker authenticationChecker)
        {
            _config = config;
            _authChecker = authenticationChecker;
        }


        public void ProcessRequest(HttpContextBase httpContext)
        {
            var requestUri = httpContext.Request.Url;
            var absolutePath = requestUri.AbsolutePath.ToLowerInvariant();

            if (RequestHelper.IsIgnoredPath(_config, absolutePath)) { return; }

            if (!RequestHelper.IsLockedDomain(_config, requestUri.Host)) { return; }

            if (!_authChecker.IsAuthenticated(httpContext))
            {
                httpContext.Response.StatusCode = 403;

                throw new HttpException(403, "Locked by Cogworks.SiteLock Module");
            }
        }
    }
}
