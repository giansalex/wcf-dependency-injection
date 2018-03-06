using NUnit.Framework;

namespace WcfService.Tests.Services
{
    [TestFixture]
    public partial class CalculatorServiceTests
    {
        static int[][] Numbers =
        {
            new [] { 2, 5 },
            new [] { 4, 2 },
            new [] { 8, 9 }
        };

        [Test]
        public void OperationAdd()
        {
            var service = CreateAdd();

            Assert.AreEqual(5, service.Operation(2, 3));
            Assert.AreNotEqual(4, service.Operation(1, 2));
        }

        [Test]
        public void OperationSubstract()
        {
            var service = CreateSubstract();

            Assert.AreEqual(3, service.Operation(5, 2));
        }

        [Test, TestCaseSource("Numbers")]
        public void OperationMultiply(int a, int b)
        {
            var service = CreateMultiply();

            Assert.AreEqual(a * b, service.Operation(a, b));
        }


        [Test]
        public void OperationPow()
        {
            var service = CreatePow();

            Assert.AreEqual(1, service.Operation(1, 2));
        }
    }
}
