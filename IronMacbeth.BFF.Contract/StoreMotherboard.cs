using System;

namespace IronMacbeth.BFF.Contract
{
    public class StoreMotherboard
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int MotherboardId { get; set; }

        public int ProductPrice { get; set; }

        public int ProductWarranty { get; set; }
    }
}