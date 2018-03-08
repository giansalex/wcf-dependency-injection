using NUnit.Framework;
using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Web;
using WcfService.Handler;

namespace WcfService.Tests.Handler
{
    [TestFixture]
    public class ElmahErrorHandlerTests
    {
        private readonly ElmahErrorHandler _handler = new ElmahErrorHandler();
        private Message _message = Message.CreateMessage(MessageVersion.Default, null);

        [Test]
        public void HandleErrorDisabled()
        {
            var result = _handler.HandleError(null);

            Assert.False(result);
        }

        [Test]
        public void ProvideFault()
        {
            _handler.ProvideFault(null, null, ref _message);
        }

        [Test]
        public void ProvideFaultWithHttpContext()
        {
            InitHttpContext();
            _handler.ProvideFault(new Exception(), null, ref _message);
        }

        private void InitHttpContext()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            )
            {
                ApplicationInstance = new HttpApplication()
            };
        }
    }
}
