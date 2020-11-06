namespace IronMacbeth.BFF.Contract
{
    public class Motherboard
    {
        public int Id { get; set; }
        
        public int DIMM { get; set; }
        
        public int LAN { get; set; }
        
        public int USB { get; set; }

        
        public string CPUSocket { get; set; }
        
        public string Northbridge { get; set; }
        
        public string Southbridge { get; set; }
        
        public string GraphicalInterface { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }
        
        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}