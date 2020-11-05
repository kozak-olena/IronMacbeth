using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Model.ToBeRemoved
{
    class Article : Base<Article>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Year { get; set; }

        public string Pages { get; set; }

        public string Availiability { get; set; }

        public string MainDocumentId { get; set; }   //book, in which the article is published// foreighn key? //rent book, not article

        public string TypeOfDocument { get; set; }

        public string NameOfArticle => Name;

        public string SellableType => "Article";
        public string InfoContainerKey => "Article";

        public override string DisplayString => throw new NotImplementedException();
    }
}
