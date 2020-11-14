using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;
using IronMacbeth.BFF.Contract;
using Internal = IronMacbeth.Client;
using Contract = IronMacbeth.BFF.Contract;

namespace IronMacbeth.Client
{
    class ServerAdapter
    {
        private static ServerAdapter _instance;
        public static ServerAdapter Instance => _instance ?? throw new Exception($"{nameof(ServerAdapter)} was not initialized. Use '{nameof(Initialize)}' method.");

        public static void Initialize(IService proxy)
        {
            if (_instance != null)
            {
                throw new Exception($"{nameof(ServerAdapter.Instance)} was already initialized.");
            }

            _instance = new ServerAdapter(proxy);
        }

        public static void ClearInstance()
        {
            _instance = null;
        }

        private IService _proxy;

        private ServerAdapter(IService proxy)
        {
            _proxy = proxy;
        }

        public DocumentsSearchResults SearchDocuments(Internal.SearchFilledFields searchFilledFields)
        {
            var contractSearch = MapInternalToContractSearchCriteria(searchFilledFields);
            var documents = _proxy.SearchDocuments(contractSearch);
            var internalBooks = MapContractToInternalSearchCriteria(documents);
            return internalBooks;
        }

        private Contract.SearchFilledFields MapInternalToContractSearchCriteria(Internal.SearchFilledFields searchFilledFields)
        {
            return new Contract.SearchFilledFields
            {
                SearchName = searchFilledFields.SearchName,
                SearchAuthor = searchFilledFields.SearchAuthor,
                SearchYearFrom = searchFilledFields.SearchYearFrom,
                SearchYearTo = searchFilledFields.SearchYearTo,
                IsArticleSelected = searchFilledFields.IsArticleSelected,
                IsBookSelected = searchFilledFields.IsBookSelected,
                IsNewspaperSelected = searchFilledFields.IsNewspaperSelected,
                IsPeriodicalSelected = searchFilledFields.IsPeriodicalSelected,
                IsThesisSelected = searchFilledFields.IsThesisSelected

            };
        }

        private Internal.DocumentsSearchResults MapContractToInternalSearchCriteria(Contract.DocumentsSearchResults documentsSearchResults)
        {
            var documents = new Internal.DocumentsSearchResults();

            documents.Books = documentsSearchResults.Books?.Select(MapContractToInternalBook).ToList() ?? new List<Book>();
            foreach (var book in documents.Books)
            {
                if (book.ImageName != null) { book.BitmapImage = GetImage(book.ImageName); }
                if (book.DescriptionName != null)
                { book.Description = GetStringFileContent(book.DescriptionName); }

            }

            documents.Articles = documentsSearchResults.Articles?.Select(MapContractToInternalArticle).ToList() ?? new List<Article>();
            documents.Periodicals = documentsSearchResults.Periodicals?.Select(MapContractToInternalPeriodical).ToList() ?? new List<Periodical>();
            foreach (var periodical in documents.Periodicals)
            {
                if (periodical.ImageName != null) { periodical.BitmapImage = GetImage(periodical.ImageName); }
                if (periodical.DescriptionName != null)
                { periodical.Description = GetStringFileContent(periodical.DescriptionName); }
            }
            documents.Newspapers = documentsSearchResults.Newspapers?.Select(MapContractToInternalNewspaper).ToList() ?? new List<Newspaper>();
            documents.Theses = documentsSearchResults.Theses?.Select(MapContractToInternalPeriodical).ToList() ?? new List<Thesis>();

            return documents;
        }

        #region Periodical

        public void CreatePeriodical(Internal.Periodical periodical)
        {
            CreateIDisplayable(periodical);
            CreateIDescribable(periodical);

            if (periodical.ElectronicVersion != null)
            {
                string fileName;
                AddFile(periodical.ElectronicVersion, out fileName);
                periodical.ElectronicVersionFileName = fileName;
            }

            var contractPeriodical = MapInternalToContractPeriodical(periodical);

            _proxy.CreatePeriodical(contractPeriodical);
        }

        public List<Internal.Periodical> GetAllPeriodicals()
        {
            var periodicals = _proxy.GetAllPeriodicals();

            var internalPeriodicals = periodicals.Select(MapContractToInternalPeriodical).ToList();

            foreach (var periodical in internalPeriodicals)
            {
                if (periodical.ImageName != null) { periodical.BitmapImage = GetImage(periodical.ImageName); }
                if (periodical.DescriptionName != null)
                { periodical.Description = GetStringFileContent(periodical.DescriptionName); }

            }

            return internalPeriodicals;
        }
        public void UpdatePeriodical(Internal.Periodical periodical)
        {
            UpdateIDisplayable(periodical);
            UpdateIDescribable(periodical);

            var contractPeriodical = MapInternalToContractPeriodical(periodical);

            _proxy.UpdatePeriodical(contractPeriodical);
        }

        public void DeletePeriodical(int id)
        {
            _proxy.DeletePeriodical(id);
        }

        private Contract.Periodical MapInternalToContractPeriodical(Internal.Periodical periodical)
        {
            return new Contract.Periodical
            {
                Id = periodical.Id,
                Name = periodical.Name,
                Year = periodical.Year,
                Pages = periodical.Pages,
                City = periodical.City,
                Location = periodical.Location,
                PublishingHouse = periodical.PublishingHouse,
                Availiability = periodical.Availiability,
                TypeOfDocument = periodical.TypeOfDocument,
                IssueNumber = periodical.IssueNumber,
                Responsible = periodical.Responsible,
                RentPrice = periodical.RentPrice,
                ElectronicVersionPrice = periodical.ElectronicVersionPrice,
                ElectronicVersionFileName = periodical.ElectronicVersionFileName,
                Rating = periodical.Rating,
                Comments = periodical.Comments,
                ImageName = periodical.ImageName,
                DescriptionName = periodical.DescriptionName
            };
        }

