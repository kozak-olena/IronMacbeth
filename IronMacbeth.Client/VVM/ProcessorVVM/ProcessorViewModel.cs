using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using IronMacbeth.Client.VVM.EditProcessorVVM;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.Client.VVM.ProcessorVVM
{
    class ProcessorViewModel : IPageViewModel, INotifyPropertyChanged
    {
        public string PageViewName => "Processors";

        private List<Processor> _items;

        public List<Processor> Items
        {
            get
            {
                UpdateCollection(true);
                return _items;
            }
            private set { _items = value; }
        }

        private string _search;
        public string Search
        {
            get { return _search; }

            set
            {
                _search = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    UpdateCollection(false);
                }
                else
                {
                    UpdateCollectionNoFilter();
                }

            }
        }

        public object SelectedItem { get; set; }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProcessorViewModel()
        {
            //new Processor
            //{
            //    BaseFrequency = "4",
            //    Level2Cache = 1,
            //    Level3Cache = 8,
            //    Socket = "Socket 1151",
            //    TurboFrequency = "4.2",
            //    NumberOfCores = 4,
            //    Lithography = 14,
            //    ProcessorCore = "Skylake-S",
            //    ProcessorGraphics = "Intel HD Graphics 530, 1150 GHz",
            //    TDP = 91,
            //    Model = "Intel Core i7-6700K",
            //    MPN = "BX80662I76700K",
            //    Description = "Intel Core i7-6700K BX80662I76700K - high-performance processor, introduced in the new generation of Intel Skylake. The model is made using 14-nm process technology and is designed for Socket 1151. Intel Core i7-6700K BX80662I76700K has at its disposal 4 cores, divided into 8 streams. The operating frequency is 4 GHz processor, but Turbo Boost is also provided, raising it up to 4.2 GHz. The main feature of Intel Core i7-6700K BX80662I76700K is unlocked multiplier, which opens up opportunities for enthusiasts."
            //    //PageViewModel = new ProcessorInfoViewModel(),
            //    //ImageSource = new BitmapImage(new Uri(@"D:\7-6700.jpg"))
            //};
            //new Processor
            //{
            //    Socket = "Socket 1150",
            //    BaseFrequency = "3.5",
            //    Level2Cache = 1,
            //    Level3Cache = 6,
            //    NumberOfCores = 4,
            //    Lithography = 22,
            //    ProcessorCore = "Haswell",
            //    ProcessorGraphics = "Intel HD Graphics 4600",
            //    TDP = 88,
            //    Model = "Intel Core i5-4690K",
            //    MPN = "BX80646I54690K",
            //    Description = "Intel Core i5-4690K BX80646I54690K - younger model of the new quad-core overclockers line Devil's Canyon. Made processor on 22 nm technology, in normal mode operates at a frequency of 3.5 GHz (3.9 GHz in Turbo mode). Unlocked multiplier allows you to manually control the overclocking Intel Core i5-4690K BX80646I54690K, which undoubtedly will delight enthusiasts. Among other things, it is worth noting equipment graphics processor Intel HD Graphics 4600 and supported by a number of proprietary technologies virtualization Intel VT-x and VT-d."
            //    //PageViewModel = new ProcessorInfoViewModel(),
            //    //ImageSource = new BitmapImage(new Uri(@"D:\5-4690.jpg"))
            //};
            //new Processor
            //{
            //    Socket = "Socket 1151",
            //    BaseFrequency = "3.5",
            //    TurboFrequency = "3.9",
            //    Level2Cache = 1,
            //    Level3Cache = 6,
            //    NumberOfCores = 4,
            //    Lithography = 14,
            //    ProcessorCore = "Skylake-S",
            //    ProcessorGraphics = "Intel HD Graphics 530, 1150 GHz",
            //    TDP = 91,
            //    Model = "Intel Core i5-6600K",
            //    MPN = "BX80662I56600K",
            //    ImageName = "1",
            //    Description = "Intel Core i5-6600K BX80662I56600K - new quad among the productive solutions. The model belongs to the line of Skylake and executed in accordance with the norms of 14 nm process technology. Intel Core i5-6600K BX80662I56600K clocked at 3.5 GHz, which increases to 3.9 GHz mode Turbo Boost. The main feature of the processor is unlocked multiplier, which opens up additional opportunities for enthusiasts. The change undergone and onboard graphics Intel Core i5-6600K BX80662I56600K, which received a new designation, and raising productivity."
            //    //PageViewModel = new ProcessorInfoViewModel(),
            //    //ImageSource = new BitmapImage(new Uri(@"D:\5-6600.jpg"))
            //};

            //Processor processor = new Processor
            //{
            //    Socket = "Socket 1150",
            //    BaseFrequency = "4",
            //    TurboFrequency = "0",
            //    Level2Cache = 1,
            //    Level3Cache = 8,
            //    NumberOfCores = 4,
            //    Lithography = 22,
            //    ProcessorCore = "asfawdgasdfgawsdagws",
            //    ProcessorGraphics = "Intel HD Graphics 4600",
            //    TDP = 88,
            //    Model = "Intel Core i7-4790K",
            //    MPN = "BX80646I74790K",
            //    Description = "Intel Core i7-4790K BX80646I74790K - quad-core processor from the family Haswell codenamed Devils Canyon. An interesting for enthusiasts and overclockers model makes the unlocked multiplier, you can manually overclock the processor, and thus increase the already considerable computing power (in normal mode operates at 4 GHz). Intel Core i7-4790K BX80646I74790K manufactured on 22 nm process technology, the GPU is equipped with Intel HD Graphics 4600 and supports a number of proprietary technologies Intel, such as Turbo Boost and Hyper-Threading.",
            //    BitmapImage = new BitmapImage(new Uri(@"D:\logo_1.png"))
            //};

            //MainViewModel.ServerAdapter.Insert(a);

            //Processor.Items.Clear();

            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();

            //foreach (var processor in Processor.Items)
            //{
            //    MainViewModel.ServerAdapter.Insert(processor);
            //}

            //var a = new Store
            //{
            //    BitmapImage = new BitmapImage(new Uri(@"D:\385_.png")),
            //    Name = "mobilluck.com.ua",
            //    Rating = 36,
            //    Delivery = "Kyiv"
            //};

            //MainViewModel.ServerAdapter.Insert(a);

            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();

            //Items = Processor.Items.OrderByDescending(item => item.NumberOfOfferings).ToList();

            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();

            Search = "";

            UpdateCollection(false);

            AddCommand = new RelayCommand(AddMethod);
            EditCommand = new RelayCommand(EditMethod) {CanExecuteFunc = CanExecuteMaintenanceMethods};
            DeleteCommand = new RelayCommand(DeleteMethod) { CanExecuteFunc = CanExecuteMaintenanceMethods };
        }

        public void AddMethod(object parameter)
        {
            var editProcessorViewModel = new EditProcessorViewModel();
            new EditProcessorWindow { DataContext = editProcessorViewModel }.ShowDialog();
            if (editProcessorViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void Update()
        {
            MainViewModel.LoadSellable();
            //Store.Items = MainViewModel.ServerAdapter.GetAll<Store>();
            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();
            //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();
        }

        public void EditMethod(object parameter)
        {
            var editProcessorViewModel = new EditProcessorViewModel(SelectedItem as Processor);
            new EditProcessorWindow { DataContext = editProcessorViewModel }.ShowDialog();
            if (editProcessorViewModel.CollectionChanged)
            {
                Update();
                UpdateCollection(false);
            }
        }

        public void DeleteMethod(object parameter)
        {
            MainViewModel.ServerAdapter.Delete(SelectedItem as Processor);
            Update();
            UpdateCollection(false);
        }

        public void UpdateCollection(bool innerCall)
        {
            _items = Processor.Items.
                OrderByDescending(item=>item.NumberOfOfferings).
                Where(item => item.Name.ToLower().Contains(Search.ToLower())).ToList();

            if (!innerCall)
            {
                OnPropertyChanged(nameof(Items));
            }
        }

        public void UpdateCollectionNoFilter()
        {
            //StoreProcessor.Items = MainViewModel.ServerAdapter.GetAll<StoreProcessor>();
            //Processor.Items = MainViewModel.ServerAdapter.GetAll<Processor>();

            _items = Processor.Items.
                OrderByDescending(item => item.NumberOfOfferings).ToList();

           OnPropertyChanged(nameof(Items));
        }

        public bool CanExecuteMaintenanceMethods(object parameter)
        {
            return SelectedItem != null;
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
