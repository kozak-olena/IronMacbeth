using System;

namespace IronMacbeth.BFF.Contract
{
    public class StoreProcessor
    {
        public int Id { get; set; }
        
        public int StoreId { get; set; }
        
        public int ProcessorId { get; set; }
        
        public int ProductPrice { get; set; }

        public int ProductWarranty { get; set; }
    }
}