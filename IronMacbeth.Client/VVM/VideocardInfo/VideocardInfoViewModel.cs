using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.VideocardInfo
{
    public class VideocardInfoViewModel : IPageViewModel
    {
            public string PageViewName => "ProcessorInfo";
            public void Update() { }

            public Videocard Videocard { get; }

            public VideocardInfoViewModel(Videocard videocard)
            {
                Videocard = videocard;
            }
    }
}