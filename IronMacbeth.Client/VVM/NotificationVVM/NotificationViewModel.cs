using System.Windows.Media.Imaging;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.NotificationVVM
{
    public class NotificationViewModel
    {
        public string Name { get; }
        public string SellableName { get; }

        public BitmapImage Image { get; }

        public NotificationViewModel(Notification notification)
        {
            Image = notification.BitmapImage;
            Name = notification.Name;
            SellableName = notification.SellableName;
        }
    }
}