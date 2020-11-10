using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.BookVVM
{
    class BookItemViewModel
    {
        private Book _item;

        public BookItemViewModel(Book item)
        {
            _item = item;
        }

        public BitmapImage BitmapImage => _item.BitmapImage;
        public string Name => _item.NameOfBook;
        public string Author => _item.Author;
        public string Availiability => _item.Availiability;

        public string Location => _item.Location;

        public string TypeOfDocument => _item.TypeOfDocument;

        public string Rating => _item.Rating;
        public string RentPrice => _item.RentPrice;

        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;

        public int NumberOfOfferings => _item.NumberOfOfferings;

        public BookInfoViewModel MoreInfoVm => new BookInfoViewModel(_item);
    }
}

