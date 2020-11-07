namespace IronMacbeth.BFF.Contract
{
    class Periodical
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ResponsibleAuthors { get; set; }

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }
        public string Availiability { get; set; }    

        public string Location { get; set; }

        public string IssueNumber { get; set; }

        public string ElectronicVersionFileName { get; set; }
        public int RentPrice { get; set; }

        public int ElectronicVersionPrice { get; set; }

        public string TypeOfDocument { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}
