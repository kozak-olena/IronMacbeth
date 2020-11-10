﻿using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.BookVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.EditBookVVM
{
    class Dispatch
    {

        IHandler[] Handlers;

        public Dispatch(IHandler[] handlers) => Handlers = handlers;

        public FilledFieldsInfo UnwrapObjectForEdit(object objectForUnwrapping)
        {
            var handler = Handlers.SingleOrDefault(x => x.CanHandleUnwrapping(objectForUnwrapping));
            if (handler != null)
            {
                return handler.Unwrap(objectForUnwrapping);
            }
            else
            {
                throw new InvalidOperationException("Can not handle operation");
            }

        }
        public void DeleteDispatch(object objectForUnwrapping)
        {
            var handler = Handlers.SingleOrDefault(x => x.CanHandleUnwrapping(objectForUnwrapping));
            if (handler != null)
            {
                handler.HandleDelete(objectForUnwrapping);
            }
            else
            {
                throw new InvalidOperationException("Can not handle operation");
            }
        }

        public void DispatchCreation(FilledFieldsInfo filledFieldsInfo)
        {

            var handler = Handlers.SingleOrDefault(x => x.CandHandle(filledFieldsInfo));
            if (handler != null)
            {
                handler.HandlerCreation(filledFieldsInfo);
            }
            else
            {
                throw new InvalidOperationException("Can not handle operation");
            }

        }

        public void DispatchUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {

            var handler = Handlers.SingleOrDefault(x => x.CandHandle(filledFieldsInfo));
            if (handler != null)
            {
                handler.HandleUpdate(filledFieldsInfo, objectForEdit);
            }
            else
            {
                throw new InvalidOperationException("Can not handle operation");
            }

        }


    }

    interface IHandler
    {
        void HandlerCreation(FilledFieldsInfo filledFieldsInfo);
        void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit);
        void HandleDelete(object objectForEdit);
        bool CandHandle(FilledFieldsInfo filledFieldsInfo);
        bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo);
        bool CanHandleUnwrapping(object objectForUnwrapping);
        FilledFieldsInfo Unwrap(object objectForUnwrapping);

    }

    #region BookHandler
    public class BookHandler : IHandler
    {
        public bool CandHandle(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Book")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Book")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanHandleUnwrapping(object objectForUnwrapping)
        {
            if (objectForUnwrapping is Book)           //?????
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleDelete(object objectForEdit)
        {
            Book article = objectForEdit as Book;
            MainViewModel.ServerAdapter.DeleteBook(article.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Book book = new Book
            {
                BitmapImage = filledFieldsInfo.BitmapImage,
                Name = filledFieldsInfo.Name,
                Author = filledFieldsInfo.Author,
                PublishingHouse = filledFieldsInfo.PublishingHouse,
                City = filledFieldsInfo.City,
                Year = filledFieldsInfo.Year,
                Pages = filledFieldsInfo.Pages,
                Availiability = filledFieldsInfo.Availiability,
                Location = filledFieldsInfo.Location,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                RentPrice = filledFieldsInfo.RentPrice,
                ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                Rating = filledFieldsInfo.Rating,
                Comments = filledFieldsInfo.Comments
            };
            MainViewModel.ServerAdapter.CreateBook(book);

        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Book book = objectForEdit as Book;
            book.Name = filledFieldsInfo.Name;
            book.Author = filledFieldsInfo.Author;
            book.PublishingHouse = filledFieldsInfo.PublishingHouse;                      //Handle exception
            book.City = filledFieldsInfo.City;
            book.Year = filledFieldsInfo.Year;
            book.Pages = filledFieldsInfo.Pages;
            book.Availiability = filledFieldsInfo.Availiability;
            book.Location = filledFieldsInfo.Location;
            book.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            book.ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice;
            book.RentPrice = filledFieldsInfo.RentPrice;
            book.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            book.Rating = filledFieldsInfo.Rating;
            book.Comments = filledFieldsInfo.Comments;

            if (!book.BitmapImage.Equals(filledFieldsInfo.BitmapImage))
            {
                book.BitmapImage = filledFieldsInfo.BitmapImage;
                book.ImageName = null;
            }
            if (book.Name != filledFieldsInfo.Name)
            {
                book.Name = filledFieldsInfo.Name;
                book.DescriptionName = null;
            }

            MainViewModel.ServerAdapter.UpdateBook(book);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Book book = (Book)objectForUnwrapping;
            filledFieldsInfo.BitmapImage = book.BitmapImage;
            filledFieldsInfo.Name = book.Name;
            filledFieldsInfo.Author = book.Author;
            filledFieldsInfo.PublishingHouse = book.PublishingHouse;                      //Handle exception
            filledFieldsInfo.City = book.City;
            filledFieldsInfo.Year = book.Year;
            filledFieldsInfo.Pages = book.Pages;
            filledFieldsInfo.ElectronicVersionPrice = book.ElectronicVersionPrice;
            filledFieldsInfo.RentPrice = book.RentPrice;
            filledFieldsInfo.Availiability = book.Availiability;
            filledFieldsInfo.Location = book.Location;
            filledFieldsInfo.TypeOfDocument = book.TypeOfDocument;
            filledFieldsInfo.ElectronicVersionFileName = book.ElectronicVersionFileName;
            filledFieldsInfo.Rating = book.Rating;
            filledFieldsInfo.Comments = book.Comments;
            return filledFieldsInfo;
        }
    }
    #endregion

    #region ArticleHandler
    public class ArticleHandler : IHandler
    {
        public bool CandHandle(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Article")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Article")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanHandleUnwrapping(object objectForUnwrapping)
        {
            if (objectForUnwrapping is Article)           //?????
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleDelete(object objectForEdit)
        {
            Article article = objectForEdit as Article;
            MainViewModel.ServerAdapter.DeleteArticle(article.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Article article = new Article
            {

                Name = filledFieldsInfo.Name,
                Author = filledFieldsInfo.Author,
                Year = filledFieldsInfo.Year,
                Pages = filledFieldsInfo.Pages,
                Availiability = filledFieldsInfo.Availiability,
                MainDocumentId = filledFieldsInfo.MainDocumentId,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                Rating = filledFieldsInfo.Rating,
                Comments = filledFieldsInfo.Comments
            };
            MainViewModel.ServerAdapter.CreateArticle(article);

        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Article article = objectForEdit as Article;

            article.Name = filledFieldsInfo.Name;
            article.Author = filledFieldsInfo.Author;
            article.Year = filledFieldsInfo.Year;
            article.Pages = filledFieldsInfo.Pages;
            article.Availiability = filledFieldsInfo.Availiability;
            article.MainDocumentId = filledFieldsInfo.MainDocumentId;
            article.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            article.ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice;
            article.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            article.Rating = filledFieldsInfo.Rating;
            article.Comments = filledFieldsInfo.Comments;

            MainViewModel.ServerAdapter.UpdateArticle(article);
        }


        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Article article = (Article)objectForUnwrapping;

            filledFieldsInfo.Name = article.Name;
            filledFieldsInfo.Author = article.Author;
            filledFieldsInfo.Year = article.Year;
            filledFieldsInfo.Pages = article.Pages;
            filledFieldsInfo.Availiability = article.Availiability;
            filledFieldsInfo.MainDocumentId = article.MainDocumentId;
            filledFieldsInfo.TypeOfDocument = article.TypeOfDocument;
            filledFieldsInfo.ElectronicVersionPrice = article.ElectronicVersionPrice;
            filledFieldsInfo.ElectronicVersionFileName = article.ElectronicVersionFileName;
            filledFieldsInfo.Rating = article.Rating;
            filledFieldsInfo.Comments = article.Comments;
            return filledFieldsInfo;
        }
    }

    #endregion

    #region PeriodicalHandler
    public class PeriodicalHandler : IHandler
    {
        public bool CandHandle(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Periodical")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Periodical")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanHandleUnwrapping(object objectForUnwrapping)
        {
            if (objectForUnwrapping is Periodical)           //?????
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleDelete(object objectForEdit)
        {
            Periodical article = objectForEdit as Periodical;
            MainViewModel.ServerAdapter.DeletePeriodical(article.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Periodical periodical = new Periodical
            {
                Name = filledFieldsInfo.Name,
                Year = filledFieldsInfo.Year,
                Pages = filledFieldsInfo.Pages,
                City = filledFieldsInfo.City,
                PublishingHouse = filledFieldsInfo.PublishingHouse,
                Availiability = filledFieldsInfo.Availiability,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                Location = filledFieldsInfo.Location,
                IssueNumber = filledFieldsInfo.IssueNumber,
                ResponsibleAuthors = filledFieldsInfo.ResponsibleAuthors,
                RentPrice = filledFieldsInfo.RentPrice,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice,
                Rating = filledFieldsInfo.Rating,
                Comments = filledFieldsInfo.Comments,
                ImageName = filledFieldsInfo.ImageName,
                DescriptionName = filledFieldsInfo.DescriptionName,
            };
            MainViewModel.ServerAdapter.CreatePeriodical(periodical);
        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Periodical periodical = objectForEdit as Periodical;
            periodical.Name = filledFieldsInfo.Name;
            periodical.Year = filledFieldsInfo.Year;
            periodical.Pages = filledFieldsInfo.Pages;
            periodical.City = filledFieldsInfo.City;
            periodical.PublishingHouse = filledFieldsInfo.PublishingHouse;
            periodical.Location = filledFieldsInfo.Location;
            periodical.Availiability = filledFieldsInfo.Availiability;
            periodical.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            periodical.IssueNumber = filledFieldsInfo.IssueNumber;
            periodical.ResponsibleAuthors = filledFieldsInfo.ResponsibleAuthors;
            periodical.RentPrice = filledFieldsInfo.RentPrice;
            periodical.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            periodical.ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice;
            periodical.Rating = filledFieldsInfo.Rating;
            periodical.Comments = filledFieldsInfo.Comments;
            periodical.ImageName = filledFieldsInfo.ImageName;
            periodical.DescriptionName = filledFieldsInfo.DescriptionName;

            MainViewModel.ServerAdapter.UpdatePeriodical(periodical);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Periodical periodical = (Periodical)objectForUnwrapping;

            filledFieldsInfo.Name = periodical.Name;
            filledFieldsInfo.Year = periodical.Year;
            filledFieldsInfo.City = periodical.City;
            filledFieldsInfo.Pages = periodical.Pages;
            filledFieldsInfo.Location = periodical.Location;
            filledFieldsInfo.PublishingHouse = periodical.PublishingHouse;
            filledFieldsInfo.Availiability = periodical.Availiability;
            filledFieldsInfo.TypeOfDocument = periodical.TypeOfDocument;
            filledFieldsInfo.IssueNumber = periodical.IssueNumber;
            filledFieldsInfo.ResponsibleAuthors = periodical.ResponsibleAuthors;
            filledFieldsInfo.RentPrice = periodical.RentPrice;
            filledFieldsInfo.ElectronicVersionFileName = periodical.ElectronicVersionFileName;
            filledFieldsInfo.ElectronicVersionPrice = periodical.ElectronicVersionPrice;
            filledFieldsInfo.Rating = periodical.Rating;
            filledFieldsInfo.Comments = periodical.Comments;
            filledFieldsInfo.ImageName = periodical.ImageName;
            filledFieldsInfo.DescriptionName = periodical.DescriptionName;
            return filledFieldsInfo;
        }
    }

    #endregion

    #region ThesisHandler
    public class ThesisHandler : IHandler
    {
        public bool CandHandle(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Thesis")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Thesis")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanHandleUnwrapping(object objectForUnwrapping)
        {
            if (objectForUnwrapping is Thesis)           //?????
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleDelete(object objectForEdit)
        {
            Thesis thesis = objectForEdit as Thesis;
            MainViewModel.ServerAdapter.DeleteThesis(thesis.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Thesis thesis = new Thesis
            {
                Name = filledFieldsInfo.Name,
                Year = filledFieldsInfo.Year,
                Author = filledFieldsInfo.Author,
                Pages = filledFieldsInfo.Pages,
                City = filledFieldsInfo.City,
                Availiability = filledFieldsInfo.Availiability,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                ResponsibleAuthors = filledFieldsInfo.ResponsibleAuthors,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice,
                Rating = filledFieldsInfo.Rating,
                Comments = filledFieldsInfo.Comments,
                DescriptionName = filledFieldsInfo.DescriptionName,
            };
            MainViewModel.ServerAdapter.CreateThesis(thesis);
        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Thesis thesis = objectForEdit as Thesis;
            thesis.Name = filledFieldsInfo.Name;
            thesis.Year = filledFieldsInfo.Year;
            thesis.Author = filledFieldsInfo.Author;
            thesis.Pages = filledFieldsInfo.Pages;
            thesis.City = filledFieldsInfo.City;
            thesis.Availiability = filledFieldsInfo.Availiability;
            thesis.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            thesis.ResponsibleAuthors = filledFieldsInfo.ResponsibleAuthors;
            thesis.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            thesis.ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice;
            thesis.Rating = filledFieldsInfo.Rating;
            thesis.Comments = filledFieldsInfo.Comments;
            thesis.ImageName = filledFieldsInfo.ImageName;
            thesis.DescriptionName = filledFieldsInfo.DescriptionName;

            MainViewModel.ServerAdapter.UpdateThesis(thesis);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Thesis thesis = (Thesis)objectForUnwrapping;

            filledFieldsInfo.Name = thesis.Name;
            filledFieldsInfo.Year = thesis.Year;
            filledFieldsInfo.Author = thesis.Author;
            filledFieldsInfo.City = thesis.City;
            filledFieldsInfo.Pages = thesis.Pages;
            filledFieldsInfo.Availiability = thesis.Availiability;
            filledFieldsInfo.TypeOfDocument = thesis.TypeOfDocument;
            filledFieldsInfo.ResponsibleAuthors = thesis.ResponsibleAuthors;
            filledFieldsInfo.ElectronicVersionFileName = thesis.ElectronicVersionFileName;
            filledFieldsInfo.ElectronicVersionPrice = thesis.ElectronicVersionPrice;
            filledFieldsInfo.Rating = thesis.Rating;
            filledFieldsInfo.Comments = thesis.Comments;
            filledFieldsInfo.ImageName = thesis.ImageName;
            filledFieldsInfo.DescriptionName = thesis.DescriptionName;
            return filledFieldsInfo;

        }
    }
    #endregion

    #region NewspaperHandler
    public class NewspaperHandler : IHandler
    {
        public bool CandHandle(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Newspaper")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CandHandleUpdate(FilledFieldsInfo filledFieldsInfo)
        {
            if (filledFieldsInfo.TypeOfDocument == "Newspaper")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanHandleUnwrapping(object objectForUnwrapping)
        {
            if (objectForUnwrapping is Newspaper)           //?????
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleDelete(object objectForEdit)
        {
            Newspaper newspaper = objectForEdit as Newspaper;
            MainViewModel.ServerAdapter.DeleteNewspaper(newspaper.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Newspaper newspaper = new Newspaper
            {
                Name = filledFieldsInfo.Name,
                Year = filledFieldsInfo.Year,
                IssueNumber = filledFieldsInfo.IssueNumber,
                RentPrice = filledFieldsInfo.RentPrice,
                City = filledFieldsInfo.City,
                ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice,
                Availiability = filledFieldsInfo.Availiability,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                Location = filledFieldsInfo.Location,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                Rating = filledFieldsInfo.Rating,
                Comments = filledFieldsInfo.Comments,
                DescriptionName = filledFieldsInfo.DescriptionName,
            };
            MainViewModel.ServerAdapter.CreateNewspaper(newspaper);
        }



        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Newspaper newspaper = objectForEdit as Newspaper;

            newspaper.Name = filledFieldsInfo.Name;
            newspaper.Year = filledFieldsInfo.Year;
            newspaper.IssueNumber = filledFieldsInfo.IssueNumber;
            newspaper.City = filledFieldsInfo.City;
            newspaper.RentPrice = filledFieldsInfo.RentPrice;
            newspaper.Availiability = filledFieldsInfo.Availiability;
            newspaper.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            newspaper.Location = filledFieldsInfo.Location;
            newspaper.ElectronicVersionPrice = filledFieldsInfo.ElectronicVersionPrice;
            newspaper.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            newspaper.Rating = filledFieldsInfo.Rating;
            newspaper.Comments = filledFieldsInfo.Comments;
            newspaper.DescriptionName = filledFieldsInfo.DescriptionName;

            MainViewModel.ServerAdapter.UpdateNewspaper(newspaper);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Newspaper newspaper = (Newspaper)objectForUnwrapping;

            filledFieldsInfo.Name = newspaper.Name;
            filledFieldsInfo.Year = newspaper.Year;
            filledFieldsInfo.City = newspaper.City;
            filledFieldsInfo.Availiability = newspaper.Availiability;
            filledFieldsInfo.IssueNumber = newspaper.IssueNumber;
            filledFieldsInfo.RentPrice = newspaper.RentPrice;
            filledFieldsInfo.TypeOfDocument = newspaper.TypeOfDocument;
            filledFieldsInfo.Location = newspaper.Location;
            filledFieldsInfo.ElectronicVersionPrice = newspaper.ElectronicVersionPrice;
            filledFieldsInfo.ElectronicVersionFileName = newspaper.ElectronicVersionFileName;
            filledFieldsInfo.Rating = newspaper.Rating;
            filledFieldsInfo.Comments = newspaper.Comments;
            filledFieldsInfo.ImageName = newspaper.ImageName;
            filledFieldsInfo.DescriptionName = newspaper.DescriptionName;
            return filledFieldsInfo;
        }
    }
    #endregion
}

