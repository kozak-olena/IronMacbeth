using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.StoreVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.ArticleInfoVVM
{
    public class ArticleInfoViewModel : IPageViewModel
    {
        public string PageViewName => "ArticleInfo";
        public void Update() { }

        public Article Article { get; }

        public List<StoreSellableItemViewModel> Stores { get; }

        public ArticleInfoViewModel(Article article)
        {
            Article = article;

            Stores =
                MainViewModel.ServerAdapter.GetAllStoresSellingMemory(article.Id)
                    .Select(x => new StoreSellableItemViewModel(x.Store, article, x.StoreMemory))          //
                    .ToList();
        }
    }
}
