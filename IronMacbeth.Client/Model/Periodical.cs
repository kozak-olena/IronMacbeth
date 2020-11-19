using IronMacbeth.Client.VVM;
using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    public class Periodical : Base, IDisplayable, IAvailiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Responsible { get; set; }

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public int Year { get; set; }

        public int Pages { get; set; }

        public int Availiability { get; set; }

        public string Location { get; set; }

        public int IssueNumber { get; set; }

        public string RentPrice { get; set; }

        public byte[] ElectronicVersion { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public string TypeOfDocument { get; set; }


        public string ImageName { get; set; }

        public string DescriptionName { get; set; }

        public string NameOfBook => Name;


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

        public int GetAvailibility()
        {
            return Availiability;
        }
    }
}
