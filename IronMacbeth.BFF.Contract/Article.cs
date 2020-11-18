using System;

namespace IronMacbeth.BFF.Contract
{
    public class Article
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public int Pages { get; set; }


        public string MainDocumentId { get; set; }

        public string TypeOfDocument { get; set; }

        public Guid? ElectronicVersionFileId { get; set; }

        public string Rating { get; set; }

        public string Comments { get; set; }
    }
}
