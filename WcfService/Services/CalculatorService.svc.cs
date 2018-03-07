using WcfService.Handler;
using WcfService.Behaviour;

namespace WcfService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceErrorBehaviour(typeof(HttpErrorHandler))]
    public class CalculatorService : ICalculatorService
    {
        private readonly IOperation _operation;

        public CalculatorService(IOperation operation)
        {
            _operation = operation;
        }

        public int Operation(int a, int b)
        {
            return _operation.Execute(a, b);
        }
    }
}
