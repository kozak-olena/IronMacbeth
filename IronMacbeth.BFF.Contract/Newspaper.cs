﻿
namespace IronMacbeth.BFF.Contract
{
    public class Newspaper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public int Year { get; set; }

        public int Availiability { get; set; }

        public string Location { get; set; }

        public string TypeOfDocument { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public int IssueNumber { get; set; }

        public string RentPrice { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
        public string NameOfBook => Name;

       
    }
}
