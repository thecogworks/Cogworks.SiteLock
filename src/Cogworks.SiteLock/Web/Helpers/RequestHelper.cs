using Cogworks.SiteLock.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cogworks.SiteLock.Web.Helpers
{
    internal class RequestHelper
    {
        internal static bool IsUmbracoAllowedPath(ISiteLockConfiguration config, string absolutePath, Uri urlReferrer)
        {
            string absolutePathLowered = absolutePath.ToLowerInvariant();

            if (absolutePathLowered == "/umbraco/default") { return true; }

            if (urlReferrer == null) { return false; }

            string urlReferrerLowered = urlReferrer.AbsolutePath.ToLowerInvariant();

            if (urlReferrerLowered.StartsWith("/dependencyhandler.axd")) { return true; }

            bool isUmbracoUrl = urlReferrerLowered.StartsWith("/umbraco");

            return isUmbracoUrl;
        }

        internal static bool IsAllowedReferrerPath(ISiteLockConfiguration config, string absolutePath, Uri urlReferrer)
        {
            if (urlReferrer == null) { return false; }

            string absolutePathLowered = absolutePath.ToLowerInvariant();

            string urlReferrerLowered = urlReferrer.AbsolutePath.ToLowerInvariant();

            bool isAllowedReferrer = IsAllowedPath(config, urlReferrerLowered);

            if (isAllowedReferrer)
            {
                // handles css files linking to images.
                config.AppendAllowedPath(absolutePathLowered);
            }

            return isAllowedReferrer;
        }

        internal static bool IsAllowedIP(ISiteLockConfiguration config, string userHostAddress)
        {
            List<string> ips = config.GetAllowedIPs();

            return ips.Contains(userHostAddress);
        }

        internal static bool IsAllowedPath(ISiteLockConfiguration config, string absolutePath)
        {
            string absolutePathLowered = absolutePath.ToLowerInvariant();

            IEnumerable<string> allowedPaths = config.GetAllowedPaths().Select(path => path.ToLowerInvariant());

            return allowedPaths.Select(item => new Regex(item)).Select(regex => regex.Match(absolutePathLowered)).Any(match => match.Success);
        }

        internal static bool IsLockedDomain(ISiteLockConfiguration config, string hostDomain)
        {
            List<string> domains = config.GetLockedDomains();

            if (domains.Any(x => x == "*"))
            {
                return true;
            }

            string hostDomainLowered = hostDomain.ToLowerInvariant();

            bool isLockedDomain = domains.Any(x => hostDomainLowered.Contains(x));

            return isLockedDomain;
        }
    }
}