using System;
using System.ServiceModel;

namespace IronMacbeth.UserManagement.Contract
{
    [ServiceContract]
    public interface IUserManagementService
    {
        [OperationContract]
        DateTime GetCurrentTime();
    }
}
