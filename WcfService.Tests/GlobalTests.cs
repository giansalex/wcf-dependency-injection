using Autofac.Integration.Wcf;
using NUnit.Framework;
using System.Reflection;

namespace WcfService.Tests
{
    [TestFixture]
    public class GlobalTests
    {
        [Test]
        public void Start()
        {
            var method = typeof(Global).GetMethod("Application_Start",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Assert.NotNull(method);
            Assert.Null(AutofacHostFactory.Container);

            method.Invoke(null, new object[] { null, null});

            Assert.NotNull(AutofacHostFactory.Container);
        }
    }
}