        private Internal.Periodical MapContractToInternalPeriodical(Contract.Periodical periodical)
        {
            return new Internal.Periodical
            {
                Id = periodical.Id,
                Name = periodical.Name,
                Year = periodical.Year,
                Pages = periodical.Pages,
                Availiability = periodical.Availiability,
                City = periodical.City,
                PublishingHouse = periodical.PublishingHouse,
                Location = periodical.Location,
                TypeOfDocument = periodical.TypeOfDocument,
                ElectronicVersionPrice = periodical.ElectronicVersionPrice,
                ElectronicVersionFileName = periodical.ElectronicVersionFileName,
                IssueNumber = periodical.IssueNumber,
                Responsible = periodical.Responsible,
                RentPrice = periodical.RentPrice,
                Rating = periodical.Rating,
                Comments = periodical.Comments,
                ImageName = periodical.ImageName,
                DescriptionName = periodical.DescriptionName
            };
        }

        #endregion

        #region Newspaper

        public void CreateNewspaper(Internal.Newspaper newspaper)
        {
            CreateIDisplayable(newspaper);
            CreateIDescribable(newspaper);
            string fileName;
            AddFile(newspaper.ElectronicVersion, out fileName);
            newspaper.ElectronicVersionFileName = fileName;

            var contractNewspaper = MapInternalToContractNewspaper(newspaper);

            _proxy.CreateNewspaper(contractNewspaper);
        }

        public List<Internal.Newspaper> GetAllNewspapers()
        {
            var newspapers = _proxy.GetAllNewspapers();

            var internalNewspapers = newspapers.Select(MapContractToInternalNewspaper).ToList();

            foreach (var newspaper in internalNewspapers)
            {
                if (newspaper.ImageName != null) { newspaper.BitmapImage = GetImage(newspaper.ImageName); }
                if (newspaper.DescriptionName != null)
                { newspaper.Description = GetStringFileContent(newspaper.DescriptionName); }
            }
            return internalNewspapers;
        }
        public void UpdateNewspaper(Internal.Newspaper newspaper)
        {
            UpdateIDisplayable(newspaper);
            UpdateIDescribable(newspaper);

            var contractNewspaper = MapInternalToContractNewspaper(newspaper);

            _proxy.UpdateNewspaper(contractNewspaper);
        }

        public void DeleteNewspaper(int id)
        {
            _proxy.DeleteNewspaper(id);
        }

        private Contract.Newspaper MapInternalToContractNewspaper(Internal.Newspaper newspaper)
        {
            return new Contract.Newspaper
            {
                Id = newspaper.Id,
                Name = newspaper.Name,
                Year = newspaper.Year,
                City = newspaper.City,
                Availiability = newspaper.Availiability,
                TypeOfDocument = newspaper.TypeOfDocument,
                IssueNumber = newspaper.IssueNumber,
                RentPrice = newspaper.RentPrice,
                Location = newspaper.Location,
                ElectronicVersionFileName = newspaper.ElectronicVersionFileName,
                Rating = newspaper.Rating,
                ElectronicVersionPrice = newspaper.ElectronicVersionPrice,
                Comments = newspaper.Comments,
                //ImageName = periodical.ImageName,
                // DescriptionName = periodical.DescriptionName
            };
        }

        private Internal.Newspaper MapContractToInternalNewspaper(Contract.Newspaper newspaper)
        {
            return new Internal.Newspaper
            {
                Id = newspaper.Id,
                Name = newspaper.Name,
                Year = newspaper.Year,
                City = newspaper.City,
                Availiability = newspaper.Availiability,
                TypeOfDocument = newspaper.TypeOfDocument,
                ElectronicVersionPrice = newspaper.ElectronicVersionPrice,
                ElectronicVersionFileName = newspaper.ElectronicVersionFileName,
                IssueNumber = newspaper.IssueNumber,
                Location = newspaper.Location,
                RentPrice = newspaper.RentPrice,
                Rating = newspaper.Rating,
                Comments = newspaper.Comments,
                // ImageName = periodical.ImageName,
                // DescriptionName = periodical.DescriptionName
            };
        }
        #endregion

        #region Thesis

        public void CreateThesis(Internal.Thesis thesis)
        {
            CreateIDisplayable(thesis);
            CreateIDescribable(thesis);
            string fileName;
            AddFile(thesis.ElectronicVersion, out fileName);
            thesis.ElectronicVersionFileName = fileName;

            var contractThesis = MapInternalToContractThesis(thesis);

            _proxy.CreateThesis(contractThesis);
        }

        public List<Internal.Thesis> GetAllThesises()
        {
            var thesises = _proxy.GetAllThesises();

            var internalThesises = thesises.Select(MapContractToInternalPeriodical).ToList();

            foreach (var thesis in internalThesises)
            {
                if (thesis.ImageName != null) { thesis.BitmapImage = GetImage(thesis.ImageName); }
                if (thesis.DescriptionName != null)
                { thesis.Description = GetStringFileContent(thesis.DescriptionName); }

            }

            return internalThesises;
        }
        public void UpdateThesis(Internal.Thesis thesis)
        {
            UpdateIDisplayable(thesis);
            UpdateIDescribable(thesis);

            var contractThesis = MapInternalToContractThesis(thesis);

            _proxy.UpdateThesis(contractThesis);
        }

        public void DeleteThesis(int id)
        {
            _proxy.DeleteThesis(id);
        }

        private Contract.Thesis MapInternalToContractThesis(Internal.Thesis thesis)
        {
            return new Contract.Thesis
            {
                Id = thesis.Id,
                Name = thesis.Name,
                Year = thesis.Year,
                Author = thesis.Author,
                Pages = thesis.Pages,
                City = thesis.City,

                Availiability = thesis.Availiability,
                TypeOfDocument = thesis.TypeOfDocument,
                Responsible = thesis.Responsible,
                ElectronicVersionPrice = thesis.ElectronicVersionPrice,
                ElectronicVersionFileName = thesis.ElectronicVersionFileName,
                Rating = thesis.Rating,
                Comments = thesis.Comments,
                // ImageName = periodical.ImageName,
                // DescriptionName = periodical.DescriptionName
            };
        }

