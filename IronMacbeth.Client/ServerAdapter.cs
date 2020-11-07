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
using IronMacbeth.Client;

namespace IronMacbeth.Client
{
    public class ServerAdapter
    {
        public static IService Proxy;

        public ServerAdapter(IService proxy)
        {
            Proxy = proxy;
        }

        #region Book

        public void CreateBook(Internal.Book book)
        {
            CreateIDisplayable(book);
            CreateIDescribable(book);
            string fileName;  
            AddFile(book.ElectronicVersion, out fileName);
            book.ElectronicVersionFileName = fileName;

            var contractBook = MapInternalToContractBook(book);

            Proxy.CreateBook(contractBook);
        }

        public List<Internal.Book> GetAllBooks()
        {
            var books = Proxy.GetAllBooks();

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

            Proxy.UpdateBook(contractBook);
        }

        public void DeleteBook(int id)
        {
            Proxy.DeleteBook(id);
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
                ElectronicVersionFileName = book.ElectronicVersionFileName,
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
            Proxy.CreateStoreBook(MapInternalToContractStoreBook(storeBooks));
        }

        public List<Internal.StoreBook> GetAllStoreBooks()
        {
            var result = Proxy.GetAllStoreBooks();

            return result.Select(MapContractToInternalStoreBook).ToList();
        }

        public void UpdateStoreBook(Internal.StoreBook storeBooks)
        {
            Proxy.UpdateStoreBooks(MapInternalToContractStoreBook(storeBooks));
        }

        public void DeleteStoreBook(int id)
        {
            Proxy.DeleteStoreBook(id);
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

            Proxy.CreateMemory(MapInternalToContractMemory(memory));
        }

        public List<Internal.Memory> GetAllMemories()
        {
            var memories = Proxy.GetAllMemories().Select(MapContractToInternalMemory).ToList();

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

            Proxy.UpdateMemory(MapInternalToContractMemory(memory));
        }

        public void DeleteMemory(int id)
        {
            Proxy.DeleteMemory(id);
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

            Proxy.CreateMotherboard(MapInternalToContractMotherboard(motherboard));
        }

        public List<Internal.Motherboard> GetAllMotherboards()
        {
            var motherboards = Proxy.GetAllMotherboards().Select(MapContractToInternalMotherboard).ToList();

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

            Proxy.UpdateMotherboard(MapInternalToContractMotherboard(motherboard));
        }

        public void DeleteMotherboard(int id)
        {
            Proxy.DeleteMotherboard(id);
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

            Proxy.CreateProcessor(MapInternalToContractProcessor(processor));
        }

        public List<Internal.Processor> GetAllProcessors()
        {
            var processors = Proxy.GetAllProcessors().Select(MapContractToInternalProcessor).ToList();

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

            Proxy.UpdateProcessor(MapInternalToContractProcessor(processor));
        }

        public void DeleteProcessor(int id)
        {
            Proxy.DeleteProcessor(id);
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
            Proxy.CreatePurchase(MapInternalToContractPurchase(purchase));
        }

        public List<Internal.Purchase> GetAllPurchases()
        {
            var purchases = Proxy.GetAllPurchases().Select(MapContractToInternalPurchase).ToList();

            return purchases;
        }

        public void UpdatePurchase(Internal.Purchase purchase)
        {
            Proxy.UpdatePurchase(MapInternalToContractPurchase(purchase));
        }

        public void DeletePurchase(int id)
        {
            Proxy.DeletePurchase(id);
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

            Proxy.CreateStore(MapInternalToContractStore(store));
        }

        public List<Internal.Store> GetAllStores()
        {
            var stores = Proxy.GetAllStores().Select(MapContractToInternalStore).ToList();

            foreach (var store in stores)
            {
                store.BitmapImage = GetImage(store.ImageName);
            }

            return stores;
        }

        public void UpdateStore(Internal.Store store)
        {
            UpdateIDisplayable(store);

            Proxy.UpdateStore(MapInternalToContractStore(store));
        }

        public void DeleteStore(int id)
        {
            Proxy.DeleteStore(id);
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
            Proxy.CreateStoreMemory(MapInternalToContractStoreMemory(storeMemory));
        }

