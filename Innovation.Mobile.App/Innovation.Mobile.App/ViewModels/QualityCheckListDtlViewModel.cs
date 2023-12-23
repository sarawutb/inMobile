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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Views;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{
    public class QualityCheckListDtlViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private string _barcode, _rmTitle;
        private bool _ishideBatch;
        private List<MaterialReceiveGroupLotNoVM> _lstLotNoByRM;
        private MaterialReceivePlanMst _selectPlan;
        private List<MaterialReceivePlanDtlBarcode> _lstDtlBarcode;
        private readonly ISettingsService _settingsService;
        public QualityCheckListDtlViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            InitializeMessenger();
        }
        public ICommand QrScanCommand => new Command(OnGetQrData);
        public ICommand BtnScanCommand => new Command(OnReadTextData);
        public ICommand ResetCommand => new Command(OnResetBarcode);
        public ICommand SearchCommand => new Command(OnGetSearchData);

        private async void OnGetSearchData()
        {
            try
            {
                var result = Barcode;
                if (result != null)
                {
                    GetRMByBarcodeDataAsync();
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }
        public List<MaterialReceiveGroupLotNoVM> lstLotNoByRM
        {
            get => _lstLotNoByRM;
            set
            {
                _lstLotNoByRM = value;
                OnPropertyChanged();
            }
        }
        public string _statusBtn = "scan";
        public string StatusBtn
        {
            get => _statusBtn;
            set
            {
                _statusBtn = value;
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
        public bool IshideBatch
        {
            get => _ishideBatch;
            set
            {
                _ishideBatch = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanMst selectPlan
        {
            get => _selectPlan;
            set
            {
                _selectPlan = value;
                OnPropertyChanged();
            }
        }
        public string RMTitle
        {
            get => _rmTitle;
            set
            {
                _rmTitle = value;
                OnPropertyChanged();
            }
        }
        public List<MaterialReceivePlanDtlBarcode> lstDtlBarcode
        {
            get => _lstDtlBarcode;
            set
            {
                _lstDtlBarcode = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object ReceivePlan)
        {
            selectPlan = (MaterialReceivePlanMst)ReceivePlan;
            _settingsService.ReceiveMstIdSetting = selectPlan.Id.ToString();
            GetLotNoByRMIDAsync();
        }
        private async Task GetLotNoByRMIDAsync()
        {
            try
            {
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                var mstid = selectPlan.Id;
                var dtl = await _materialAutoReceivingService.GetRMChecklistByReceiveMstAndTypeIDAsync(mstid, 2);
                selectPlan.receivePlanDtl = new List<MaterialReceivePlanDtl>(dtl);
                if (selectPlan.typeSuplierId == 1)
                {
                    foreach (var itembarcode in selectPlan.receivePlanDtl[0].ReceivePlanBarcode)
                    {
                        if (itembarcode.receivePlanDtlBarcodeStatus < 3)
                        {
                            itembarcode.StatusQA = null;
                        }
                    }
                }
                var checkstatusdtl = selectPlan.receivePlanDtl.Where(x => x.receivePlanDtlStatus != 3).Count();
                var checkbarcode = _selectPlan.receivePlanDtl.Where(x => x.ReceivePlanBarcode.Count > 0).ToList();
                if (checkbarcode.Count > 0)
                {
                    var dtllst = _selectPlan.receivePlanDtl.FirstOrDefault();
                    var word = dtllst.rmId.Substring(0, 1);
                    if (word == "I" || word == "S")
                    {
                        lstLotNoByRM = (from a in _selectPlan.receivePlanDtl
                                        select new MaterialReceiveGroupLotNoVM()
                                        {
                                            rmId = a.rmId,
                                            rmName = a.rmName,
                                            firstReceiveDate = a.ReceivePlanBarcode.Count > 0 ? a.firstReceiveDate : null,
                                            LotNo_Group = a.lotNo != null ? a.lotNo : "",
                                            countQty = a.ReceivePlanBarcode.Count() == 0 ? a.countQty : a.ReceivePlanBarcode.Count(),
                                            weightPerUnit = a.weightPerUnit,
                                            SumWeight = (decimal)(a.ReceivePlanBarcode.Count() > 0 ? a.ReceivePlanBarcode.Sum(x => x.qty) : 0),
                                            IsCheck = a.ReceivePlanBarcode.Where(x => x.StatusQA != null && x.rmId == a.rmId && x.LotNo_Group == (a.lotNo != null ? a.lotNo : "")).Count() > 0,
                                            LabelColor = a.colorCode,
                                        }).ToList();
                    }
                    else
                    {
                        lstLotNoByRM = (from a in _selectPlan.receivePlanDtl
                                        select new MaterialReceiveGroupLotNoVM()
                                        {
                                            rmId = a.rmId,
                                            rmName = a.rmName,
                                            rmIdRecieve = a.rmIdReceive,
                                            firstReceiveDate = a.ReceivePlanBarcode.Count > 0 ? a.firstReceiveDate : null,
                                            LotNo_Group = a.lotNo != null ? a.lotNo : "",
                                            countQty = a.ReceivePlanBarcode.Count() == 1 ? a.countQty : a.ReceivePlanBarcode.Count(),
                                            weightPerUnit = a.weightPerUnit,
                                            SumWeight = (decimal)(a.ReceivePlanBarcode.Count() > 0 ? a.ReceivePlanBarcode.Sum(x => x.qty) : 0),
                                            IsCheck = a.ReceivePlanBarcode.Where(x => x.StatusQA != null && x.rmId == a.rmId && x.LotNo_Group == (a.lotNo != null ? a.lotNo : "")).Count() > 0,
                                            LabelColor = a.colorCode,
                                        }).ToList();
                    }

                }
                else
                {
                    lstLotNoByRM = null;
                }
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                if (checkstatusdtl < 1)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error,"รายการนี้ถูกเช็คหมดแล้ว");
                }

            }
            catch (HttpRequestExceptionEx e)
            {
                lstLotNoByRM = null;
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error,"โหลดข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                lstLotNoByRM = null;
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }

        }

        private async void OnGetQrData()
        {
            if (!string.IsNullOrEmpty(Barcode))
            {
                OnReadTextData();
                return;
            }
            await GetQrData();
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

        private void OnResetBarcode()
        {
            Barcode = string.Empty;
        }
        private async Task GetQrData()
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    Barcode = result;
                    GetRMByBarcodeDataAsync();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ระบบแสกนผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private async Task GetRMByBarcodeDataAsync() /// check key link
        {
            var mst = selectPlan.Clone<MaterialReceivePlanMst>();
            var dtlbarcode = selectPlan.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
            var rmname = Barcode.Substring(0, 1);
            if (rmname == "I" || rmname == "S")
            {
                var RM = dtlbarcode.Where(x => x.barcode + x.batchNo == Barcode).FirstOrDefault();
                if (RM != null)
                {
                    var pilot = RM.piLotNo;
                    var dtlid = RM.receivePlanDtlId;
                    var dtllotgroup = RM.LotNo_Group;
                    var dtllst = mst.receivePlanDtl.Where(x => x.Id == dtlid && x.lotNo == dtllotgroup).ToList();
                    var barlst = dtllst.SelectMany(x => x.ReceivePlanBarcode).ToList();
                    var checkpilot = barlst.Where(x => x.piLotNo == pilot).ToList();
                    var dtl = new List<MaterialReceivePlanDtl>();
                    var selectdtl = dtllst.FirstOrDefault();
                    selectdtl.ReceivePlanBarcode = checkpilot;
                    dtl.Add(selectdtl);
                    mst.receivePlanDtl = dtl;
                    //_settingsService.BarcodeSetting = Barcode;
                    await _navigationService.NavigateToAsync<QualityCheckListDtlBarcodeViewModel>(mst);
                }
                else
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่มีข้อมูลบาร์โค้ดนี้");
                }
            }
            else
            {
                var RM = dtlbarcode.Where(x => x.barcode == Barcode).FirstOrDefault();
                if (RM != null)
                {
                    var dtlid = RM.receivePlanDtlId;
                    var dtllot = RM.LotNo_Group;
                    var dtllst = mst.receivePlanDtl.Where(x => x.lotNo == dtllot && x.Id == dtlid).ToList();
                    mst.receivePlanDtl = dtllst;
                    //_settingsService.BarcodeSetting = Barcode;
                    await _navigationService.NavigateToAsync<QualityCheckListDtlBarcodeViewModel>(mst);
                }
                else
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่มีข้อมูลบาร์โค้ดนี้");
                }
            }
        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<QualityCheckListDtlBarcodeView, bool>(this, MessagingConstants.RecheckQuality,
              (qualityCheckListDtlBarcodeView, recheckQuality) => OnRefreshQualityPageAsync(recheckQuality));
        }
        public async void OnRefreshQualityPageAsync(bool repage)
        {
            if (Convert.ToUInt32(_settingsService.ReceiveMstIdSetting) == selectPlan.Id)
            {
                if (repage)
                {
                    if (_selectPlan.typeSuplierId != 1)
                    {
                        GetLotNoByRMIDAsync();
                    }
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
