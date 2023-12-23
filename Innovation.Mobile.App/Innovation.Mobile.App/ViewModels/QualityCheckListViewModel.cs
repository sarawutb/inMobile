using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Models;
using System.Collections.ObjectModel;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.Extensions;
using System.Windows.Input;
using System.Linq;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Templates;
using Innovation.Mobile.App.Views;
using System.Threading.Tasks;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{

    public class QualityCheckListViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private DateTime _dateStart, _dateEnd;
        private List<string> _lstTitle;
        private string _docNo;
        private bool _isRefreshing;
        private ObservableCollection<MaterialReceivePlanMst> _receivePlanMst;
        public QualityCheckListViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService,
            IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _dateStart = DateTime.Now;
            _dateEnd = DateTime.Now;
            _lstTitle = new List<string>();
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            OnGetReceivingPlan();
            LoadTitle();
            InitializeMessenger();
        }

        public ICommand SearchCommand => new Command(OnGetReceivingPlan);
        public ICommand ReceivePlanTappedCommand => new Command<MaterialReceivePlanMst>(OnCheckPlanTapped);
        public ICommand RefreshCommand => new Command(OnRefresh);
        public ICommand QrScanCommand => new Command(OnGetQrData);
        public ICommand BtnScanCommand => new Command(OnReadTextData);

        private void LoadTitle()
        {
            _lstTitle.Add("PO No.");
            _lstTitle.Add("Request No.");
        }
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
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
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

        public string DocNo
        {
            get => _docNo;
            set
            {
                _docNo = value;
                OnPropertyChanged();
            }
        }

        private string _statusBtn = "scan";
        public string StatusBtn
        {
            get => _statusBtn;
            set
            {
                _statusBtn = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MaterialReceivePlanMst> ReceivePlan
        {
            get => _receivePlanMst;
            set
            {
                _receivePlanMst = value;
                OnPropertyChanged();
            }

        }
        private string _barcode;
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                ChangeTextBtn();
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
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    if (!string.IsNullOrEmpty(Barcode))
                    {
                        Barcode = string.Empty;
                    }
                    ReceivePlan = (await _materialAutoReceivingService.GetMaterialReceivePlanMstQAAsync(_dateStart, _dateEnd)).ToObservableCollection();
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                    //foreach (var item in ReceivePlan)
                    //{
                    //    if(item.receivePlanMstStatus == 3)
                    //    {
                    //        item.colorName = "Green";
                    //        item.ColorCode = "#00FF04";
                    //    }
                    //    else
                    //    {
                    //        item.ColorCode = "#FFFFFF";
                    //        item.colorName = "White";
                    //    }
                    //}
                }
                else
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT");

                }
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }

        private void OnCheckPlanTapped(MaterialReceivePlanMst selectReceivePlan)
        {
            _navigationService.NavigateToAsync<QualityCheckListDtlViewModel>(selectReceivePlan);
        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<QualityCheckListDtlView, bool>(this, MessagingConstants.RecheckQuality,
               (qualityCheckListDtlBarcodeViewModel, recheckQuality) => OnRefreshQualityPage(recheckQuality));
            MessagingCenter.Subscribe<MenuViewModel, bool>(this, MessagingConstants.ReAgainPage,
               (MenuViewModel, recheckPage) => OnRefreshQualityPage(recheckPage));
        }
        public void OnRefreshQualityPage(bool repage)
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

                    ReceivePlan = (await _materialAutoReceivingService.GetMaterialReceivePlanMstQAAsync(_dateStart, _dateEnd)).ToObservableCollection();

                    IsRefreshing = false;
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
            }
        }

        private async void OnGetQrData()
        {
            try
            {
                if (!string.IsNullOrEmpty(Barcode))
                {
                    await GetReadBarcodeTextData(Barcode);
                    return;
                }
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    Barcode = result;
                    await GetRMByBarcodeDataAsync();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ระบบแสกนผิดพลาด กรุณาติดต่อ ICT");
            }
        }

        private async void OnReadTextData()
        {
            await GetReadBarcodeTextData(Barcode);
        }

        private async Task GetReadBarcodeTextData(string barcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barcode))
                {
                    Barcode = barcode;
                    await GetRMByBarcodeDataAsync();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ระบบแสกนผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private async Task GetRMByBarcodeDataAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    IsBusy = true;
                    MaterialReceivePlanMst scanBarcodeReceivePlan = await _materialAutoReceivingService.GetReceivePlanByBarcode(Barcode);
                    IsBusy = false;

                    if (scanBarcodeReceivePlan != null)
                    {
                        if (scanBarcodeReceivePlan.typeSuplierId == 1)
                        {
                            foreach (var itembarcode in scanBarcodeReceivePlan.receivePlanDtl[0].ReceivePlanBarcode)
                            {
                                if (itembarcode.receivePlanDtlBarcodeStatus < 3)
                                {
                                    itembarcode.StatusQA = null;
                                }
                            }
                        }

                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _navigationService.NavigateToAsync<QualityCheckListDtlBarcodeViewModel>(scanBarcodeReceivePlan);
                    }
                    else
                    {
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่มีข้อมูลบาร์โค้ดนี้");
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                }
                catch (Exception ex)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(ex.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาดในการดึงข้อมูล กรุณาติดต่อ ICT\r\nError Message: " + ex.ToString());
                }
            }
        }

        private void ChangeTextBtn()
        {
            if (!string.IsNullOrEmpty(Barcode))
            {
                StatusBtn = "search";
            }
            else
            {
                StatusBtn = "scan";
            }
        }
    }
}