        public List<Internal.StoreMemory> GetAllStoreMemories()
        {
            var result = Proxy.GetAllStoreMemories().Select(MapContractToInternalStoreMemory).ToList();

            return result;
        }

        public void UpdateStoreMemory(Internal.StoreMemory storeMemory)
        {
            Proxy.UpdateStoreMemory(MapInternalToContractStoreMemory(storeMemory));
        }

        public void DeleteStoreMemory(int id)
        {
            Proxy.DeleteStoreMemory(id);
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
            Proxy.CreateStoreMotherboard(MapInternalToContractStoreMotherboard(storeMotherboard));
        }

        public List<Internal.StoreMotherboard> GetAllStoreMotherboards()
        {
            var result = Proxy.GetAllStoreMotherboards().Select(MapContractToInternalStoreMotherboard).ToList();

            return result;
        }

        public void UpdateStoreMotherboard(Internal.StoreMotherboard storeMotherboard)
        {
            Proxy.UpdateStoreMotherboard(MapInternalToContractStoreMotherboard(storeMotherboard));
        }

        public void DeleteStoreMotherboard(int id)
        {
            Proxy.DeleteStoreMotherboard(id);
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
            Proxy.CreateStoreProcessor(MapInternalToContractStoreProcessor(storeProcessor));
        }

        public List<Internal.StoreProcessor> GetAllStoreProcessors()
        {
            var result = Proxy.GetAllStoreProcessors().Select(MapContractToInternalStoreProcessor).ToList();

            return result;
        }

        public void UpdateStoreProcessor(Internal.StoreProcessor storeProcessor)
        {
            Proxy.UpdateStoreProcessor(MapInternalToContractStoreProcessor(storeProcessor));
        }

        public void DeleteStoreProcessor(int id)
        {
            Proxy.DeleteStoreProcessor(id);
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
            Proxy.CreateStoreVideocard(MapInternalToContractStoreVideocard(storeVideocard));
        }

        public List<Internal.StoreVideocard> GetAllStoreVideoCards()
        {
            var result = Proxy.GetAllStoreVideoCards().Select(MapContractToInternalStoreVideocard).ToList();

            return result;
        }

        public void UpdateStoreVideocard(Internal.StoreVideocard storeVideocard)
        {
            Proxy.UpdateStoreVideocard(MapInternalToContractStoreVideocard(storeVideocard));
        }

        public void DeleteStoreVideocard(int id)
        {
            Proxy.DeleteStoreVideocard(id);
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

        public void Register(Internal.User user)
        {
            Proxy.Register(MapInternalToContractUser(user));
        }

        public Internal.User LogIn(string login, string password)
        {
            var result = Proxy.LogIn(login, password);

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

        private Contract.User MapInternalToContractUser(Internal.User user)
        {
            return new Contract.User
            {
                Login = user.Login,
                Password = user.Password,
                AccessLevel = user.AccessLevel
            };
        }

        private Internal.User MapContractToInternalUser(Contract.User user)
        {
            return new Internal.User
            {
                Login = user.Login,
                Password = user.Password,
                AccessLevel = user.AccessLevel
            };
        }

        #endregion

        #region Videocard

        public void CreateVideoCard(Internal.Videocard videocard)
        {
            CreateIDisplayable(videocard);
            CreateIDescribable(videocard);

            Proxy.CreateVideoCard(MapInternalToContractVideocard(videocard));
        }

        public List<Internal.Videocard> GetAllVideoCards()
        {
            var videoCards = Proxy.GetAllVideoCards().Select(MapContractToInternalVideocard).ToList();

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

            Proxy.UpdateVideoCard(MapInternalToContractVideocard(videocard));
        }

        public void DeleteVideoCard(int id)
        {
            Proxy.DeleteVideoCard(id);
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

            Proxy.AddFile(bytes, out fileName);
        }

        public void AddFile(byte[] file, out string fileName)
        {
            Proxy.AddFile(file, out fileName);
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
            byte[] bytes = Proxy.GetFile(fileName);

            return Deserialize(bytes) as string;
        }

        public static BitmapImage GetImage(string fileName)
        {
            byte[] bytes = Proxy.GetFile(fileName);

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

        public bool Ping()
        {
            return Proxy.Ping();
        }
    }
}