namespace IronMacbeth.Client
{
    class RentPeriodical : Base
    {
        public int Id { get; set; }

        public int RentId { get; set; }

        public int PeriodicalId { get; set; }
        public string Date { get; set; }

        public int UserId { get; set; }   //user, who rented book

        private int _productPrice;
        public int ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                Modified = true;
            }
        }

        public new bool Modified { get; set; }
    }
}
