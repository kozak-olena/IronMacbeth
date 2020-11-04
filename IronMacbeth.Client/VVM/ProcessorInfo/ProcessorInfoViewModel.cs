using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.ProcessorInfo
{
    public class ProcessorInfoViewModel:IPageViewModel
    {
        public string PageViewName => "ProcessorInfo";
        public void Update() { }

        public Processor Processor { get; }

        public ProcessorInfoViewModel(Processor processor)
        {
            Processor = processor;
        }
    }
}