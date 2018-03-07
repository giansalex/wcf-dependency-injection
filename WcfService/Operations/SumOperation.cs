using System;

namespace WcfService.Services
{
    public class SumOperation : IOperation
    {
        public int Execute(int a, int b)
        {
            if (a == 0)
            {
                throw new Exception("bad parameters");
            }
            return a + b;
        }
    }
}