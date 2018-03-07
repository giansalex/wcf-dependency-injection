using System.ServiceModel;
using System.Threading.Tasks;

namespace WcfService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICalculatorService 
    {

        [OperationContract]
        Task<int> Operation(int a, int b);
    }
}
