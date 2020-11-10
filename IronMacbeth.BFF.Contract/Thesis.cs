using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    public class Thesis
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string ResponsibleAuthors { get; set; }

        public string City { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }

        public string Availiability { get; set; }



        public string TypeOfDocument { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public string ElectronicVersionPrice { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}
