using IronMacbeth.Client.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IronMacbeth.Client.VVM.SearchPageViewModel
{
    public class SearchViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Search";

        public void Update()
        {
        }

        public ICommand ExitCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand DeleteAllComand { get; }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
