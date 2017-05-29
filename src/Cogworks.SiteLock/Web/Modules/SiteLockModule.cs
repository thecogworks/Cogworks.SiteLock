using Cogworks.SiteLock.Web.Authentication;
using Cogworks.SiteLock.Web.Configuration;
using System;
using System.Web;


namespace Cogworks.SiteLock.Web.Modules
{
    public class SiteLockModule : IHttpModule
    {
        public void Dispose() { }

        private readonly RequestProcessor _requestProcessor;

        public SiteLockModule()
        {
            var config = new SiteLockConfiguration();
            var authChecker = new AuthenticationChecker();
            _requestProcessor = new RequestProcessor(config, authChecker);
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
        }


        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var app = ((HttpApplication)sender);

            var httpContextWrapper = new HttpContextWrapper(app.Context);
            _requestProcessor.ProcessRequest(httpContextWrapper);
        }
    }
}
