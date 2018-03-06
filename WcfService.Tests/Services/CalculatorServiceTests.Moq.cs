using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Services;

namespace WcfService.Tests.Services
{
    public partial class CalculatorServiceTests
    {
        private ICalculatorService CreateAdd()
        {
            return GetByOperation((a, b) => a + b);
        }

        private ICalculatorService CreateSubstract()
        {
            return GetByOperation((a, b) => a - b);
        }

        private ICalculatorService CreateMultiply()
        {
            return GetByOperation((a, b) => a * b);
        }

        private ICalculatorService CreatePow()
        {
            return GetByOperation((a, b) => (int)Math.Pow(a, b));
        }


        private ICalculatorService GetByOperation(Func<int, int, int> operation)
        {
            var mock = new Mock<IOperation>();
            mock
                .Setup(r => r.Execute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(operation);

            return new CalculatorService(mock.Object);
        }
    }
}