        private Internal.Thesis MapContractToInternalPeriodical(Contract.Thesis thesis)
        {
            return new Internal.Thesis
            {
                Id = thesis.Id,
                Name = thesis.Name,
                Year = thesis.Year,
                Author = thesis.Author,
                City = thesis.City,
                Pages = thesis.Pages,
                Availiability = thesis.Availiability,
                TypeOfDocument = thesis.TypeOfDocument,
                Responsible = thesis.Responsible,
                ElectronicVersionPrice = thesis.ElectronicVersionPrice,
                ElectronicVersionFileName = thesis.ElectronicVersionFileName,
                Rating = thesis.Rating,
                Comments = thesis.Comments,
                //ImageName = periodical.ImageName,
                //DescriptionName = periodical.DescriptionName
            };
        }

        #endregion

        #region Article

        public void CreateArticle(Internal.Article article)
        {
            //CreateIDisplayable(article);
            //CreateIDescribable(article);
            string fileName;
            if (article.ElectronicVersion != null)
            {
                AddFile(article.ElectronicVersion, out fileName);
                article.ElectronicVersionFileName = fileName;
            }

            var contractArticle = MapInternalToContractArticle(article);

            _proxy.CreateArticle(contractArticle);
        }

        public List<Internal.Article> GetAllArticles()
        {
            var articles = _proxy.GetAllArticles();

            var internalArticles = articles.Select(MapContractToInternalArticle).ToList();

            foreach (var article in internalArticles)
            {
                if (article.ImageName != null) { article.BitmapImage = GetImage(article.ImageName); }
                //if (article.DescriptionName != null)
                //{ article.Description = GetStringFileContent(article.DescriptionName); }

            }

            return internalArticles;
        }

        private Contract.Article MapInternalToContractSearchCriteria(Internal.Article article)
        {
            return new Contract.Article
            {
            };
        }



        public void UpdateArticle(Internal.Article article)
        {
            UpdateIDisplayable(article);
            UpdateIDescribable(article);

            var contractArticle = MapInternalToContractArticle(article);

            _proxy.UpdateArticle(contractArticle);
        }

        public void DeleteArticle(int id)
        {
            _proxy.DeleteArticle(id);
        }

        private Contract.Article MapInternalToContractArticle(Internal.Article article)
        {
            return new Contract.Article
            {
                Id = article.Id,
                Name = article.Name,
                Author = article.Author,
                Year = article.Year,
                Pages = article.Pages,
                MainDocumentId = article.MainDocumentId,
                Availiability = article.Availiability,
                TypeOfDocument = article.TypeOfDocument,
                ElectronicVersionPrice = article.ElectronicVersionPrice,
                ElectronicVersionFileName = article.ElectronicVersionFileName,
                Rating = article.Rating,
                Comments = article.Comments,
                /// ImageName = article.ImageName,
                // DescriptionName = article.DescriptionName
            };
        }




        private Internal.Article MapContractToInternalArticle(Contract.Article article)
        {
            return new Internal.Article
            {
                Id = article.Id,
                Name = article.Name,
                Author = article.Author,
                Year = article.Year,
                Pages = article.Pages,
                MainDocumentId = article.MainDocumentId,
                Availiability = article.Availiability,
                TypeOfDocument = article.TypeOfDocument,
                ElectronicVersionPrice = article.ElectronicVersionPrice,
                ElectronicVersionFileName = article.ElectronicVersionFileName,
                Rating = article.Rating,
                Comments = article.Comments,
                // ImageName = article.ImageName,
                // DescriptionName = article.DescriptionName
            };
        }

        #endregion

        #region Book

        public void CreateBook(Internal.Book book)
        {
            CreateIDisplayable(book);
            CreateIDescribable(book);
            string fileName;
            if (book.ElectronicVersion != null)
            {
                AddFile(book.ElectronicVersion, out fileName);
                book.ElectronicVersionFileName = fileName;
            }

            var contractBook = MapInternalToContractBook(book);

            _proxy.CreateBook(contractBook);
        }

        public List<Internal.Book> GetAllBooks()
        {
            var books = _proxy.GetAllBooks();

            var internalBooks = books.Select(MapContractToInternalBook).ToList();

            foreach (var book in internalBooks)
            {
                if (book.ImageName != null) { book.BitmapImage = GetImage(book.ImageName); }
                if (book.DescriptionName != null)
                { book.Description = GetStringFileContent(book.DescriptionName); }

            }

            return internalBooks;
        }





        public void UpdateBook(Internal.Book book)
        {
            UpdateIDisplayable(book);
            UpdateIDescribable(book);

            var contractBook = MapInternalToContractBook(book);

            _proxy.UpdateBook(contractBook);
        }

        public void DeleteBook(int id)
        {
            _proxy.DeleteBook(id);
        }

        private Contract.Book MapInternalToContractBook(Internal.Book book)
        {
            return new Contract.Book
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                PublishingHouse = book.PublishingHouse,
                City = book.City,
                Year = book.Year,
                Pages = book.Pages,                               //TODO:add more attributes
                Availiability = book.Availiability,
                Location = book.Location,
                TypeOfDocument = book.TypeOfDocument,
                RentPrice = book.RentPrice,
                ElectronicVersionFileName = book.ElectronicVersionFileName,
                ElectronicVersionPrice = book.ElectronicVersionPrice,

                Rating = book.Rating,
                Comments = book.Comments,
                ImageName = book.ImageName,
                DescriptionName = book.DescriptionName
            };
        }

