using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    public class Book : Base<Book>, IInformationContainer, ISellable
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }
        public string Availiability { get; set; }   //electronic version???
        public string Location { get; set; }
        public string NameOfBook => Name;
        public string SellableType => "Book";
        public string InfoContainerKey => "Book";

        public override string DisplayString =>
            $"Book: Id: {Id}";   // public override string DisplayString => $"Book: Name: {Name}";

        public string ImageName { get; set; }
        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; }
        }
        [NonSerialized]
        private BitmapImage _bitmapImage;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [NonSerialized]
        private string _description;

        public string DescriptionName { get; set; }

        public int NumberOfOfferings   //the same as availability?
        {
            get { return 0; }
        }
    }
}
