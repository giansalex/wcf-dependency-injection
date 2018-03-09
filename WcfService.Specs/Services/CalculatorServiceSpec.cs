using Machine.Specifications;
using WcfService.Services;
using Moq;
using FluentAssertions;

namespace WcfService.Specs.Services
{
    [Subject("Operation")]
    public abstract class CalculatorServiceSpec
    {
        Establish context = () => {
            var mock = new Mock<IOperation>();
            mock
                .Setup(r => r.Execute(Moq.It.IsAny<int>(), Moq.It.IsAny<int>()))
                .Returns((int a, int b) => a + b);

            Subject = new CalculatorService(mock.Object);
        };

        Because of = async () => {
            Token = await Subject.Operation(2, 4);
        };

        Machine.Specifications.It should_be_the_sum = () => {
            Token.Should().Be(20);
        };

        static CalculatorService Subject;
        static int Token;
    }
}
