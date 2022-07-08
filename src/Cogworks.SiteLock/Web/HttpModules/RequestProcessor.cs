using Cogworks.Essentials.Extensions;
using Cogworks.SiteLock.Web.Authentication;
using Cogworks.SiteLock.Web.Configuration;
using Cogworks.SiteLock.Web.Extensions;
using Cogworks.SiteLock.Web.Helpers;
using System.Web;

namespace Cogworks.SiteLock.Web.HttpModules
{
    public class RequestProcessor
    {
        private readonly ISiteLockConfiguration _config;
        private readonly IAuthenticationChecker _authChecker;

        public RequestProcessor(ISiteLockConfiguration config, IAuthenticationChecker authenticationChecker)
        {
            _config = config;
            _authChecker = authenticationChecker;
        }

        public void ProcessRequest(HttpContextBase httpContext)
        {
            var requestUri = httpContext.Request.Url;
            if (!requestUri.HasValue()) { return; }

            var absolutePath = requestUri.AbsolutePath;
            var urlReferrer = httpContext.Request.UrlReferrer;
            var ipAddress = httpContext.Request.GetIpAddress();

            if (RequestHelper.IsLockedDomain(_config, requestUri.Host))
            {
                if (RequestHelper.IsAllowedIP(_config, GetUserHostAddress(httpContext))) { return; }

            if (RequestHelper.IsAllowedIP(_config, ipAddress)) { return; }

            if (RequestHelper.IsAllowedReferrerPath(_config, absolutePath, urlReferrer)) { return; }

            if (RequestHelper.IsAllowedPath(_config, absolutePath)) { return; }

            if (RequestHelper.IsUmbracoAllowedPath(_config, absolutePath, urlReferrer)) { return; }

            if (_authChecker.IsAuthenticated(httpContext)) { return; }

            httpContext.Response.StatusCode = 403;
            throw new HttpException(403, "Locked by Cogworks.SiteLock Module");
        }

        /// <summary>
        /// Attempt to get the IP address of the client (as a string)
        /// </summary>
        /// <returns></returns>
        private static string GetUserHostAddress(HttpContextBase httpContext)
        {
            string ipAddress = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] ipAddresses = ipAddress.Split(',');

                if (ipAddresses.Length != 0)
                {
                    return ipAddresses[0];
                }
            }

            return httpContext.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}