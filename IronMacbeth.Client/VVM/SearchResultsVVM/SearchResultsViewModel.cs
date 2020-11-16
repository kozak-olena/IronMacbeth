
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.ArticleItemVVM;
using IronMacbeth.Client.VVM.BookVVM;
using IronMacbeth.Client.VVM.NewspaperItemVVM;
using IronMacbeth.Client.VVM.PeriodicalItemVVM;
using IronMacbeth.Client.VVM.ThesisItemVVM;
using System;
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

        public List<IDocumentViewModel> Items { get; private set; }

        public ICommand AddtoMyOrdersCommand { get; }
        public ICommand OrderToReadingRoomCommand { get; }


        private object _selectedItem;

        public Order order;

        public SearchResultsDispatch SearchResultsDispatch;
        public void AddToMyOrdersMethod(object parameter)
        {
            //order = new Order();
            _selectedItem = SelectedItem.GetItem();
            SearchResultsDispatch = new SearchResultsDispatch(_selectedItem);
        }

        public void OrderToReadingRoomMethod(object parameter)
        {
            _selectedItem = SelectedItem.GetItem();
            SearchResultsDispatch = new SearchResultsDispatch(_selectedItem);
        }

        private SearchFilledFields _searchFilledFields;
        public SearchResultsViewModel(SearchFilledFields searchFilledFields)
        {
            _searchFilledFields = searchFilledFields;
            AddtoMyOrdersCommand = new RelayCommand(AddToMyOrdersMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
            OrderToReadingRoomCommand = new RelayCommand(OrderToReadingRoomMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public IDocumentViewModel SelectedItem { get; set; }

        public bool CanExecuteMaintenanceMethods(object parameter)
        {
            return SelectedItem != null;
        }

        public void ShowCollection()
        {
            Items = new List<IDocumentViewModel>();
            DocumentsSearchResults documentsSearchResults = ServerAdapter.Instance.SearchDocuments(_searchFilledFields);

            Items.AddRange(documentsSearchResults.Books.Select(x => new BookItemViewModel(x)));
            Items.AddRange(documentsSearchResults.Articles.Select(x => new ArticleItemViewModel(x)));
            Items.AddRange(documentsSearchResults.Periodicals.Select(x => new PeriodicalItemViewModel(x)));
            Items.AddRange(documentsSearchResults.Newspapers.Select(x => new NewspaperItemViewModel(x)));
            Items.AddRange(documentsSearchResults.Theses.Select(x => new ThesisItemViewModel(x)));
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
