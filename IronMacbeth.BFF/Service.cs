using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using IronMacbeth.BFF.Contract;
using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    public class Service : IService
    {
        #region Order
        public void CreateOrder(Contract.CreateOrder orderInfo)
        {
            var order = new Order
            {
                Id = orderInfo.Id,
                UserLogin = orderInfo.UserLogin,
                BookId = orderInfo.BookId,
                ArticleId = orderInfo.ArticleId,
                PeriodicalId = orderInfo.PeriodicalId,
                ThesesId = orderInfo.ThesesId,
                NewspaperId = orderInfo.NewspaperId,
                TypeOfOrder = orderInfo.TypeOfOrder,
                StatusOfOrder = orderInfo.StatusOfOrder,
                DateOfReturn = orderInfo.DateOfReturn,
                DateOfOrder = orderInfo.DateOfReturn,
                ReceiveDate = orderInfo.ReceiveDate
            };

            using (var dbContext = new DbContext())
            {
                dbContext.Add(order);
                dbContext.SaveChanges();
            }
        }

        public List<Contract.Order> GetAllOrders()
        {
            using (var dbContext = new DbContext())
            {
                User currentUser = GetLoggedInUserInternal();
                IQueryable<Order> intermediate = dbContext.Orders.Include(x => x.Book).Include(x => x.Article).Include(x => x.Periodical).Include(x => x.Theses).Include(x => x.Newspaper);
                if (currentUser != null)
                {
                    intermediate = intermediate.Where(x => x.UserLogin == currentUser.Login);
                }

                var result = intermediate.ToList();

                return result.Select(x => new Contract.Order
                {
                    Id = x.Id,
                    UserLogin = x.UserLogin,
                    Book = x.Book,
                    Article = x.Article,
                    Periodical = x.Periodical,
                    Theses = x.Theses,
                    Newspaper = x.Newspaper,
                    TypeOfOrder = x.TypeOfOrder,
                    StatusOfOrder = x.StatusOfOrder,
                    DateOfOrder = x.DateOfOrder,
                    DateOfReturn = x.DateOfReturn
                }).ToList();
            }

        }

        public void DeleteOrder(int id)
        {

            using (var dbContext = new DbContext())
            {
                dbContext.Orders.Remove(new Order { Id = id });
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region ReadingRoomOrder
        public void CreateReadingRoomOrder(ReadingRoomOrder orderInfo)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(orderInfo);
                dbContext.SaveChanges();
            }
        }
        #endregion



        #region Search
        public DocumentsSearchResults SearchDocuments(SearchFilledFields searchFilledFields)
        {
            var result = new DocumentsSearchResults();
            if (searchFilledFields.IsArticleSelected)
            {
                List<Article> articles = GetArticlesByCriteria(searchFilledFields);
                result.Articles = articles;
            }
            if (searchFilledFields.IsBookSelected)
            {
                List<Book> books = GetBooksByCriteria(searchFilledFields);
                result.Books = books;
            }
            if (searchFilledFields.IsPeriodicalSelected)
            {
                List<Periodical> periodicals = GetPeriodicalsByCriteria(searchFilledFields);
                result.Periodicals = periodicals;
            }
            if (searchFilledFields.IsNewspaperSelected)
            {
                List<Newspaper> newspapers = GetNewspapersByCriteria(searchFilledFields);
                result.Newspapers = newspapers;
            }
            if (searchFilledFields.IsThesisSelected)
            {
                List<Thesis> theses = GetThesesByCriteria(searchFilledFields);
                result.Theses = theses;
            }
            return result;
        }
        #endregion

        #region  Periodical
        public void CreatePeriodical(Periodical periodical)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(periodical);

                dbContext.SaveChanges();
            }
        }

        public List<Periodical> GetAllPeriodicals()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Periodicals.ToList();
            }
        }

        public void UpdatePeriodical(Periodical article)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(article);

                dbContext.SaveChanges();
            }
        }
        private List<Periodical> GetPeriodicalsByCriteria(SearchFilledFields searchFilledFields)
        {
            if (searchFilledFields.SearchAuthor != null) return new List<Periodical>();
            using (var dbContext = new DbContext())
            {
                IQueryable<Periodical> intermediate = dbContext.Periodicals;

                if (searchFilledFields.SearchName != null)
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchYearFrom != null && searchFilledFields.SearchYearTo == null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom);
                }
                if (searchFilledFields.SearchYearTo != null && searchFilledFields.SearchYearFrom == null)
                {
                    intermediate = intermediate.Where(x => x.Year < searchFilledFields.SearchYearTo);
                }
                if (searchFilledFields.SearchYearFrom != null & searchFilledFields.SearchYearTo != null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom && x.Year < searchFilledFields.SearchYearTo);
                }

                return intermediate.ToList();
            }
        }

        public void DeletePeriodical(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Periodicals.Remove(new Periodical { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Thesis
        public void CreateThesis(Thesis thesis)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(thesis);

                dbContext.SaveChanges();
            }
        }

        public List<Thesis> GetAllThesises()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Thesises.ToList();
            }
        }

        public void UpdateThesis(Thesis thesis)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(thesis);

                dbContext.SaveChanges();
            }
        }

        private List<Thesis> GetThesesByCriteria(SearchFilledFields searchFilledFields)
        {
            using (var dbContext = new DbContext())
            {
                IQueryable<Thesis> intermediate = dbContext.Thesises;
                if (searchFilledFields.SearchName != null)
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchYearFrom != null && searchFilledFields.SearchYearTo == null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom);
                }
                if (searchFilledFields.SearchYearTo != null && searchFilledFields.SearchYearFrom == null)
                {
                    intermediate = intermediate.Where(x => x.Year < searchFilledFields.SearchYearTo);
                }
                if (searchFilledFields.SearchYearFrom != null & searchFilledFields.SearchYearTo != null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom && x.Year < searchFilledFields.SearchYearTo);
                }
                return intermediate.ToList();
            }
        }
        public void DeleteThesis(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Thesises.Remove(new Thesis { Id = id });

                dbContext.SaveChanges();
            }

        }
        #endregion

        #region Newspaper

        public void CreateNewspaper(Newspaper newspaper)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(newspaper);

                dbContext.SaveChanges();
            }
        }

        public List<Newspaper> GetAllNewspapers()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Newspapers.ToList();
            }
        }

        public void UpdateNewspaper(Newspaper newspaper)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(newspaper);

                dbContext.SaveChanges();
            }
        }

        private List<Newspaper> GetNewspapersByCriteria(SearchFilledFields searchFilledFields)
        {
            if (searchFilledFields.SearchAuthor != null) return new List<Newspaper>();
            using (var dbContext = new DbContext())
            {
                IQueryable<Newspaper> intermediate = dbContext.Newspapers;
                if (searchFilledFields.SearchName != null)
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchYearFrom != null && searchFilledFields.SearchYearTo == null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom);
                }
                if (searchFilledFields.SearchYearTo != null && searchFilledFields.SearchYearFrom == null)
                {
                    intermediate = intermediate.Where(x => x.Year < searchFilledFields.SearchYearTo);
                }
                if (searchFilledFields.SearchYearFrom != null & searchFilledFields.SearchYearTo != null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom && x.Year < searchFilledFields.SearchYearTo);
                }
                return intermediate.ToList();
            }

        }


        public void DeleteNewspaper(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Newspapers.Remove(new Newspaper { Id = id });

                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Article
        public void CreateArticle(Article article)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(article);

                dbContext.SaveChanges();
            }
        }

        public List<Article> GetAllArticles()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Articles.ToList();
            }
        }

        private List<Article> GetArticlesByCriteria(SearchFilledFields searchFilledFields)
        {
            using (var dbContext = new DbContext())
            {
                IQueryable<Article> intermediate = dbContext.Articles;
                if (searchFilledFields.SearchName != null)
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchAuthor != null)
                {
                    intermediate = intermediate.Where(x => x.Author == searchFilledFields.SearchAuthor);
                }
                if (searchFilledFields.SearchYearFrom != null && searchFilledFields.SearchYearTo == null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom);
                }
                if (searchFilledFields.SearchYearTo != null && searchFilledFields.SearchYearFrom == null)
                {
                    intermediate = intermediate.Where(x => x.Year < searchFilledFields.SearchYearTo);
                }
                if (searchFilledFields.SearchYearFrom != null & searchFilledFields.SearchYearTo != null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom && x.Year < searchFilledFields.SearchYearTo);
                }
                return intermediate.ToList();
            }
        }

        public void UpdateArticle(Article article)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(article);

                dbContext.SaveChanges();
            }
        }

        public void DeleteArticle(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Articles.Remove(new Article { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Book
        public void CreateBook(Book book)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(book);

                dbContext.SaveChanges();
            }
        }

        private List<Book> GetBooksByCriteria(SearchFilledFields searchFilledFields)
        {
            using (var dbContext = new DbContext())
            {
                IQueryable<Book> intermediate = dbContext.Books;

                if (searchFilledFields.SearchName != null)
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchAuthor != null)
                {
                    intermediate = intermediate.Where(x => x.Author == searchFilledFields.SearchAuthor);
                }
                if (searchFilledFields.SearchYearFrom != null && searchFilledFields.SearchYearTo == null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom);
                }
                if (searchFilledFields.SearchYearTo != null && searchFilledFields.SearchYearFrom == null)
                {
                    intermediate = intermediate.Where(x => x.Year < searchFilledFields.SearchYearTo);
                }
                if (searchFilledFields.SearchYearFrom != null & searchFilledFields.SearchYearTo != null)
                {
                    intermediate = intermediate.Where(x => x.Year > searchFilledFields.SearchYearFrom && x.Year < searchFilledFields.SearchYearTo);
                }

                return intermediate.ToList();
            }
        }

        public List<Book> GetAllBooks()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Books.ToList();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(book);

                dbContext.SaveChanges();
            }
        }

        public void DeleteBook(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Books.Remove(new Book { Id = id });

                dbContext.SaveChanges();
            }
        }
        #endregion

        #region StoreBook

        public void CreateStoreBook(StoreBook storeBook)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeBook);

                dbContext.SaveChanges();
            }
        }

        public List<StoreBook> GetAllStoreBooks()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreBook.ToList();
            }
        }

        public void UpdateStoreBooks(StoreBook storeBook)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeBook);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreBook(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreBook.Remove(new StoreBook { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Memory

        public void CreateMemory(Memory memory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(memory);

                dbContext.SaveChanges();
            }
        }

        public List<Memory> GetAllMemories()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Memories.ToList();
            }
        }

        public void UpdateMemory(Memory memory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(memory);

                dbContext.SaveChanges();
            }
        }

        public void DeleteMemory(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Memories.Remove(new Memory { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Motherboard

        public void CreateMotherboard(Motherboard motherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(motherboard);

                dbContext.SaveChanges();
            }
        }

        public List<Motherboard> GetAllMotherboards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Motherboards.ToList();
            }
        }

        public void UpdateMotherboard(Motherboard motherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(motherboard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteMotherboard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Motherboards.Remove(new Motherboard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Processor

        public void CreateProcessor(Processor processor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(processor);

                dbContext.SaveChanges();
            }
        }

        public List<Processor> GetAllProcessors()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Processors.ToList();
            }
        }

        public void UpdateProcessor(Processor processor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(processor);

                dbContext.SaveChanges();
            }
        }

        public void DeleteProcessor(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Processors.Remove(new Processor { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Purchase

        public void CreatePurchase(Purchase purchase)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(purchase);

                dbContext.SaveChanges();
            }
        }

        public List<Purchase> GetAllPurchases()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Purchases.ToList();
            }
        }

        public void UpdatePurchase(Purchase purchase)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(purchase);

                dbContext.SaveChanges();
            }
        }

        public void DeletePurchase(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Purchases.Remove(new Purchase { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Store

        public void CreateStore(Store store)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(store);

                dbContext.SaveChanges();
            }
        }

        public List<Store> GetAllStores()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Stores.ToList();
            }
        }

        public void UpdateStore(Store store)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(store);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStore(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Stores.Remove(new Store { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreMemory

        public void CreateStoreMemory(StoreMemory storeMemory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeMemory);

                dbContext.SaveChanges();
            }
        }

        public List<StoreMemory> GetAllStoreMemories()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreMemories.ToList();
            }
        }

        public void UpdateStoreMemory(StoreMemory storeMemory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeMemory);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreMemory(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreMemories.Remove(new StoreMemory { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreMotherboard

        public void CreateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeMotherboard);

                dbContext.SaveChanges();
            }
        }

        public List<StoreMotherboard> GetAllStoreMotherboards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreMotherboards.ToList();
            }
        }

        public void UpdateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeMotherboard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreMotherboard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreMotherboards.Remove(new StoreMotherboard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreProcessor

        public void CreateStoreProcessor(StoreProcessor storeProcessor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeProcessor);

                dbContext.SaveChanges();
            }
        }

        public List<StoreProcessor> GetAllStoreProcessors()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreProcessors.ToList();
            }
        }

        public void UpdateStoreProcessor(StoreProcessor storeProcessor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeProcessor);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreProcessor(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreProcessors.Remove(new StoreProcessor { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreVideocard

        public void CreateStoreVideocard(StoreVideocard storeVideocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeVideocard);

                dbContext.SaveChanges();
            }
        }

        public List<StoreVideocard> GetAllStoreVideoCards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreVideoCards.ToList();
            }
        }

        public void UpdateStoreVideocard(StoreVideocard storeVideocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeVideocard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreVideocard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreVideoCards.Remove(new StoreVideocard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region User

        public Contract.User GetLoggedInUser()
        {
            var internalUser = GetLoggedInUserInternal();

            var contractUser = new Contract.User { Login = internalUser.Login, UserRole = internalUser.UserRole };

            return contractUser;
        }

        #endregion

        #region Videocard

        public void CreateVideoCard(Videocard videocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(videocard);

                dbContext.SaveChanges();
            }
        }

        public List<Videocard> GetAllVideoCards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.VideoCards.ToList();
            }
        }

        public void UpdateVideoCard(Videocard videocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(videocard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteVideoCard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.VideoCards.Remove(new Videocard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        public string AddFile(Stream fileStream)
        {
            var fileName = GetFileId();

            FileInfo fileInfo = new FileInfo($"Files\\{fileName}");
            using (var stream = fileInfo.Open(FileMode.CreateNew, FileAccess.Write))
            {
                fileStream.CopyTo(stream);
                stream.Close();
            }

            return fileName;
        }

        private static string GetFileId()
        {
            string result;

            Directory.CreateDirectory("Files");

            FileInfo fileInfo = new FileInfo("Files\\idStorage");
            if (fileInfo.Exists)
            {
                int id;
                using (TextReader reader = fileInfo.OpenText())
                {
                    id = int.Parse(reader.ReadLine());
                }
                id++;
                result = id.ToString();
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            else
            {
                result = "1";
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            return result;
        }

        public Stream GetFile(string fileName)
        {
            // do not dispose stream here. It's disposed by WCF whenever it's done sending it to the client
            var fileStream = File.OpenRead($"Files\\{fileName}");

            return fileStream;
        }

        private User GetLoggedInUserInternal()
        {
            var identity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;

            var userLogin = identity.Claims.Single(x => string.Equals(x.Type, identity.NameClaimType, StringComparison.OrdinalIgnoreCase)).Value;

            using (var dbContext = new DbContext())
            {
                var internalUser = dbContext.Users.SingleOrDefault(x => x.Login == userLogin);

                return internalUser;
            }
        }
    }
}