using IronMacbeth.Client.VVM.PeriodicalInfoVVM;
using System;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Client.VVM.PeriodicalItemVVM
{
    public class PeriodicalItemViewModel : IDocumentViewModel
    {
        private Periodical _item;

        public PeriodicalItemViewModel(Periodical item)
        {
            _item = item;
        }

        public string Name => _item.Name;
        public BitmapImage BitmapImage => _item.BitmapImage;
        public string Responsible => _item.ResponsibleAuthors;
        public string Availiability => _item.Availiability;
        public string IssueNumber => _item.IssueNumber;
        public string TypeOfDocument => _item.TypeOfDocument;
        public string Rating => _item.Rating;
        public string ElectronicVersionPrice => _item.ElectronicVersionPrice;
        public string RentPrice => _item.RentPrice;
        public string Location => _item.Location;

        public PeriodicalInfoViewModel MoreInfoVm => new PeriodicalInfoViewModel(_item);

        public object GetItem()
        {
            return _item;
        }

    }
}
