using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    public class Notification
    {
        public string Name { get; set; }
        public string SellableName { get; set; }

        public string ImageName { get; set; }

        public byte[] BitmapBytes { get; set; }

        private bool _bitmapConverted;

        private BitmapImage _bitmapImage;
        //select imagename From videocard where id=(SELECT distinct videocardid FROM storevideocard);
        public BitmapImage BitmapImage
        {
            get
            {
                if (!_bitmapConverted)
                {
                    Bitmap bitmap = ServerAdapter.Deserialize(BitmapBytes) as Bitmap;
                    _bitmapImage = ServerAdapter.BitmapToBitmapImage(bitmap);
                    _bitmapConverted = true;
                }
                return _bitmapImage;
            }
        }
    }
}