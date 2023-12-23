using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels.Base;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Extensions;
using System.IO;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{
    public class InternalReceivingBarcodeDetailViewModel : ViewModelBase
    {
        private MaterialReceivePlanMst _mstselectReceivePlan;
        private MaterialReceivePlanMst _receivePlanMst;
        private MaterialReceivePlanDtl _materialReceivePlanDtl;
        private List<MaterialReceivePlanDtl> _receivePlanDtl;
        private List<MaterialReceivePlanDtlBarcode> _lstReceiveDtlBarcode;
        private MaterialReceivePlanDtlBarcode _selectbarcode;
        private List<CheckList> _lists;
        private MaterialReceivePlanMst _mstplan;
        private CheckList _otherlists;
        private ObservableCollection<MaterialReceivePlanDtlBarcode> _receivePlanBarcode;
        private ObservableCollection<CheckList> _checkList;
        private ObservableCollection<PlanCheck> _plancheckList;
        private string _barcode, _descripton, _lotno;
        private int _barcodeQty;
        private bool _isLockSwitch, _isLockBtn, _isEdit, _lockprint;
        private bool _isopenscrollview, _ishideBatch;
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;

        public InternalReceivingBarcodeDetailViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService,
            IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingServicc)
            : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            LockBtn = true;
            LockSwitch = true;
            LockBtnPrint = false;
            _barcodeQty = 1;
            InitializeMessenger();
        }
        public ICommand SendDataCommand => new Command(OnSaveCheckList);
        public ICommand RePrintCommand => new Command(OnRePrint);
        public ICommand WeightChangedCommand => new Command<string>(OnWeightChanged);

        public ObservableCollection<CheckList> CheckListData
        {
            get => _checkList;
            set
            {
                _checkList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MaterialReceivePlanDtlBarcode> ReceivePlanDtlBarcode
        {
            get => _receivePlanBarcode;
            set
            {
                _receivePlanBarcode = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PlanCheck> PlanCheckListData
        {
            get => _plancheckList;
            set
            {
                _plancheckList = value;
                OnPropertyChanged();
            }
        }
        public bool IsOpenScrollview
        {
            get => _isopenscrollview;
            set
            {
                _isopenscrollview = value;
                OnPropertyChanged();
            }
        }
        public string LotNoShow
        {
            get => _lotno;
            set
            {
                _lotno = value;
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
        public int BarcodeQty
        {
            get => _barcodeQty;
            set
            {
                _barcodeQty = value;
                OnPropertyChanged();
            }
        }

        public bool LockSwitch
        {
            get => _isLockSwitch;
            set
            {
                _isLockSwitch = value;
                OnPropertyChanged();
            }
        }
        public bool LockBtn
        {
            get => _isLockBtn;
            set
            {
                _isLockBtn = value;
                OnPropertyChanged();
            }
        }
        public bool LockBtnPrint
        {
            get => _lockprint;
            set
            {
                _lockprint = value;
                OnPropertyChanged();
            }
        }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        public MaterialReceivePlanMst MstselectReceivePlan
        {
            get => _mstselectReceivePlan;
            set
            {
                _mstselectReceivePlan = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanMst MstPlan
        {
            get => _mstplan;
            set
            {
                _mstplan = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanMst ReceivePlanMst
        {
            get => _receivePlanMst;
            set
            {
                _receivePlanMst = value;
                OnPropertyChanged();
            }
        }
        public List<MaterialReceivePlanDtl> ReceivePlanDtl
        {
            get => _receivePlanDtl;
            set
            {
                _receivePlanDtl = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanDtl MaterialReceivePlanDtl
        {
            get => _materialReceivePlanDtl;
            set
            {
                _materialReceivePlanDtl = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanDtlBarcode selectBarcode
        {
            get => _selectbarcode;
            set
            {
                _selectbarcode = value;
                OnPropertyChanged();
            }
        }

        public List<CheckList> CheckLists
        {
            get => _lists;
            set
            {
                _lists = value;
                OnPropertyChanged();
            }
        }
        public CheckList OtherCheck
        {
            get => _otherlists;
            set
            {
                _otherlists = value;
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
        public void OnRePrint()
        {

            GetReturnDataAsync(MstselectReceivePlan);
        }
        public async Task GetReturnDataAsync(MaterialReceivePlanMst mst)
        {
            try
            {
                if (BarcodeQty <= 0)
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาใส่จำนวนที่ต้องการพิมพ์");
                }
                else
                {
                    var data = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(mst);
                    PrintLabel(data);
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่สามารถดึงข้อมูลบาร์โค้ดได้ กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่สามารถดึงข้อมูลบาร์โค้ดได้ กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }
        public override async Task InitializeAsync(object ReceivePlan)
        {
            MstselectReceivePlan = (MaterialReceivePlanMst)ReceivePlan;
            GetCheckListAsync();
            //GetRMByBarcodeDataAsync();
        }
        public void OnWeightChanged(string value)
        {
            var a = int.TryParse(value, out int retval);
            BarcodeQty = retval;
        }

        private async Task GetRMByBarcodeDataAsync()
        {
            try
            {
                var mstid = MstselectReceivePlan.Id;
                Barcode = _settingsService.BarcodeSetting;
                if (MstselectReceivePlan.receivePlanDtl == null)
                {
                    var dtl = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(mstid, 0);
                    MstselectReceivePlan.receivePlanDtl = new List<MaterialReceivePlanDtl>(dtl);
                }
                ReceivePlanDtl = MstselectReceivePlan.receivePlanDtl.ToList();
                lstReceiveDtlBarcode = MstselectReceivePlan.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                var rmname = lstReceiveDtlBarcode[0].rmId.Substring(0, 1);
                if (rmname == "I" || rmname == "S")
                {
                    var RM = lstReceiveDtlBarcode.Where(x => x.barcode + x.batchNo == Barcode).FirstOrDefault();
                    if (RM != null)
                    {
                        selectBarcode = RM;
                        IshideBatch = true;
                        LotNoShow = selectBarcode.lotNo + selectBarcode.lotNonum;
                    }
                }
                else
                {
                    var RM = lstReceiveDtlBarcode.Where(x => x.barcode == Barcode).FirstOrDefault();
                    if (RM != null)
                    {
                        selectBarcode = RM;
                        IshideBatch = false;
                        LotNoShow = selectBarcode.lotNo + selectBarcode.lotNonum;
                    }
                }

            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }

        public async Task GetCheckListAsync()
        {
            var mstid = MstselectReceivePlan.Id;
            try
            {
                IsBusy = true;
                var check = await _materialAutoReceivingService.GetCheckList();
                IsBusy = false;
                CheckLists = check.Where(x => x.CheckTypeId == 1).ToList();
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }

            var lstother = CheckLists.Where(x => x.Order_NO == 8).ToList();
            lstother.ForEach(x => x.IsOpenEnt = true);

            var mst = MstselectReceivePlan;
            var dtl = mst.receivePlanDtl[0];
            var bar = dtl.ReceivePlanBarcode[0];
            selectBarcode = bar;
            LotNoShow = selectBarcode.lotNo;

            if (MstselectReceivePlan.state == Enumerations.StateData.State.DoNothing && MstselectReceivePlan.typeSuplierId != 1)
            {
                LockBtnPrint = true;
            }


            if (bar.receivePlanDtlBarcodeStatus >= 2)
            {
                LockBtn = false;
            }
            else
            {
                LockBtn = true;
            }

            if (bar.lstPlanCheck != null)
            {
                if (bar.state == Enumerations.StateData.State.Insert)
                {
                    _isEdit = false;
                }
                else if (bar.state == Enumerations.StateData.State.Update)
                {
                    _isEdit = true;
                }

                IsOpenScrollview = true;
                foreach (var item in bar.lstPlanCheck)
                {
                    foreach (var itemCL in CheckLists)
                    {
                        if (item.CheckListId == itemCL.ID)
                        {
                            itemCL.IsCheck = item.IsCheck;
                            itemCL.Description = item.Description;
                        }
                    }
                }
            }
        }
        private async Task<bool> ValidationData(int step)
        {
            bool retval = true;
            if (step == 4) // Check Print
            {
                bool chkPrint = true;
                var lst = ReceivePlanMst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                foreach (var item in lst)
                {
                    int yearExpire = Convert.ToInt32(item.expireDate.Substring(0, 4));
                    int monthExpire = Convert.ToInt32(item.expireDate.Substring(4, 2));
                    int dayExpire = Convert.ToInt32(item.expireDate.Substring(6, 2));
                    var expDate = new DateTime(yearExpire, monthExpire, dayExpire);
                    var word = item.rmId.Substring(0, 1);

                    if (expDate <= DateTime.Now)
                    {
                        chkPrint = false;
                    }
                }

                if (!chkPrint)
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถบาร์โค้ดพิมพ์ได้ เนื่องจากบาร์โค้ดหมดอายุแล้ว");
                }
            }

            return retval;
        }

        private async void OnSaveCheckList()
        {
            try
            {
                List<PlanCheck> lst = new List<PlanCheck>();

                foreach (var item in CheckLists)
                {
                    lst.Add(new PlanCheck
                    {
                        IsCheck = item.IsCheck,
                        CheckListId = item.ID,
                        Description = item.Description,
                        CheckedBy = _settingsService.UserSetting.UserId,
                        CheckedDate = DateTime.Now,
                        state = !_isEdit ? Enumerations.StateData.State.Insert : Enumerations.StateData.State.Update
                    });
                }

                var mst = MstselectReceivePlan;
                var dtl = mst.receivePlanDtl[0];
                var bar = dtl.ReceivePlanBarcode[0];
                bar.receivePlanDtlBarcodeStatus = 2;
                bar.receivePlanDtlBarcodeStatusNameThai = "รอ QAD ทำการตรวจสอบ";
                bar.receiveBy = int.Parse(_settingsService.UserIdSetting);
                bar.state = Enumerations.StateData.State.Update;
                bar.lstPlanCheck = new List<PlanCheck>(lst);

                var check = await _materialAutoReceivingService.GetCheckList();
                CheckLists = check.Where(x => x.CheckTypeId == 2).ToList();
                if (CheckLists.Count > 0)
                {
                    var lstPlanCheckQA = new List<PlanCheck>();
                    CheckLists.ForEach(x =>
                    {
                        lstPlanCheckQA.Add(new PlanCheck
                        {
                            IsCheck = true,
                            Barcode = bar.barcode,
                            CheckListId = x.ID,
                            CheckedBy = int.Parse(_settingsService.UserIdSetting),
                            CheckedDate = DateTime.Now,
                            Description = null
                        });
                    });
                    bar.lstPlanCheckQA = lstPlanCheckQA;
                }
                if (mst.typeSuplierId == 1)
                {
                    dtl.ReceivePlanBarcode[0] = bar;
                    dtl.ReceivePlanBarcode[0].StatusQA = null;
                    dtl.receivePlanDtlStatus = 2;
                    dtl.reviseBy = int.Parse(_settingsService.UserIdSetting);
                    dtl.state = Enumerations.StateData.State.Update;
                    mst.receivePlanDtl[0] = dtl;
                    mst.receivePlanMstStatus = 2;
                    mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                    mst.state = Enumerations.StateData.State.Update;
                    MstPlan = mst;
                    IsBusy = true;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    //var mstplan = new MaterialReceivePlanMst();
                    if (MstPlan != null)
                    {
                        //mstplan = await _materialAutoReceivingService.SaveRecieving(MstPlan);
                        await SaveRecievingAndYesCheckListAsync(MstPlan);
                    }
                    //if (mstplan != null)
                    //{
                    //    mstplan = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(mst);
                    //    if (mstplan.receivePlanDtl.Count > 0)
                    //    {
                    //        var clone = mstplan.Clone<MaterialReceivePlanMst>();
                    //        ReceivePlanMst = mstplan;
                    //        var chkPrint = await ValidationData(4);
                    //        if (chkPrint)
                    //        {
                    //            PrintLabel(clone);
                    //        }
                    //        int typeid = 1;
                    //        MessagingCenter.Send(this, MessagingConstants.ChecklistType, typeid);
                    //        await _navigationService.NavigateBackAsync();
                    //    }
                    //    else
                    //    {
                    //        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    //        await _dialogService.ShowDialog(
                    //        "ข้อมูลลาเบลผิดพลาด",
                    //        "เตือน",
                    //        "ตกลง"
                    //        );
                    //    }
                    //}

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                }
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "การบันทึกข้อมูลสำเร็จ");
                await _navigationService.NavigateBackAsync();
            }
            catch (HttpRequestExceptionEx e)
            {
                IsBusy = false;
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);

                if (e.Message == "Connection reset")
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเชื่อมต่อผิดพลาด กรุณาลองใหม่");
                }
                else
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
            }
            catch (Exception e)
            {
                IsBusy = false;
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());

                if (e.Message == "Connection reset")
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเชื่อมต่อผิดพลาด " + Environment.NewLine + "กรุณาลองใหม่");
                }
                else
                {
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
            }

        }

        private async Task SaveRecievingAndYesCheckListAsync(MaterialReceivePlanMst selectReceivePlanMst)
        {
            try
            {
                var chkData = await ValidationData(2);
                if (chkData)
                {
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    var checkdtl = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(selectReceivePlanMst.Id, selectReceivePlanMst.receivePlanDtl[0].Id);

                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 3);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.StatusQA = true);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receiveBy = int.Parse(_settingsService.UserIdSetting));
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = StateData.State.Update);
                    var word = selectReceivePlanMst.receivePlanDtl[0].rmId.Substring(0, 1);
                    if (word == "I" || word == "S")
                    {
                        ReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id).ToList();
                        var selectbarcodedtl = ReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var chklotno = ReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
                        var selectbardtl = chklotno.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var bardtl = selectbardtl.Where(x => x.piLotNo == selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0].piLotNo).ToList();
                        foreach (var items in bardtl)
                        {
                            selectbarcodedtl.Remove(items);
                        }
                        var chkstatus = selectbarcodedtl.Where(x => x.receivePlanDtlBarcodeStatus != 3).Count();
                        if (chkstatus < 1)
                        {
                            selectReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 3;
                        }
                        selectReceivePlanMst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                        selectReceivePlanMst.receivePlanDtl[0].state = StateData.State.Update;
                        var selectdtl = checkdtl.ToList();
                        var chkdtl = selectdtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        foreach (var items in bardtl)
                        {
                            chkdtl.Remove(items);
                        }
                        var chkstatusall = chkdtl.Where(x => x.receivePlanDtlBarcodeStatus != 3).Count();
                        if (chkstatusall < 1)
                        {
                            selectReceivePlanMst.receivePlanMstStatus = 3;
                        }
                    }
                    else
                    {
                        ReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id).ToList();
                        var chklotno = ReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
                        foreach (var items in chklotno)
                        {
                            ReceivePlanDtl.Remove(items);
                        }
                        ReceivePlanDtl.Add(selectReceivePlanMst.receivePlanDtl[0]);
                        var selectbarcode = ReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var chkstatus = selectbarcode.Where(x => x.receivePlanDtlBarcodeStatus != 3).Count();
                        if (chkstatus < 1)
                        {
                            selectReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 3;
                        }
                        selectReceivePlanMst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                        selectReceivePlanMst.receivePlanDtl[0].state = StateData.State.Update;
                        var chkdtlbarcode = checkdtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var selctbarcode = chkdtlbarcode.Where(x => x.LotNo_Group == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
                        foreach (var items in selctbarcode)
                        {
                            chkdtlbarcode.Remove(items);
                        }
                        var chkstatusall = chkdtlbarcode.Where(x => x.receivePlanDtlBarcodeStatus != 3).Count();
                        if (chkstatusall < 1)
                        {
                            selectReceivePlanMst.receivePlanMstStatus = 3;
                        }
                    }
                    selectReceivePlanMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                    selectReceivePlanMst.state = StateData.State.Update;
                    if (selectReceivePlanMst != null)
                    {
                        await _materialAutoReceivingService.SaveRecieving(selectReceivePlanMst);
                    }

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "บันทึกสำเร็จ");
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
        }

        private async Task PrintLabel(MaterialReceivePlanMst mst)
        {
            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                           "ต้องการพิมพ์ลาเบลหรือไม่",
                                           async () =>
                                           {
                                               ReceivePlanDtlBarcode = mst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToObservableCollection<MaterialReceivePlanDtlBarcode>();
                                               var word = mst.receivePlanDtl[0].rmId.Substring(0, 1);

                                               if (word == "P")
                                               {
                                                   var color = mst.receivePlanDtl.Select(x => x.colorName).ToList();
                                                   DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                   await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเปลี่ยนเครื่องพิมพ์เป็น A5 แล้วใช้กระดาษสี" + color[0],
                                                       () =>
                                                       {
                                                           var dtlimg = mst.receivePlanDtl[0].Imagedtl;
                                                           var ms = new MemoryStream(dtlimg);
                                                           var printer = DependencyService.Get<IPrintService>();
                                                           var result = printer.PrintPdfFile(ms, MediaSize.A5);
                                                       });
                                               }
                                               else
                                               {
                                                   try
                                                   {
                                                       var lstBarcode = ReceivePlanDtlBarcode.ToList();
                                                       for (int i = 1; i <= BarcodeQty; i++)
                                                       {
                                                           foreach (var itemBc in lstBarcode)
                                                           {
                                                               var GHSmst = await _materialAutoReceivingService.GetReportGHSByRM(itemBc.rmId, Convert.ToInt32(mst.ownerSite));
                                                               itemBc.rmGHSlst = GHSmst;
                                                               var printer = DependencyService.Get<IPrintService>();
                                                               var result = printer.CommandPrint(_settingsService.PrintPortFormSetting, _settingsService.PrintIPAdressFormSetting, itemBc);
                                                           }
                                                       }
                                                       await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "พิมพ์ลาเบลเสร็จสิ้น");
                                                   }
                                                   catch (HttpRequestExceptionEx e)
                                                   {
                                                       _loggingService.Error(e.Message);
                                                       await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                                                   }
                                                   catch (Exception e)
                                                   {
                                                       _loggingService.Error(e.ToString());
                                                       await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                                                   }
                                               }

                                           });


        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<App, bool>(this, MessagingConstants.Closeapp,
               (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
        }
        public async void OnRefreshExPageAsync(bool expage)
        {
            if (Convert.ToInt32(_settingsService.ReceiveMstIdSetting) == MstselectReceivePlan.Id && Convert.ToInt32(_settingsService.ReceiveDtlIdSetting) == MstselectReceivePlan.receivePlanDtl[0].Id)
            {
                if (expage && LockBtnPrint == false)
                {
                    try
                    {
                        if (MstselectReceivePlan != null)
                        {
                            MstselectReceivePlan.IseditStatus = false;
                            await _materialAutoReceivingService.GetUpdatermInMstStatus(MstselectReceivePlan);
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ยกเลิกการแก้ไขข้อมูลแล้ว", async () =>
                            {
                                await _navigationService.NavigateBackAsync();
                            });
                        }
                    }
                    catch (HttpRequestExceptionEx e)
                    {
                        _loggingService.Error(e.Message);
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การปลดล็อคข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

                    }
                    catch (Exception e)
                    {
                        _loggingService.Error(e.ToString());
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การปลดล็อคข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                    }
                }
            }
        }
    }
}
