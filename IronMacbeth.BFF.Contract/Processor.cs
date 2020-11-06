using System;

namespace IronMacbeth.BFF.Contract
{
    public class Processor
    {
        public int Id { get; set; }
        
        public int NumberOfCores { get; set; }
        
        public int Lithography { get; set; }
        
        public int TDP { get; set; }
        
        public int Level2Cache { get; set; }
        
        public int Level3Cache { get; set; }

        
        public string BaseFrequency { get; set; }
        
        public string TurboFrequency { get; set; }
        
        public string Socket { get; set; }
        
        public string ProcessorCore { get; set; }
        
        public string ProcessorGraphics { get; set; }
        
        public string Model { get; set; }
        
        public string MPN { get; set; }

        public string ImageName { get; set; }

        public string DescriptionName { get; set; }
    }
}