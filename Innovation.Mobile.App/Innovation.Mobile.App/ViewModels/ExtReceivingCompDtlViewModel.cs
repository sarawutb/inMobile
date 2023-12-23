using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class ExtReceivingCompDtlViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private MaterialReceivePlanMst _materialReceivePlanMst;
        private bool _modeEdit, _isEditExpire, _setExpire, _isEdit, _lockBtn, _lockPrint;
        private string _batch_Start, _batch_End, _rmTitle;
        private decimal _totalWeight, _sumWeight, _weight;
        private DateTime _productionDate, _expireDate;
        private ObservableCollection<LinefileVM> _lstLinefile;
        private LinefileVM _selectLine;
        private ObservableCollection<MaterialReceivePlanDtlBarcode> _receivePlanBarcode;
        private List<CheckList> _checkLists;

        public ExtReceivingCompDtlViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            _settingsService = settingsService;
            LoadDefault();
            LoadCheckListAsync();
            IsEdit = true;
            ModeEdit = true;
            LockBtn = true;
        }

        public ICommand AddBatchCommand => new Command(OnAddBatch);
        public ICommand SaveClickedCommand => new Command(OnSaveClicked);
        public ICommand BatchStartChangedCommand => new Command<string>(OnBatchStartChanged);
        public ICommand BatchEndChangedCommand => new Command<string>(OnBatchEndChanged);
        public ICommand ProdDateSelectedCommand => new Command(OnProdDateSelected);
        public ICommand SetExpDateToggledCommand => new Command(OnSetExpDateToggled);
        public ICommand TextWeightChangedCommand => new Command(OnTextWeightChanged);
        public ICommand BatchStartUnfocusedCommand => new Command(OnBatchStartUnfocused);
        public ICommand BatchEndUnfocusedCommand => new Command(OnBatchEndUnfocused);
        public ICommand DeleteCommand => new Command(OnDelete);
        public ICommand PrintCommand => new Command(OnPrintAsync);
        public ICommand BarcodeTappedCommand => new Command<MaterialReceivePlanDtlBarcode>(OnBarcodeTapped);

        public MaterialReceivePlanMst MaterialReceivePlanMst
        {
            get => _materialReceivePlanMst;
            set
            {
                _materialReceivePlanMst = value;
                OnPropertyChanged();
            }
        }
        public string Batch_Start
        {
            get => _batch_Start;
            set
            {
                _batch_Start = value;
                OnPropertyChanged();
            }
        }
        public bool LockPrint
        {
            get => _lockPrint;
            set
            {
                _lockPrint = value;
                OnPropertyChanged();
            }
        }
        public string Batch_End
        {
            get => _batch_End;
            set
            {
                _batch_End = value;
                OnPropertyChanged();
            }
        }
        public decimal Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }
        public decimal TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged();
            }
        }
        public decimal SumWeight
        {
            get => _sumWeight;
            set
            {
                _sumWeight = value;
                OnPropertyChanged();
            }
        }
        public bool ModeEdit
        {
            get => _modeEdit;
            set
            {
                _modeEdit = value;
                OnPropertyChanged();
            }
        }
        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
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
        public bool IsEditExpire
        {
            get => _isEditExpire;
            set
            {
                _isEditExpire = value;
                OnPropertyChanged();
            }
        }
        public bool SetExpire
        {
            get => _setExpire;
            set
            {
                _setExpire = value;
                OnPropertyChanged();
            }
        }
        public DateTime ProductionDate
        {
            get => _productionDate;
            set
            {
                _productionDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime ExpireDate
        {
            get => _expireDate;
            set
            {
                _expireDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<LinefileVM> lstLinefile
        {
            get => _lstLinefile;
            set
            {
                _lstLinefile = value;
                OnPropertyChanged();
            }
        }
        public LinefileVM SelectLine
        {
            get => _selectLine;
            set
            {
                _selectLine = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MaterialReceivePlanDtlBarcode> ReceivePlanBarcode
        {
            get => _receivePlanBarcode;
            set
            {
                _receivePlanBarcode = value;
                OnPropertyChanged();
            }
        }
        public bool LockBtn
        {
            get => _lockBtn;
            set
            {
                _lockBtn = value;
                OnPropertyChanged();
            }
        }
        public List<CheckList> CheckLists
        {
            get => _checkLists;
            set
            {
                _checkLists = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object ReceivePlan)
        {
            MaterialReceivePlanMst = (MaterialReceivePlanMst)ReceivePlan;
            RMTitle = MaterialReceivePlanMst.receivePlanDtl[0].rmId + " " + _materialReceivePlanMst.receivePlanDtl[0].rmName;
            IsEditExpire = MaterialReceivePlanMst.forSetEXPDate;
            OnProdDateSelected();
            LoadForEdit();
        }

        private async void OnAddBatch()
        {
            AddBatch();
        }

        private async Task AddBatch()
        {
            bool chk = await ValidationData(1);
            if (chk)
            {
                AddBatchData();
                GetTotalWeight();
                ClearData();
            }
        }

        private void GetTotalWeight()
        {
            TotalWeight = (decimal)ReceivePlanBarcode.Sum(x => x.qty);
        }

        private void ClearData()
        {
            Batch_Start = "";
            Batch_End = "";
            Weight = 0;
        }

        private async Task<bool> ValidationData(int mode)
        {
            bool retval = true;

            if (mode == 1) // addBatch
            {
                if (string.IsNullOrWhiteSpace(Batch_Start))
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอกหมายเลข Batch");
                }
                else if (Weight <= 0)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอกน้ำหนักให้ถูกต้อง");
                }
                else if (SelectLine == null)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือก Line ผลิตให้ถูกต้อง");
                }
                else
                {
                    int intBatchStart = 0, intBatchEnd = 0;

                    var a = int.TryParse(Batch_Start, out intBatchStart);

                    if (!string.IsNullOrWhiteSpace(Batch_End))
                    {
                        var b = int.TryParse(Batch_End, out intBatchEnd);

                        if (intBatchStart > intBatchEnd)
                        {
                            retval = false;
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอกหมายเลข Batch ให้ถูกต้อง");
                        }
                    }

                    if (intBatchEnd == 0)
                    {
                        intBatchEnd = intBatchStart;
                    }
                    if (ReceivePlanBarcode != null)
                    {
                        foreach (var item in ReceivePlanBarcode)
                        {
                            var batch = Convert.ToInt32(item.batchNo);

                            for (int i = intBatchStart; i <= intBatchEnd; i++)
                            {
                                if (batch == i)
                                {
                                    retval = false;
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "Batch " + i.ToString("0000") + " ที่เพิ่มเข้ามามีอยู่ในรายการแล้ว กรุณาตรวจสอบอีกครั้ง");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (mode == 2) // delete
            {
                var amt = ReceivePlanBarcode.Where(x => x.IsSelect == true).Count();
                if (amt == 0)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือก Batch ที่ต้องการลบ");
                }
                else
                {
                    var lstDel = ReceivePlanBarcode.Clone();
                    lstDel = lstDel.Where(x => x.IsSelect == true).ToObservableCollection();
                    foreach (var item in lstDel)
                    {
                        if (item.state == Enumerations.StateData.State.Insert)
                        {
                            var a = ReceivePlanBarcode.Where(x => x.batchNo == item.batchNo).FirstOrDefault();
                            ReceivePlanBarcode.Remove(a);
                        }
                        else
                        {
                            item.state = Enumerations.StateData.State.Delete;
                        }
                    }
                    GetTotalWeight();

                    lstDel = ReceivePlanBarcode.Where(x => x.IsSelect == true).ToObservableCollection();
                    if (lstDel.Count > 0)
                    {
                        foreach (var item in lstDel)
                        {
                            try
                            {
                                IsBusy = true;
                                //DependencyService.Get<ILoadPageAndroid>().ShowLoadingPage();
                                var chk = await _materialAutoReceivingService.GetWeightingByBarcodeAsync(item.barcode + item.batchNo);
                                //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                IsBusy = false;

                                if (chk)
                                {
                                    retval = false;
                                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.barcode + " ได้ถูกนำไปใช้งานแล้ว");
                                    break;
                                }
                            }
                            catch (HttpRequestExceptionEx e)
                            {
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                _loggingService.Error(e.Message);
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                break;
                            }
                            catch (Exception e)
                            {
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                _loggingService.Error(e.ToString());
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());

                                break;
                            }
                        }
                    }
                    else
                    {
                        retval = false;
                    }
                }
            }
            else if (mode == 3) // Save
            {
                if (ReceivePlanBarcode == null)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "เพิ่มข้อมูล Batch");
                }
                else
                {
                    var amt = ReceivePlanBarcode.Count();

                    if (amt == 0)
                    {
                        retval = false;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "เพิ่มข้อมูล Batch");
                    }
                    else
                    {
                        var mst = MaterialReceivePlanMst;
                        var dtl = mst.receivePlanDtl[0];
                        var amount = dtl.ReceivePlanBarcode.Where(x => x.lstPlanCheck == null).Count();

                        if (amount == dtl.ReceivePlanBarcode.Count)
                        {
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            retval = false;
                            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                "ทำการตรวจสอบวัตถุดิบเรียบร้อยแล้ว ใช่หรือไม่",
                                () =>
                                {
                                    foreach (var item in dtl.ReceivePlanBarcode)
                                    {
                                        List<PlanCheck> lst = new List<PlanCheck>();
                                        foreach (var itemCL in CheckLists)
                                        {
                                            lst.Add(new PlanCheck
                                            {
                                                IsCheck = false,
                                                CheckListId = itemCL.ID,
                                                CheckedBy = _settingsService.UserSetting.UserId,
                                                CheckedDate = DateTime.Now,
                                                state = Enumerations.StateData.State.Insert
                                            });
                                        }

                                        item.receivePlanDtlBarcodeStatus = 2;
                                        item.lstPlanCheck = new List<PlanCheck>(lst);
                                    }
                                });
                        }
                        else
                        {
                            var lstbar = dtl.ReceivePlanBarcode.Where(x => x.lstPlanCheck == null);
                            foreach (var item in lstbar)
                            {
                                List<PlanCheck> lst = new List<PlanCheck>();
                                foreach (var itemCL in CheckLists)
                                {
                                    lst.Add(new PlanCheck
                                    {
                                        IsCheck = false,
                                        CheckListId = itemCL.ID,
                                        CheckedBy = _settingsService.UserSetting.UserId,
                                        CheckedDate = DateTime.Now,
                                        state = Enumerations.StateData.State.Insert
                                    });
                                }

                                item.receivePlanDtlBarcodeStatus = 2;
                                item.lstPlanCheck = new List<PlanCheck>(lst);
                            }
                        }
                    }
                }
            }
            else if (mode == 4) // Check Edit
            {
                var dtl = _materialReceivePlanMst.receivePlanDtl[0];
                var lstBarcode = dtl.ReceivePlanBarcode;
                if (lstBarcode != null)
                {
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    foreach (var item in lstBarcode)
                    {
                        try
                        {
                            IsBusy = true;

                            var chk = await _materialAutoReceivingService.GetWeightingByBarcodeAsync(item.barcode + item.batchNo);

                            IsBusy = false;

                            if (chk)
                            {
                                retval = false;
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.barcode + " ได้ถูกนำไปใช้งานแล้ว");
                                break;
                            }
                        }
                        catch (HttpRequestExceptionEx e)
                        {
                            retval = false;
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            _loggingService.Error(e.Message);
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                            break;
                        }
                        catch (Exception e)
                        {
                            retval = false;
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            _loggingService.Error(e.ToString());
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                            break;
                        }
                    }

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                }
                else
                {
                    retval = false;
                }
            }
            else if (mode == 5)//chk statusqa
            {
                var dtl = _materialReceivePlanMst.receivePlanDtl[0];
                var lstBarcode = dtl.ReceivePlanBarcode;
                if (lstBarcode[0].receivePlanDtlBarcodeStatus == 3)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ลบไม่ได้เนื่องจากทำการรับเข้าเรียบร้อยแล้ว");
                }
            }

            return retval;
        }

        private void AddBatchData()
        {
            //var sumbarcode = ReceivePlanBarcode.Count > 0 || ReceivePlanBarcode != null ? ReceivePlanBarcode.Sum(x => x.qty) : 0;
            int intBatchStart = 0, intBatchEnd = 0;
            int batchamount = 1;
            List<MaterialReceivePlanDtlBarcode> lst = new List<MaterialReceivePlanDtlBarcode>();
            var mst = MaterialReceivePlanMst;
            var dtl = MaterialReceivePlanMst.receivePlanDtl[0];
            string lotno = ProductionDate.ToString("yyMMdd");
            string strBatchStart = Batch_Start;
            int keeptime = Convert.ToInt32(mst.receivePlanDtl[0].keepTime.ToString("0"));
            string expDate = ExpireDate.ToString("yyyyMMdd");

            if (!string.IsNullOrWhiteSpace(Batch_End))
            {
                var a = int.TryParse(Batch_Start, out intBatchStart);
                var b = int.TryParse(Batch_End, out intBatchEnd);
                batchamount = intBatchEnd - intBatchStart + 1;
            }

            if (ReceivePlanBarcode != null)
            {
                lst = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanBarcode);
            }


            for (int i = 0; i <= batchamount - 1; i++)
            {
                var a = int.TryParse(strBatchStart, out intBatchStart);
                string batch = (intBatchStart + i).ToString("0000");

                lst.Add(new MaterialReceivePlanDtlBarcode
                {
                    state = Enumerations.StateData.State.Insert,
                    receivePlanMstId = mst.materialRequestMstId,
                    receivePlanDtlId = dtl.Id,
                    rmId = dtl.rmId,
                    rmName = dtl.rmName,
                    lotNo = lotno,
                    LotNo_Group = lotno,
                    batchNo = batch,
                    qty = Weight,
                    receivePlanDtlBarcodeStatus = 1,
                    receivePlanDtlBarcodeStatusNameThai = "รอ LRD ทำการตรวจสอบ",
                    receiveBy = _settingsService.UserSetting.UserId,
                    receiveByName = _settingsService.UserSetting.UserWindow,
                    productionDate = ProductionDate.ToString("yyyyMMdd"),
                    expireDate = expDate,
                    reviseBy = int.Parse(_settingsService.UserIdSetting),
                    receiveDate = DateTime.Now
                });
            }

            ReceivePlanBarcode = lst.ToObservableCollection();
            dtl.lineId = SelectLine.Lcode;
            dtl.receivePlanDtlStatus = 2;
            dtl.lotNo = lotno;
            dtl.reviseBy = _settingsService.UserSetting.UserId;
            dtl.ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanBarcode);
            mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
            mst.state = Enumerations.StateData.State.Update;
        }

        private async void OnSaveClicked()
        {
            SaveClicked();
        }

        private async Task SaveClicked()
        {
            var chk = await ValidationData(3);
            if (chk)
            {
                var dtl = MaterialReceivePlanMst.receivePlanDtl[0];
                var chkbarcode = ReceivePlanBarcode.Where(x => x.receivePlanDtlBarcodeStatus == 1).ToList();
                if (chkbarcode.Count > 1)
                {
                    chkbarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 2);
                    chkbarcode.ForEach(x => x.state = Enumerations.StateData.State.Update);
                    chkbarcode.ForEach(x => x.reviseBy = int.Parse(_settingsService.UserIdSetting));
                    ReceivePlanBarcode = chkbarcode.ToObservableCollection();
                }
                dtl.ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanBarcode);
                if (dtl.state == Enumerations.StateData.State.DoNothing)
                {
                    dtl.state = Enumerations.StateData.State.Update;
                }
                MaterialReceivePlanMst.receivePlanMstStatus = 2;
                MaterialReceivePlanMst.reviseBy = _settingsService.UserSetting.UserId;
                MaterialReceivePlanMst.state = Enumerations.StateData.State.Update;

                try
                {
                    var selectbarcode = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ToList();
                    var lotgroup = MaterialReceivePlanMst.receivePlanDtl[0].lotNo;
                    var pilot = selectbarcode.Select(x => x.piLotNo).FirstOrDefault();
                    var batchno = selectbarcode.Select(x => x.batchNo).FirstOrDefault();
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                    var mst = new MaterialReceivePlanMst();
                    var mstlst = new MaterialReceivePlanMst();
                    if (MaterialReceivePlanMst != null)
                    {
                        mst = await _materialAutoReceivingService.SaveRecieving(MaterialReceivePlanMst);
                        mstlst = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(mst);
                    }

                    IsBusy = false;

                    if (mst != null)
                    {
                        var clone = mstlst.Clone<MaterialReceivePlanMst>();
                        var print = clone.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var selectlstbarcode = print.Where(x => x.LotNo_Group == lotgroup).ToList();
                        var dtlone = clone.receivePlanDtl[0];
                        dtlone.ReceivePlanBarcode = selectlstbarcode;
                        var lstdtl = new List<MaterialReceivePlanDtl>();
                        lstdtl.Add(dtlone);
                        clone.receivePlanDtl = lstdtl;
                        MaterialReceivePlanMst = clone;
                        PrintLabel(clone);
                        await _navigationService.NavigateBackAsync();
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(ex.ToString());

                    if (ex.Message == "Connection reset")
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเชื่อมต่อผิดพลาด : " + ex.Message + " กรุณาลองใหม่");
                    }
                    else
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
                    }
                }
            }
        }

        private async Task PrintLabel(MaterialReceivePlanMst mst)
        {
            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                           "ต้องการพิมพ์ลาเบลหรือไม่",
                                           async () =>
                                           {
                                               try
                                               {
                                                   var lstbarcode = mst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToObservableCollection<MaterialReceivePlanDtlBarcode>();
                                                   var bclist = new List<MaterialReceivePlanDtlBarcode>();
                                                   bclist = lstbarcode.ToList();
                                                   foreach (var itemBc in bclist)
                                                   {
                                                       var GHSmst = await _materialAutoReceivingService.GetReportGHSByRM(itemBc.rmId, Convert.ToInt32(mst.ownerSite));
                                                       itemBc.rmGHSlst = GHSmst;
                                                       var printer = DependencyService.Get<IPrintService>();
                                                       var result = printer.CommandPrint(_settingsService.PrintPortFormSetting, _settingsService.PrintIPAdressFormSetting, itemBc);
                                                   }
                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "พิมพ์ลาเบลเสร็จสิ้น");
                                               }
                                               catch (HttpRequestExceptionEx e)
                                               {
                                                   _loggingService.Error(e.Message);
                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                               }
                                               catch (Exception e)
                                               {
                                                   _loggingService.Error(e.ToString());
                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                                               }
                                           });
        }

        private async void LoadDefault()
        {
            ProductionDate = DateTime.Now;

            try
            {
                IsBusy = true;
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                var linefiles = (await _materialAutoReceivingService.GetLinefileAsync()).ToObservableCollection();
                linefiles.Insert(0, new LinefileVM { Lcode = "00", Lname = "" });
                lstLinefile = linefiles;

                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                IsBusy = false;
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูล Line File ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูล Line File ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }

        private void OnBatchStartChanged(string value)
        {
            Batch_Start = value;
            GetSumWeight();
        }

        private void OnBatchEndChanged(string value)
        {
            Batch_End = value;
            GetSumWeight();
        }

        private void OnProdDateSelected()
        {
            var dtl = MaterialReceivePlanMst.receivePlanDtl[0];
            DateTime exp = ProductionDate;
            int keeptime = Convert.ToInt32(dtl.keepTime.ToString("0"));
            if (IsEditExpire)
            {
                if (!SetExpire)
                {
                    ExpireDate = exp.AddDays(keeptime);
                }
            }
            else
            {
                ExpireDate = exp.AddDays(keeptime);
            }
        }

        private void OnSetExpDateToggled()
        {
            OnProdDateSelected();
        }

        private async void OnTextWeightChanged(object obj)
        {
            if (obj is TextChangedEventArgs value)
            {
                if (value.NewTextValue == "")
                {
                    Weight = 0;
                }
            }
            GetSumWeight();
        }

        private void GetSumWeight()
        {
            int intBatchStart = 1, intBatchEnd = 0;
            int batchamount = 1;

            if (!string.IsNullOrWhiteSpace(Batch_End))
            {
                var a = int.TryParse(Batch_Start, out intBatchStart);
                var b = int.TryParse(Batch_End, out intBatchEnd);
                batchamount = intBatchEnd - intBatchStart + 1;
            }

            SumWeight = Weight * batchamount;
        }

        private void OnBatchStartUnfocused()
        {
            var a = int.TryParse(Batch_Start, out int intBatch);
            Batch_Start = intBatch.ToString("0000");
            if (Batch_Start == "0000")
            {
                Batch_Start = "0001";
            }
        }

        private void OnBatchEndUnfocused()
        {
            if (!string.IsNullOrWhiteSpace(Batch_End))
            {
                var a = int.TryParse(Batch_End, out int intBatch);
                Batch_End = intBatch.ToString("0000");
            }
        }

        private async void OnDelete()
        {
            if (ReceivePlanBarcode != null)
            {
                var pilotnochk = "";
                int selcount = ReceivePlanBarcode.Where(x => x.IsSelect == true).Count();
                var chkstatusqa = await ValidationData(5);
                if (chkstatusqa)
                {
                    if (selcount > 0)
                    {
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                       "ต้องการลบ Batch ที่เลือก " + selcount.ToString() + " รายการใช่หรือไม่",
                                       async () =>
                                       {
                                           {
                                               var chk = await ValidationData(2);
                                               if (chk)
                                               {

                                                   if (ReceivePlanBarcode.Count() > 0)
                                                   {
                                                       DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                                                       var dtllst = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(MaterialReceivePlanMst.Id, MaterialReceivePlanMst.receivePlanDtl[0].Id);
                                                       var lstbarcode = dtllst.SelectMany(x => x.ReceivePlanBarcode).ToList();
                                                       var dtl = MaterialReceivePlanMst.receivePlanDtl[0];
                                                       dtl.state = Enumerations.StateData.State.Update;
                                                       dtl.reviseBy = _settingsService.UserSetting.UserId;
                                                       dtl.reviseNo += 1;
                                                       dtl.ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanBarcode);
                                                       var selectdtl = dtl.ReceivePlanBarcode.Where(x => x.IsSelect == true).ToList();
                                                       foreach (var items in selectdtl)
                                                       {
                                                           var selectbarcode = lstbarcode.Where(x => x.Id == items.Id).FirstOrDefault();
                                                           if (selectbarcode != null)
                                                           {
                                                               lstbarcode.Remove(selectbarcode);
                                                           }
                                                       }
                                                       if (lstbarcode.Count < 1)
                                                       {
                                                           dtl.receivePlanDtlStatus = 1;
                                                       }
                                                       if (selectdtl != null)
                                                       {
                                                           selectdtl.ForEach(x => x.state = Enumerations.StateData.State.Delete);
                                                           var qastatus = selectdtl.Where(x => x.StatusQA != null).ToList();
                                                           if (qastatus.Count > 0)
                                                           {
                                                               foreach (var item in qastatus)
                                                               {
                                                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ลบ Batch ที่เลือก " + item.batchNo + " ไม่ได้");
                                                                   selectdtl.Remove(item);
                                                               }
                                                           }
                                                           if (selectdtl.Count > 1)
                                                           {
                                                               pilotnochk = selectdtl[0].piLotNo;
                                                           }
                                                           dtl.ReceivePlanBarcode = selectdtl;
                                                       }
                                                       List<MaterialReceivePlanDtl> lstDtl = new List<MaterialReceivePlanDtl>();
                                                       lstDtl.Add(dtl);
                                                       if (lstDtl[0].ReceivePlanBarcode.Count > 0)
                                                       {
                                                           MaterialReceivePlanMst.state = Enumerations.StateData.State.Update;
                                                           MaterialReceivePlanMst.receivePlanMstStatus = 2;
                                                           MaterialReceivePlanMst.reviseBy = _settingsService.UserSetting.UserId;
                                                           MaterialReceivePlanMst.reviseNo += 1;
                                                           MaterialReceivePlanMst.receivePlanDtl = lstDtl;

                                                           try
                                                           {
                                                               IsBusy = true;

                                                               if (MaterialReceivePlanMst != null)
                                                               {
                                                                   await _materialAutoReceivingService.SaveRecieving(MaterialReceivePlanMst);
                                                               }

                                                               var lstdtlshow = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(MaterialReceivePlanMst.Id, MaterialReceivePlanMst.receivePlanDtl[0].Id);

                                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                               IsBusy = false;

                                                               MaterialReceivePlanMst.receivePlanDtl = new List<MaterialReceivePlanDtl>(lstdtlshow);
                                                               var selectbarcode = MaterialReceivePlanMst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                                                               ReceivePlanBarcode = selectbarcode.Where(x => x.piLotNo == pilotnochk).ToObservableCollection();
                                                               if (ReceivePlanBarcode.Count < 1)
                                                               {
                                                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ข้อมูลในรายการถูกลบทั้งหมดแล้ว", async () =>
                                                                   {
                                                                       await _navigationService.NavigateBackAsync();
                                                                   });
                                                               }
                                                           }
                                                           catch (HttpRequestExceptionEx e)
                                                           {
                                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                               _loggingService.Error(e.Message);
                                                               await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเพิ่มข้อมูลบาร์โค้ดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                                           }
                                                           catch (Exception e)
                                                           {
                                                               DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                               _loggingService.Error(e.ToString());
                                                               await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเพิ่มข้อมูลบาร์โค้ดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                                                           }
                                                       }
                                                   }
                                               }
                                           }
                                       }
                                       );
                    }
                }
            }
        }

        private async Task LoadForEdit()
        {
            var chk = await ValidationData(4);
            if (chk)
            {
                var lstBarcode = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode;
                if (lstBarcode != null)
                {
                    if (lstBarcode.Count != 0)
                    {
                        var barcode = lstBarcode[0];
                        if (barcode.productionDate != null)
                        {
                            int yearProduction = Convert.ToInt32(barcode.productionDate.Substring(0, 4));
                            int monthProduction = Convert.ToInt32(barcode.productionDate.Substring(4, 2));
                            int dayProduction = Convert.ToInt32(barcode.productionDate.Substring(6, 2));
                            ProductionDate = new DateTime(yearProduction, monthProduction, dayProduction);
                        }

                        if (barcode.expireDate != null)
                        {
                            int yearExpire = Convert.ToInt32(barcode.expireDate.Substring(0, 4));
                            int monthExpire = Convert.ToInt32(barcode.expireDate.Substring(4, 2));
                            int dayExpire = Convert.ToInt32(barcode.expireDate.Substring(6, 2));
                            ExpireDate = new DateTime(yearExpire, monthExpire, dayExpire);
                        }

                        SelectLine = lstLinefile.Where(x => x.Lcode == MaterialReceivePlanMst.receivePlanDtl[0].lineId).FirstOrDefault();

                        if (string.IsNullOrWhiteSpace(barcode.LotNo_Group))
                        {
                            ModeEdit = false;
                        }
                        else
                        {
                            ModeEdit = true;
                            LockPrint = true;
                        }

                        if (barcode.receivePlanDtlBarcodeStatus >= 2)
                        {
                            LockBtn = false;
                        }
                        else
                        {
                            LockBtn = true;
                        }

                        IsEdit = !ModeEdit;
                        ReceivePlanBarcode = lstBarcode.ToObservableCollection();
                        GetTotalWeight();
                    }
                }
            }
        }
        private async void OnBarcodeTapped(MaterialReceivePlanDtlBarcode obj)
        {
            SetCheckList(obj);
        }
        private async Task SetCheckList(MaterialReceivePlanDtlBarcode obj)
        {
            var mst = MaterialReceivePlanMst.Clone();
            var dtl = mst.receivePlanDtl[0];

            List<MaterialReceivePlanDtlBarcode> lstbar = new List<MaterialReceivePlanDtlBarcode>();
            lstbar.Add(obj);
            dtl.ReceivePlanBarcode = lstbar;
            await _navigationService.NavigateToAsync<InternalReceivingBarcodeDetailViewModel>(mst);
        }

        public async Task LoadCheckListAsync()
        {
            try
            {
                IsBusy = true;
                var check = await _materialAutoReceivingService.GetCheckList();
                IsBusy = false;
                CheckLists = check.Where(x => x.CheckTypeId == 1).ToList();
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลเช็คลิสต์ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลเช็คลิสต์ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }
        }
        public void OnPrintAsync()
        {
            PrintAsync();
        }

        private async Task PrintAsync()
        {
            if (!IsBusy)
            {
                try
                {

                    var selectbarcode = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.IsSelect == true).ToList();
                    var isselct = selectbarcode.Select(x => x.IsSelect).ToList();
                    var idbarcode = selectbarcode.Select(x => x.Id).ToList();
                    var lotgroup = MaterialReceivePlanMst.receivePlanDtl[0].lotNo;
                    var pilot = selectbarcode.Select(x => x.piLotNo).FirstOrDefault();
                    //var batchno = selectbarcode.Select(x => x.batchNo).FirstOrDefault();
                    var mst = MaterialReceivePlanMst.Clone<MaterialReceivePlanMst>();
                    var dtllst = new List<MaterialReceivePlanDtl>();
                    dtllst.Add(MaterialReceivePlanMst.receivePlanDtl[0]);
                    mst.receivePlanDtl = dtllst;
                    mst.receivePlanDtl[0].ReceivePlanBarcode = selectbarcode;

                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    IsBusy = true;
                    var print = new MaterialReceivePlanMst();
                    print = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(mst);
                    IsBusy = false;
                    var clone = print.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                    var selectlstbarcode = clone.Where(x => x.LotNo_Group == lotgroup).ToList();
                    var checkbarcode = selectlstbarcode.Where(x => x.piLotNo == pilot).ToList();
                    var dtl = print.receivePlanDtl[0];
                    var selectcode = new List<MaterialReceivePlanDtlBarcode>();
                    foreach (var item in idbarcode)
                    {
                        selectcode.Add(checkbarcode.Where(x => x.Id == item).FirstOrDefault());
                    }
                    dtl.ReceivePlanBarcode = selectcode; /// 
                    var lstdtl = new List<MaterialReceivePlanDtl>();
                    lstdtl.Add(dtl);
                    print.receivePlanDtl = lstdtl;
                    if (print != null)
                    {
                        PrintLabel(print);
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    IsBusy = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลบาร์โค้ดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    IsBusy = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลบาร์โค้ดผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
            }
        }
    }
}
