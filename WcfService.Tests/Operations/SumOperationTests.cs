using NUnit.Framework;
using System;
using WcfService.Services;

namespace WcfService.Tests.Operations
{
    [TestFixture]
    public class SumOperationTests
    {
        private readonly SumOperation _operation = new SumOperation();

        [Test]
        public void ExecuteSuccess()
        {
            Assert.AreEqual(-4, _operation.Execute(1, -5));
            Assert.AreEqual(8, _operation.Execute(3, 5));
            Assert.AreNotEqual(1, _operation.Execute(1, 5));
        }

        [Test]
        public void ExecuteWithException()
        {
            Assert.Throws<ArgumentException>(() => _operation.Execute(1, 0));
            Assert.DoesNotThrow(() => _operation.Execute(0, 1));
        }
    }
}
