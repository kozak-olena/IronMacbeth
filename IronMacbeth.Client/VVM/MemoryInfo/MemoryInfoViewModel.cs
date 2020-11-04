using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MemoryInfo
{
    public class MemoryInfoViewModel : IPageViewModel
    {
            public string PageViewName => "MemoryInfo";
            public void Update() { }

            public Memory Memory { get; }

            public MemoryInfoViewModel(Memory memory)
            {
                Memory = memory;
            }
    }
}