namespace IronMacbeth.BFF.Contract
{
    public class Videocard
    {
        public int Id { get; set; }
        
        public int Memory { get; set; }
        
        public int GPUFrequency { get; set; }
        
        public int MemoryFrequency { get; set; }
        
        public int Bus { get; set; }

        public string GPU { get; set; }
        
        public string MemoryType { get; set; }
        
        public string Interface { get; set; }
        
        public string Cooling { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }
        
        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}