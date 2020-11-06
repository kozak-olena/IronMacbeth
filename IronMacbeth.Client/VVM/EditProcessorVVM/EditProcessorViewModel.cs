using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.ViewModel;
using Microsoft.Win32;

namespace IronMacbeth.Client.VVM.EditProcessorVVM
{
    public class EditProcessorViewModel:IPageViewModel,INotifyPropertyChanged
    {
        public string PageViewName => "Processor";
        public Processor Processor { get; private set; }

        public bool CollectionChanged { get; private set; }

        public string ImagePath { get; set; }
        public BitmapImage BitmapImage { get; set; }

        public string Description { get; set; }

        public string NumberOfCores { get; set; }
        public string Lithography { get; set; }
        public string TDP { get; set; }
        public string Level2Cache { get; set; }
        public string Level3Cache { get; set; }

        public string BaseFrequency { get; set; }
        public string TurboFrequency { get; set; }
        public string Socket { get; set; }
        public string ProcessorCore { get; set; }
        public string ProcessorGraphics { get; set; }
        public string Model { get; set; }
        public string MPN { get; set; }


        public ICommand CloseCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ApplyChangesCommand { get; set; }

        public EditProcessorViewModel(Processor processor = null)
        {
            Description = "";
            Processor = processor;
            if (Processor != null)
            {
                ImagePath = "<image>";
                BitmapImage = Processor.BitmapImage;

                NumberOfCores = Processor.NumberOfCores.ToString();
                Lithography = Processor.Lithography.ToString();
                TDP = Processor.TDP.ToString();
                Level2Cache = Processor.Level2Cache.ToString();
                Level3Cache = Processor.Level3Cache.ToString();

                BaseFrequency = Processor.BaseFrequency;
                TurboFrequency = Processor.TurboFrequency;
                Socket = Processor.Socket;
                ProcessorCore = Processor.ProcessorCore;
                ProcessorGraphics = Processor.ProcessorGraphics;
                Model = Processor.Model;
                MPN = Processor.MPN;
                Description = Processor.Description;
            }

            CloseCommand = new RelayCommand(CloseMethod);
            SelectImageCommand = new RelayCommand(SelectImageMethod);
            ApplyChangesCommand = new RelayCommand(ApplyChangesMethod)
            {
                CanExecuteFunc = ApplyChangesCanExecute
            };
        }

        public void ApplyChangesMethod(object parameter)
        {
            if (Processor != null)
            {
                Processor.NumberOfCores = int.Parse(NumberOfCores);
                Processor.Lithography = int.Parse(Lithography);
                Processor.TDP = int.Parse(TDP);
                Processor.Level2Cache = int.Parse(Level2Cache);
                Processor.Level3Cache = int.Parse(Level3Cache);

                Processor.BaseFrequency = BaseFrequency;
                Processor.TurboFrequency = TurboFrequency;
                Processor.Socket = Socket;
                Processor.ProcessorCore = ProcessorCore;
                Processor.ProcessorGraphics = ProcessorGraphics;
                Processor.Model = Model;
                Processor.MPN = MPN;

                if (!Processor.BitmapImage.Equals(BitmapImage))
                {
                    Processor.BitmapImage = BitmapImage;
                    Processor.ImageName = null;
                }
                if (Processor.Description != Description)
                {
                    Processor.Description = Description;
                    Processor.DescriptionName = null;
                }

                MainViewModel.ServerAdapter.UpdateProcessor(Processor);
            }
            else
            {
                Processor = new Processor
                {
                    BitmapImage = BitmapImage,
                    NumberOfCores = int.Parse(NumberOfCores),
                    Lithography = int.Parse(Lithography),
                    TDP = int.Parse(TDP),
                    Level2Cache = int.Parse(Level2Cache),
                    Level3Cache = int.Parse(Level3Cache),                 
                    BaseFrequency = BaseFrequency,
                    TurboFrequency = TurboFrequency,
                    Socket = Socket,
                    ProcessorCore = ProcessorCore,
                    ProcessorGraphics = ProcessorGraphics,
                    Model = Model,
                    MPN = MPN,
                    Description = Description
                };
                MainViewModel.ServerAdapter.CreateProcessor(Processor);
            }

            CollectionChanged = true;

            CloseMethod(parameter);
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void SelectImageMethod(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (.png)|*.png",
                FilterIndex = 1,
                Multiselect = false
            };

            bool? userClickedOk = openFileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(ImagePath));

                BitmapImage = new BitmapImage(new Uri(ImagePath));
                OnPropertyChanged(nameof(BitmapImage));
            }
        }

        public bool ApplyChangesCanExecute(object parameter)
        {
            int n;
            return int.TryParse(NumberOfCores,out n) &&
                   int.TryParse(Lithography, out n) &&
                   int.TryParse(TDP, out n) &&
                   int.TryParse(Level2Cache, out n) &&
                   int.TryParse(Level3Cache, out n) &&
                   !string.IsNullOrWhiteSpace(Model) &&
                   !string.IsNullOrWhiteSpace(MPN) &&
                   !string.IsNullOrWhiteSpace(BaseFrequency) &&
                   !string.IsNullOrWhiteSpace(Socket) &&
                   !string.IsNullOrWhiteSpace(ProcessorCore) &&
                   !string.IsNullOrWhiteSpace(ProcessorGraphics) &&
                   !string.IsNullOrWhiteSpace(ImagePath);
        }

        public void Update() { }

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