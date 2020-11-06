using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client
{
    [Serializable]
    public class Store : Base, IDisplayable
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; }
        
        public string Delivery { get; set; }
        
        public string OwnerId { get; set; }

        #region IDisplayable
        
        public string ImageName { get; set; }

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; }
        }

        [NonSerialized]
        private BitmapImage _bitmapImage;
        #endregion
    }
}
