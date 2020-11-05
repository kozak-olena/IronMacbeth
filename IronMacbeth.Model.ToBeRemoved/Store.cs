using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class Store : Base<Store>, IDisplayable
    {
        [Database]
        public int Id { get; set; }

        [Database]
        public string Name { get; set; }
        [Database]
        public string Delivery { get; set; }
        [Database]
        public string OwnerId { get; set; }
        public override string DisplayString => $"Store: Id: {Id}";

        #region IDisplayable
        [Database]
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
