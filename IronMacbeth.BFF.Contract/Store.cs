using System;

namespace IronMacbeth.BFF.Contract
{
    public class Store
    {
        public int Id { get; set; }
  
        public string Name { get; set; }
        
        public string Delivery { get; set; }
        
        public string OwnerId { get; set; }

        public string ImageName { get; set; }
    }
}
