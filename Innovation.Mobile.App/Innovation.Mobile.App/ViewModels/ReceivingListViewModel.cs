using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                _receivePlan = value;
                OnPropertyChanged();
            }
        }

        private async void OnGetReceivingPlan()
        {
            //เลือกรูป
            //var photoPicker = DependencyService.Get<IPhotoPickerService>();
            //var photoStream = await photoPicker.GetImageStreamAsync();

            //Print
            //var printer = DependencyService.Get<IPrintService>();
            //var result = printer.PrintImage(photoStream);

            try
            {
                if (string.IsNullOrWhiteSpace(_docNo))
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                    ReceivePlan = (await _materialAutoReceivingService.GetMaterialReceivePlanMstAsync(_dateStart, _dateEnd)).ToObservableCollection();
                    //var mstsite = mst.Where(x => x.ownerSite == int.Parse(_settingsService.SiteIdSetting)).ToObservableCollection();
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                }
                else
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT");

                }
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage(); 
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
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
                try
                {
                    IsBusy = true;
                    IsRefreshing = true;
                    ReceivePlan = (await _materialAutoReceivingService.GetMaterialReceivePlanMstAsync(_dateStart, _dateEnd)).ToObservableCollection();
                    IsRefreshing = false;
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                }
            }
        }
    }
}
