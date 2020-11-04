using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using IronMacbeth.Client.Annotations;
using IronMacbeth.Client.VVM.Home;
using IronMacbeth.Client.VVM.LogInVVM;
using IronMacbeth.Client.VVM.MemoryInfo;
using IronMacbeth.Client.VVM.MemoryVVM;
using IronMacbeth.Client.VVM.MotherboardInfo;
using IronMacbeth.Client.VVM.MotherboardVVM;
using IronMacbeth.Client.VVM.NotificationVVM;
using IronMacbeth.Client.VVM.ProcessorInfo;
using IronMacbeth.Client.VVM.ProcessorVVM;
using IronMacbeth.Client.VVM.PuchaseVVM;
using IronMacbeth.Client.VVM.PurchaseVVM;
using IronMacbeth.Client.VVM;
using IronMacbeth.Client.VVM.StoreVVM;
using IronMacbeth.Client.VVM.VideocardInfo;
using IronMacbeth.Client.VVM.VideocardVVM;
using IronMacbeth.Model.ToBeRemoved;
using Timer = System.Timers.Timer;

namespace IronMacbeth.Client.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged, IServiceCallback
    {
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;

                _userLoggedIn = value != null;

                OnPropertyChanged(nameof(MenuVisibility));
                OnPropertyChanged(nameof(Login));
            }
        }

        public static ServerAdapter ServerAdapter { get; private set; }
        public static IService Proxy { get; private set; }


        public ICommand BackCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand LogInCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ReconnectCommand { get; }
        public ICommand ChangePageCommand { get; }
        public ICommand ShowStoresCommand { get; }
        public ICommand ShowPurchasesCommand { get; }
        public ICommand HideNotificationCommand { get; }
        public ICommand ViewNotificationCommand { get; }
        public ICommand AnimationCompletedCommand { get; }


        private User _user;

        private bool _userLoggedIn;

        private IPageViewModel _currentPageViewModel;

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        private NotificationViewModel _notificationPageViewModel;
        private readonly Queue<NotificationViewModel> _notificationViewModels; 

        public NotificationViewModel NotificationPageViewModel
        {
            get { return _notificationPageViewModel; }
            set
            {
                if (_notificationPageViewModel != value)
                {
                    _notificationPageViewModel = value;
                    OnPropertyChanged(nameof(NotificationPageViewModel));
                }
            }
        }

        public string Login => User?.Login;

        public Visibility MenuVisibility => _userLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AdminToolsVisibility
        {
            get
            {
                if (User != null && User.AccessLevel == 9)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public List<IPageViewModel> PageViewModels { get; }


        private readonly Stack<IPageViewModel> _previousPages;
        private readonly Stack<IPageViewModel> _nextPages;
        private readonly Dictionary<string, Type> _infoViewModels;

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                if (ConnectionMenuShown && value)
                {
                    ConnectionMenuText = "Connected";
                    ConnectionMenuHideRequired = true;
                }
                if (!value && !ReconnectButtonShown)
                {
                    ConnectionMenuText = "No Connection";
                    ReconnectButtonShown = true;
                }
                if (value && ReconnectButtonShown)
                {
                    ReconnectButtonShown = false;
                }
                if (!value && !ConnectionMenuShown)
                {
                    ConnectionMenuShown = true;
                }

                _connected = value;
                OnPropertyChanged(nameof(Connected));
            }
        }

        private bool _connectionMenuShown;
        public bool ConnectionMenuShown
        {
            get { return _connectionMenuShown; }
            set
            {
                _connectionMenuShown = value;
                OnPropertyChanged(nameof(ConnectionMenuShown));
            }
        }

        private bool _connectionMenuHideRequired;
        public bool ConnectionMenuHideRequired
        {
            get { return _connectionMenuHideRequired; }
            set
            {
                ConnectionMenuShown = false;
                _connectionMenuHideRequired = value;
                OnPropertyChanged(nameof(ConnectionMenuHideRequired));
            }
        }

        private bool _reconnectButtonShown;
        public bool ReconnectButtonShown
        {
            get { return _reconnectButtonShown; }
            set
            {
                _reconnectButtonShown = value;
                OnPropertyChanged(nameof(ReconnectButtonShown));
            }
        }

        private string _connectionMenuText;
        public string ConnectionMenuText
        {
            get { return _connectionMenuText; }
            set
            {
                _connectionMenuText = value;
                OnPropertyChanged(nameof(ConnectionMenuText));
            }
        }

        private Timer connectionCheckTimer;

        private Task<bool> checkTask = new Task<bool>(() =>
        {
            try
            {
                return Proxy.Ping();
            }
            catch
            {
                return false;
            }
        });

        public MainViewModel()
        {
            connectionCheckTimer = new Timer();
            connectionCheckTimer.Interval = 10000;
            connectionCheckTimer.Elapsed += ConnectionCheck;
            connectionCheckTimer.Start();
            _userLoggedIn = false;

            ConnectAsync();

            BackCommand = new RelayCommand(BackMethod);
            CloseCommand = new RelayCommand(CloseMethod);
            LogInCommand = new RelayCommand(LogInMethod) { CanExecuteFunc = CanExecuteAutorizationMethods };
            LogOutCommand = new RelayCommand(LogOutMethod) { CanExecuteFunc = CanExecuteLogOutMethod };
            ForwardCommand = new RelayCommand(ForwardMethod);
            RegisterCommand = new RelayCommand(RegisterMethod) { CanExecuteFunc = CanExecuteAutorizationMethods };
            ReconnectCommand = new RelayCommand(ReconnectMethod);
            ChangePageCommand = new RelayCommand(ChangePageMethod);
            ShowStoresCommand = new RelayCommand(ShowStoresMethod);
            ShowPurchasesCommand = new RelayCommand(ShowPurchasesMethod);
            HideNotificationCommand = new RelayCommand(HideNotificationMethod);
            ViewNotificationCommand = new RelayCommand(ViewNotificationMethod);
            AnimationCompletedCommand = new RelayCommand(OnAnimationCompleted);

            PageViewModels = new List<IPageViewModel>
            {
                new HomeViewModel(),
                new MemoryViewModel(),
                new ProcessorViewModel(),
                new VideocardViewModel(),
                new MotherboardViewModel()
            };

            _previousPages = new Stack<IPageViewModel>();
            _nextPages = new Stack<IPageViewModel>();
            _notificationViewModels = new Queue<NotificationViewModel>();

            CurrentPageViewModel = new HomeViewModel();

            _infoViewModels = new Dictionary<string, Type>
            {
                {"Memory",typeof(MemoryInfoViewModel)},
                {"Processor",typeof(ProcessorInfoViewModel)},
                {"Videocard",typeof(VideocardInfoViewModel)},
                {"Motherboard",typeof(MotherboardInfoViewModel)},
                {"Purchase",typeof(PurchaseViewModel)}
            };
        }

        private void LogOutMethod(object parameter)
        {
            Proxy.LogOut(User);
            User = null;
        }

        public void ShowStoresMethod(object parameter)
        {
            new UserStoresWindow { DataContext = new UserStoresViewModel(User) }.ShowDialog();
        }

        public void ShowPurchasesMethod(object parameter)
        {
            new StorePurchaseWindow { DataContext = new StorePurchaseViewModel(User) }.ShowDialog();
        }

        public void HideNotificationMethod(object parameter)
        {
            if (_notificationViewModels.Count != 0)
            {
                NotificationPageViewModel = _notificationViewModels.Dequeue();
            }
            else
            {
                NotificationPageViewModel = null;
            }
        }

        public void ViewNotificationMethod(object parameter)
        {
            NotificationPageViewModel = null;
            _notificationViewModels.Clear();

            ShowPurchasesMethod(parameter);
        }

        public void CloseMethod(object parameter)
        {
            (parameter as Window)?.Close();
        }

        public void LogInMethod(object parameter)
        {
            LogInViewModel logInViewModel = new LogInViewModel();
            new LogInWindow { DataContext = logInViewModel }.ShowDialog();
            User = logInViewModel.User;
        }

        public void RegisterMethod(object parameter)
        {
            LogInViewModel logInViewModel = new LogInViewModel(false);
            new LogInWindow { DataContext = logInViewModel }.ShowDialog();
            User = logInViewModel.User;
        }

        public bool CanExecuteAutorizationMethods(object parameter)
        {
            return !_userLoggedIn;
        }

        public bool CanExecuteLogOutMethod(object parameter)
        {
            return _userLoggedIn;
        }

        public void ChangePageMethod(object parameter)
        {
            Type type = parameter.GetType();
            if (type.GetInterfaces().Contains(typeof(IPageViewModel)))
            {
                var viewModel = parameter as IPageViewModel;
                if (!PageViewModels.Contains(viewModel))
                    PageViewModels.Add(viewModel);

                if (type != CurrentPageViewModel.GetType())
                {
                    _previousPages.Push(CurrentPageViewModel);

                    CurrentPageViewModel = viewModel;

                    viewModel.Update();
                }
            }
            else if (type.GetInterfaces().Contains(typeof(IInformationContainer)))
            {
                var infoContainer = parameter as IInformationContainer;
                if (_infoViewModels.ContainsKey(infoContainer.InfoContainerKey))
                {
                    Type VMType = _infoViewModels[infoContainer.InfoContainerKey];
                    IPageViewModel viewModel = Activator.CreateInstance(VMType, parameter) as IPageViewModel;
                    ChangePageMethod(viewModel);
                }
            }
        }

        public void BackMethod(object parameter)
        {
            if (_previousPages.Count != 0)
            {
                if (parameter == null)
                {
                    _nextPages.Push(CurrentPageViewModel);
                }
                CurrentPageViewModel = _previousPages.Pop();
            }
        }
        public void ForwardMethod(object parameter)
        {
            if (_nextPages.Count != 0)
            {
                _previousPages.Push(CurrentPageViewModel);
                CurrentPageViewModel = _nextPages.Pop();
            }
        }

        public static void LoadSellable()
        {
            Store.Items = ServerAdapter.GetAll<Store>();
            Memory.Items = ServerAdapter.GetAll<Memory>();
            Processor.Items = ServerAdapter.GetAll<Processor>();
            Videocard.Items = ServerAdapter.GetAll<Videocard>();
            Motherboard.Items = ServerAdapter.GetAll<Motherboard>();
            LoadSellableLinks();
        }

        public static void LoadSellableLinks()
        {
            StoreMemory.Items = ServerAdapter.GetAll<StoreMemory>();
            StoreProcessor.Items = ServerAdapter.GetAll<StoreProcessor>();
            StoreVideocard.Items = ServerAdapter.GetAll<StoreVideocard>();
            StoreMotherboard.Items = ServerAdapter.GetAll<StoreMotherboard>();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public string GetName()
        {
            return User.Login;
        }

        public void NotifyUserJoined(string userName)
        {
        }

        public void NotifyNewMessage(string message)
        {
        }

        public void NotifyNewPurchase(Notification notification)
        {
            _notificationViewModels.Enqueue(new NotificationViewModel(notification));
            if (NotificationPageViewModel == null)
            {
                NotificationPageViewModel = _notificationViewModels.Dequeue();
            }
        }

        private bool Connect()
        {
            DuplexChannelFactory<IService> channelFactory = new DuplexChannelFactory<IService>(this, "WCFServiceEndPoint");

            Proxy = channelFactory.CreateChannel();

            ServerAdapter = new ServerAdapter(Proxy);

            try
            {
                return Proxy.Ping();
            }
            catch
            {
                return false;
            }
        }

        private async void ConnectAsync()
        {
            ConnectionMenuText = "Connecting...";
            ConnectionMenuShown = true;

            Connected = await Task<bool>.Factory.StartNew(Connect);
        }

        private async void ConnectionCheck(object sender,ElapsedEventArgs args)
        {
            if (checkTask.Status != TaskStatus.Running && Connected)
            {
                checkTask = new Task<bool>(() =>
                {
                    try
                    {
                        return Proxy.Ping();
                    }
                    catch
                    {
                        return false;
                    }
                });

                checkTask.Start();

                Connected = await checkTask;
            }
        }

        public void OnAnimationCompleted(object parameter)
        {
            Console.Beep();
        }

        private void ReconnectMethod(object parameter)
        {
            ConnectAsync();
        }
    }
}