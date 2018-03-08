using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Config;

namespace WcfService.Tests.Config
{
    [TestFixture]
    public class BootstrapTests
    {
        [Test]
        public void SuccessResolve()
        {
            var container = Bootstrap.Execute();
        }
    }
}
