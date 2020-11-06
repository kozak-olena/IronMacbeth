using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    class Periodical : Base, ISellable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ResponsibleAuthors { get; set; }  //Київ. нац. ун-т ім. Тараса Шевченка ; голов. ред. Л.І. Шевченко ; редкол.: Ф.С. Бацевич, А. Брацкі, П.Ю. Гриценко [та ін.]

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }
        public string Availiability { get; set; }    

        public string Location { get; set; }

        public string IssueNumber { get; set; }

        public string ElectronicVersion { get; set; }

        public string TypeOfDocument { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string NameOfBook => Name;

        public string SellableType => "Periodical";

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
