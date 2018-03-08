using NUnit.Framework;
using WcfService.Config;
using Autofac;
using WcfService.Services;
using System;
using Autofac.Core.Registration;

namespace WcfService.Tests.Config
{
    [TestFixture]
    public class BootstrapTests
    {
        private IContainer _container;

        [SetUp]
        public void Init()
        {
            _container = Bootstrap.Execute();
        }

        [Test, TestCaseSource(nameof(GetArguments))]
        public void SuccessResolve(Type abstaction, Type implement)
        {
            var instance = _container.Resolve(abstaction);

            Assert.IsNotNull(instance);
            Assert.IsInstanceOf(implement, instance);
        }


        [Test]
        public void ErrorResolve()
        {
            Assert.Throws<ComponentNotRegisteredException>(() => _container.Resolve<BootstrapTests>());
        }

        public static object[] GetArguments()
        {
            return new[]
            {
                new object[] { typeof(IOperation), typeof(SumOperation) },
                new object[] { typeof(ICalculatorService), typeof(CalculatorService) }
            };
        }
    }
}
