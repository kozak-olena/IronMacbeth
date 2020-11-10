using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    public class Article : Base, ISellable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }

        public string Availiability { get; set; }

        public string MainDocumentId { get; set; }   //book, in which the article is published// foreighn key? //rent book, not article

        public string TypeOfDocument { get; set; }

        public byte[] ElectronicVersion { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public string ElectronicVersionPrice { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string NameOfArticle => Name;

        public string SellableType => "Article";

        public string ImageName { get; set; }
        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; }
        }

        public int NumberOfOfferings   //the same as availability?
        {
            get { return 0; }
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
    }
}
