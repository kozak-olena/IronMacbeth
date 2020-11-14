
namespace IronMacbeth.BFF.Contract
{
    public class Newspaper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public int Year { get; set; }

        public string Availiability { get; set; }

        public string Location { get; set; }

        public string TypeOfDocument { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public string IssueNumber { get; set; }

        public string RentPrice { get; set; }

        public string ElectronicVersionPrice { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
        public string NameOfBook => Name;

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}
