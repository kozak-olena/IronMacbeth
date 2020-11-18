using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    public class Thesis : Base
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Responsible { get; set; }

        public string City { get; set; }

        public int Year { get; set; }

        public int Pages { get; set; }

        public string TypeOfDocument { get; set; }

        public byte[] ElectronicVersion { get; set; }

        public string ElectronicVersionFileName { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }

        public string NameOfBook => Name;

    }
}
