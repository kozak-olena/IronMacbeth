using System.ServiceModel;

namespace IronMacbeth.BFF.Contract
{
    [ServiceContract]
    public interface IAnonymousService
    {
        [OperationContract]
        UserRegistrationStatus Register(string login, string password);

        [OperationContract]
        bool Ping();
    }
}
