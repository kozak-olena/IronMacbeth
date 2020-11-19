using IronMacbeth.Client.VVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    public class Newspaper : Base, IAvailiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public int Year { get; set; }

        public int Availiability { get; set; }

        public string Location { get; set; }

        public int IssueNumber { get; set; }

        public string TypeOfDocument { get; set; }

        public byte[] ElectronicVersion { get; set; }

        public string ElectronicVersionFileName { get; set; }
        public string RentPrice { get; set; }

        public string NameOfBook => Name;

        public int GetAvailibility()
        {
            return Availiability;
        }
    }
}
