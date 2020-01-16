using System.Collections.Generic;

namespace Cogworks.SiteLock.Web.Configuration
{
    /// <summary>
    /// SiteLock.config keys
    /// </summary>
    public interface ISiteLockConfiguration
    {
        /// <summary>
        /// Locks on these domains
        /// </summary>
        List<string> GetLockedDomains();

        /// <summary>
        /// Never lock site on these domains
        /// </summary>
        /// <returns></returns>
        List<string> GetAllowedPaths();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="absolutePathLowered"></param>
        void AppendAllowedPath(string absolutePathLowered);

        /// <summary>
        /// Never lock for these IPs
        /// </summary>
        List<string> GetAllowedIPs();
    }
}