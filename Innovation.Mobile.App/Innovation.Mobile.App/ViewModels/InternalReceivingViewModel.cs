using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class InternalReceivingViewModel : ViewModelBase
    {
        private string _barcode;
        private MaterialReceivePlanMst _selectReceivePlan;
        private List<MaterialReceivePlanDtlBarcode> _lstReceiveDtlBarcode;
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private bool _ishideBatch;
        public InternalReceivingViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            InitializeMessenger();
        }

        public ICommand QrScanCommand => new Command(OnGetQrData);
        public ICommand BtnScanCommand => new Command(OnReadTextData);
        public ICommand SearchCommand => new Command(OnGetSearchData);
        public ICommand ResetCommand => new Command(OnResetBarcode);
        private async void OnGetSearchData()
        {
            try
            {
                var result = Barcode;
                if (result != null)
                {
                    await GetRMByBarcodeData();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + ex.ToString());
            }
        }
        public bool IshideBatch
        {
            get => _ishideBatch;
            set
            {
                _ishideBatch = value;
                OnPropertyChanged();
            }
        }
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

        public MaterialReceivePlanMst selectReceivePlan
        {
            get => _selectReceivePlan;
            set
            {
                _selectReceivePlan = value;
                OnPropertyChanged();
            }
        }

        public List<MaterialReceivePlanDtlBarcode> lstReceiveDtlBarcode
        {
            get => _lstReceiveDtlBarcode;
            set
            {
                _lstReceiveDtlBarcode = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object ReceivePlan)
        {
            selectReceivePlan = (MaterialReceivePlanMst)ReceivePlan;
            GetRMDtlBarcodeListAsync();
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
                    await GetRMByBarcodeData();// แก้แจ้งเตือนไม่แสดง รอ Host
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private void OnResetBarcode()
        {
            Barcode = string.Empty;
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
                    await GetRMByBarcodeData();// แก้แจ้งเตือนไม่แสดง รอ Host
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private async Task GetRMByBarcodeData() // แก้แจ้งเตือนไม่แสดง รอ Host
        {
            var rmname = lstReceiveDtlBarcode[0].rmId.Substring(0, 1);
            if (rmname == "I" || rmname == "S")
            {
                var RM = lstReceiveDtlBarcode.Where(x => x.barcode + x.batchNo == Barcode).FirstOrDefault();
                if (RM != null)
                {
                    _settingsService.BarcodeSetting = Barcode;
                    await _navigationService.NavigateToAsync<InternalReceivingBarcodeDetailViewModel>(selectReceivePlan);
                    //GetRMDtlBarcodeListAsync();
                }
                else
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่มีข้อมูลบาร์โค้ดนี้");
                }
            }
            else
            {
                var RM = lstReceiveDtlBarcode.Where(x => x.barcode == Barcode).FirstOrDefault();
                if (RM != null)
                {
                    _settingsService.BarcodeSetting = Barcode;
                    var dtlbarcode = selectReceivePlan.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                    var selectbarcode = dtlbarcode.Where(x => x.barcode == Barcode).ToList();
                    var dtlid = selectbarcode.Select(x => x.receivePlanDtlId).FirstOrDefault();
                    var dtllot = selectbarcode.Select(x => x.LotNo_Group).FirstOrDefault();
                    var dtllst = selectReceivePlan.receivePlanDtl.Where(x => x.Id == dtlid && x.lotNo == dtllot).ToList();
                    dtllst[0].ReceivePlanBarcode = selectbarcode;
                    var mst = selectReceivePlan.Clone<MaterialReceivePlanMst>();
                    mst.receivePlanDtl = dtllst;
                    await _navigationService.NavigateToAsync<InternalReceivingBarcodeDetailViewModel>(mst);
                    //GetRMDtlBarcodeListAsync();
                }
                else
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่มีข้อมูลบาร์โค้ดนี้");
                }
            }
        }
        private async void GetRMDtlBarcodeListAsync()
        {
            int dtlid;
            var mstid = selectReceivePlan.Id;
            if (selectReceivePlan.typeSuplierId != 1)
            {
                dtlid = selectReceivePlan.receivePlanDtl[0].Id;
            }
            else
            {
                dtlid = 0;
            }
            try
            {
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                var dtl = (await _materialAutoReceivingService.GetRMChecklistByReceiveMstAndTypeIDAsync(mstid, 1, dtlid)).ToList();
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                selectReceivePlan.receivePlanDtl = new List<MaterialReceivePlanDtl>(dtl);
                var bacodedtl = selectReceivePlan.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                lstReceiveDtlBarcode = bacodedtl.ToList();

                foreach (var item in lstReceiveDtlBarcode)
                {
                    if (item.receivePlanDtlBarcodeStatus >= 2)
                    {
                        item.ColorStatusCode = "#00FF04";
                    }
                    else
                    {
                        item.ColorStatusCode = "#FFFFFF";
                    }
                }
                if (lstReceiveDtlBarcode.Count == 0)
                {
                    if (selectReceivePlan.typeSuplierId == 1)
                    {
                        await _navigationService.NavigateBackAsync();
                    }
                    else
                    {
                        MessagingCenter.Send(this, MessagingConstants.ReExternal, true);
                        await _navigationService.NavigateBackAsync();
                    }

                }
                else
                {
                    var rmname = lstReceiveDtlBarcode[0].rmId.Substring(0, 1);
                    if (rmname == "I" || rmname == "S")
                    {
                        IshideBatch = true;
                    }
                    else
                    {
                        IshideBatch = false;
                    }
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<InternalReceivingBarcodeDetailViewModel, bool>(this, MessagingConstants.Internal,
                (internalReceivingBarcodeDetailViewModel, repagedtlinternal) => OnRefreshPage(repagedtlinternal));
        }
        public void OnRefreshPage(bool onRefreshPage)
        {
            if (onRefreshPage)
            {
                GetRMDtlBarcodeListAsync();
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
