using IronMacbeth.Client.VVM.ThesisInfoVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string ResponsibleAuthors => _item.ResponsibleAuthors;
        public string Rating => _item.Rating;
        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;


        public ThesisInfoViewModel MoreInfoVm => new ThesisInfoViewModel(_item);

        public object GetItem()
        {
            return _item;
        }
    }
}
