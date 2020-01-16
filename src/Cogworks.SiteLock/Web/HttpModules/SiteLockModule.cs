using Cogworks.SiteLock.Web.Authentication;
using Cogworks.SiteLock.Web.Configuration;
using System;
using System.Web;

namespace Cogworks.SiteLock.Web.HttpModules
{
    public class SiteLockModule : IHttpModule
    {
        public void Dispose()
        {
        }

        private readonly RequestProcessor _requestProcessor;

        public SiteLockModule()
        {
            SiteLockConfiguration config = new SiteLockConfiguration();
            AuthenticationChecker authChecker = new AuthenticationChecker();
            _requestProcessor = new RequestProcessor(config, authChecker);
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
        }

        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = ((HttpApplication)sender);

            HttpContextWrapper httpContextWrapper = new HttpContextWrapper(app.Context);
            _requestProcessor.ProcessRequest(httpContextWrapper);
        }
    }
}