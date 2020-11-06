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
        public string PublishingHouse => _item.PublishingHouse;
        public string City => _item.City;

        public string Year => _item.Year;

        public string Pages => _item.Pages;
        public string Availiability => _item.Availiability;

        public string Location => _item.Location;

        public string TypeOfDocument => _item.TypeOfDocument;
        public string ElectronicVersion => _item.ElectronicVersion;

        public string Rating => _item.Rating;

        public string Comments => _item.Comments;
        // public int AveragePrice => _item.AveragePrice;
        //public int MinPrice => _item.MinPrice;
        // public int MaxPrice => _item.MaxPrice;
        public int NumberOfOfferings => _item.NumberOfOfferings;
        //public string Model => _item.Model;
        // public string MPN => _item.MPN;
        //  public string Standart => _item.Standart;
        //public string Timings => _item.Timings;
        //public string Voltage => _item.Voltage;

        public BookInfoViewModel MoreInfoVm => new BookInfoViewModel(_item);
    }
}

