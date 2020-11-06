using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    public interface IDisplayable
    {
        string ImageName { get; set; }
        BitmapImage BitmapImage { get; set; }
    }
}