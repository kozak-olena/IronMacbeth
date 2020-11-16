using IronMacbeth.Client.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client
{
    public class ReadingRoomOrder : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string UserLogin { get; set; }

        public Book Book { get; set; }

        public Article Article { get; set; }

        public Periodical Periodical { get; set; }

        public Newspaper Newspaper { get; set; }

        public Thesis Thesis { get; set; }

        public string TypeOfOrder { get; set; }

        public string StatusOfOrder { get; set; }

        public DateTime DateOfOrder { get; set; }

        public DateTime ReceiveDate { get; set; }

        public object GetOrderedItem()
        {
            return Book ?? Article ?? Periodical ?? Newspaper ?? (object)Thesis;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion}
    }
}
