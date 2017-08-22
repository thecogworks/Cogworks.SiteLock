using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Cogworks.SiteLock.Web.Configuration
{
    public interface ISiteLockConfiguration
    {
        List<string> GetLockedDomains();
        List<string> GetAllowedPaths();
        void AppendAllowedPath(string absolutePathLowered);
        List<string> GetAllowedIPs();
    }

    public class SiteLockConfiguration : ISiteLockConfiguration
    {
        private static string DomainsKey = typeof(SiteLockConfiguration) + "_domains";
        private static string AllowedPathsKey = typeof(SiteLockConfiguration) + "_allowedPaths";
        private static string AllowedIpsKey = typeof(SiteLockConfiguration) + "_allowedIps";


        public List<string> GetAllowedIPs()
        {
            var value = HttpRuntime.Cache[AllowedIpsKey] as List<string>;

            if (value == null)
            {
                value = GetValues("allowedIps", "ip");

                HttpRuntime.Cache.Insert(AllowedIpsKey, value, null);
            }

            return value;
        }

        public List<string> GetLockedDomains()
        {
            var value = HttpRuntime.Cache[DomainsKey] as List<string>;

            if (value == null)
            {
                value = GetValues("lockedDomains", "domain");

                HttpRuntime.Cache.Insert(DomainsKey, value, null);
            }

            return value;
        }



        public List<string> GetAllowedPaths()
        {
            var value = HttpRuntime.Cache[AllowedPathsKey] as List<string>;

            if (value == null)
            {
                value = GetValues("allowedPaths", "path");

                HttpRuntime.Cache.Insert(AllowedPathsKey, value, null);
            }

            return value;
        }


        public void AppendAllowedPath(string absolutePathLowered)
        {
            var allowedPaths = GetAllowedPaths();

            var hasAbsolutePath = allowedPaths.Contains(absolutePathLowered);
            if (!hasAbsolutePath)
            {
                allowedPaths.Add(absolutePathLowered);
            }
        }


        public bool IsEnabled()
        {
            var doc = GetDocument();

            var enabledAttribute = doc.Root.Attribute("enabled");

            if (enabledAttribute == null)
            {
                return true;
            }

            return bool.Parse(enabledAttribute.Value);
        }



        private List<string> GetValues(string containerName, string elementName)
        {
            var doc = GetDocument();

            var value = doc.Root.Element(containerName).Elements(elementName).Select(x => x.Value.Trim()).ToList();

            return value;
        }


        private XDocument GetDocument()
        {
            var doc = XDocument.Load(HostingEnvironment.MapPath("/config/SiteLock.config"));

            return doc;
        }

     
    }
}