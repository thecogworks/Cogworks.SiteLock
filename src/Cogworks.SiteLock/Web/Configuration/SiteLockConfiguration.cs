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
        List<string> GetIgnoredPaths();
    }

    public class SiteLockConfiguration : ISiteLockConfiguration
    {
        private static string DomainsKey = typeof(SiteLockConfiguration) + "_domains";
        private static string IgnorePathsKey = typeof(SiteLockConfiguration) + "_ignorePaths";


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



        public List<string> GetIgnoredPaths()
        {
            var value = HttpRuntime.Cache[IgnorePathsKey] as List<string>;

            if (value == null)
            {
                value = GetValues("ignoredPaths", "path");

                HttpRuntime.Cache.Insert(IgnorePathsKey, value, null);
            }

            return value;
        }



        private List<string> GetValues(string containerName, string elementName)
        {
            var doc = XDocument.Load(HostingEnvironment.MapPath("/config/SiteLock.config"));

            var value = doc.Root.Element(containerName).Elements(elementName).Select(x => x.Value).ToList();

            return value;
        }
    }
}