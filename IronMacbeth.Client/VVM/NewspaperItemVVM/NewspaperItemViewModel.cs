using IronMacbeth.Client.VVM.PeriodicalInfoVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Client.VVM.NewspaperItemVVM
{
    public class NewspaperItemViewModel : IDocumentViewModel
    {
        private Newspaper _item;

        public NewspaperItemViewModel(Newspaper item)
        {
            _item = item;
        }

        public string Name => _item.Name;
        public string Availiability => _item.Availiability;
        public string IssueNumber => _item.IssueNumber;
        public string Year => _item.Year;
        public string City => _item.City;
        public string Location => _item.Location;
        public string TypeOfDocument => _item.TypeOfDocument;
        public string Rating => _item.Rating;
        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;
        public string RentPrice => _item.RentPrice;


        public NewspaperInfoViewModel MoreInfoVm => new NewspaperInfoViewModel(_item);

        public object GetItem()
        {
            return _item;
        }
    }
}
