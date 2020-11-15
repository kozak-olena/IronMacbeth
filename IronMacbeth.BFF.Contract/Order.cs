using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
    public class Order
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public Book Book { get; set; }
        public Article Article { get; set; }
        public Periodical Periodical { get; set; }
        public Newspaper Newspaper { get; set; }
        public Thesis Theses { get; set; }

        
        public string TypeOfOrder { get; set; } = "Issueing document";
    }
}
