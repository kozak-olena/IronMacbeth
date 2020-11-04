using System.ServiceModel;

namespace IronMacbeth.Model.ToBeRemoved
{
    [ServiceContract]
    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyUserJoined(string userName);

        [OperationContract(IsOneWay = true)]
        void NotifyNewMessage(string message);

        [OperationContract(IsOneWay = true)]
        void NotifyNewPurchase(Notification notification);
    }
}