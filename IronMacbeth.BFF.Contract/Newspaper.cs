using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    class Newspaper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Year { get; set; }
        public string Availiability { get; set; }

        public string Location { get; set; }

        public string TypeOfDocument { get; set; }
        public string ElectronicVersion { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
        public string NameOfBook => Name;

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}
