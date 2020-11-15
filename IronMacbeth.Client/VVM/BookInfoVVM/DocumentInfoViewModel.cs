using IronMacbeth.Client.VVM.EditBookVVM;

namespace IronMacbeth.Client.VVM.BookVVM
{
    public  class DocumentInfoViewModel : IPageViewModel
    {
        public string PageViewName => "BookInfo";
        public void Update() { }

        public FilledFieldsInfo FilledFieldsInfo { get; set; }

        private object _objectForEdit;

        private Dispatch _dispatch;
        public DocumentInfoViewModel(object item)
        {
            _dispatch = new Dispatch(new IHandler[] { new BookHandler(), new ArticleHandler(), new PeriodicalHandler(), new ThesisHandler(), new NewspaperHandler() });
            _objectForEdit = item;
            if (_objectForEdit != null)
            {
                FilledFieldsInfo = _dispatch.UnwrapObjectForEdit(item);
            }
            else
            {
                FilledFieldsInfo = new FilledFieldsInfo();
            }
        }
    }
}
