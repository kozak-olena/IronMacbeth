
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.ArticleItemVVM;
using IronMacbeth.Client.VVM.BookVVM;
using IronMacbeth.Client.VVM.NewspaperItemVVM;
using IronMacbeth.Client.VVM.PeriodicalItemVVM;
using IronMacbeth.Client.VVM.ThesisItemVVM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IronMacbeth.Client.VVM.SearchResultsVVM
{
    public class SearchResultsViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "SearchResults";
        public void Update() { ShowCollection(); }

        private List<IDocumentViewModel> _items;
        public List<IDocumentViewModel> Items
        {
            get
            {
                //UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        public ICommand AddtoMyOrdersCommand { get; }
        public ICommand OrderToReadingRoomCommand { get; }

        //private Order _order;

        //public Order Order
        //{
        //    get { return _order; }
        //    set
        //    {
        //        _order = value;
        //    }
        //}

        private object _selectedItem;
        public Order order;

        public void AddtoMyOrdersMethod(object parameter)
        {
            _selectedItem = SelectedItem.GetItem();
            order = new Order();
            if (_selectedItem is Book)
            {
                Book book = (Book)_selectedItem;
                order.BookId = book.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
            }
            else if (_selectedItem is Article)
            {
                Article book = (Article)_selectedItem;
                order.ArticleId = book.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
            }
            else if (_selectedItem is Periodical)
            {
                Periodical book = (Periodical)_selectedItem;
                order.PeriodicalId = book.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
            }
            else if (_selectedItem is Newspaper)
            {
                Newspaper book = (Newspaper)_selectedItem;
                order.NewspaperId = book.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
            }
            else if (_selectedItem is Thesis)
            {
                Thesis book = (Thesis)_selectedItem;
                order.ThesesID = book.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
            }

        }

        public void OrderToReadingRoomMethod(object parameter)
        {
        }

        private SearchFilledFields _searchFilledFields;
        public SearchResultsViewModel(SearchFilledFields searchFilledFields)
        {
            _searchFilledFields = searchFilledFields;
            AddtoMyOrdersCommand = new RelayCommand(AddtoMyOrdersMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            OrderToReadingRoomCommand = new RelayCommand(OrderToReadingRoomMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public IDocumentViewModel SelectedItem { get; set; }

        public bool CanExecuteMaintenanceMethods(object parameter)
        {
            return SelectedItem != null;
        }

        public void ShowCollection()
        {
            _items = new List<IDocumentViewModel>();
            DocumentsSearchResults documentsSearchResults = MainViewModel.ServerAdapter.SearchDocuments(_searchFilledFields);

            _items.AddRange(documentsSearchResults.Books.Select(x => new BookItemViewModel(x)));
            _items.AddRange(documentsSearchResults.Articles.Select(x => new ArticleItemViewModel(x)));
            _items.AddRange(documentsSearchResults.Periodicals.Select(x => new PeriodicalItemViewModel(x)));
            _items.AddRange(documentsSearchResults.Newspapers.Select(x => new NewspaperItemViewModel(x)));
            _items.AddRange(documentsSearchResults.Theses.Select(x => new ThesisItemViewModel(x)));
            OnPropertyChanged(nameof(Items));
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
