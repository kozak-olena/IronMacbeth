namespace IronMacbeth.BFF.Contract
{
    class RentPeriodical
    {
        public int Id { get; set; }

        public int RentId { get; set; }

        public int PeriodicalId { get; set; }

        public string Date { get; set; }

        public int UserId { get; set; }

        public int ProductPrice { get; set; }
    }
}
