using System;
using System.Collections.Generic;
using System.IO;
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
        string AddFile(Stream fileStream);

        [OperationContract]
        Stream GetFile(string fileName);

    
    }
}
