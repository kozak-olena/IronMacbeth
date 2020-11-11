using IronMacbeth.Client.VVM.BookVVM;
 

namespace IronMacbeth.Client.VVM.ThesisItemVVM
{
    public class ThesisItemViewModel : IDocumentViewModel
    {
        private Thesis _item;

        public ThesisItemViewModel(Thesis item)
        {
            _item = item;
        }

        public string Name => _item.Name;
        public string Author => _item.Author;
        public string Availiability => _item.Availiability;
        public string TypeOfDocument => _item.TypeOfDocument;
        public string Year => _item.Year;

        public string Pages => _item.Pages;
        public string City => _item.City;
        public string ElectonicVersionPrice => _item.ElectronicVersionPrice;
        public string Responsible => _item.Responsible;
        public string Rating => _item.Rating;
        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;


        public DocumentInfoViewModel MoreInfoVm => new DocumentInfoViewModel(_item);

        public object GetItem()
        {
            return _item;
        }
    }
}
