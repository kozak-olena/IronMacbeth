namespace IronMacbeth.BFF.Contract
{
    class RentBook
    {
        public int Id { get; set; }

        public int RentId { get; set; }

        public string Date { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public int ProductPrice { get; set; }
    }
}
