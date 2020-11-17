namespace IronMacbeth.Client.VVM.ArticleInfoVVM
{
    public class ArticleInfoViewModel : IPageViewModel
    {
        public string PageViewName => "ArticleInfo";
        public void Update() { }

        public Article Article { get; }

        public ArticleInfoViewModel(Article article)
        {
            Article = article;
        }
    }
}
