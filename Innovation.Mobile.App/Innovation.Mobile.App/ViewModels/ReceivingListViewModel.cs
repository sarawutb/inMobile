using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Data;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class ReceivingListViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ISettingsService _settingsService;
        private readonly ILoggingService _loggingService;
        private DateTime _dateStart, _dateEnd;
        private List<string> _lstTitle;
        private string _docNo;
        private bool _isRefreshing;
        private ObservableCollection<MaterialReceivePlanMst> _receivePlan;
        public ReceivingListViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService,
            IMaterialAutoReceivingService materialAutoReceivingService, ISettingsService settingsService,
            ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _dateStart = DateTime.Now;
            _dateEnd = DateTime.Now;
            _lstTitle = new List<string>();
            LoadTitle();
            _materialAutoReceivingService = materialAutoReceivingService;
            _settingsService = settingsService;
            _loggingService = loggingService;
            InitializeMessenger();
            OnGetReceivingPlan();
        }

        private void LoadTitle()
        {
            _lstTitle.Add("PO No.");
            _lstTitle.Add("Request No.");
        }

        public ICommand SearchCommand => new Command(OnGetReceivingPlan);
        public ICommand ReceivePlanTappedCommand => new Command<MaterialReceivePlanMst>(ReceivePlanTapped);
        public ICommand RefreshCommand => new Command(OnRefresh);

        public DateTime DateStart
        {
            get => _dateStart;
            set
            {
                _dateStart = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateEnd
        {
            get => _dateEnd;
            set
            {
                _dateEnd = value;
                OnPropertyChanged();
            }
        }
        public List<string> lstTitle
        {
            get => _lstTitle;
            set
            {
                _lstTitle = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public string DocNo
        {
            get => _docNo;
            set
            {
                _docNo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MaterialReceivePlanMst> ReceivePlan
        {
            get => _receivePlan;
            set
            {
                var LstTest = new ObservableCollection<MaterialReceivePlanMst>();
                for (var I = 1; I <= 100; I++)
                {
                    LstTest.Add(value.FirstOrDefault());
                }
                _receivePlan = LstTest;
                OnPropertyChanged();
            }
        }

        private async void OnGetReceivingPlan()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_docNo))
                {
                    IsBusy = true;
                    var Data = await DependencyService.Get<ILoadingService>().Loading(_materialAutoReceivingService.GetMaterialReceivePlanMstAsync(_dateStart, _dateEnd), false, TimeoutLoading: 3000);
                    ReceivePlan = Data.ToObservableCollection();
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ReceivePlanTapped(MaterialReceivePlanMst selectReceivePlan)
        {
            switch (selectReceivePlan.typeSuplierId)
            {
                case (int)TypeSupplier.Internal:
                    _navigationService.NavigateToAsync<InternalReceivingViewModel>(selectReceivePlan);
                    break;
                case (int)TypeSupplier.External:
                    _navigationService.NavigateToAsync<ExternalReceivingViewModel>(selectReceivePlan);
                    break;
            }
        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<ExternalReceivingView, bool>(this, MessagingConstants.ReCieve,
               (extReceivingDtlView, recheck) => OnRefreshPage(recheck));
            MessagingCenter.Subscribe<InternalReceivingView, bool>(this, MessagingConstants.ReExternal,
               (internalReceivingView, recheck) => OnRefreshPage(recheck));
            MessagingCenter.Subscribe<MenuViewModel, bool>(this, MessagingConstants.ReAgainPage,
               (MenuViewModel, recheckPage) => OnRefreshPage(recheckPage));
        }
        public void OnRefreshPage(bool repage)
        {
            if (repage)
            {
                OnGetReceivingPlan();
            }
        }
        private async void OnRefresh(object obj)
        {
            if (!IsBusy)
            {
                OnGetReceivingPlan();
            }
        }
    }
}
