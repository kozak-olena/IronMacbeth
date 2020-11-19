﻿using IronMacbeth.BFF.Contract;
using IronMacbeth.FileStorage.Contract;
using IronMacbeth.UserManagement.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading;

namespace IronMacbeth.BFF
{
    public class Service : IService
    {
        private readonly IFileStorageService _fileStorageServiceClient;

        public Service()
        {
            _fileStorageServiceClient = new ChannelFactory<IFileStorageService>("IronMacbeth.FileStorageEndpoint").CreateChannel();
        }

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
                DateOfOrder = orderInfo.DateOfOrer,
                ReceiveDate = orderInfo.ReceiveDate,
                UserName = orderInfo.UserName,
                UserSurname = orderInfo.UserSurname,
                PhoneNumber = orderInfo.PhoneNumber
            };

            using (var dbContext = new DbContext())
            {
                dbContext.Add(order);
                dbContext.SaveChanges();
            }
        }

        public void UpdateOrder(Contract.Order order, Contract.SpecifiedOrderFields specifyOrderFields)
        {
            var Corder = new Order()
            {
                Id = order.Id,
                UserLogin = order.UserLogin,
                TypeOfOrder = order.TypeOfOrder,
                BookId = order.BookId,
                ArticleId = order.ArticleId,
                PeriodicalId = order.PeriodicalId,
                ThesesId = order.ThesesId,
                NewspaperId = order.NewspaperId,
                DateOfOrder = order.DateOfOrder,
                Book = order.Book,
                Article = order.Article,
                Periodical = order.Periodical,
                Newspaper = order.Newspaper,
                Theses = order.Theses,
                StatusOfOrder = specifyOrderFields.Status,
                ReceiveDate = specifyOrderFields.ReceiveDate,
                DateOfReturn = specifyOrderFields.DateOfReturning
            };
            using (var dbContext = new DbContext())
            {
                dbContext.Update(Corder);
                //dbContext.Orders.Attach(order);
                //dbContext.Entry(order).Property(x => x.StatusOfOrder).IsModified = true;
                //dbContext.Entry(order).Property(x => x.ReceiveDate).IsModified = true;
                //dbContext.Entry(order).Property(x => x.DateOfReturn).IsModified = true;
                dbContext.SaveChanges();
            }
        }

        public bool CheckOrder(int id, DocumentType documentType)
        {
            using (var dbContext = new DbContext())
            {

                if (documentType == DocumentType.Book)
                {
                    List<Book> intermediate = dbContext.Books.ToList();
                    return intermediate.Any(x => x.Id == id);
                }
                if (documentType == DocumentType.Article)
                {
                    List<Article> intermediate = dbContext.Articles.ToList();
                    return intermediate.Any(x => x.Id == id);
                }
                if (documentType == DocumentType.Periodical)
                {
                    List<Periodical> intermediate = dbContext.Periodicals.ToList();
                    return intermediate.Any(x => x.Id == id);
                }
                if (documentType == DocumentType.Thesis)
                {
                    List<Thesis> intermediate = dbContext.Thesises.ToList();
                    return intermediate.Any(x => x.Id == id);

                }
                if (documentType == DocumentType.Newspaper)
                {
                    List<Newspaper> intermediate = dbContext.Newspapers.ToList();
                    return intermediate.Any(x => x.Id == id);
                }
                else { throw new InvalidOperationException("Can not get bool"); }


            }
        }

        public List<Contract.Order> GetAllOrders()
        {
            using (var dbContext = new DbContext())
            {
                User currentUser = GetLoggedInUserInternal();
                IQueryable<Order> intermediate = dbContext.Orders.Include(x => x.Book).Include(x => x.Article).Include(x => x.Periodical).Include(x => x.Theses).Include(x => x.Newspaper);
                if (currentUser.UserRole != UserRole.Admin)
                {
                    intermediate = intermediate.Where(x => x.UserLogin == currentUser.Login);
                }

                var result = intermediate.ToList();

                return result.Select(x => new Contract.Order
                {
                    Id = x.Id,
                    UserLogin = x.UserLogin,
                    UserName = x.UserName,
                    UserSurname = x.UserSurname,
                    PhoneNumber = x.PhoneNumber,
                    Book = x.Book,
                    Article = x.Article,
                    Periodical = x.Periodical,
                    Theses = x.Theses,
                    Newspaper = x.Newspaper,
                    TypeOfOrder = x.TypeOfOrder,
                    StatusOfOrder = x.StatusOfOrder,
                    DateOfOrder = x.DateOfOrder,
                    DateOfReturn = x.DateOfReturn,
                    ReceiveDate = x.ReceiveDate
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

        public void UpdatePeriodical(Periodical periodical)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(periodical);

                dbContext.SaveChanges();
            }
        }
        private List<Periodical> GetPeriodicalsByCriteria(SearchFilledFields searchFilledFields)
        {
            if (searchFilledFields.SearchAuthor != null) return new List<Periodical>();
            using (var dbContext = new DbContext())
            {
                IQueryable<Periodical> intermediate = dbContext.Periodicals;

                if (searchFilledFields.SearchName != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchName))
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
                if (searchFilledFields.SearchName != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchName))
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchAuthor != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchAuthor))
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
                if (searchFilledFields.SearchName != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchName))
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchAuthor != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchAuthor))
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

                if (searchFilledFields.SearchName != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchAuthor))
                {
                    intermediate = intermediate.Where(x => x.Name == searchFilledFields.SearchName);
                }
                if (searchFilledFields.SearchAuthor != null && !String.IsNullOrWhiteSpace(searchFilledFields.SearchAuthor))
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

        #region User

        public Contract.User GetLoggedInUser()
        {
            //TODO: remove following. This is just to test service to service communication
            #region The following

            var channelFactory = new ChannelFactory<IUserManagementService>("IronMacbeth.UserManagementEndpoint");

            var serviceClient = channelFactory.CreateChannel();

            var time = serviceClient.GetCurrentTime();

            Console.WriteLine($"Asked UserManagement the time. It was {time}");

            #endregion

            var internalUser = GetLoggedInUserInternal();

            var contractUser = new Contract.User { Login = internalUser.Login, Name = internalUser.Name, Surname = internalUser.Surname, PhoneNumber = internalUser.PhoneNumber, UserRole = internalUser.UserRole };

            return contractUser;
        }

        #endregion

        #region File Storage

        public Guid AddFile(Stream fileStream)
        {
            var fileId = _fileStorageServiceClient.AddFile(fileStream);

            return fileId;
        }

        public Stream GetFile(Guid fileId)
        {
            // do not dispose stream here. It's disposed by WCF whenever it's done sending it to the client
            var fileStream = _fileStorageServiceClient.GetFile(fileId);

            return fileStream;
        }

        #endregion

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