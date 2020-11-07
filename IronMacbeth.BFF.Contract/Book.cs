﻿namespace IronMacbeth.BFF.Contract
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string PublishingHouse { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }
        public string Availiability { get; set; }

        public string Location { get; set; }

        public string TypeOfDocument { get; set; }
        public string ElectronicVersion { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}