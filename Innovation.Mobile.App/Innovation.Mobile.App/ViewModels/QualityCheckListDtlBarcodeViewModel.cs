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
using System.Threading.Tasks;
using Innovation.Mobile.App.Constants;
using Polly.Caching;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{
    public class QualityCheckListDtlBarcodeViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private List<CheckList> _lists;
        private bool _isrecheck;
        private bool _isLockcheck, _isLockBtn, _ishideBatch, _remark, _lockRecheck, _isCancelQualityCheckBtnVisible;
        private int _typeID, _countqtydtl;
        private string _barcode, _statusbarcode, _expiredate;
        private List<ReceivePlanRecheckCauseVM> _rechecklst;
        private ReceivePlanRecheckCauseVM _selectrecheck;
        private MaterialReceivePlanMst _selectReceivePlan;
        private List<MaterialReceivePlanMst> _materialReceivePlanMsts;
        private List<MaterialReceivePlanDtl> _materialReceivePlanDtl;
        private List<MaterialReceivePlanDtlBarcode> _lstReceiveDtlBarcode;
        private List<PlanCheck> _planwhcheck, _planQacheck;
        private MaterialReceivePlanDtl _selectreceivePlanDtl;
        private MaterialReceivePlanDtlBarcode _selectbarcode;


        public QualityCheckListDtlBarcodeViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            IsLockCheck = true;
            _isLockBtn = true;
            _isLockcheck = true;
            LockRecheck = true;
            TypeID = 2;
        }
        public ICommand SendReCheckCommand => new Command(SendReCheckAsync);
        public ICommand ShowRemarkCommand => new Command(GetShowRemark);
        public ICommand HideRemarkCommand => new Command(GetHideRemark);
        public ICommand SendDataYesCommand => new Command(GetSendYesCheckListAsync);
        public ICommand SendDataNoCommand => new Command(GetSendNoCheckListAsync);
        public ICommand CancelQualityCheckCommand => new Command(OnCancelQualityCheckAsync);

        public bool LockRecheck
        {
            get => _lockRecheck;
            set
            {
                _lockRecheck = value;
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

        public bool IsLockCheck
        {
            get => _isLockcheck;
            set
            {
                _isLockcheck = value;
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
        public bool IsQaReCheck
        {
            get => _isrecheck;
            set
            {
                _isrecheck = value;
                OnPropertyChanged();
            }
        }
        public bool ShowRemark
        {
            get => _remark;
            set
            {
                _remark = value;
                OnPropertyChanged();
            }
        }
        public string StatusBarcode
        {
            get => _statusbarcode;
            set
            {
                _statusbarcode = value;
                OnPropertyChanged();
            }
        }
        public string ExpireDateLot
        {
            get => _expiredate;
            set
            {
                _expiredate = value;
                OnPropertyChanged();
            }
        }
        public int TypeID
        {
            get => _typeID;
            set
            {
                _typeID = value;
                OnPropertyChanged();
            }
        }
        public int CountQTY
        {
            get => _countqtydtl;
            set
            {
                _countqtydtl = value;
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
        public List<ReceivePlanRecheckCauseVM> Recheck
        {
            get => _rechecklst;
            set
            {
                _rechecklst = value;
                OnPropertyChanged();
            }
        }
        public ReceivePlanRecheckCauseVM SelectRecheck
        {
            get => _selectrecheck;
            set
            {
                _selectrecheck = value;
                OnPropertyChanged();
            }
        }
        public List<PlanCheck> PlanWHCheck
        {
            get => _planwhcheck;
            set
            {
                _planwhcheck = value;
                OnPropertyChanged();
            }
        }
        public List<PlanCheck> PlanQaCheck
        {
            get => _planQacheck;
            set
            {
                _planQacheck = value;
                OnPropertyChanged();
            }
        }
        public List<CheckList> CheckQaList
        {
            get => _lists;
            set
            {
                _lists = value;
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
        public MaterialReceivePlanDtl selectReceiveDtl
        {
            get => _selectreceivePlanDtl;
            set
            {
                _selectreceivePlanDtl = value;
                OnPropertyChanged();
            }
        }

        public List<MaterialReceivePlanDtl> MaterialReceivePlanDtl
        {
            get => _materialReceivePlanDtl;
            set
            {
                _materialReceivePlanDtl = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanMst selectReceivePlanMst
        {
            get => _selectReceivePlan;
            set
            {
                _selectReceivePlan = value;
                OnPropertyChanged();
            }
        }

        public bool IsCancelQualityCheckBtnVisible
        {
            get => _isCancelQualityCheckBtnVisible;
            set
            {
                _isCancelQualityCheckBtnVisible = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object ReceivePlan)
        {
            selectReceivePlanMst = (MaterialReceivePlanMst)ReceivePlan;
            LoadRMLotAsync();
            LoadCheckStatusAsync();
        }
        private void GetShowRemark()
        {
            LockBtn = false;
            ShowRemark = true;
        }
        private void GetHideRemark()
        {
            ShowRemark = false;
            LockBtn = true;
        }
        private async void LoadRMLotAsync()
        {
            var chkbar = selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.receivePlanDtlBarcodeStatus == 1).Count();
            if (chkbar > 1)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณารอแผนก LRD ตรวจเช็ค");
                LockBtn = false;
                LockRecheck = false;
            }
            selectReceiveDtl = selectReceivePlanMst.receivePlanDtl[0];
            CountQTY = (int)selectReceiveDtl.ReceivePlanBarcode.Sum(x => x.qty);
            StatusBarcode = selectReceiveDtl.ReceivePlanBarcode[0].receivePlanDtlBarcodeStatusNameThai;
            ExpireDateLot = selectReceiveDtl.ReceivePlanBarcode[0].expireDate != null ? selectReceiveDtl.ReceivePlanBarcode[0].expireDate.ToString() : "";
            PlanWHCheck = selectReceiveDtl.ReceivePlanBarcode.SelectMany(x => x.lstPlanCheck).ToList();
            try
            {
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                IsBusy = true;
                var rechecks = await _materialAutoReceivingService.GetRecheckCauseAsync();
                IsBusy = false;
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                Recheck = rechecks.ToList();
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
        public async void LoadCheckStatusAsync()
        {
            try
            {
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                IsBusy = true;
                var check = await _materialAutoReceivingService.GetCheckList();
                CheckQaList = check.Where(x => x.CheckTypeId == 2).ToList();
                IsBusy = false;
                var dtlbarcode = selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.receivePlanDtlBarcodeStatus == 3).ToList();
                IsCancelQualityCheckBtnVisible = false;

                if (dtlbarcode.Count() > 0)
                {
                    IsLockCheck = false;
                    LockBtn = false;
                    LockRecheck = false;

                    if (HasCancelQualityCheckPermission())
                    {
                        IsCancelQualityCheckBtnVisible = true;
                    }

                    var qalst = dtlbarcode[0].lstPlanCheckQA;
                    //var qalst = dtlbarcode.SelectMany(x => x.lstPlanCheckQA).ToList();
                    foreach (var item in qalst)
                    {
                        foreach (var itemCL in CheckQaList)
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
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }

        }

        private bool HasCancelQualityCheckPermission()
        {
            return _settingsService.UserSetting.ProgramPermission.Modules.Where(x => x.ModuleCode == "CancelQualityCheck").FirstOrDefault() != null;
        }

        public async void SendReCheckAsync()
        {
            try
            {
                var chkData = await ValidationData(1);
                if (chkData)
                {
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 1);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.StatusQA = null);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.ReceivePlanRecheckCauseId = (int)SelectRecheck.ID);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.reviseBy = int.Parse(_settingsService.UserIdSetting));
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = StateData.State.Update);
                    selectReceivePlanMst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                    selectReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 2;
                    selectReceivePlanMst.receivePlanDtl[0].state = StateData.State.Update;
                    selectReceivePlanMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                    selectReceivePlanMst.receivePlanMstStatus = 2;
                    selectReceivePlanMst.state = StateData.State.Update;
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    if (selectReceivePlanMst != null)
                    {
                        await _materialAutoReceivingService.SaveRecieving(selectReceivePlanMst);
                    }
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ส่งให้เช็คอีกครั้งแล้ว");
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();

                }

            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
        }

        private async Task<bool> ValidationData(int step)
        {
            bool retval = true;
            if (step == 1) // recheck
            {
                if (SelectRecheck == null)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือกเหตุผลในการส่งเช็คอีกครั้ง");
                }
                else
                {
                    if (selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode != null || selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count > 0)
                    {
                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                        foreach (var item in selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode)
                        {
                            try
                            {
                                IsBusy = true;
                                var chk = await _materialAutoReceivingService.GetWeightingByBarcodeAsync(item.barcode);
                                IsBusy = false;
                                if (chk)
                                {
                                    retval = false;
                                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.barcode + " ได้ถูกนำไปใช้งานแล้ว");
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                retval = false;
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาดในการโหลดน้ำหนักข้อมูล กรุณาติดต่อ ICT");
                                break;
                            }
                        }
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    }
                    else
                    {
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบข้อมูล");
                    }
                }


            }
            else if (step == 2) // Save
            {
                var mst = selectReceivePlanMst;
                var dtl = mst.receivePlanDtl[0];
                var amount = dtl.ReceivePlanBarcode.Where(x => x.lstPlanCheckQA == null).Count();
                if (amount != 0)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                        "ทำการตรวจสอบวัตถุดิบเรียบร้อยแล้ว ใช่หรือไม่ ?\nตรวจสอบวัตถุดิบ",
                        () =>
                        {
                            try
                            {
                                foreach (var item in dtl.ReceivePlanBarcode)
                                {
                                    List<PlanCheck> lst = new List<PlanCheck>();
                                    foreach (var itemCL in CheckQaList)
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
                                    item.lstPlanCheckQA = new List<PlanCheck>(lst);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            };
                        });
                }
                else
                {
                    foreach (var item in dtl.ReceivePlanBarcode)
                    {
                        List<PlanCheck> lst = new List<PlanCheck>();
                        foreach (var itemCL in CheckQaList)
                        {
                            lst.Add(new PlanCheck
                            {
                                IsCheck = itemCL.IsCheck,
                                CheckListId = itemCL.ID,
                                CheckedBy = _settingsService.UserSetting.UserId,
                                CheckedDate = DateTime.Now,
                                state = Enumerations.StateData.State.Insert
                            });
                        }
                        item.lstPlanCheckQA = new List<PlanCheck>(lst);
                    }
                }
            }
            return retval;
        }

        private async void GetSendYesCheckListAsync()
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
                        MaterialReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id).ToList();
                        var selectbarcodedtl = MaterialReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var chklotno = MaterialReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
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
                        MaterialReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id).ToList();
                        var chklotno = MaterialReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
                        foreach (var items in chklotno)
                        {
                            MaterialReceivePlanDtl.Remove(items);
                        }
                        MaterialReceivePlanDtl.Add(selectReceivePlanMst.receivePlanDtl[0]);
                        var selectbarcode = MaterialReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
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
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "บันทึกสำเร็จ");
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
        }
        private async void GetSendNoCheckListAsync()
        {
            try
            {
                var chkData = await ValidationData(2);
                if (chkData)
                {
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    var checkdtl = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(selectReceivePlanMst.Id, selectReceivePlanMst.receivePlanDtl[0].Id);

                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 3);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.StatusQA = false);
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receiveBy = int.Parse(_settingsService.UserIdSetting));
                    selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = StateData.State.Update);
                    var word = selectReceivePlanMst.receivePlanDtl[0].rmId.Substring(0, 1);
                    if (word == "I" || word == "S")
                    {
                        MaterialReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id).ToList();
                        var selectbarcodedtl = MaterialReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                        var chklotno = MaterialReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
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
                        MaterialReceivePlanDtl = checkdtl.Where(x => x.Id == selectReceivePlanMst.receivePlanDtl[0].Id && x.keyLink == selectReceivePlanMst.receivePlanDtl[0].keyLink).ToList();
                        var chklotno = MaterialReceivePlanDtl.Where(x => x.lotNo == selectReceivePlanMst.receivePlanDtl[0].lotNo).ToList();
                        foreach (var items in chklotno)
                        {
                            MaterialReceivePlanDtl.Remove(items);
                        }
                        MaterialReceivePlanDtl.Add(selectReceivePlanMst.receivePlanDtl[0]);
                        var selectbarcode = MaterialReceivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
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
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "บันทึกสำเร็จ");
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
            catch (Exception e)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลตรวจเช็คผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                await _navigationService.NavigateBackAsync();
            }
        }

        private async void OnCancelQualityCheckAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                             "ต้องการยกเลิกการตรวจสอบ ใช่หรือไม่ ? \nยกเลิกตรวจสอบ",
                             async () =>
                             {
                                 try
                                 {
                                     var cancelReceivePlanQualityCheck = new CancelReceivePlanCheckVM
                                     {
                                         DocNo = selectReceivePlanMst.documentRequestNo,
                                         LotNo = selectReceivePlanMst.receivePlanDtl[0].lotNo,
                                         RmId = selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0].rmId
                                     };

                                     DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                                     IsBusy = true;
                                     await _materialAutoReceivingService.CancelReceivePlanQualityCheck(cancelReceivePlanQualityCheck);
                                     IsBusy = false;
                                     DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                     await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "ยกเลิกการตรวจสอบเรียบร้อย");
                                     MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                                     await _navigationService.NavigateBackAsync();
                                 }
                                 catch (Exception ex)
                                 {
                                     throw ex;
                                 }
                             });
                }
                catch (HttpRequestExceptionEx e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาดในการยกเลิกการตรวจสอบ กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();
                }
                catch (Exception e)
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Equals(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาดในการยกเลิกการตรวจสอบ กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                    MessagingCenter.Send(this, MessagingConstants.RecheckQuality, true);
                    await _navigationService.NavigateBackAsync();
                }
            }
        }
    }
}
