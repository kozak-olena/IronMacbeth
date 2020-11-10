using IronMacbeth.Client.VVM.ArticleInfoVVM;
 

namespace IronMacbeth.Client.VVM.ArticleItemVVM
{
    class ArticleItemViewModel
    {
        private Article _item;

        public ArticleItemViewModel(Article item)
        {
            _item = item;
        }

        public string Name => _item.Name;
        public string Author => _item.Author;
        public string Availiability => _item.Availiability;
        public string MainDocument => _item.MainDocumentId;
        public string TypeOfDocument => _item.TypeOfDocument;
        public string Rating => _item.Rating;
        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;

        public ArticleInfoViewModel MoreInfoVm => new ArticleInfoViewModel(_item);
    }
}
