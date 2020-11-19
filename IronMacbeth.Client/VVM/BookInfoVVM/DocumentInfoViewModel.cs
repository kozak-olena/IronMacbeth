using IronMacbeth.Client.VVM.EditBookVVM;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.BookVVM
{
    public  class DocumentInfoViewModel : IPageViewModel
    {
        private readonly object _objectForEdit;

        private readonly Dispatch _dispatch;

        public string PageViewName => "BookInfo";

        public FilledFieldsInfo FilledFieldsInfo { get; set; }

        public BitmapImage Image => FilledFieldsInfo.Image?.BitmapImage;

        public DocumentInfoViewModel(object item)
        {
            _dispatch = new Dispatch(new IHandler[] { new BookHandler(), new ArticleHandler(), new PeriodicalHandler(), new ThesisHandler(), new NewspaperHandler() });
            _objectForEdit = item;
            if (_objectForEdit != null)
            {
                FilledFieldsInfo = _dispatch.UnwrapObjectForEdit(_objectForEdit);
            }
            else
            {
                FilledFieldsInfo = new FilledFieldsInfo();
            }
        }

        public void Update() { }
    }
}
