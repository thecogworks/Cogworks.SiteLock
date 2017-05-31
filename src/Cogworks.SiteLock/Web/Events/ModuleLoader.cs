using System.Web;

[assembly: PreApplicationStartMethod(typeof(Cogworks.SiteLock.Web.Events.ModuleLoader), "LoadModule")]
namespace Cogworks.SiteLock.Web.Events
{
    public class ModuleLoader
    {
        public static void LoadModule()
        {
            HttpApplication.RegisterModule(typeof(Cogworks.SiteLock.Web.HttpModules.SiteLockModule));
        }
    }
}
