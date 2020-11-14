
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
using System.Windows;
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
                MessageBox.Show($"Book \"{book.Name}\" added to your orders", "Book added", MessageBoxButton.OK,
                      MessageBoxImage.Information);
            }
            else if (_selectedItem is Article)
            {
                Article article = (Article)_selectedItem;
                order.ArticleId = article.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
                MessageBox.Show($"Article \"{article.Name}\" added to your orders", "Article added", MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }
            else if (_selectedItem is Periodical)
            {
                Periodical periodical = (Periodical)_selectedItem;
                order.PeriodicalId = periodical.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
                MessageBox.Show($"Periodical \"{periodical.Name}\" added to your orders", "Periodical added", MessageBoxButton.OK,
                  MessageBoxImage.Information);
            }
            else if (_selectedItem is Newspaper)
            {
                Newspaper newspaper = (Newspaper)_selectedItem;
                order.NewspaperId = newspaper.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
                MessageBox.Show($"Newspaper \"{newspaper.Name}\" added to your orders", "Newspaper added", MessageBoxButton.OK,
                 MessageBoxImage.Information);
            }
            else if (_selectedItem is Thesis)
            {
                Thesis theses = (Thesis)_selectedItem;
                order.ThesesID = theses.Id;
                order.UserLogin = "me";
                MainViewModel.ServerAdapter.CreateOrder(order);
                MessageBox.Show($"Theses \"{theses.Name}\" added to your orders", "Theses added", MessageBoxButton.OK,
                MessageBoxImage.Information);
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
