using IronMacbeth.Client.ViewModel;
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

        public void HandleDelete(object objectToDelete)
        {
            Book book = objectToDelete as Book;
            ServerAdapter.Instance.DeleteBook(book.Id);
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
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                
            };
            ServerAdapter.Instance.CreateBook(book);

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
            book.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
             

            if (!book.BitmapImage.Equals(filledFieldsInfo.BitmapImage))
            {
                book.BitmapImage = filledFieldsInfo.BitmapImage;
                book.ImageName = null;
            }
            

            ServerAdapter.Instance.UpdateBook(book);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Book book = (Book)objectForUnwrapping;
            filledFieldsInfo.BitmapImage = book.BitmapImage;
            filledFieldsInfo.Name = book.Name;
            filledFieldsInfo.Author = book.Author;
            filledFieldsInfo.PublishingHouse = book.PublishingHouse;                      //TODO:Handle exception
            filledFieldsInfo.City = book.City;
            filledFieldsInfo.Year = book.Year;
            filledFieldsInfo.Pages = book.Pages;
            filledFieldsInfo.RentPrice = book.RentPrice;
            filledFieldsInfo.Availiability = book.Availiability;
            filledFieldsInfo.Location = book.Location;
            filledFieldsInfo.TypeOfDocument = book.TypeOfDocument;
            filledFieldsInfo.ElectronicVersionFileName = book.ElectronicVersionFileName;
            
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
            if (objectForUnwrapping is Article)
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
            ServerAdapter.Instance.DeleteArticle(article.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Article article = new Article
            {

                Name = filledFieldsInfo.Name,
                Author = filledFieldsInfo.Author,
                Year = filledFieldsInfo.Year,
                Pages = filledFieldsInfo.Pages,
                MainDocumentId = filledFieldsInfo.MainDocumentId,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                 
            };
            ServerAdapter.Instance.CreateArticle(article);

        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Article article = objectForEdit as Article;

            article.Name = filledFieldsInfo.Name;
            article.Author = filledFieldsInfo.Author;
            article.Year = filledFieldsInfo.Year;
            article.Pages = filledFieldsInfo.Pages;
            article.MainDocumentId = filledFieldsInfo.MainDocumentId;
            article.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            article.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            

            ServerAdapter.Instance.UpdateArticle(article);
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
            filledFieldsInfo.MainDocumentId = article.MainDocumentId;
            filledFieldsInfo.TypeOfDocument = article.TypeOfDocument;
            filledFieldsInfo.ElectronicVersionFileName = article.ElectronicVersionFileName;
             
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
            if (objectForUnwrapping is Periodical)
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
            ServerAdapter.Instance.DeletePeriodical(article.Id);
        }

        public void HandlerCreation(FilledFieldsInfo filledFieldsInfo)
        {
            Periodical periodical = new Periodical
            {
                BitmapImage = filledFieldsInfo.BitmapImage,
                Name = filledFieldsInfo.Name,
                Year = filledFieldsInfo.Year,
                Pages = filledFieldsInfo.Pages,
                City = filledFieldsInfo.City,
                PublishingHouse = filledFieldsInfo.PublishingHouse,
                Availiability = filledFieldsInfo.Availiability,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                Location = filledFieldsInfo.Location,
                IssueNumber = filledFieldsInfo.IssueNumber,
                Responsible = filledFieldsInfo.Responsible,
                RentPrice = filledFieldsInfo.RentPrice,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                
                ImageName = filledFieldsInfo.ImageName,

            };
            ServerAdapter.Instance.CreatePeriodical(periodical);
        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Periodical periodical = objectForEdit as Periodical;
            periodical.Name = filledFieldsInfo.Name;
            periodical.Year = filledFieldsInfo.Year;
            periodical.Pages = filledFieldsInfo.Pages;
            periodical.City = filledFieldsInfo.City;
            periodical.BitmapImage = filledFieldsInfo.BitmapImage;
            periodical.PublishingHouse = filledFieldsInfo.PublishingHouse;
            periodical.Location = filledFieldsInfo.Location;
            periodical.Availiability = filledFieldsInfo.Availiability;
            periodical.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            periodical.IssueNumber = filledFieldsInfo.IssueNumber;
            periodical.Responsible = filledFieldsInfo.Responsible;
            periodical.RentPrice = filledFieldsInfo.RentPrice;
            periodical.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            
            periodical.ImageName = filledFieldsInfo.ImageName;

            if (!periodical.BitmapImage.Equals(filledFieldsInfo.BitmapImage))
            {
                periodical.BitmapImage = filledFieldsInfo.BitmapImage;
                periodical.ImageName = null;
            }
            

            ServerAdapter.Instance.UpdatePeriodical(periodical);
        }

        public FilledFieldsInfo Unwrap(object objectForUnwrapping)
        {
            if (!CanHandleUnwrapping(objectForUnwrapping)) { throw new InvalidOperationException(); }
            FilledFieldsInfo filledFieldsInfo = new FilledFieldsInfo();
            Periodical periodical = (Periodical)objectForUnwrapping;
            filledFieldsInfo.BitmapImage = periodical.BitmapImage;
            filledFieldsInfo.Name = periodical.Name;
            filledFieldsInfo.Year = periodical.Year;
            filledFieldsInfo.City = periodical.City;
            filledFieldsInfo.Pages = periodical.Pages;
            filledFieldsInfo.Location = periodical.Location;
            filledFieldsInfo.PublishingHouse = periodical.PublishingHouse;
            filledFieldsInfo.Availiability = periodical.Availiability;
            filledFieldsInfo.TypeOfDocument = periodical.TypeOfDocument;
            filledFieldsInfo.IssueNumber = periodical.IssueNumber;
            filledFieldsInfo.Responsible = periodical.Responsible;
            filledFieldsInfo.RentPrice = periodical.RentPrice;
            filledFieldsInfo.ElectronicVersionFileName = periodical.ElectronicVersionFileName;
             
            filledFieldsInfo.ImageName = periodical.ImageName;
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
            if (objectForUnwrapping is Thesis)
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
            ServerAdapter.Instance.DeleteThesis(thesis.Id);
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
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                Responsible = filledFieldsInfo.Responsible,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                
            };
            ServerAdapter.Instance.CreateThesis(thesis);
        }

        public void HandleUpdate(FilledFieldsInfo filledFieldsInfo, object objectForEdit)
        {
            Thesis thesis = objectForEdit as Thesis;
            thesis.Name = filledFieldsInfo.Name;
            thesis.Year = filledFieldsInfo.Year;
            thesis.Author = filledFieldsInfo.Author;
            thesis.Pages = filledFieldsInfo.Pages;
            thesis.City = filledFieldsInfo.City;
            thesis.TypeOfDocument = filledFieldsInfo.TypeOfDocument;
            thesis.Responsible = filledFieldsInfo.Responsible;
            thesis.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
            


            ServerAdapter.Instance.UpdateThesis(thesis);
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
            filledFieldsInfo.TypeOfDocument = thesis.TypeOfDocument;
            filledFieldsInfo.Responsible = thesis.Responsible;
            filledFieldsInfo.ElectronicVersionFileName = thesis.ElectronicVersionFileName;
             
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
            if (objectForUnwrapping is Newspaper)
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
            ServerAdapter.Instance.DeleteNewspaper(newspaper.Id);
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
                Availiability = filledFieldsInfo.Availiability,
                TypeOfDocument = filledFieldsInfo.TypeOfDocument,
                Location = filledFieldsInfo.Location,
                ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName,
                 
            };
            ServerAdapter.Instance.CreateNewspaper(newspaper);
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
            newspaper.ElectronicVersionFileName = filledFieldsInfo.ElectronicVersionFileName;
             
            ServerAdapter.Instance.UpdateNewspaper(newspaper);
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
            filledFieldsInfo.ElectronicVersionFileName = newspaper.ElectronicVersionFileName;
            
            return filledFieldsInfo;
        }
    }
    #endregion
}

