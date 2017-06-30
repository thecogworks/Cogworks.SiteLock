using Cogworks.SiteLock.Web.Configuration;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(Cogworks.SiteLock.Web.Events.ModuleLoader), "LoadModule")]
namespace Cogworks.SiteLock.Web.Events
{
    public class ModuleLoader
    {
        public static void LoadModule()
        {
            var config = new SiteLockConfiguration();

            if (config.IsEnabled())
            {
                HttpApplication.RegisterModule(typeof(Cogworks.SiteLock.Web.HttpModules.SiteLockModule));
            }
        }
    }
}
