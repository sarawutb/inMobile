using Acr.UserDialogs.Infrastructure;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Innovation.Mobile.App.ViewModels
{
    public class PickingListDtlViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ISettingsService _settingsService;
        private readonly ILoggingService _loggingService;
        private ObservableCollection<MaterialPickingDtlVM> _lstPickingDtl;
        private List<MaterialPickingDtlVM> _lstPickingGroupDtl, _lstPickingBarcode;
        private MaterialPickingMstVM _materialPickingMst;
        private MaterialPickingDtlVM _selectPickingDtl;
        private List<MaterialRequestMSTReferenceVM> _materialRequestMSTReferences;
        private string _title, _barcode, _rmIDDtl, _pickingStatus;
        private decimal _weightPerUnit, _residue, _barcodeBalance, _requestweight, _isPackTotal, _requestpack;
        private int _pickAmount;
        private bool _IsPickAmount, _Ispack, _isfullpack;
        private RmBalVM _rmBalData;
        public PickingListDtlViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService
            , IMaterialAutoReceivingService materialAutoReceivingService
            , ISettingsService settingsService
            , ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _materialAutoReceivingService = materialAutoReceivingService;
            _settingsService = settingsService;
            _loggingService = loggingService;
        }

        public ICommand RMTappedCommand => new Command<MaterialPickingDtlVM>(OnRMTapped);
        public ICommand FIFOCommand => new Command<MaterialPickingDtlVM>(OnFIFOShow);
        public ICommand RmPackCommand => new Command(OnSetRMFullPack);
        public ICommand QrScanCommand => new Command(OnGetQrData);
        public ICommand BtnScanCommand => new Command(OnReadTextData);
        public ICommand PickAmountChangedCommand => new Command<string>(OnResidueCaculate);
        public ICommand SaveBarcodeCommand => new Command(OnSaveBarcode);
        public ICommand DeleteBarcodeCommand => new Command(OnDeleteBarcode);
        public ICommand PrintBarcodeCommand => new Command(OnPrintBarcode);
        public ICommand ScanCommand => new Command(OnGetBarcodeData);
        public ICommand SendDTLCommand => new Command<MaterialPickingDtlVM>(OnSendDTL);
        public ICommand NothingCommand => new Command<MaterialPickingDtlVM>(OnNothing);
        public ICommand PrintDeliveryReportCommand => new Command(OnPrintDeliveryReport);

        public override async Task InitializeAsync(object PickingPlan)
        {
            MaterialPickingMst = (MaterialPickingMstVM)PickingPlan;
            Title = MaterialPickingMst.RequestDocNo;
            GetPickingListDtl();
            IsPickAmount = false;
        }

        public ObservableCollection<MaterialPickingDtlVM> lstPickingDtl
        {
            get => _lstPickingDtl;
            set
            {
                _lstPickingDtl = value;
                OnPropertyChanged();
            }
        }
        public MaterialPickingMstVM MaterialPickingMst
        {
            get => _materialPickingMst;
            set
            {
                _materialPickingMst = value;
                OnPropertyChanged();
            }
        }
        public decimal Request_weight
        {
            get => _requestweight;
            set
            {
                _requestweight = value;
                OnPropertyChanged();
            }
        }
        public decimal Request_Pack
        {
            get => _requestpack;
            set
            {
                _requestpack = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public MaterialPickingDtlVM SelectPickingDtl
        {
            get => _selectPickingDtl;
            set
            {
                _selectPickingDtl = value;
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
        public decimal WeightPerUnit
        {
            get => _weightPerUnit;
            set
            {
                _weightPerUnit = value;
                OnPropertyChanged();
            }
        }
        public decimal PackTotal
        {
            get => _isPackTotal;
            set
            {
                _isPackTotal = value;
                OnPropertyChanged();
            }
        }
        public bool IsPickAmount
        {
            get => _IsPickAmount;
            set
            {
                _IsPickAmount = value;
                OnPropertyChanged();
            }
        }
        public bool IsPack
        {
            get => _Ispack;
            set
            {
                _Ispack = value;
                OnPropertyChanged();
            }
        }
        public bool IsFullPack
        {
            get => _isfullpack;
            set
            {
                _isfullpack = value;
                OnPropertyChanged();
            }
        }
        public int PickAmount
        {
            get => _pickAmount;
            set
            {
                _pickAmount = value;
                OnPropertyChanged();
            }
        }
        public decimal Residue
        {
            get => _residue;
            set
            {
                _residue = value;
                OnPropertyChanged();
            }
        }
        public RmBalVM RmBalData
        {
            get => _rmBalData;
            set
            {
                _rmBalData = value;
                OnPropertyChanged();
            }
        }
        public List<MaterialPickingDtlVM> lstPickingGroupDtl
        {
            get => _lstPickingGroupDtl;
            set
            {
                _lstPickingGroupDtl = value;
                OnPropertyChanged();
            }
        }
        public decimal BarcodeBalance
        {
            get => _barcodeBalance;
            set
            {
                _barcodeBalance = value;
                OnPropertyChanged();
            }

        }
        public List<MaterialPickingDtlVM> lstPickingBarcode
        {
            get => _lstPickingBarcode;
            set
            {
                _lstPickingBarcode = value;
                OnPropertyChanged();
            }
        }
        public string RMIDDtl
        {
            get => _rmIDDtl;
            set
            {
                _rmIDDtl = value;
                OnPropertyChanged();
            }
        }
        public string PickingStatus
        {
            get => _pickingStatus;
            set
            {
                _pickingStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _printDeliveryReportBtnVisibled;
        public bool PrintDeliveryReportBtnVisibled
        {
            get => _printDeliveryReportBtnVisibled;
            set
            {
                _printDeliveryReportBtnVisibled = value;
                OnPropertyChanged();
            }
        }

        private void OnRMTapped(MaterialPickingDtlVM obj)
        {
            SelectPickingDtl = obj;
            var test = lstPickingDtl;
            if (obj != null)
            {
                RMIDDtl = obj.RmId;
                var statuspick = obj.PickingDtlStatusNameTH;
                if (statuspick == null || statuspick == "")
                {
                    PickingStatus = "รอหยิบ";
                }
                else
                {
                    PickingStatus = statuspick;
                }
                foreach (var items in lstPickingDtl)
                {
                    items.Package_QTY_Show = items.Package_QTY == 0 ? 1 : items.Package_QTY;
                }
                var statuschk = lstPickingDtl.Where(x => x.RmId == obj.RmId && x.Barcode != "").ToList();
                if (statuschk.Count != 0)
                {
                    var chklot = statuschk.Where(x => x.Request_LotNo == obj.Request_LotNo || x.Request_LotNo == "Free Lot").FirstOrDefault();
                    if (chklot != null)
                    {
                        if (obj.Request_LotNo == chklot.Request_LotNo)
                        {
                            lstPickingBarcode = statuschk.Where(x => x.Request_LotNo == obj.Request_LotNo && x.ARP_Material_PickingList_DTL_Status_ID <= 5).ToList();// แก้ Lotno
                        }
                        else
                        {
                            lstPickingBarcode = null;
                        }
                    }
                    else
                    {
                        lstPickingBarcode = null;
                    }
                }
                else
                {
                    lstPickingBarcode = null;
                }
            }
        }

        private async Task GetPickingListDtl()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                    lstPickingDtl = (await _materialAutoReceivingService.GetMaterialPickingDtlAsync(MaterialPickingMst.Id, 5)).ToObservableCollection();
                    if ((MaterialPickingMst.pickingPurposeId == 4)
                        && (((MaterialPickingMst.ReceiveSite != 1 && MaterialPickingMst.TransferToSite != 5)
                        || (MaterialPickingMst.ReceiveSite != 5 && MaterialPickingMst.TransferToSite != 1))
                        && (MaterialPickingMst.ReceiveSite != MaterialPickingMst.TransferToSite))
                        )
                    {
                        foreach (var itemdtl in lstPickingDtl)
                        {
                            var maprm = (await _materialAutoReceivingService.GetMaterialProductionCrossSiteAsync(itemdtl.ChangeCodeRM, (int)MaterialPickingMst.ReceiveSite, (int)MaterialPickingMst.TransferToSite)).ToList();
                            if (maprm.Count < 1)
                            {
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบการ Map วัตถุดิบ โค้ด: " + itemdtl.ChangeCodeRM + "(RM Supplier) ที่ " + MaterialPickingMst.TransferToSiteName + " กรุณา Map วัตถุดิบก่อน");
                                await _navigationService.NavigateBackAsync();
                                break;
                            }
                        }
                    }
                    ShowGroupPickingDtlAsync(lstPickingDtl);
                    MaterialPickingMst.pickingDtl = new List<MaterialPickingDtlVM>(lstPickingDtl);
                    OnRMTapped(SelectPickingDtl);

                    if (lstPickingDtl.Count == 0)
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "ไม่มีรายการที่ต้องหยิบขณะนี้", async () =>
                        {
                            await _navigationService.NavigateBackAsync();
                        });
                    }

                    _materialRequestMSTReferences = await _materialAutoReceivingService.GetMaterialRequestMSTReferences(MaterialPickingMst.MaterialRequestMstId);

                    if (_materialRequestMSTReferences.Any(x => x.DocRefTypeId == 2))
                    {
                        if (lstPickingDtl.All(x => x.ARP_Material_PickingList_DTL_Status_ID == 5))
                        {
                            PrintDeliveryReportBtnVisibled = true;
                        }
                        else
                        {
                            PrintDeliveryReportBtnVisibled = false;
                        }
                    }
                    else
                    {
                        PrintDeliveryReportBtnVisibled = false;
                    }

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                }
                catch (Exception e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

                }
            }
        }

        private async void ShowGroupPickingDtlAsync(ObservableCollection<MaterialPickingDtlVM> lstRequestPickingDtl)
        {
            var materialPickingDtls = (await _materialAutoReceivingService.GetMaterialPickingDtlAsync(MaterialPickingMst.Id, 5)).ToObservableCollection();
            var groupdtl = (from b in materialPickingDtls
                            group b by new
                            {
                                RMCode = b.RmId,
                                RmName = b.RmName,
                                LotNo = b.Request_LotNo,
                                RequestWeight = b.RequestWeight,

                            } into dtlgrp
                            select new MaterialPickingDtlVM()
                            {
                                RmId = dtlgrp.Key.RMCode,
                                RmName = dtlgrp.Key.RmName,
                                Request_LotNo = dtlgrp.Key.LotNo,
                                RequestWeight = dtlgrp.Key.RequestWeight,
                                Net_Weight = dtlgrp.Sum(x => x.Net_Weight),
                                Total_Weight = (decimal)(dtlgrp.Sum(x => x.Total_Weight)
                                        + dtlgrp.Where(x => x.ARP_Material_PickingList_Weighing_Status_ID == 3).Sum(x => x.Residue_Weight)),
                                Residue_Weight = dtlgrp.Where(x => x.ARP_Material_PickingList_Weighing_Status_ID != 3).Sum(x => x.Request_Residue_Weight),
                            }).ToList(); /// เช็คดู
            var grouptest = groupdtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID <= 5).ToList();

            var GroupDtl = (from a in lstRequestPickingDtl
                            group a by new
                            {
                                RMCode = a.RmId,
                                RmName = a.RmName,
                                LotNo = a.Request_LotNo,
                                RequestWeight = a.RequestWeight,
                                ARP_Material_PickingList_DTL_Status_ID = a.ARP_Material_PickingList_DTL_Status_ID,
                                PickingDtlStatusNameTH = a.PickingDtlStatusNameTH,

                            } into grp
                            select new MaterialPickingDtlVM()
                            {
                                RmId = grp.Key.RMCode,
                                RmName = grp.Key.RmName,
                                Request_LotNo = grp.Key.LotNo,
                                RequestWeight = grp.Key.RequestWeight,
                                Net_Weight = grp.Sum(x => x.Net_Weight),
                                Total_Weight = (decimal)(grp.Sum(x => x.Total_Weight)
                                  + grp.Where(x => x.ARP_Material_PickingList_Weighing_Status_ID == 3).Sum(x => x.Residue_Weight)),
                                Residue_Weight = grp.Where(x => x.ARP_Material_PickingList_Weighing_Status_ID != 3).Sum(x => x.Request_Residue_Weight),
                                ARP_Material_PickingList_DTL_Status_ID = grp.Key.ARP_Material_PickingList_DTL_Status_ID,
                                PickingDtlStatusNameTH = grp.Key.PickingDtlStatusNameTH,
                            }).ToList();

            var grplst = new List<MaterialPickingDtlVM>();
            foreach (var item in grouptest)
            {
                var selectgrp = GroupDtl.Where(x => x.RmId == item.RmId && x.Request_LotNo == item.Request_LotNo && x.PILotNo == item.PILotNo).FirstOrDefault();
                if (selectgrp != null)
                {
                    selectgrp.Total_Weight = item.Total_Weight;
                    grplst.Add(selectgrp);
                }
            }
            lstPickingGroupDtl = grplst;
        }

        private void OnFIFOShow(MaterialPickingDtlVM selectFIFO)
        {
            OnRMTapped(selectFIFO);
            SelectPickingDtl = selectFIFO;
            _navigationService.NavigateToAsync<PickingFIFOViewModel>(SelectPickingDtl);
        }

        private async void OnGetQrData(object obj)
        {
            Entry mytext = (Entry)obj;
            DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
            if (!string.IsNullOrEmpty(Barcode))
            {
                await GetReadBarcodeTextData(Barcode);
            }
            else
            {
                await GetQrData(mytext);
            }
            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
        }

        private async void OnReadTextData()
        {
            await GetReadBarcodeTextData(Barcode);
        }

        private async Task GetQrData(Entry mytext)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    Barcode = result;

                    if (!IsBusy)
                    {
                        try
                        {
                            IsBusy = true;

                            var bal = await _materialAutoReceivingService.GetRMCompoundByBarcodeAsync(Barcode);

                            RmBalData = bal;
                            if (RmBalData != null)
                            {
                                var word = RmBalData.RM_ID.Substring(0, 1);
                                if (word == "I" || word == "S")
                                {
                                    IsPickAmount = false;
                                }
                                else
                                {
                                    IsPickAmount = true;
                                    mytext.Focus();
                                    mytext.SelectionLength = mytext.Text.Length;
                                }
                            }
                            IsBusy = false;

                            CheckBarcode();
                        }
                        catch (HttpRequestExceptionEx e)
                        {
                            IsBusy = false;
                            _loggingService.Error(e.Message);
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                        }
                        catch (Exception e)
                        {
                            IsBusy = false;
                            _loggingService.Error(e.ToString());
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClearBarcodeData();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private async Task GetReadBarcodeTextData(string barcode)
        {
            try
            {
                Barcode = barcode;
                if (!IsBusy)
                {
                    try
                    {
                        IsBusy = true;
                        var bal = new RmBalVM();
                        try
                        {
                            bal = await _materialAutoReceivingService.GetRMCompoundByBarcodeAsync(Barcode);
                        }
                        catch
                        {
                            bal = null;
                        }
                        RmBalData = bal;
                        if (RmBalData != null)
                        {
                            var word = RmBalData.RM_ID.Substring(0, 1);
                            if (word == "I" || word == "S")
                            {
                                IsPickAmount = false;
                            }
                            else
                            {
                                IsPickAmount = true;
                            }
                        }
                        IsBusy = false;

                        CheckBarcode();
                    }
                    catch (HttpRequestExceptionEx e)
                    {
                        IsBusy = false;
                        _loggingService.Error(e.Message);
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                    }
                    catch (Exception e)
                    {
                        IsBusy = false;
                        _loggingService.Error(e.ToString());
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ClearBarcodeData();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }

        private void ClearBarcodeData()
        {
            RmBalData = null;
            IsPack = false;
            Barcode = "";
            BarcodeBalance = 0m;
            WeightPerUnit = 0m;
            PickAmount = 0;
            Residue = 0m;
            Request_weight = 0m;
        }

        private async void CheckBarcode()
        {
            if (_rmBalData != null)
            {
                var chkData = _lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID).ToList();
                if (chkData.Count != 0)
                {
                    if (_rmBalData.Site_ID != int.Parse(_settingsService.SiteIdSetting))
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + _rmBalData.BARCODE + " อยู่ Site อื่น");
                        IsPickAmount = false;
                    }
                    else if ((bool)_rmBalData.LOCK)
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + _rmBalData.BARCODE + " QA Lock อยู่");
                        IsPickAmount = false;
                    }
                    else if (_rmBalData.Owner_Site_ID.ToString() != _settingsService.SiteIdSetting)
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + _rmBalData.BARCODE + " Owner Site ไม่ถูกต้อง");
                        IsPickAmount = false;
                    }
                    else if ((decimal)_rmBalData.QTY <= 0m)
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + _rmBalData.BARCODE + "ไม่มีน้ำหนักคงเหลือ");
                        IsPickAmount = false;
                        RmBalData = null;
                    }
                    else
                    {
                        if (chkData[0].Request_LotNo == "Free Lot")
                        {
                            if (!IsBusy)
                            {
                                try
                                {
                                    IsBusy = true;
                                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                    var defaultRM = await _materialAutoReceivingService.GetDefaultContWeightAsync(_rmBalData.RM_ID, _rmBalData.RmSuppCode);

                                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                    IsBusy = false;

                                    if (defaultRM != null)
                                    {
                                        WeightPerUnit = defaultRM.contWeight;
                                    }

                                    BarcodeBalance = (decimal)_rmBalData.QTY;
                                    var dtl = lstPickingDtl.Where(x => x.RmId == RmBalData.RM_ID).FirstOrDefault();
                                    OnRMTapped(dtl);
                                    PickAmountCalculate();
                                }
                                catch (HttpRequestExceptionEx e)
                                {
                                    IsBusy = false;
                                    _loggingService.Error(e.Message);
                                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                                }
                                catch (Exception e)
                                {
                                    IsBusy = false;
                                    _loggingService.Error(e.ToString());
                                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                                }
                            }
                        }
                        else
                        {
                            var chk = chkData.Where(x => x.Request_LotNo == _rmBalData.LOT_NO).FirstOrDefault();
                            if (chk != null)
                            {
                                if (!IsBusy)
                                {
                                    try
                                    {
                                        IsBusy = true;
                                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                        var defaultRM = await _materialAutoReceivingService.GetDefaultContWeightAsync(_rmBalData.RM_ID, _rmBalData.RmSuppCode);

                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        IsBusy = false;

                                        if (defaultRM != null)
                                        {
                                            WeightPerUnit = defaultRM.contWeight;
                                        }

                                        BarcodeBalance = (decimal)_rmBalData.QTY;
                                        var dtl = lstPickingDtl.Where(x => x.RmId == RmBalData.RM_ID && x.Request_LotNo == RmBalData.LOT_NO).FirstOrDefault();
                                        OnRMTapped(chk);
                                        PickAmountCalculate();
                                    }
                                    catch (HttpRequestExceptionEx e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.Message);
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                                    }
                                    catch (Exception e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.ToString());
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

                                    }
                                }
                            }
                            else
                            {
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "Lot " + _rmBalData.LOT_NO + " ไม่อยู่ใน Lot ที่ต้องการหยิบของ RM Code:" + _rmBalData.RM_ID);
                                IsPickAmount = false;
                                RmBalData = null;
                            }
                        }

                    }
                }
                else
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบ RM Code" + _rmBalData.RM_ID);
                    IsPickAmount = false;
                    RmBalData = null;
                }
            }
            else
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบ Barcode : " + _barcode + " ที่แสกน");
                ClearBarcodeData();
            }
        }
        private async void OnSetRMFullPack()///
        {
            Residue = 0m;
            IsFullPack = true;

            if (RmBalData.QTY > Request_Pack)
            {
                PackTotal = Request_Pack;
                Request_weight = (decimal)(PackTotal - Request_Pack);
            }
            else
            {
                Request_weight = (decimal)(Request_Pack - RmBalData.QTY);
            }
            //SaveBarcode();
        }

        private void PickAmountCalculate()
        {
            decimal weightRequestBal = 0;
            var requestData = _lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID && (x.Request_LotNo == _rmBalData.LOT_NO || x.Request_LotNo == "Free Lot")).FirstOrDefault();
            if (_lstPickingBarcode != null)
            {
                if (_lstPickingBarcode.Count != 0)
                {
                    var barcodeData = _lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID).FirstOrDefault();
                    weightRequestBal = requestData.RequestWeight - barcodeData.Net_Weight;
                    OnRMTapped(barcodeData);
                }
                else
                {
                    weightRequestBal = requestData.RequestWeight;
                }
            }
            else
            {
                weightRequestBal = requestData.RequestWeight;
            }

            if (PickAmount != 0)
            {
                PickAmount = Convert.ToInt32(Math.Floor(weightRequestBal / PickAmount));
            }
            else
            {
                PickAmount = 0;
            }

            OnResidueCaculate(PickAmount.ToString());
        }

        private void OnResidueCaculate(string value)
        {
            var a = int.TryParse(value, out int retval);
            PickAmount = retval;
            decimal weightRequestBal = 0, pickSum = 0, residueweight = 0;
            if (_rmBalData == null)
            {
                Residue = 0;
                return;
            }
            var requestDataDtl = _lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID).FirstOrDefault();
            var requestData = _lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID && (x.Request_LotNo == _rmBalData.LOT_NO || x.Request_LotNo == "Free Lot")).ToList();
            if (_lstPickingDtl != null)
            {
                if (_lstPickingDtl.Count != 0)
                {
                    weightRequestBal = (decimal)(requestData[0].RequestWeight - (requestData.Sum(x => x.Total_Weight) + requestData.Sum(x => x.Request_Residue_Weight)));
                }
                else
                {
                    weightRequestBal = requestDataDtl.RequestWeight;
                }
            }
            else
            {
                weightRequestBal = requestDataDtl.RequestWeight;
            }

            if (PickAmount != 0)
            {
                var intPickAmount = Convert.ToInt32(Math.Floor(weightRequestBal / (PickAmount * _weightPerUnit)));
                pickSum = PickAmount * _weightPerUnit;
            }
            residueweight = (decimal)_rmBalData.QTY - pickSum;
            Request_weight = pickSum != 0 ? weightRequestBal - (pickSum) : weightRequestBal;
            if (_rmBalData.QTY > weightRequestBal)
            {
                Residue = weightRequestBal - pickSum;
            }
            else
            {
                Residue = residueweight;
            }

            if (_rmBalData.QTY > (PickAmount * WeightPerUnit))
            {
                if (WeightPerUnit > Residue && Residue > 0)
                {
                    GetRMpack();
                    if (IsPack)
                    {
                        if ((decimal)_rmBalData.QTY == Request_weight)
                        {
                            PackTotal = (decimal)_rmBalData.QTY;
                        }
                        else if ((decimal)_rmBalData.QTY > Request_weight)
                        {
                            PackTotal = Request_weight;
                        }
                    }
                    else
                    {
                        PackTotal = (decimal)_rmBalData.QTY;
                    }
                    Request_Pack = weightRequestBal;

                }
                else
                {
                    IsPack = false;
                }

            }
            else
            {
                IsPack = false;
            }


        }
        private async void GetRMpack()
        {
            try
            {
                IsPack = await _materialAutoReceivingService.GetCheckPickRMPack(RmBalData.IN_ID, RmBalData.RM_ID, RmBalData.LOT_NO, RmBalData.PI_LOT_NO);
            }
            catch (HttpRequestExceptionEx e)
            {
                IsBusy = false;
                _loggingService.Error(e.Message);
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                IsBusy = false;
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
            }

        }

        private async void OnSaveBarcode()
        {
            SaveBarcode();
        }

        private async Task SaveBarcode()
        {
            bool checkSave = await ValidationData(1);
            if (!checkSave)
            {
                return;
            }
            MaterialPickingDtlVM barData = null;
            List<MaterialPickingDtlVM> lstDtl = new List<MaterialPickingDtlVM>();
            var dtlclone = lstPickingDtl.Clone();
            decimal total = 0;
            if (lstPickingBarcode != null)
            {
                if (lstPickingBarcode.Count != 0)
                {
                    total = (from a in lstPickingBarcode
                             where a.RmId == _rmBalData.RM_ID
                             select a).Sum(z => z.Total_Weight);
                }
                else
                {
                    total = 0;
                }

            }
            else
            {
                total = 0;
            }

            var dtlData = lstPickingDtl.Where(x => x.RmId == _rmBalData.RM_ID && x.Barcode == "" && (x.Request_LotNo == RmBalData.LOT_NO || x.Request_LotNo == "Free Lot")).FirstOrDefault();
            if (dtlData != null)
            {
                dtlData.IsWeight = Residue != 0m;
                dtlData.Barcode = RmBalData.BARCODE;
                dtlData.Weight_Per_Package = WeightPerUnit;
                dtlData.Total_Weight = IsFullPack == true ? PackTotal : WeightPerUnit * PickAmount;
                dtlData.Package_QTY = PickAmount;
                dtlData.Request_Residue_Weight = Residue;
                dtlData.Residue_Weight = 0;
                dtlData.Net_Weight = IsFullPack == true ? PackTotal : WeightPerUnit * PickAmount;
                dtlData.Picking_By = _settingsService.UserSetting.UserId;
                dtlData.Picking_By_Name = _settingsService.UserFullNameSetting;
                dtlData.Picking_Date = DateTime.Now;
                dtlData.ARP_Material_PickingList_Weighing_Status_ID = 1;
                dtlData.ARP_Material_PickingList_DTL_Status_ID = 2;
                dtlData.BatchNo = RmBalData.BatchNo;
                dtlData.PILotNo = _rmBalData.PI_LOT_NO;
                dtlData.LotNo = _rmBalData.LOT_NO;
                dtlData.CanPrint = Residue == 0m;
                dtlData.Package_QTY_Show = PickAmount == 0 ? 1 : PickAmount;
                dtlData.state = Enumerations.StateData.State.Insert;
                barData = dtlData;
            }
            else
            {
                var dtl = _materialPickingMst.pickingDtl.Where(x => x.RmId == _rmBalData.RM_ID && (x.Request_LotNo == RmBalData.LOT_NO || x.Request_LotNo == "Free Lot")).FirstOrDefault();
                barData = new MaterialPickingDtlVM()
                {
                    IsWeight = Residue != 0m,
                    MaterialPickinglistMstId = dtl.MaterialPickinglistMstId,
                    RmId = dtl.RmId,
                    RmName = dtl.RmName,
                    ChangeCodeRM = dtl.ChangeCodeRM,
                    Request_LotNo = dtl.Request_LotNo == null ? "" : dtl.Request_LotNo,
                    RequestWeight = dtl.RequestWeight,
                    Barcode = RmBalData.BARCODE,
                    Weight_Per_Package = WeightPerUnit,
                    Total_Weight = WeightPerUnit * PickAmount,
                    Request_Residue_Weight = Residue,
                    Package_QTY = PickAmount,
                    Residue_Weight = 0,
                    BatchNo = _rmBalData.BatchNo,
                    PILotNo = _rmBalData.PI_LOT_NO,
                    LotNo = _rmBalData.LOT_NO,
                    Net_Weight = WeightPerUnit * PickAmount,
                    Picking_By = _settingsService.UserSetting.UserId,
                    Picking_By_Name = _settingsService.UserFullNameSetting,
                    Picking_Date = DateTime.Now,
                    ARP_Material_PickingList_Weighing_Status_ID = 1,
                    ARP_Material_PickingList_DTL_Status_ID = 2,
                    CanPrint = Residue == 0m,
                    Package_QTY_Show = PickAmount == 0 ? 1 : PickAmount,
                    state = Enumerations.StateData.State.Insert
                };
            }

            lstDtl.Add(barData);

            MaterialPickingMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
            MaterialPickingMst.PickingStatusId = 2;
            MaterialPickingMst.state = Enumerations.StateData.State.Update;
            MaterialPickingMst.pickingDtl = lstDtl;

            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                    await _materialAutoReceivingService.SavePicking(MaterialPickingMst);

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                    if (!IsBusy)
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "การบันทึกข้อมูลสำเร็จ");
                        Request_weight = 0m;
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    barData = null;
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                }
                catch (Exception e)
                {
                    IsBusy = false;
                    barData = null;
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
            }

            RefreshData(barData);
        }

        private void RefreshData(MaterialPickingDtlVM barData = null)
        {
            ClearBarcodeData();
            GetPickingListDtl();
            OnRMTapped(barData);
        }

        private async Task<bool> ValidationData(int mode)
        {
            bool retval = true;

            if (mode == 1) //add barcode
            {
                if (RmBalData == null)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาแสกนบาร์โค้ด");
                }
                else if (PickAmount < 0)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "จำนวนเต็มไม่สามารถใส่เป็นจำนวนติดลบได้");

                }
                else if (Residue < 0m)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "จำนวนเศษไม่ถูกต้อง");

                }
                else if (PickAmount == 0 && Residue == 0m)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "จำนวนเต็มและจำนวนเศษไม่ถูกต้อง");
                }
                else if ((WeightPerUnit * PickAmount) > BarcodeBalance)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "จำนวนที่หยิบมีมากกว่าน้ำหนักคงเหลือในระบบ กรุณาตรวจสอบ");
                }
                else
                {
                    var dtl = lstPickingDtl
                        .Where(x => x.RmId == RmBalData.RM_ID)
                        .Where(x => x.Request_LotNo == "Free Lot" ? true : x.Request_LotNo == RmBalData.LOT_NO)
                        .LastOrDefault();
                    var dtlData = new MaterialPickingDtlVM();
                    if (dtl.RmId.Substring(0, 1) == "I" || dtl.RmId.Substring(0, 1) == "S")
                    {
                        dtlData = _lstPickingDtl.Where(x => x.Barcode + x.BatchNo == _rmBalData.BARCODE + _rmBalData.BatchNo).FirstOrDefault();
                    }
                    else
                    {
                        dtlData = _lstPickingDtl.Where(x => x.Barcode == _rmBalData.BARCODE).FirstOrDefault();
                    }
                    if (dtlData != null)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "มีบาร์โค้ด : " + _rmBalData.BARCODE + " นี้อยู่แล้ว");
                        IsPickAmount = false;
                    }
                    else if ((bool)_rmBalData.LOCK)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด : " + _rmBalData.BARCODE + " นี้ QA Lock อยู่");
                        IsPickAmount = false;
                    }
                    else if (dtl != null)
                    {
                        if (dtl.ARP_Material_PickingList_DTL_Status_ID > 2)
                        {
                            retval = false;
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถเพิ่มรายการได้ เนื่องจากวัตถุดิบนี้อยู่ในกระบวนการอื่นแล้ว");
                            IsPickAmount = false;
                        }
                    }
                }
            }
            else if (mode == 2) //Delete
            {
                if (!IsBusy)
                {
                    try
                    {
                        var lstDel = lstPickingBarcode.Where(x => x.IsSelect == true).ToList();
                        if (lstDel != null)
                        {
                            IsBusy = true;
                            DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                            foreach (var item in lstDel)
                            {
                                var weightingData = await _materialAutoReceivingService.GetPickingWeightingByBarcodeAsync(item.MaterialPickinglistMstId, item.Barcode);
                                item.CanDelete = !weightingData;
                            }

                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            IsBusy = false;

                            var chk = lstDel.Where(x => x.CanDelete == false).Count();
                            if (chk > 0)
                            {
                                retval = false;
                                foreach (var item in lstDel)
                                {
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.Barcode + " นี้ถูกนำไปชั่งแล้่ว");
                                    break;
                                }
                            }
                        }
                    }
                    catch (HttpRequestExceptionEx e)
                    {
                        IsBusy = false;
                        _loggingService.Error(e.Message);
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                    }
                    catch (Exception e)
                    {
                        IsBusy = false;
                        _loggingService.Error(e.ToString());
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                    }
                }
            }
            else if (mode == 3) //Send Weighing
            {
                var lstDtl = lstPickingDtl.Where(x => x.RmId == SelectPickingDtl.RmId && (x.Request_LotNo == SelectPickingDtl.Request_LotNo || x.Request_LotNo == "Free Lot")).ToList();
                if (MaterialPickingMst.isChangeRm == true)
                {
                    decimal chkWeight = (decimal)lstDtl.Sum(x => x.Total_Weight + x.Request_Residue_Weight);
                    decimal chkResidue = (decimal)lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID == 2
                                        && x.ARP_Material_PickingList_Weighing_Status_ID == 1).Sum(x => x.Request_Residue_Weight);
                    if (chkResidue == 0m)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine + "เนื่องจากไม่มีน้ำหนักเศษ");
                    }
                    else if (lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID > 2 && x.IsWeight == true).Count() > 0)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine + "เนื่องจากมีการส่งชั่งน้ำหนักไปแล้ว");
                    }
                }
                else if (MaterialPickingMst.isChangeRm == false && MaterialPickingMst.pickingPurposeId == 8)
                {
                    decimal chkResidue = (decimal)lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID == 2
                                        && x.ARP_Material_PickingList_Weighing_Status_ID == 1).Sum(x => x.Request_Residue_Weight);
                    if (chkResidue == 0m)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine + "เนื่องจากไม่มีน้ำหนักเศษ");
                    }
                    else if (lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID > 2 && x.IsWeight == true).Count() > 0)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine + "เนื่องจากมีการส่งชั่งน้ำหนักไปแล้ว");
                    }
                }
                else
                {
                    decimal chkResidue = (decimal)lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID == 2
                                        && x.ARP_Material_PickingList_Weighing_Status_ID == 1).Sum(x => x.Request_Residue_Weight);
                    if (chkResidue == 0m)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine+ "เนื่องจากไม่มีน้ำหนักเศษ");
                    }
                    else if (lstDtl.Where(x => x.ARP_Material_PickingList_DTL_Status_ID > 2 && x.IsWeight == true).Count() > 0)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "รายการวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ไม่สามารถส่งชั่งได้" + Environment.NewLine + "เนื่องจากมีการส่งชั่งน้ำหนักไปแล้ว");
                    }
                }

            }
            else if (mode == 4) //Print
            {
                if (MaterialPickingMst.isChangeRm == true)
                {
                    bool chkpackprint;
                    var lstPrint = lstPickingBarcode.Where(x => x.IsSelect == true).ToList();
                    var chkisweight = lstPrint.Where(x => x.IsWeight == true).Count();
                    if (chkisweight < 1)
                    {
                        foreach (var items in lstPrint)
                        {
                            var chkrm = await _materialAutoReceivingService.GetRMCompoundByBarcodeAsync(items.Barcode);
                            chkpackprint = await _materialAutoReceivingService.GetCheckPickRMPack(chkrm.IN_ID, chkrm.RM_ID, chkrm.LOT_NO, chkrm.PI_LOT_NO);
                            items.CanPrint = chkpackprint == true ? true : (items.Weight_Per_Package == (items.Total_Weight / items.Package_QTY));
                        }
                    }
                    var chklst = lstPrint.Where(x => x.CanPrint == false).Count();
                    var chk = lstPrint.Where(x => x.CanPrint == false).Count();
                    if (chklst > 0)
                    {
                        retval = false;
                        foreach (var item in lstPrint)
                        {
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.Barcode + " นี้ ไม่สามารถพิมพ์ลาเบลได้");
                        }
                    }
                }
                else if (MaterialPickingMst.isChangeRm == false && MaterialPickingMst.pickingPurposeId == 8)
                {
                    var listdlt = lstPickingDtl.Where(x => x.RmId == SelectPickingDtl.RmId && (x.Request_LotNo == SelectPickingDtl.Request_LotNo || x.Request_LotNo == "Free Lot")).ToList();
                    var lsiprint = lstPickingDtl.Where(x => x.IsSelect == true).ToList();
                    foreach (var item in lsiprint)
                    {
                        if (item.IsSelect == true && item.IsWeight == false)
                        {
                            item.CanPrint = (item.Weight_Per_Package == (item.Total_Weight / item.Package_QTY));
                        }
                        var chkre = listdlt.Where(x => x.LotNo == item.LotNo).ToList();
                        var re = chkre.Sum(x => x.Request_Residue_Weight);
                        if (re > 0)
                        {
                            retval = false;
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.Barcode + " ในล็อต " + item.LotNo + " นี้ " + "ไม่สามารถพิมพ์ลาเบลได้ เนื่องจากมีน้ำหนักมีเศษ");
                        }

                    }
                }
                else
                {
                    var listdltp = lstPickingDtl.Where(x => x.RmId == SelectPickingDtl.RmId && (x.Request_LotNo == SelectPickingDtl.Request_LotNo || x.Request_LotNo == "Free Lot")).ToList();
                    var lsiprintp = lstPickingDtl.Where(x => x.IsSelect == true).ToList();
                    foreach (var item in lsiprintp)
                    {
                        if (item.IsSelect == true && item.IsWeight == false)
                        {
                            item.CanPrint = IsPack == true ? IsPack : (item.Weight_Per_Package == (item.Total_Weight / item.Package_QTY));
                        }
                        var chkre = listdltp.Where(x => x.LotNo == item.LotNo).ToList();
                        var re = chkre.Sum(x => x.Request_Residue_Weight);
                        if (re > 0)
                        {
                            retval = false;
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.Barcode + " ในล็อต " + item.LotNo + " นี้ " + "ไม่สามารถพิมพ์ลาเบลได้ เนื่องจากมีน้ำหนักมีเศษ");
                        }

                    }
                }

            }
            else if (mode == 5) // send nothing
            {
                List<MaterialPickingDtlVM> lst = lstPickingDtl.Where(x => x.RmId == SelectPickingDtl.RmId && (x.Request_LotNo == SelectPickingDtl.Request_LotNo || x.Request_LotNo == "Free Lot")).ToList();
                foreach (var item in lst)
                {
                    if (item.ARP_Material_PickingList_Weighing_Status_ID == 3)
                    {

                    }
                    else if (item.ARP_Material_PickingList_DTL_Status_ID > 1)
                    {
                        retval = false;
                        if (item.ARP_Material_PickingList_DTL_Status_ID == 2)
                        {
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถยืนยันการไม่พบวัตถุดิบนี้ได้ เนื่องจากมีการทำรายการที่ต้องส่งชั่ง");
                        }
                        else
                        {
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถยืนยันการไม่พบวัตถุดิบนี้ได้ เนื่องจากมีการทำรายการในขั้นตอนอื่นแล้ว");
                        }
                        break;
                    }
                }

            }

            return retval;
        }

        private async void OnDeleteBarcode()
        {
            if (lstPickingBarcode != null)
            {
                DeleteBarcode();
            }
        }

        private async Task DeleteBarcode()
        {
            int selcount = lstPickingBarcode.Where(x => x.IsSelect == true).Count();
            if (selcount <= 0)
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือกรายการที่ต้องการลบ");
                return;
            }
            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                           "ต้องการลบบาร์โค้ดที่เลือก " + selcount.ToString() + " รายการใช่หรือไม่ ?",
                           async () =>
                           {
                               var chk = await ValidationData(2);
                               if (!chk)
                               {
                                   return;
                               }
                               var lstDel = lstPickingBarcode.Where(x => x.CanDelete == true).ToList();
                               string toDeleteBarcode = string.Empty;
                               List<MaterialPickingDtlVM> lstDtl = new List<MaterialPickingDtlVM>();
                               foreach (var item in lstDel)
                               {
                                   var del = lstPickingDtl.Where(x => x.Id == item.Id).FirstOrDefault();
                                   if (del != null)
                                   {
                                       del.state = Enumerations.StateData.State.Delete;
                                       toDeleteBarcode = del.Barcode;
                                       lstDtl.Add(del);
                                   }
                               }
                               MaterialPickingMst.PickingStatusId = 2;
                               MaterialPickingMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                               MaterialPickingMst.state = Enumerations.StateData.State.Update;
                               MaterialPickingMst.pickingDtl = new List<MaterialPickingDtlVM>(lstDtl);

                               try
                               {

                                   IsBusy = true;
                                   DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                   await _materialAutoReceivingService.SavePicking(MaterialPickingMst);

                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                   IsBusy = false;
                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "ลบบาร์โค้ด " + toDeleteBarcode + " นี้เรียบร้อย");
                                   RefreshData(SelectPickingDtl);
                               }
                               catch (HttpRequestExceptionEx e)
                               {
                                   IsBusy = false;
                                   _loggingService.Error(e.Message);
                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                               }
                               catch (Exception e)
                               {
                                   IsBusy = false;
                                   _loggingService.Error(e.ToString());
                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

                               }
                           }
                           );


        }

        private async void OnGetBarcodeData()
        {
            GetBarcodeData();
        }

        private async Task GetBarcodeData()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                    RmBalData = await _materialAutoReceivingService.GetRMCompoundByBarcodeAsync(Barcode);

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;

                    CheckBarcode();
                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                }
                catch (Exception e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

                }
            }
        }

        private async void OnSendDTL(MaterialPickingDtlVM selectSend)
        {
            OnRMTapped(selectSend);
            SendDTL(selectSend);
        }

        private async Task SendDTL(MaterialPickingDtlVM selectSend)
        {
            SelectPickingDtl = selectSend;
            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem, "ต้องการยืนยันการส่งชั่งวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ใช่หรือไม่ ?\nยืนยันการส่งชั่งวัตถุดิบ",
            async () =>
            {
                try
                {
                    var chk = await ValidationData(3);
                    if (chk)
                    {
                        var lstDtl = lstPickingDtl.Where(x => x.RmId == selectSend.RmId && (x.Request_LotNo == selectSend.Request_LotNo || x.Request_LotNo == "Free Lot")).ToList();
                        if (MaterialPickingMst.isChangeRm == true)
                        {
                            var selectbar = lstDtl.Where(x => x.IsWeight == false).ToList();
                            if (lstDtl.Count > 0)
                            {
                                foreach (var item in lstDtl)
                                {
                                    item.ARP_Material_PickingList_DTL_Status_ID = item.ARP_Material_PickingList_Weighing_Status_ID == 3 ? 5 : 3;
                                    item.state = Enumerations.StateData.State.Update;
                                }

                                MaterialPickingMst.PickingStatusId = 2;
                                MaterialPickingMst.reviseBy = _settingsService.UserSetting.UserId;
                                MaterialPickingMst.state = Enumerations.StateData.State.Update;
                                MaterialPickingMst.pickingDtl = lstDtl;
                                MaterialPickingMstVM PickMstData = new MaterialPickingMstVM();
                                var pickMst = MaterialPickingMst.Clone<MaterialPickingMstVM>();
                                var lstnotIsweight = pickMst.pickingDtl.Where(x => x.IsWeight == false).ToList();
                                if (lstnotIsweight.Count > 0)
                                {
                                    foreach (var itemdtl in lstnotIsweight)
                                    {
                                        pickMst.pickingDtl.Remove(itemdtl);
                                    }
                                }

                                if (!IsBusy)
                                {
                                    try
                                    {
                                        IsBusy = true;
                                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                        await _materialAutoReceivingService.SavePicking(pickMst);

                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "การบันทึกข้อมูลสำเร็จ");
                                        IsBusy = false;

                                        RefreshData(SelectPickingDtl);
                                    }
                                    catch (HttpRequestExceptionEx e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.Message);
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                                    }
                                    catch (Exception e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.ToString());
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (lstDtl.Count > 0)
                            {
                                foreach (var item in lstDtl)
                                {
                                    if (item.Request_Residue_Weight == 0)
                                    {
                                        item.ARP_Material_PickingList_DTL_Status_ID = 5;
                                    }
                                    else
                                    {
                                        item.ARP_Material_PickingList_DTL_Status_ID = item.ARP_Material_PickingList_Weighing_Status_ID == 3 ? 5 : 3;
                                    }
                                    item.state = Enumerations.StateData.State.Update;
                                }

                                MaterialPickingMst.PickingStatusId = 2;
                                MaterialPickingMst.reviseBy = _settingsService.UserSetting.UserId;
                                MaterialPickingMst.state = Enumerations.StateData.State.Update;
                                MaterialPickingMst.pickingDtl = lstDtl;
                                if (!IsBusy)
                                {
                                    try
                                    {
                                        IsBusy = true;
                                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                        await _materialAutoReceivingService.SavePicking(MaterialPickingMst);

                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        IsBusy = false;

                                        RefreshData(SelectPickingDtl);
                                    }
                                    catch (HttpRequestExceptionEx e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.Message);
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                    }
                                    catch (Exception e)
                                    {
                                        IsBusy = false;
                                        _loggingService.Error(e.ToString());
                                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex) { throw ex; };
            });
        }

        public async void OnPrintBarcode()
        {
            PrintBarcode();
        }

        private async Task PrintBarcode()
        {
            var chk = await ValidationData(4);
            if (chk)
            {
                GetLabel();
            }
        }

        private async Task GetLabel()
        {
            int stutusWeight = 1;
            var dtl = lstPickingDtl.Where(x => x.RmId == lstPickingBarcode[0].RmId && x.ARP_Material_PickingList_Weighing_Status_ID == 3).FirstOrDefault();
            if (dtl != null)
            {
                stutusWeight = 3;
            }
            var lstPrint = lstPickingBarcode.Where(x => x.IsSelect == true).ToList();
            foreach (var item in lstPrint)
            {
                item.ARP_Material_PickingList_Weighing_Status_ID = stutusWeight;
                item.ARP_Material_PickingList_DTL_Status_ID = 5;
                item.state = Enumerations.StateData.State.Update;
            }

            MaterialPickingMst.PickingStatusId = 2;
            MaterialPickingMst.pickingDtl = lstPrint;
            IsBusy = true;

            var dtllst = (await _materialAutoReceivingService.GetMaterialPickingDtlAsync(MaterialPickingMst.Id, 6)).ToList();//status all

            IsBusy = false;
            var statusmst = dtllst.Where(x => x.ARP_Material_PickingList_DTL_Status_ID != 5 || x.ARP_Material_PickingList_DTL_Status_ID != 6).ToList();
            if (statusmst.Count <= 1)
            {
                MaterialPickingMst.PickingStatusId = 3;
            }
            MaterialPickingMst.reviseBy = _settingsService.UserSetting.UserId;
            MaterialPickingMst.state = Enumerations.StateData.State.Update;
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    if (MaterialPickingMst.isChangeRm == true)
                    {
                        var printRM = await _materialAutoReceivingService.GetPickingBarcodePrintAsync(MaterialPickingMst);

                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        IsBusy = false;
                        foreach (var item in printRM.pickingDtl)
                        {
                            if (item.ChangeCodeRM.Substring(0, 1) == "P")
                            {
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาใส่กระดาษสี" + item.colorName);
                                var ms = new MemoryStream(item.PicBarcode);
                                var printer = DependencyService.Get<IPrintService>();
                                var result = printer.PrintImageA5(ms);
                            }
                            else
                            {
                                var ms = new MemoryStream(item.PicBarcode);
                                var printer = DependencyService.Get<IPrintService>();
                                var result = printer.PrintImage(ms);
                                //var printerCM = DependencyService.Get<IPrintService>();
                                //var result = printerCM.CommandPrintPickingList(_settingsService.PrintPortFormSetting,_settingsService.PrintIPAdressFormSetting,item);
                            }

                        }
                    }
                    else if (MaterialPickingMst.isChangeRm == false && MaterialPickingMst.pickingPurposeId == 8)
                    {
                        var printLabel = await _materialAutoReceivingService.GetPickingLabelPrint(MaterialPickingMst);

                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        IsBusy = false;
                        foreach (var item in printLabel.pickingDtl)
                        {
                            var ms = new MemoryStream(item.PicBarcode);
                            var printer = DependencyService.Get<IPrintService>();
                            if (item.RmId.Substring(0, 1) == "P")
                            {
                                var result = printer.PrintImageA5(ms);
                            }
                            else
                            {
                                var resultrm = printer.PrintImage(ms);
                            }
                        }
                    }
                    else
                    {
                        var updateRm = await _materialAutoReceivingService.GetPickingBarcodePrintAsync(MaterialPickingMst);
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        IsBusy = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "บันทึกข้อมูลเสร็จสิ้น");
                    }

                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลการพิมพ์ใบส่งของผิดพลาด กรุณาติดต่อ ICT.\r\n Error Message: " + e.Message);
                }
                catch (Exception e)
                {
                    IsBusy = false;
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลการพิมพ์ใบส่งของผิดพลาด กรุณาติดต่อ ICT.\r\n Error Message: " + e.ToString());
                }
            }
            var itemselect = dtl == null ? SelectPickingDtl : dtl;
            RefreshData(itemselect);
        }

        public async void OnPrintDeliveryReport()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    var reportGetCondition = new DeliveryReportGetConditionVM
                    {
                        OutId = _materialRequestMSTReferences.Where(x => x.DocRefTypeId == 2).First().DocRefValue,
                        RequestMstId = MaterialPickingMst.MaterialRequestMstId,
                        RequestMstReceiveSite = (int)MaterialPickingMst.ReceiveSite,
                        RequestMstTransferSite = MaterialPickingMst.TransferToSite,
                        ReportId = 7,
                        AppId = DependencyService.Get<IXmlLoadData>().LoadData(),
                        CompId = "0102",
                        UserId = _settingsService.UserSetting.UserId
                    };

                    byte[] reportDataBytes = await _materialAutoReceivingService.GetDeliveryReport(reportGetCondition);
                    var ms = new MemoryStream(reportDataBytes);
                    var printer = DependencyService.Get<IPrintService>();
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                    printer.PrintPdfFile(ms, MediaSize.A4);
                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลการพิมพ์ใบส่งของผิดพลาด กรุณาติดต่อ ICT.\r\n Error Message: " + e.Message);
                }
                catch (Exception e)
                {
                    IsBusy = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลการพิมพ์ใบส่งของผิดพลาด กรุณาติดต่อ ICT.\r\n Error Message: " + e.ToString());
                }
            }
        }

        private void OnNothing(MaterialPickingDtlVM selectNo)
        {
            OnRMTapped(selectNo);
            SendNothing(selectNo);
        }

        private async Task SendNothing(MaterialPickingDtlVM selectNo)
        {
            SelectPickingDtl = selectNo;
            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                           "ไม่พบวัตถุดิบ " + _selectPickingDtl.RmId + " นี้ใช่หรือไม่ ?\nยืนยันการไม่พบวัตถุดิบ",
                           async () =>
                           {
                               try
                               {
                                   var chk = await ValidationData(5);
                                   if (chk)
                                   {
                                       List<MaterialPickingDtlVM> lstDtl = new List<MaterialPickingDtlVM>();
                                       var dtlData = lstPickingDtl.Where(x => x.RmId == selectNo.RmId && x.Barcode == "").FirstOrDefault();
                                       if (dtlData != null)
                                       {
                                           dtlData.IsWeight = false;
                                           dtlData.Total_Weight = 0;
                                           dtlData.Package_QTY = 0;
                                           dtlData.Residue_Weight = 0;
                                           dtlData.Net_Weight = 0;
                                           dtlData.Picking_By = _settingsService.UserSetting.UserId;
                                           dtlData.Picking_By_Name = _settingsService.UserFullNameSetting;
                                           dtlData.Picking_Date = DateTime.Now;
                                           dtlData.ARP_Material_PickingList_Weighing_Status_ID = 1;
                                           dtlData.ARP_Material_PickingList_DTL_Status_ID = 6;
                                           dtlData.state = Enumerations.StateData.State.Insert;
                                           lstDtl.Add(dtlData);
                                       }
                                       else
                                       {
                                           lstDtl = lstPickingDtl.Where(x => x.RmId == selectNo.RmId).ToList();
                                           foreach (var item in lstDtl)
                                           {
                                               item.ARP_Material_PickingList_DTL_Status_ID = 5;
                                               item.Picking_By = _settingsService.UserSetting.UserId;
                                               item.Picking_By_Name = _settingsService.UserFullNameSetting;
                                               item.Picking_Date = DateTime.Now;
                                               item.state = Enumerations.StateData.State.Update;
                                           }
                                       }

                                       MaterialPickingMst.PickingStatusId = 2;
                                       MaterialPickingMst.reviseBy = _settingsService.UserSetting.UserId;
                                       MaterialPickingMst.state = Enumerations.StateData.State.Update;
                                       var dtllst = (await _materialAutoReceivingService.GetMaterialPickingDtlAsync(MaterialPickingMst.Id, 6)).ToList(); //all status
                                       var countStatus = dtllst.Where(x => x.ARP_Material_PickingList_DTL_Status_ID != 5 || x.ARP_Material_PickingList_DTL_Status_ID != 6).Count();
                                       if (countStatus == 0)
                                       {
                                           MaterialPickingMst.PickingStatusId = 3;
                                       }

                                       MaterialPickingMst.pickingDtl = lstDtl;

                                       if (!IsBusy)
                                       {
                                           try
                                           {
                                               IsBusy = true;
                                               DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                                               await _materialAutoReceivingService.SavePicking(MaterialPickingMst);

                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                               IsBusy = false;

                                               RefreshData(null);
                                           }
                                           catch (HttpRequestExceptionEx e)
                                           {
                                               IsBusy = false;
                                               _loggingService.Error(e.Message);
                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                               await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การยืนยันไม่พบวัตถุดิบผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                           }
                                           catch (Exception e)
                                           {
                                               IsBusy = false;
                                               _loggingService.Error(e.ToString());
                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                               await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การยืนยันไม่พบวัตถุดิบผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                                           }
                                       }
                                   }

                               }
                               catch (Exception ex)
                               {
                                   throw ex;
                               }
                           }
                           );
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
