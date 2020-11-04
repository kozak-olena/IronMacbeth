using System.ServiceModel;

namespace IronMacbeth.Model.ToBeRemoved
{
    [ServiceContract(
    Name = "Service",
    Namespace = "Service",
    SessionMode = SessionMode.Required,
    CallbackContract = typeof(IServiceCallback))]

    public interface IService
    {
        [OperationContract]
        User LogIn(string login, string password);

        [OperationContract]
        void LogOut(User user);

        [OperationContract]
        byte[] GetAll(string typeName);

        [OperationContract]
        void Insert(byte[] item, string typeName);

        [OperationContract]
        void Delete(byte[] item);

        [OperationContract]
        void DeleteLink(byte[] item);

        [OperationContract]
        void Update(byte[] item);

        [OperationContract]
        void AddFile(byte[] file, out string fileName);

        [OperationContract]
        byte[] GetFile(string fileName);

        [OperationContract]
        bool Ping();
    }
}