        private Internal.Book MapContractToInternalBook(Contract.Book book)
        {
            return new Internal.Book
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                PublishingHouse = book.PublishingHouse,              //TODO: add more attributes
                City = book.City,
                Year = book.Year,
                Pages = book.Pages,
                Availiability = book.Availiability,
                Location = book.Location,
                TypeOfDocument = book.TypeOfDocument,
                ElectronicVersionPrice = book.ElectronicVersionPrice,
                RentPrice = book.RentPrice,
                ElectronicVersionFileName = book.ElectronicVersionFileName,
                Rating = book.Rating,
                Comments = book.Comments,
                ImageName = book.ImageName,
                DescriptionName = book.DescriptionName
            };
        }

        #endregion

        #region StoreBook

        public void CreateStoreBook(Internal.StoreBook storeBooks)
        {
            _proxy.CreateStoreBook(MapInternalToContractStoreBook(storeBooks));
        }

        public List<Internal.StoreBook> GetAllStoreBooks()
        {
            var result = _proxy.GetAllStoreBooks();

            return result.Select(MapContractToInternalStoreBook).ToList();
        }

        public void UpdateStoreBook(Internal.StoreBook storeBooks)
        {
            _proxy.UpdateStoreBooks(MapInternalToContractStoreBook(storeBooks));
        }

        public void DeleteStoreBook(int id)
        {
            _proxy.DeleteStoreBook(id);
        }

        public Internal.Book GetBookFromStoreBook(Internal.StoreBook storeBooks)
        {
            return GetAllBooks().Find(item => item.Id == storeBooks.BookId);
        }

        private Contract.StoreBook MapInternalToContractStoreBook(Internal.StoreBook storeBook)
        {
            return new Contract.StoreBook
            {
                Id = storeBook.Id,
                StoreId = storeBook.StoreId,
                BookId = storeBook.BookId,
                UserId = storeBook.UserId,
                ProductPrice = storeBook.ProductPrice
            };
        }

        private Internal.StoreBook MapContractToInternalStoreBook(Contract.StoreBook storeBook)
        {
            return new Internal.StoreBook
            {
                Id = storeBook.Id,
                StoreId = storeBook.StoreId,
                BookId = storeBook.BookId,
                UserId = storeBook.UserId,
                ProductPrice = storeBook.ProductPrice
            };
        }

        #endregion

        #region Memory

        public void CreateMemory(Internal.Memory memory)
        {
            CreateIDisplayable(memory);
            CreateIDescribable(memory);

            _proxy.CreateMemory(MapInternalToContractMemory(memory));
        }

        public List<Internal.Memory> GetAllMemories()
        {
            var memories = _proxy.GetAllMemories().Select(MapContractToInternalMemory).ToList();

            foreach (var memory in memories)
            {
                memory.BitmapImage = GetImage(memory.ImageName);
                memory.Description = GetStringFileContent(memory.DescriptionName);
            }

            return memories;
        }

        public void UpdateMemory(Internal.Memory memory)
        {
            UpdateIDisplayable(memory);
            UpdateIDescribable(memory);

            _proxy.UpdateMemory(MapInternalToContractMemory(memory));
        }

        public void DeleteMemory(int id)
        {
            _proxy.DeleteMemory(id);
        }

        public List<(Internal.Store Store, Internal.StoreMemory StoreMemory)> GetAllStoresSellingMemory(int id)
        {
            var result =
                GetAllStoreMemories()
                    .Where(x => x.MemoryId == id)
                    .Join
                    (
                        GetAllStores(),
                        storeMemory => storeMemory.StoreId,
                        store => store.Id,
                        (storeMemory, store) => (store, storeMemory)
                    )
                    .ToList();

            return result;
        }

        private Contract.Memory MapInternalToContractMemory(Internal.Memory memory)
        {
            return new Contract.Memory
            {
                Id = memory.Id,
                Size = memory.Size,
                Frequency = memory.Frequency,
                Type = memory.Type,
                Standart = memory.Standart,
                Timings = memory.Timings,
                Voltage = memory.Voltage,
                FormFactor = memory.FormFactor,
                Model = memory.Model,
                MPN = memory.MPN,
                ImageName = memory.ImageName,
                DescriptionName = memory.DescriptionName
            };
        }

        private Internal.Memory MapContractToInternalMemory(Contract.Memory memory)
        {
            return new Internal.Memory
            {
                Id = memory.Id,
                Size = memory.Size,
                Frequency = memory.Frequency,
                Type = memory.Type,
                Standart = memory.Standart,
                Timings = memory.Timings,
                Voltage = memory.Voltage,
                FormFactor = memory.FormFactor,
                Model = memory.Model,
                MPN = memory.MPN,
                ImageName = memory.ImageName,
                DescriptionName = memory.DescriptionName
            };
        }


        #endregion

        #region Motherboard

        public void CreateMotherboard(Internal.Motherboard motherboard)
        {
            CreateIDisplayable(motherboard);
            CreateIDescribable(motherboard);

            _proxy.CreateMotherboard(MapInternalToContractMotherboard(motherboard));
        }

        public List<Internal.Motherboard> GetAllMotherboards()
        {
            var motherboards = _proxy.GetAllMotherboards().Select(MapContractToInternalMotherboard).ToList();

            foreach (var motherboard in motherboards)
            {
                motherboard.BitmapImage = GetImage(motherboard.ImageName);
                motherboard.Description = GetStringFileContent(motherboard.DescriptionName);
            }

            return motherboards;
        }

        public void UpdateMotherboard(Internal.Motherboard motherboard)
        {
            UpdateIDisplayable(motherboard);
            UpdateIDescribable(motherboard);

            _proxy.UpdateMotherboard(MapInternalToContractMotherboard(motherboard));
        }

        public void DeleteMotherboard(int id)
        {
            _proxy.DeleteMotherboard(id);
        }

        public List<(Internal.Store Store, Internal.StoreMotherboard StoreMotherboard)> GetAllStoresSellingMotherboard(int id)
        {
            var result =
                GetAllStoreMotherboards()
                    .Where(x => x.MotherboardId == id)
                    .Join
                    (
                        GetAllStores(),
                        storeMotherboard => storeMotherboard.StoreId,
                        store => store.Id,
                        (storeMotherboard, store) => (store, storeMotherboard)
                    )
                    .ToList();

            return result;
        }

        private Contract.Motherboard MapInternalToContractMotherboard(Internal.Motherboard motherboard)
        {
            return new Contract.Motherboard
            {
                Id = motherboard.Id,
                DIMM = motherboard.DIMM,
                LAN = motherboard.LAN,
                USB = motherboard.USB,
                CPUSocket = motherboard.CPUSocket,
                Northbridge = motherboard.Northbridge,
                Southbridge = motherboard.Southbridge,
                GraphicalInterface = motherboard.GraphicalInterface,
                Model = motherboard.Model,
                MPN = motherboard.MPN,
                ImageName = motherboard.ImageName,
                DescriptionName = motherboard.DescriptionName
            };
        }

        private Internal.Motherboard MapContractToInternalMotherboard(Contract.Motherboard motherboard)
        {
            return new Internal.Motherboard
            {
                Id = motherboard.Id,
                DIMM = motherboard.DIMM,
                LAN = motherboard.LAN,
                USB = motherboard.USB,
                CPUSocket = motherboard.CPUSocket,
                Northbridge = motherboard.Northbridge,
                Southbridge = motherboard.Southbridge,
                GraphicalInterface = motherboard.GraphicalInterface,
                Model = motherboard.Model,
                MPN = motherboard.MPN,
                ImageName = motherboard.ImageName,
                DescriptionName = motherboard.DescriptionName
            };
        }

        #endregion

        #region Processor

        public void CreateProcessor(Internal.Processor processor)
        {
            CreateIDisplayable(processor);
            CreateIDescribable(processor);

            _proxy.CreateProcessor(MapInternalToContractProcessor(processor));
        }

        public List<Internal.Processor> GetAllProcessors()
        {
            var processors = _proxy.GetAllProcessors().Select(MapContractToInternalProcessor).ToList();

            foreach (var processor in processors)
            {
                processor.BitmapImage = GetImage(processor.ImageName);
                processor.Description = GetStringFileContent(processor.DescriptionName);
            }

            return processors;
        }

        public void UpdateProcessor(Internal.Processor processor)
        {
            UpdateIDisplayable(processor);
            UpdateIDescribable(processor);

            _proxy.UpdateProcessor(MapInternalToContractProcessor(processor));
        }

        public void DeleteProcessor(int id)
        {
            _proxy.DeleteProcessor(id);
        }

        public List<(Internal.Store Store, Internal.StoreProcessor StoreProcessor)> GetAllStoresSellingProcessor(int id)
        {
            var result =
                GetAllStoreProcessors()
                    .Where(x => x.ProcessorId == id)
                    .Join
                    (
                        GetAllStores(),
                        storeProcessor => storeProcessor.StoreId,
                        store => store.Id,
                        (storeProcessor, store) => (store, storeProcessor)
                    )
                    .ToList();

            return result;
        }

        private Contract.Processor MapInternalToContractProcessor(Internal.Processor processor)
        {
            return new Contract.Processor
            {
                Id = processor.Id,
                NumberOfCores = processor.NumberOfCores,
                Lithography = processor.Lithography,
                TDP = processor.TDP,
                Level2Cache = processor.Level2Cache,
                Level3Cache = processor.Level3Cache,
                BaseFrequency = processor.BaseFrequency,
                TurboFrequency = processor.TurboFrequency,
                Socket = processor.Socket,
                ProcessorCore = processor.ProcessorCore,
                ProcessorGraphics = processor.ProcessorGraphics,
                Model = processor.Model,
                MPN = processor.MPN,
                ImageName = processor.ImageName,
                DescriptionName = processor.DescriptionName
            };
        }

        private Internal.Processor MapContractToInternalProcessor(Contract.Processor processor)
        {
            return new Internal.Processor
            {
                Id = processor.Id,
                NumberOfCores = processor.NumberOfCores,
                Lithography = processor.Lithography,
                TDP = processor.TDP,
                Level2Cache = processor.Level2Cache,
                Level3Cache = processor.Level3Cache,
                BaseFrequency = processor.BaseFrequency,
                TurboFrequency = processor.TurboFrequency,
                Socket = processor.Socket,
                ProcessorCore = processor.ProcessorCore,
                ProcessorGraphics = processor.ProcessorGraphics,
                Model = processor.Model,
                MPN = processor.MPN,
                ImageName = processor.ImageName,
                DescriptionName = processor.DescriptionName
            };
        }

        #endregion

        #region Purchase

        public void CreatePurchase(Internal.Purchase purchase)
        {
            _proxy.CreatePurchase(MapInternalToContractPurchase(purchase));
        }

        public List<Internal.Purchase> GetAllPurchases()
        {
            var purchases = _proxy.GetAllPurchases().Select(MapContractToInternalPurchase).ToList();

            return purchases;
        }

        public void UpdatePurchase(Internal.Purchase purchase)
        {
            _proxy.UpdatePurchase(MapInternalToContractPurchase(purchase));
        }

        public void DeletePurchase(int id)
        {
            _proxy.DeletePurchase(id);
        }

        public Internal.Store GetStoreFromPurchase(Internal.Purchase purchase)
        {
            ISellableLink sellableLink;

            if (purchase.LinkName == typeof(Internal.StoreMemory).FullName)
            {
                sellableLink = GetAllStoreMemories().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(Internal.StoreMotherboard).FullName)
            {
                sellableLink = GetAllStoreMotherboards().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(Internal.StoreProcessor).FullName)
            {
                sellableLink = GetAllStoreProcessors().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(Internal.StoreVideocard).FullName)
            {
                sellableLink = GetAllStoreVideoCards().Find(x => x.Id == purchase.LinkId);
            }
            else
            {
                throw new NotSupportedException($"ISellableLink of type '{purchase.LinkName}' is not supported.");
            }

            var result = GetAllStores().Find(item => item.Id == sellableLink.StoreId);

            return result;
        }

        private Contract.Purchase MapInternalToContractPurchase(Internal.Purchase purchase)
        {
            return new Contract.Purchase
            {
                Id = purchase.Id,
                LinkId = purchase.LinkId,
                Number = purchase.Number,
                LinkName = purchase.LinkName,
                FirstName = purchase.FirstName,
                SecondName = purchase.SecondName,
                Email = purchase.Email,
                Date = purchase.Date,
                IsRead = purchase.IsRead
            };
        }

        private Internal.Purchase MapContractToInternalPurchase(Contract.Purchase purchase)
        {
            return new Internal.Purchase
            {
                Id = purchase.Id,
                LinkId = purchase.LinkId,
                Number = purchase.Number,
                LinkName = purchase.LinkName,
                FirstName = purchase.FirstName,
                SecondName = purchase.SecondName,
                Email = purchase.Email,
                Date = purchase.Date,
                IsRead = purchase.IsRead
            };
        }

        #endregion

        #region Store

        public void CreateStore(Internal.Store store)
        {
            CreateIDisplayable(store);

            _proxy.CreateStore(MapInternalToContractStore(store));
        }

        public List<Internal.Store> GetAllStores()
        {
            var stores = _proxy.GetAllStores().Select(MapContractToInternalStore).ToList();

            foreach (var store in stores)
            {
                store.BitmapImage = GetImage(store.ImageName);
            }

            return stores;
        }

        public void UpdateStore(Internal.Store store)
        {
            UpdateIDisplayable(store);

            _proxy.UpdateStore(MapInternalToContractStore(store));
        }

        public void DeleteStore(int id)
        {
            _proxy.DeleteStore(id);
        }

        public List<ISellableLink> GetStoreSellableLinks(int storeId)
        {
            List<ISellableLink> sellableLinks = new List<ISellableLink>();

            sellableLinks.AddRange(GetAllStoreProcessors().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreVideoCards().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreMotherboards().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreMemories().Where(item => item.StoreId == storeId));

            return sellableLinks;
        }

        public List<ISellable> GetStoreSellables(int storeId)
        {
            List<ISellable> sellables = new List<ISellable>();

            sellables.AddRange(GetAllStoreProcessors().
                    Where(item => item.StoreId == storeId).
                    Select(item => GetProcessorFromStoreProcessor(item)).ToList());

            sellables.AddRange(GetAllStoreVideoCards().
                    Where(item => item.StoreId == storeId).
                    Select(item => GetVideoCardFromStoreVideoCard(item)).ToList());

            sellables.AddRange(GetAllStoreMotherboards().
                    Where(item => item.MotherboardId == storeId).
                    Select(item => GetMotherboardFromStoreMotherboard(item)).ToList());

            sellables.AddRange(GetAllStoreMemories().
                    Where(item => item.MemoryId == storeId).
                    Select(item => GetMemoryFromStoreMemory(item)).ToList());

            return sellables;
        }

        private Contract.Store MapInternalToContractStore(Internal.Store store)
        {
            return new Contract.Store
            {
                Id = store.Id,
                Name = store.Name,
                Delivery = store.Delivery,
                OwnerId = store.OwnerId,
                ImageName = store.ImageName
            };
        }

        private Internal.Store MapContractToInternalStore(Contract.Store store)
        {
            return new Internal.Store
            {
                Id = store.Id,
                Name = store.Name,
                Delivery = store.Delivery,
                OwnerId = store.OwnerId,
                ImageName = store.ImageName
            };
        }

        #endregion

        #region StoreMemory

        public void CreateStoreMemory(Internal.StoreMemory storeMemory)
        {
            _proxy.CreateStoreMemory(MapInternalToContractStoreMemory(storeMemory));
        }

        public List<Internal.StoreMemory> GetAllStoreMemories()
        {
            var result = _proxy.GetAllStoreMemories().Select(MapContractToInternalStoreMemory).ToList();

            return result;
        }

        public void UpdateStoreMemory(Internal.StoreMemory storeMemory)
        {
            _proxy.UpdateStoreMemory(MapInternalToContractStoreMemory(storeMemory));
        }

        public void DeleteStoreMemory(int id)
        {
            _proxy.DeleteStoreMemory(id);
        }

        public Internal.Memory GetMemoryFromStoreMemory(Internal.StoreMemory storeMemory)
        {
            return GetAllMemories().Find(item => item.Id == storeMemory.MemoryId);
        }

        private Contract.StoreMemory MapInternalToContractStoreMemory(Internal.StoreMemory storeMemory)
        {
            return new Contract.StoreMemory
            {
                Id = storeMemory.Id,
                StoreId = storeMemory.StoreId,
                MemoryId = storeMemory.MemoryId,
                ProductPrice = storeMemory.ProductPrice,
                ProductWarranty = storeMemory.ProductWarranty
            };
        }

        private Internal.StoreMemory MapContractToInternalStoreMemory(Contract.StoreMemory storeMemory)
        {
            return new Internal.StoreMemory
            {
                Id = storeMemory.Id,
                StoreId = storeMemory.StoreId,
                MemoryId = storeMemory.MemoryId,
                ProductPrice = storeMemory.ProductPrice,
                ProductWarranty = storeMemory.ProductWarranty
            };
        }

        #endregion

        #region StoreMotherboard

        public void CreateStoreMotherboard(Internal.StoreMotherboard storeMotherboard)
        {
            _proxy.CreateStoreMotherboard(MapInternalToContractStoreMotherboard(storeMotherboard));
        }

        public List<Internal.StoreMotherboard> GetAllStoreMotherboards()
        {
            var result = _proxy.GetAllStoreMotherboards().Select(MapContractToInternalStoreMotherboard).ToList();

            return result;
        }

        public void UpdateStoreMotherboard(Internal.StoreMotherboard storeMotherboard)
        {
            _proxy.UpdateStoreMotherboard(MapInternalToContractStoreMotherboard(storeMotherboard));
        }

        public void DeleteStoreMotherboard(int id)
        {
            _proxy.DeleteStoreMotherboard(id);
        }

        public Internal.Motherboard GetMotherboardFromStoreMotherboard(Internal.StoreMotherboard storeMotherboard)
        {
            return GetAllMotherboards().Find(item => item.Id == storeMotherboard.MotherboardId);
        }

        private Contract.StoreMotherboard MapInternalToContractStoreMotherboard(Internal.StoreMotherboard storeMotherboard)
        {
            return new Contract.StoreMotherboard
            {
                Id = storeMotherboard.Id,
                StoreId = storeMotherboard.StoreId,
                MotherboardId = storeMotherboard.MotherboardId,
                ProductPrice = storeMotherboard.ProductPrice,
                ProductWarranty = storeMotherboard.ProductWarranty
            };
        }

        private Internal.StoreMotherboard MapContractToInternalStoreMotherboard(Contract.StoreMotherboard storeMotherboard)
        {
            return new Internal.StoreMotherboard
            {
                Id = storeMotherboard.Id,
                StoreId = storeMotherboard.StoreId,
                MotherboardId = storeMotherboard.MotherboardId,
                ProductPrice = storeMotherboard.ProductPrice,
                ProductWarranty = storeMotherboard.ProductWarranty
            };
        }

        #endregion

        #region StoreProcessor

        public void CreateStoreProcessor(Internal.StoreProcessor storeProcessor)
        {
            _proxy.CreateStoreProcessor(MapInternalToContractStoreProcessor(storeProcessor));
        }

        public List<Internal.StoreProcessor> GetAllStoreProcessors()
        {
            var result = _proxy.GetAllStoreProcessors().Select(MapContractToInternalStoreProcessor).ToList();

            return result;
        }

        public void UpdateStoreProcessor(Internal.StoreProcessor storeProcessor)
        {
            _proxy.UpdateStoreProcessor(MapInternalToContractStoreProcessor(storeProcessor));
        }

        public void DeleteStoreProcessor(int id)
        {
            _proxy.DeleteStoreProcessor(id);
        }

        public Internal.Processor GetProcessorFromStoreProcessor(Internal.StoreProcessor storeProcessor)
        {
            return GetAllProcessors().Find(item => item.Id == storeProcessor.ProcessorId);
        }

        private Contract.StoreProcessor MapInternalToContractStoreProcessor(Internal.StoreProcessor storeProcessor)
        {
            return new Contract.StoreProcessor
            {
                Id = storeProcessor.Id,
                StoreId = storeProcessor.StoreId,
                ProcessorId = storeProcessor.ProcessorId,
                ProductPrice = storeProcessor.ProductPrice,
                ProductWarranty = storeProcessor.ProductWarranty
            };
        }

        private Internal.StoreProcessor MapContractToInternalStoreProcessor(Contract.StoreProcessor storeProcessor)
        {
            return new Internal.StoreProcessor
            {
                Id = storeProcessor.Id,
                StoreId = storeProcessor.StoreId,
                ProcessorId = storeProcessor.ProcessorId,
                ProductPrice = storeProcessor.ProductPrice,
                ProductWarranty = storeProcessor.ProductWarranty
            };
        }

        #endregion

        #region StoreVideocard

        public void CreateStoreVideocard(Internal.StoreVideocard storeVideocard)
        {
            _proxy.CreateStoreVideocard(MapInternalToContractStoreVideocard(storeVideocard));
        }

        public List<Internal.StoreVideocard> GetAllStoreVideoCards()
        {
            var result = _proxy.GetAllStoreVideoCards().Select(MapContractToInternalStoreVideocard).ToList();

            return result;
        }

        public void UpdateStoreVideocard(Internal.StoreVideocard storeVideocard)
        {
            _proxy.UpdateStoreVideocard(MapInternalToContractStoreVideocard(storeVideocard));
        }

        public void DeleteStoreVideocard(int id)
        {
            _proxy.DeleteStoreVideocard(id);
        }

        public Internal.Videocard GetVideoCardFromStoreVideoCard(Internal.StoreVideocard storeVideocard)
        {
            return GetAllVideoCards().Find(item => item.Id == storeVideocard.VideocardId);
        }

        private Contract.StoreVideocard MapInternalToContractStoreVideocard(Internal.StoreVideocard storeVideocard)
        {
            return new Contract.StoreVideocard
            {
                Id = storeVideocard.Id,
                StoreId = storeVideocard.StoreId,
                VideocardId = storeVideocard.VideocardId,
                ProductPrice = storeVideocard.ProductPrice,
                ProductWarranty = storeVideocard.ProductWarranty
            };
        }

        private Internal.StoreVideocard MapContractToInternalStoreVideocard(Contract.StoreVideocard storeVideocard)
        {
            return new Internal.StoreVideocard
            {
                Id = storeVideocard.Id,
                StoreId = storeVideocard.StoreId,
                VideocardId = storeVideocard.VideocardId,
                ProductPrice = storeVideocard.ProductPrice,
                ProductWarranty = storeVideocard.ProductWarranty
            };
        }

        #endregion

        #region User

        public Internal.User LogIn()
        {
            var result = _proxy.GetLoggedInUser();

            if (result != null)
            {
                return MapContractToInternalUser(result);
            }

            return null;
        }

        public List<Internal.Store> GetUserStores(Internal.User user)
        {
            return GetAllStores().Where(item => item.OwnerId == user.Login).ToList();
        }

        private Internal.User MapContractToInternalUser(Contract.User user)
        {
            return new Internal.User
            {
                Login = user.Login,
                UserRole = user.UserRole
            };
        }

        #endregion

        #region Videocard

        public void CreateVideoCard(Internal.Videocard videocard)
        {
            CreateIDisplayable(videocard);
            CreateIDescribable(videocard);

            _proxy.CreateVideoCard(MapInternalToContractVideocard(videocard));
        }

        public List<Internal.Videocard> GetAllVideoCards()
        {
            var videoCards = _proxy.GetAllVideoCards().Select(MapContractToInternalVideocard).ToList();

            foreach (var videoCart in videoCards)
            {
                videoCart.BitmapImage = GetImage(videoCart.ImageName);
                videoCart.Description = GetStringFileContent(videoCart.DescriptionName);
            }

            return videoCards;
        }

        public void UpdateVideoCard(Internal.Videocard videocard)
        {
            UpdateIDisplayable(videocard);
            UpdateIDescribable(videocard);

            _proxy.UpdateVideoCard(MapInternalToContractVideocard(videocard));
        }

        public void DeleteVideoCard(int id)
        {
            _proxy.DeleteVideoCard(id);
        }

        public List<(Internal.Store Store, Internal.StoreVideocard StoreVideoCard)> GetAllStoresSellingVideoCard(int id)
        {
            var result =
                GetAllStoreVideoCards()
                    .Where(x => x.VideocardId == id)
                    .Join
                    (
                        GetAllStores(),
                        storeVideoCard => storeVideoCard.StoreId,
                        store => store.Id,
                        (storeVideoCard, store) => (store, storeVideoCard)
                    )
                    .ToList();

            return result;
        }

        private Contract.Videocard MapInternalToContractVideocard(Internal.Videocard videocard)
        {
            return new Contract.Videocard
            {
                Id = videocard.Id,
                Memory = videocard.Memory,
                GPUFrequency = videocard.GPUFrequency,
                MemoryFrequency = videocard.MemoryFrequency,
                Bus = videocard.Bus,
                GPU = videocard.GPU,
                MemoryType = videocard.MemoryType,
                Interface = videocard.Interface,
                Cooling = videocard.Cooling,
                Model = videocard.Model,
                MPN = videocard.MPN,
                ImageName = videocard.ImageName,
                DescriptionName = videocard.DescriptionName
            };
        }

        private Internal.Videocard MapContractToInternalVideocard(Contract.Videocard videocard)
        {
            return new Internal.Videocard
            {
                Id = videocard.Id,
                Memory = videocard.Memory,
                GPUFrequency = videocard.GPUFrequency,
                MemoryFrequency = videocard.MemoryFrequency,
                Bus = videocard.Bus,
                GPU = videocard.GPU,
                MemoryType = videocard.MemoryType,
                Interface = videocard.Interface,
                Cooling = videocard.Cooling,
                Model = videocard.Model,
                MPN = videocard.MPN,
                ImageName = videocard.ImageName,
                DescriptionName = videocard.DescriptionName
            };
        }

        #endregion

        public void AddTextFile(string text, out string fileName)
        {
            byte[] bytes = Serialize(text);

            AddFile(bytes, out fileName);
        }

        public void AddFile(byte[] file, out string fileName)
        {
            using (var memoryStream = new MemoryStream(file))
            {
                fileName = _proxy.AddFile(memoryStream);
            }
        }

        private void CreateIDisplayable(IDisplayable displayable)
        {
            if (displayable.BitmapImage != null)
            {
                string fileName;

                Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                byte[] image = Serialize(bitmap);

                AddFile(image, out fileName);

                displayable.ImageName = fileName;
            }
        }

        private void CreateIDescribable(IDescribable describable)
        {
            if (describable.Description != null)
            {
                string fileName;

                AddTextFile(describable.Description, out fileName);

                describable.DescriptionName = fileName;
            }
        }

        private void UpdateIDisplayable(IDisplayable displayable)
        {
            if (string.IsNullOrWhiteSpace(displayable.ImageName) && displayable.BitmapImage != null)
            {
                Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                byte[] image = Serialize(bitmap);

                AddFile(image, out var fileName);

                displayable.ImageName = fileName;
            }
        }

        private void UpdateIDescribable(IDescribable describable)
        {
            if (string.IsNullOrWhiteSpace(describable.DescriptionName) && describable.Description != null)
            {
                AddTextFile(describable.Description, out var fileName);

                describable.DescriptionName = fileName;
            }
        }

        private static byte[] Serialize(object item)
        {
            byte[] bytes;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, item);
                bytes = stream.ToArray();
            }

            return bytes;
        }

        private string GetStringFileContent(string fileName)
        {
            byte[] bytes;

            using (MemoryStream stream = new MemoryStream())
            using (Stream serverStream = _proxy.GetFile(fileName))
            {
                serverStream.CopyTo(stream);
                bytes = stream.ToArray();
            }
            return Deserialize(bytes) as string;
        }

        public BitmapImage GetImage(string fileName)
        {
            byte[] bytes;

            using (MemoryStream stream = new MemoryStream())
            using (Stream serverStream = _proxy.GetFile(fileName))
            {
                serverStream.CopyTo(stream);
                bytes = stream.ToArray();
            }

            Bitmap bitmap = Deserialize(bytes) as Bitmap;

            return BitmapToBitmapImage(bitmap);
        }

        public static object Deserialize(byte[] bytes)
        {
            object item;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                item = binaryFormatter.Deserialize(stream);
            }
            return item;
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }
            return bitmap;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage;

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}