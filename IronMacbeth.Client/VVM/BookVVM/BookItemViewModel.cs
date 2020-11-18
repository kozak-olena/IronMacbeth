using IronMacbeth.Client.VVM.EditBookVVM;
using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.BookVVM
{
    class BookItemViewModel : IDocumentViewModel
    {
        private Book _item;

        public BookItemViewModel(Book item)
        {
            _item = item;
        }

        public BitmapImage BitmapImage => _item.BitmapImage;
        public string Name => _item.NameOfBook;
        public string Author => _item.Author;
        public string PublishingHouse => _item.PublishingHouse;

        public string City => _item.City;
        public int Year => _item.Year;
        public int Pages => _item.Pages;
        public string ElectronicVersionFileName => _item.ElectronicVersionFileName;
        public string Comments => _item.Comments;
        public string Location => _item.Location;

        public string TypeOfDocument => _item.TypeOfDocument;

        public string Rating => _item.Rating;
        public string RentPrice => _item.RentPrice;

        public int NumberOfOfferings => _item.Availiability;

        public DocumentInfoViewModel MoreInfoVm => new DocumentInfoViewModel(_item);

        public object GetItem()
        {
            return _item;
        }
    }
}


