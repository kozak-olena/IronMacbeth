using System.IO;
using System.ServiceModel;

namespace IronMacbeth.FileStorage.Contract
{
    [ServiceContract]
    public interface IFileStorageService
    {
        [OperationContract]
        string AddFile(Stream fileStream);

        [OperationContract]
        Stream GetFile(string fileName);
    }
}
