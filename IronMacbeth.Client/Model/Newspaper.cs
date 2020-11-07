using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    class Newspaper : Base, ISellable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Year { get; set; }
        public string Availiability { get; set; }   //electronic version???

        public string Location { get; set; }
        public string IssueNumber { get; set; }
        public string TypeOfDocument { get; set; }
        public byte[] ElectronicVersion { get; set; }

        public string ElectronicVersionFileName { get; set; }
        public int RentPrice { get; set; }

        public int ElectronicVersionPrice { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
        public string NameOfBook => Name;

        public string SellableType => "Newspaper";

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
