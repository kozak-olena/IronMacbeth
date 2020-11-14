using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace IronMacbeth.BFF.Contract
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        DocumentsSearchResults SearchDocuments(SearchFilledFields searchFilledFields);

        #region Book

        [OperationContract]
        void CreateBook(Book book);

         

        [OperationContract]
        List<Book> GetAllBooks();

        [OperationContract]
        void UpdateBook(Book book);

        [OperationContract]
        void DeleteBook(int id);

        #endregion

        #region Article
        [OperationContract]
        void CreateArticle(Article article);

        [OperationContract]
        List<Article> GetAllArticles();

        [OperationContract]
        void UpdateArticle(Article article);

        [OperationContract]
        void DeleteArticle(int id);
        #endregion

        #region Periodical

        [OperationContract]
        void CreatePeriodical(Periodical periodical);

        [OperationContract]
        List<Periodical> GetAllPeriodicals();

        [OperationContract]
        void UpdatePeriodical(Periodical periodical);

        [OperationContract]
        void DeletePeriodical(int id);
        #endregion

        #region Thesis

        [OperationContract]
        void CreateThesis(Thesis thesis);

        [OperationContract]
        List<Thesis> GetAllThesises();

        [OperationContract]
        void UpdateThesis(Thesis thesis);

        [OperationContract]
        void DeleteThesis(int id);
        #endregion

        #region Newspaper

        [OperationContract]
        void CreateNewspaper(Newspaper newspaper);

        [OperationContract]
        List<Newspaper> GetAllNewspapers();

        [OperationContract]
        void UpdateNewspaper(Newspaper newspaper);

        [OperationContract]
        void DeleteNewspaper(int id);
        #endregion

        #region StoreBook

        [OperationContract]
        void CreateStoreBook(StoreBook storeBooks);

        [OperationContract]
        List<StoreBook> GetAllStoreBooks();

        [OperationContract]
        void UpdateStoreBooks(StoreBook storeBooks);

        [OperationContract]
        void DeleteStoreBook(int id);

        #endregion

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

        [OperationContract]
        string AddFile(Stream fileStream);

        [OperationContract]
        Stream GetFile(string fileName);

        [OperationContract]
        bool Ping();
    }
}