using System;

namespace WcfService.Services
{
    public class SumOperation : IOperation
    {
        public int Execute(int a, int b)
        {
            EnsuerNotZero(b);

            return a + b;
        }

        private void EnsuerNotZero(int value)
        {
            if (value == 0)
            {
                throw new Exception("No allow zero for second parameter");
            }
        }
    }
}