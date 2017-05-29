using Cogworks.SiteLock.Web.Configuration;
using System.Linq;

namespace Cogworks.SiteLock.Web.Helpers
{
    internal class RequestHelper
    {
        internal static bool IsIgnoredPath(ISiteLockConfiguration config, string absolutePath)
        {
            var ignorePaths = config.GetIgnoredPaths();
            var isIgnoredPath = ignorePaths.Any(x => absolutePath.Contains(x));

            return isIgnoredPath;
        }


        internal static bool IsLockedDomain(ISiteLockConfiguration config, string hostDomain)
        {
            var domains = config.GetLockedDomains();

            if (domains.Any(x => x == "*"))
            {
                return true;
            }

            var isLockedDomain = domains.Any(x => hostDomain.Contains(x));

            return isLockedDomain;
        }
    }
}
