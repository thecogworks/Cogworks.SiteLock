using System.Web;

namespace Cogworks.SiteLock.Web.Extensions
{
    internal static class RequestExtensions
    {
        public static string GetIpAddress(this HttpRequestBase request)
        {
            string ipAddress = request.ServerVariables["CF-Connecting-IP"];
            if (string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return request.UserHostAddress;
        }
    }
}
