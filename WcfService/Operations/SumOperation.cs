namespace WcfService.Services
{
    public class SumOperation : IOperation
    {
        public int Execute(int a, int b)
        {
            return a + b;
        }
    }
}