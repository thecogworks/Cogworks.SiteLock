using Cogworks.Essentials.Extensions;
using System.Web;

namespace Cogworks.SiteLock.Web.Extensions
{
    internal static class RequestExtensions
    {
        public static string GetIpAddress(this HttpRequestBase request)
        {
            var ipAddress = request.ServerVariables["CF-Connecting-IP"];

            if (!ipAddress.HasValue())
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (!ipAddress.HasValue())
            {
                return request.UserHostAddress;
            }

            var addresses = ipAddress.Split(',');

            return addresses.Length != 0 ? addresses[0] : request.UserHostAddress;
        }
    }
}