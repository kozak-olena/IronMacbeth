using IronMacbeth.Client.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.MyOrdersVVM
{
    class MyOrdersViewModel : IPageViewModel, INotifyPropertyChanged
    {

        public string PageViewName => "My orders";

        private List<IDocumentViewModel> _items;

        public List<IDocumentViewModel> Items
        {
            get
            {
                // UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        public void Update() { }

        public IDocumentViewModel SelectedItem { get; set; }

        public MyOrdersViewModel() 
        {

        }
        public void CreateCoolection()
        {
            _items = new List<IDocumentViewModel>();
            //  _items.AddRange
            //(
            //    MainViewModel.ServerAdapter.GetAllBooks()
            //        .OrderByDescending(item => item.NumberOfOfferings)
            //        .Where(item => item.Name.ToLower().Contains(Search.ToLower()))
            //        .Select(x => new BookItemViewModel(x))
            //        .ToList()
            //);
        }


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
