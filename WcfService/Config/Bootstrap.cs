using Autofac;
using WcfService.Services;

namespace WcfService.Config
{
    public static class Bootstrap
    {
        public static IContainer Execute()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<CalculatorService>()
                 .As<ICalculatorService>();

            builder
                .RegisterType<SumOperation>()
                .As<IOperation>()
                .SingleInstance();
            

            return builder.Build();
        }
    }
}