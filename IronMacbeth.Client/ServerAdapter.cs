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

        #region Order
        public void CreateOrder(Internal.Order orderInfo)
        {
            var contractOrder = MapInternalToContractCreateOrder(orderInfo);
            _proxy.CreateOrder(contractOrder);
        }
        private Contract.CreateOrder MapInternalToContractCreateOrder(Internal.Order orderInfo)
        {
            return new Contract.CreateOrder
            {
                Id = orderInfo.Id,
                UserLogin = orderInfo.UserLogin,
                BookId = orderInfo.Book?.Id,
                ArticleId = orderInfo.Article?.Id,
                PeriodicalId = orderInfo.Periodical?.Id,
                NewspaperId = orderInfo.Newspaper?.Id,
                ThesesId = orderInfo.Thesis?.Id,
                TypeOfOrder = orderInfo.TypeOfOrder,
                StatusOfOrder = orderInfo.StatusOfOrder,
                ReceiveDate = orderInfo.ReceiveDate,
                DateOfOrer = orderInfo.DateOfOrder,
                DateOfReturn = orderInfo.DateOfReturn
            };
        }

        public List<Internal.Order> GetAllOrders()
        {
            var orders = _proxy.GetAllOrders();

            var internalOrders = orders.Select(MapContractToInternalOrder).ToList();

            return internalOrders;
        }


        private Internal.Order MapContractToInternalOrder(Contract.Order order)
        {
            return new Internal.Order
            {
                Id = order.Id,
                UserLogin = order.UserLogin,
                TypeOfOrder = order.TypeOfOrder,
                StatusOfOrder = order.StatusOfOrder,
                DateOfOrder = order.DateOfOrder.ToLocalTime(),
                DateOfReturn = order.DateOfReturn.ToLocalTime(),
                ReceiveDate = order.ReceiveDate.ToLocalTime(),
                Book = order.Book != null ? MapContractToInternalBook(order.Book) : null,
                Article = order.Article != null ? MapContractToInternalArticle(order.Article) : null,
                Periodical = order.Periodical != null ? MapContractToInternalPeriodical(order.Periodical) : null,
                Newspaper = order.Newspaper != null ? MapContractToInternalNewspaper(order.Newspaper) : null,
                Thesis = order.Theses != null ? MapContractToInternalTheses(order.Theses) : null
            };
        }

        public void DeleteOrder(int id)
        {
            _proxy.DeleteOrder(id);
        }

        public void UpdateOrder(Internal.Order order, Internal.SpecifiedOrderFields specifyOrderFields)
        {
            var contractOrder = MapInternalToContractUpdateOrder(order);
            var contractSpecifiedOrderFields = MapInternalToContractOrderFields(specifyOrderFields);
            _proxy.UpdateOrder(contractOrder, contractSpecifiedOrderFields);
        }


        private Contract.Order MapInternalToContractUpdateOrder(Internal.Order order)
        {
            return new Contract.Order
            {
                Id = order.Id,
                UserLogin = order.UserLogin,
                BookId = order.Book?.Id,
                ArticleId = order.Article?.Id,
                PeriodicalId = order.Periodical?.Id,
                NewspaperId = order.Newspaper?.Id,
                ThesesId = order.Thesis?.Id,
                TypeOfOrder = order.TypeOfOrder,
                //StatusOfOrder = order.StatusOfOrder,
                //ReceiveDate = order.ReceiveDate,
                DateOfOrder = order.DateOfOrder,
                DateOfReturn = order.DateOfReturn
            };
        }
        private Contract.SpecifiedOrderFields MapInternalToContractOrderFields(Internal.SpecifiedOrderFields specifyOrderFields)
        {
            return new Contract.SpecifiedOrderFields
            {
                DateOfReturning = specifyOrderFields.DateOfReturning,
                Status = specifyOrderFields.Status,
                ReceiveDate = specifyOrderFields.ReceiveDate
            };
        }





        #endregion



        #region SearchDocument
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
            documents.Theses = documentsSearchResults.Theses?.Select(MapContractToInternalTheses).ToList() ?? new List<Thesis>();

            return documents;
        }
        #endregion

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

            var internalThesises = thesises.Select(MapContractToInternalTheses).ToList();

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

        private Internal.Thesis MapContractToInternalTheses(Contract.Thesis thesis)
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

        private Internal.User MapContractToInternalUser(Contract.User user)
        {
            return new Internal.User
            {
                Login = user.Login,
                UserRole = user.UserRole
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