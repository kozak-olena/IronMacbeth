using System;

namespace IronMacbeth.BFF.Contract
{
    public class StoreMemory
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public int MemoryId { get; set; }

        public int ProductPrice { get; set; }

        public int ProductWarranty { get; set; }
    }
}