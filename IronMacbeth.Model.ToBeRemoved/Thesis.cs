using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    class Thesis : Base<Thesis>, ISellable    //(IRENTABLE)
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Responsible { get; set; } //Крицевий О. Т.; Чернівецький держ. ун-т// Євген Нахлік : НАНУ ; Ін-т літератури ім. Т.Г. Шевченка

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }
        public string Availiability { get; set; }   //electronic version???

        public string Location { get; set; }

        public string TypeOfDocument { get; set; }
        public string ElectronicVersion { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
        public string NameOfBook => Name;

        public string SellableType => "Thesis";

        public override string DisplayString =>
            $"Thesis: Id: {Id}";   // public override string DisplayString => $"Book: Name: {Name}";

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
