 

namespace IronMacbeth.Model.ToBeRemoved
{
    class RentPeriodical : Base<RentPeriodical>
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

        public override string DisplayString =>
           $"RentPeriodical: RentId: {RentId} PeriodicalId: {PeriodicalId}";   //TODO: id of order or rent
    }
}
