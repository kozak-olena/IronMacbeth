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

       // public MemoryInfoViewModel MoreInfoVm => new MemoryInfoViewModel(_item);

       
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

