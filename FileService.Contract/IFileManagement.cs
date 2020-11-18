using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.FileManagement.Contract
{
    [ServiceContract]
    public interface IFileManagement
    {
        [OperationContract]
        int Calculate(int i, int a);
    }
}
