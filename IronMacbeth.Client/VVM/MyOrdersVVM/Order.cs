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
    public class Order : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public int BookId { get; set; }
        public int ArticleId { get; set; }
        public int PeriodicalId { get; set; }
        public int NewspaperId { get; set; }
        public int ThesesID { get; set; }

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
