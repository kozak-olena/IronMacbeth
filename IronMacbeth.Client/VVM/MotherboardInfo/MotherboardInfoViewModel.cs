using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.MotherboardInfo
{
    public class MotherboardInfoViewModel : IPageViewModel
    {
            public string PageViewName => "MotherboardInfo";
            public void Update() { }

            public Motherboard Motherboard { get; }

            public MotherboardInfoViewModel(Motherboard motherboard)
            {
                Motherboard = motherboard;
            }
    }
}