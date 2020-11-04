using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    public interface IDisplayable
    {
        string ImageName { get; set; }
        BitmapImage BitmapImage { get; set; }
    }
}