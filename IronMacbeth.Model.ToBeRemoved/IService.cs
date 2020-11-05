using System.Collections.Generic;
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
        #region Memory

        [OperationContract]
        void CreateMemory(Memory memory);

        [OperationContract]
        List<Memory> GetAllMemories();

        [OperationContract]
        void UpdateMemory(Memory memory);

        [OperationContract]
        void DeleteMemory(int id);

        #endregion

        #region Motherboard

        [OperationContract]
        void CreateMotherboard(Motherboard motherboard);

        [OperationContract]
        List<Motherboard> GetAllMotherboards();

        [OperationContract]
        void UpdateMotherboard(Motherboard motherboard);

        [OperationContract]
        void DeleteMotherboard(int id);

        #endregion

        #region Processor

        [OperationContract]
        void CreateProcessor(Processor processor);

        [OperationContract]
        List<Processor> GetAllProcessors();

        [OperationContract]
        void UpdateProcessor(Processor processor);

        [OperationContract]
        void DeleteProcessor(int id);

        #endregion

        #region Purchase

        [OperationContract]
        void CreatePurchase(Purchase purchase);

        [OperationContract]
        List<Purchase> GetAllPurchases();

        [OperationContract]
        void UpdatePurchase(Purchase purchase);

        [OperationContract]
        void DeletePurchase(int id);

        #endregion

        #region Store

        [OperationContract]
        void CreateStore(Store store);

        [OperationContract]
        List<Store> GetAllStores();

        [OperationContract]
        void UpdateStore(Store store);

        [OperationContract]
        void DeleteStore(int id);

        #endregion

        #region StoreMemory

        [OperationContract]
        void CreateStoreMemory(StoreMemory storeMemory);

        [OperationContract]
        List<StoreMemory> GetAllStoreMemories();

        [OperationContract]
        void UpdateStoreMemory(StoreMemory storeMemory);

        [OperationContract]
        void DeleteStoreMemory(int id);

        #endregion

        #region StoreMotherboard

        [OperationContract]
        void CreateStoreMotherboard(StoreMotherboard storeMotherboard);

        [OperationContract]
        List<StoreMotherboard> GetAllStoreMotherboards();

        [OperationContract]
        void UpdateStoreMotherboard(StoreMotherboard storeMotherboard);

        [OperationContract]
        void DeleteStoreMotherboard(int id);

        #endregion

        #region StoreProcessor

        [OperationContract]
        void CreateStoreProcessor(StoreProcessor storeProcessor);

        [OperationContract]
        List<StoreProcessor> GetAllStoreProcessors();

        [OperationContract]
        void UpdateStoreProcessor(StoreProcessor storeProcessor);

        [OperationContract]
        void DeleteStoreProcessor(int id);

        #endregion

        #region StoreVideocard

        [OperationContract]
        void CreateStoreVideocard(StoreVideocard storeVideocard);

        [OperationContract]
        List<StoreVideocard> GetAllStoreVideoCards();

        [OperationContract]
        void UpdateStoreVideocard(StoreVideocard storeVideocard);

        [OperationContract]
        void DeleteStoreVideocard(int id);

        #endregion

        #region User

        [OperationContract]
        void Register(User user);

        [OperationContract]
        User LogIn(string login, string password);

        #endregion

        #region Videocard

        [OperationContract]
        void CreateVideoCard(Videocard videocard);

        [OperationContract]
        List<Videocard> GetAllVideoCards();

        [OperationContract]
        void UpdateVideoCard(Videocard videocard);

        [OperationContract]
        void DeleteVideoCard(int id);

        #endregion


        //[OperationContract]
        //byte[] GetAll(string typeName);

        //[OperationContract]
        //void Insert(byte[] item, string typeName);

        //[OperationContract]
        //void Delete(byte[] item);

        //[OperationContract]
        //void DeleteLink(byte[] item);

        //[OperationContract]
        //void Update(byte[] item);

        [OperationContract]
        void AddFile(byte[] file, out string fileName);

        [OperationContract]
        byte[] GetFile(string fileName);

        [OperationContract]
        bool Ping();
    }
}