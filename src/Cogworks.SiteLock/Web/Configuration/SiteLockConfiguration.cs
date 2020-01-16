using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Cogworks.SiteLock.Web.Configuration
{
    public class SiteLockConfiguration : ISiteLockConfiguration
    {
        private static readonly string DomainsKey = typeof(SiteLockConfiguration) + "_domains";
        private static readonly string AllowedPathsKey = typeof(SiteLockConfiguration) + "_allowedPaths";
        private static readonly string AllowedIpsKey = typeof(SiteLockConfiguration) + "_allowedIps";

        public List<string> GetAllowedIPs()
        {
            if (HttpRuntime.Cache[AllowedIpsKey] is List<string> value) return value;

            value = GetValues("allowedIps", "ip");

            HttpRuntime.Cache.Insert(AllowedIpsKey, value, null);

            return value;
        }

        public List<string> GetLockedDomains()
        {
            if (HttpRuntime.Cache[DomainsKey] is List<string> value) return value;

            value = GetValues("lockedDomains", "domain");

            HttpRuntime.Cache.Insert(DomainsKey, value, null);

            return value;
        }

        public List<string> GetAllowedPaths()
        {
            if (HttpRuntime.Cache[AllowedPathsKey] is List<string> value) return value;

            value = GetValues("allowedPaths", "path");

            HttpRuntime.Cache.Insert(AllowedPathsKey, value, null);

            return value;
        }

        public void AppendAllowedPath(string absolutePathLowered)
        {
            List<string> allowedPaths = GetAllowedPaths();

            bool hasAbsolutePath = allowedPaths.Contains(absolutePathLowered);
            if (!hasAbsolutePath)
            {
                allowedPaths.Add(absolutePathLowered);
            }
        }

        public bool IsEnabled()
        {
            XDocument doc = GetDocument();
            if (doc?.Root == null) return false;

            XAttribute enabledAttribute = doc.Root.Attribute("enabled");

            return enabledAttribute == null || bool.Parse(enabledAttribute.Value);
        }

        private static List<string> GetValues(string containerName, string elementName)
        {
            XDocument doc = GetDocument();

            List<string> value = doc?.Root?.Element(containerName)?.Elements(elementName).Select(x => x.Value.Trim()).ToList();

            return value;
        }

        private static XDocument GetDocument()
        {
            XDocument doc = XDocument.Load(HostingEnvironment.MapPath("/config/SiteLock.config") ?? "");

            return doc;
        }
    }
}