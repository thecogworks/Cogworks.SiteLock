using System;
using System.Collections.Generic;
using Cogworks.SiteLock.Web.HttpModules;
using Moq;
using Cogworks.SiteLock.Web.Configuration;
using System.Web;
using Xunit;
using Cogworks.SiteLock.Web.Authentication;

namespace Cogworks.SiteLock.Test
{

    public class When_Processing_Request
    {
        // constants
        const string AbsolutePath = "/my-page/";

        RequestProcessor _requestProcessor;

        // mocks
        Mock<ISiteLockConfiguration> _configMock;
        Mock<HttpContextBase> _contextMock;
        Mock<HttpRequestBase> _httpRequestMock;
        Mock<HttpResponseBase> _httpResponseMock;
        Mock<IAuthenticationChecker> _authCheckerMock;
        Uri _uriStub;


        public When_Processing_Request()
        {
            _configMock = new Mock<ISiteLockConfiguration>();
            _configMock.Setup(x => x.GetAllowedPaths()).Returns(new List<string>());

            _authCheckerMock = new Mock<IAuthenticationChecker>();
            _requestProcessor = new RequestProcessor(_configMock.Object, _authCheckerMock.Object);
            _contextMock = new Mock<HttpContextBase>();

            _httpRequestMock = new Mock<HttpRequestBase>();
            _httpResponseMock = new Mock<HttpResponseBase>();

            _uriStub = new Uri("http://thecogworks.com" + AbsolutePath);
            _httpRequestMock.Setup(x => x.Url).Returns(_uriStub);

            _httpRequestMock.Setup(x => x.UserHostAddress).Returns("8.8.8.8");

            _contextMock.Setup(x => x.Request).Returns(_httpRequestMock.Object);
            _contextMock.Setup(x => x.Response).Returns(_httpResponseMock.Object);
        }



        [Fact]
        public void IP_Exists_In_Configuration_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string> { "8.8.8.8" });

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }



        [Fact]
        public void AbsolutePath_Exists_In_Configuration_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string>());
            _configMock.Setup(x => x.GetAllowedPaths()).Returns(new List<string> { AbsolutePath });

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }


        [Fact]
        public void Referrer_Is_Allowed_Path_Then_Append_AbsolutePath_So_That_Linked_Assets_Can_Be_Served_And_Then_Allow_Request_To_Continue()
        {
            const string cssPath = "/css/main.css";
            const string allowedPath = "/an-allowed-path/";

            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _configMock.Setup(x => x.GetAllowedPaths()).Returns(new List<string> { allowedPath });

            _httpRequestMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://thecogworks.com" + allowedPath));

            _uriStub = new Uri("http://thecogworks.com" + cssPath);
            _httpRequestMock.Setup(x => x.Url).Returns(_uriStub);

            _requestProcessor.ProcessRequest(_contextMock.Object);

            _configMock.Verify(x => x.AppendAllowedPath(cssPath));
        }


        [Fact]
        public void Wildcard_Exists_In_Configuration_And_User_Not_LoggedIn_Then_Throw_Exception()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "*" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(false);



            Assert.Throws<HttpException>(() => _requestProcessor.ProcessRequest(_contextMock.Object));
            _httpResponseMock.VerifySet(x => x.StatusCode = 403);
        }


        [Fact]
        public void Wildcard_Exists_In_Configuration_And_User_Is_LoggedIn_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "*" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(true);

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }


        [Fact]
        public void Domain_Exists_In_Configuration_And_User_Not_LoggedIn_Then_Throw_Exception()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(false);

            Assert.Throws<HttpException>(() => _requestProcessor.ProcessRequest(_contextMock.Object));
            _httpResponseMock.VerifySet(x => x.StatusCode = 403);
        }


        [Fact]
        public void Domain_Exists_In_Configuration_And_User_Is_LoggedIn_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(true);

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }


        [Fact]
        public void Url_Is_Umbraco_Url_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(true);

            var absoluteUrl = new Uri("http://thecogworks.com/dependencyhandler.axd");
            _httpRequestMock.Setup(x => x.Url).Returns(absoluteUrl);

            _httpRequestMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://thecogworks.com/umbraco/"));

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }


        [Fact]
        public void Referrer_Is_DependencyHandler_Then_Allow_Request_To_Continue()
        {
            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());

            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(true);

            var absoluteUrl = new Uri("http://thecogworks.com/umbraco/logo.png");
            _httpRequestMock.Setup(x => x.Url).Returns(absoluteUrl);

            _httpRequestMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://thecogworks.com/dependencyhandler.axd"));

            _requestProcessor.ProcessRequest(_contextMock.Object);
        }


        [Fact]
        public void Is_Umbraco_Api_Url_And_User_Not_LoggedIn_Then_Throw_Exception()
        {
            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(true);

            var absoluteUrl = new Uri("http://thecogworks.com/umbraco/api/");
            _httpRequestMock.Setup(x => x.Url).Returns(absoluteUrl);

            _configMock.Setup(x => x.GetLockedDomains()).Returns(new List<string> { "thecogworks.com" });
            _configMock.Setup(x => x.GetAllowedIPs()).Returns(new List<string>());
            _authCheckerMock.Setup(x => x.IsAuthenticated(_contextMock.Object)).Returns(false);

            Assert.Throws<HttpException>(() => _requestProcessor.ProcessRequest(_contextMock.Object));
            _httpResponseMock.VerifySet(x => x.StatusCode = 403);
        }
    }
}
