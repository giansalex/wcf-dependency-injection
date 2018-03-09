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
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Assert.NotNull(method);
            Assert.Null(AutofacHostFactory.Container);

            method.Invoke(new Global(), new object[] { null, null});

            Assert.NotNull(AutofacHostFactory.Container);
        }
    }
}
