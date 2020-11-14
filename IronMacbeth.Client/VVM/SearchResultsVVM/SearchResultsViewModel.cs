
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

        private SearchFilledFields _searchFilledFields;
        public SearchResultsViewModel(SearchFilledFields searchFilledFields)
        {
            _searchFilledFields = searchFilledFields;
        }

        public IDocumentViewModel SelectedItem { get; set; }

        public void ShowCollection()
        {
            _items = new List<IDocumentViewModel>();
            DocumentsSearchResults documentsSearchResults = ServerAdapter.Instance.SearchDocuments(_searchFilledFields);

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
