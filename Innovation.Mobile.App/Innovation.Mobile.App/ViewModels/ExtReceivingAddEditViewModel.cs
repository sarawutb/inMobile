using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
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
using Xamarin.Forms;
using System.Text.RegularExpressions;
using Innovation.Mobile.App.Views;
using Innovation.Mobile.App.Exceptions;
using System.Runtime.CompilerServices;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{
    public class ExtReceivingAddEditViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private MaterialReceivePlanMst _materialReceivePlanMst;
        private DateTime _firstDate, _productionDate, _expireDate, _accountDate;
        private string _lotNo_Group, _rmTitle;
        private int _qty, _maxlen, _maxlenLotgroup;
        private UnitCountVM _selectUnitCount;
        private ObservableCollection<UnitCountVM> _lstUnitCount;
        private decimal _weightPerUnit, _totalWeight;
        private bool _isgenLotNo, _iscanceledit, isEditExpire, _modeEdit
            , _lockbtn, _isswitch, _isFirstDate, _isTotalWeight
            , _isreprint, _islockedit, _isswitchlot, _lockentryedit, _lockgenbarcode
            , _lockeditdata, _isEnCalcelEdit,
            _isLockAcc
            , _Ischecklist;
        private ObservableCollection<MaterialReceivePlanDtlBarcode> _receivePlanBarcode;
        private List<MaterialReceivePlanDtlBarcode> _keepreceivePlanBarcode;
        private MaterialReceivePlanDtlBarcode _selectPlanBarcode;
        private DefaultContAndUnit _defaultContAndUnit;
        private List<MaterialReceivePlanDtl> _receivedtlplan;
        private List<CheckList> _checkLists;

        public ExtReceivingAddEditViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService = loggingService;
            LoadUnitProfile();
            LoadCheckListAsync();
            _maxlenLotgroup = 45;
            LockBtn = true;
            _isLockAcc = true;
            IsFirstDate = true;
            IsLockEntryEdit = true;
            InitializeMessenger();
        }
        public ICommand ReprintClickedCommand => new Command(OnRePrintAsync);
        public ICommand GenChangeDate => new Command(OnChangeDate);
        public ICommand GenLotNoToggledCommand => new Command(OnGenLotNoToggled);
        public ICommand TextChangedCommand => new Command<string>(OnTextChanged);
        public ICommand WeightPerUnitChangedCommand => new Command(OnWeightPerUnitChanged);
        public ICommand SaveClickedCommand => new Command(OnSaveClicked);
        public ICommand BarcodeTappedCommand => new Command<MaterialReceivePlanDtlBarcode>(OnBarcodeTapped);
        public ICommand ProdDateSelectedCommand => new Command(OnProdDateSelected);
        public ICommand ppChangedCommand => new Command(OnppChanged);
        public ICommand EditBarcodeCommand => new Command(OnEditDataBarcode);
        public ICommand CancelEditBarcodeCommand => new Command(OnCancelEditBarcode);
        public ICommand BarcodeClickedCommand => new Command(OnBarcodeClickedAsync);
        public ICommand TapCommand => new Command(OnTabChangeFrame);
        public ICommand LotChangedCommand => new Command(OnEnableSwitch);
        public ICommand SelectedIndexUnit => new Command(OnChangeUnit);

        public override async Task InitializeAsync(object ReceivePlan)
        {
            DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
            MaterialReceivePlanMst = (MaterialReceivePlanMst)ReceivePlan;
            RMTitle = MaterialReceivePlanMst.receivePlanDtl[0].rmId + " " + _materialReceivePlanMst.receivePlanDtl[0].rmName;
            LoadDefault();
            LoadForEdit();
            IsTotalWeight = true;
            IsEnCalcelEdit = true;
            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
        }
        public int MaxlenLotGrroup
        {
            get => _maxlenLotgroup;
            set
            {
                _maxlenLotgroup = value;
                OnPropertyChanged();
            }
        }
        public int Maxlenlot
        {
            get => _maxlen;
            set
            {
                _maxlen = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnCalcelEdit
        {
            get => _isEnCalcelEdit;
            set
            {
                _isEnCalcelEdit = value;
                OnPropertyChanged();
            }

        }
        public bool LockEditData
        {
            get => _lockeditdata;
            set
            {
                _lockeditdata = value;
                OnPropertyChanged();
            }
        }
        public bool IsFirstDate
        {
            get => _isFirstDate;
            set
            {
                _isFirstDate = value;
                OnPropertyChanged();
            }
        }
        public bool IsRePrint
        {
            get => _isreprint;
            set
            {
                _isreprint = value;
                OnPropertyChanged();
            }
        }
        public bool IsCancelEdit
        {
            get => _iscanceledit;
            set
            {
                _iscanceledit = value;
                OnPropertyChanged();
            }
        }
        public bool IsHideEdit
        {
            get => _islockedit;
            set
            {
                _islockedit = value;
                OnPropertyChanged();
            }
        }
        public bool LockBtn
        {
            get => _lockbtn;
            set
            {
                _lockbtn = value;
                OnPropertyChanged();
            }
        }
        public bool IsSwitch
        {
            get => _isswitch;
            set
            {
                _isswitch = value;
                OnPropertyChanged();
            }
        }
        public bool IsTotalWeight
        {
            get => _isTotalWeight;
            set
            {
                _isTotalWeight = value;
                OnPropertyChanged();
            }
        }
        public bool IsSwitchLot
        {
            get => _isswitchlot;
            set
            {
                _isswitchlot = value;
                OnPropertyChanged();
            }
        }
        public bool IsLockgenBarcode
        {
            get => _lockgenbarcode;
            set
            {
                _lockgenbarcode = value;
                OnPropertyChanged();
            }
        }
        public bool IsLockEntryEdit
        {
            get => _lockentryedit;
            set
            {
                _lockentryedit = value;
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
        public bool IsLockAcc
        {
            get => _isLockAcc;
            set
            {
                _isLockAcc = value;
                OnPropertyChanged();
            }
        }
        public bool Ischecklist
        {
            get => _Ischecklist;
            set
            {
                _Ischecklist = value;
                OnPropertyChanged();
            }
        }
        public MaterialReceivePlanMst MaterialReceivePlanMst
        {
            get => _materialReceivePlanMst;
            set
            {
                _materialReceivePlanMst = value;
                OnPropertyChanged();
            }
        }
        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                _firstDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime AccountDate
        {
            get => _accountDate;
            set
            {
                _accountDate = value;
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
        public string LotNo_Group
        {
            get => _lotNo_Group;
            set
            {
                _lotNo_Group = value;
                OnPropertyChanged();
            }
        }
        public int Qty
        {
            get => _qty;
            set
            {
                _qty = value;
                OnPropertyChanged();
            }
        }
        public UnitCountVM SelectUnitCount
        {
            get => _selectUnitCount;
            set
            {
                _selectUnitCount = value;
                OnPropertyChanged();
            }
        }
        public List<MaterialReceivePlanDtl> KeepDtlPlan
        {
            get => _receivedtlplan;
            set
            {
                _receivedtlplan = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UnitCountVM> lstUnitCount
        {
            get => _lstUnitCount;
            set
            {
                _lstUnitCount = value;
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
        public decimal TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged();
            }
        }
        public bool IsGenLotNo
        {
            get => _isgenLotNo;
            set
            {
                _isgenLotNo = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditExpire
        {
            get => isEditExpire;
            set
            {
                isEditExpire = value;
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
        public List<MaterialReceivePlanDtlBarcode> KeepReceivePlanDtlBarcode
        {
            get => _keepreceivePlanBarcode;
            set
            {
                _keepreceivePlanBarcode = value;
                OnPropertyChanged();
            }
        }

        public MaterialReceivePlanDtlBarcode SelectPlanBarcode
        {
            get => _selectPlanBarcode;
            set
            {
                _selectPlanBarcode = value;
                OnPropertyChanged();
            }
        }
        public DefaultContAndUnit DefaultContAndUnit
        {
            get => _defaultContAndUnit;
            set
            {
                _defaultContAndUnit = value;
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
        public List<CheckList> CheckLists
        {
            get => _checkLists;
            set
            {
                _checkLists = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadUnitProfile()
        {
            try
            {
                IsBusy = true;
                lstUnitCount = (await _materialAutoReceivingService.GetUnitCountAsync()).ToObservableCollection();
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูล Unit ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูล Unit ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }

        private async void OnGenLotNoToggled()
        {
            GenLotNoToggled();
        }

        public async Task EditLotNoAsync()
        {
            try
            {
                await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                             "ต้องการแก้ไขรายการข้อมูล Lot:" + MaterialReceivePlanMst.receivePlanDtl[0].lotNo + " หรือไม่ \r\n*เอกสารรับเข้าจะทำการล็อคไม่ให้คนอื่นแก้ไข",
                             async () =>
                             {
                                 try
                                 {
                                     IsCancelEdit = true;
                                     IsHideEdit = false;
                                     IsRePrint = false;
                                     IsSwitchLot = true;
                                     IsLockEntryEdit = false;
                                     LockBtn = true;
                                     LockEditData = true;
                                     if (MaterialReceivePlanMst != null)
                                     {
                                         MaterialReceivePlanMst.IseditStatus = true;
                                         await _materialAutoReceivingService.GetUpdatermInMstStatus(MaterialReceivePlanMst);
                                         MaterialReceivePlanMst.state = Enumerations.StateData.State.Update;
                                         MaterialReceivePlanMst.reviseBy = Convert.ToInt32(_settingsService.UserIdSetting);
                                         if (MaterialReceivePlanMst.receivePlanDtl.Count > 0)
                                         {
                                             MaterialReceivePlanMst.receivePlanDtl.ForEach(ds => ds.state = Enumerations.StateData.State.Update);
                                             MaterialReceivePlanMst.receivePlanDtl.ForEach(dr => dr.reviseBy = Convert.ToInt32(_settingsService.UserIdSetting));
                                             if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count > 0)
                                             {
                                                 MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(bc => bc.state = Enumerations.StateData.State.Update);
                                                 MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(bc => bc.receivePlanDtlBarcodeStatus = 1);
                                                 MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(bcs => bcs.reviseBy = Convert.ToInt32(_settingsService.UserIdSetting));
                                             }
                                         }
                                     }
                                 }
                                 catch (Exception ex)
                                 {
                                     throw ex;
                                 }
                             });

            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การล็อคข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การล็อคข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }

        }
        public async Task CancelEditLotNoAsync()
        {
            try
            {
                await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                              "ต้องการยกเลิกแก้ไขรายการข้อมูล Lot:" + MaterialReceivePlanMst.receivePlanDtl[0].lotNo + " หรือไม่",
                              async () =>
                              {
                                  try
                                  {
                                      IsCancelEdit = false;
                                      IsHideEdit = true;
                                      IsRePrint = true;
                                      IsLockEntryEdit = false;
                                      LockBtn = false;
                                      LockEditData = false;
                                      _modeEdit = true;
                                      var lstdatabarcode = new List<MaterialReceivePlanDtlBarcode>();
                                      if (KeepDtlPlan.Count > 0)
                                      {
                                          LotNo_Group = KeepDtlPlan[0].lotNo;
                                          Qty = KeepDtlPlan[0].countQty;
                                          SelectUnitCount = lstUnitCount.Where(z => z.unitCode == KeepDtlPlan[0].unitCode).FirstOrDefault();
                                          WeightPerUnit = KeepDtlPlan[0].weightPerUnit;
                                          TotalWeight = (decimal)KeepDtlPlan[0].ReceivePlanBarcode.Sum(x => x.qty);
                                      }
                                      if (KeepReceivePlanDtlBarcode.Count > 0)
                                      {
                                          lstdatabarcode = KeepReceivePlanDtlBarcode.Clone<List<MaterialReceivePlanDtlBarcode>>();
                                          ReceivePlanDtlBarcode = lstdatabarcode.ToObservableCollection();
                                          if (ReceivePlanDtlBarcode.Count > 1)
                                          {
                                              IsGenLotNo = true;
                                          }
                                      }

                                      if (MaterialReceivePlanMst != null)
                                      {
                                          MaterialReceivePlanMst.receivePlanDtl = KeepDtlPlan;
                                          MaterialReceivePlanMst.IseditStatus = false;
                                          await _materialAutoReceivingService.GetUpdatermInMstStatus(MaterialReceivePlanMst);
                                      }
                                  }
                                  catch (Exception ex)
                                  {
                                      throw ex;
                                  }
                              });

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
        private void OnCancelEditBarcode()
        {
            CancelEditLotNoAsync();
        }
        private async Task GenLotNoToggled()
        {
            if (_isgenLotNo)
            {
                IsTotalWeight = false;
                if (IsCancelEdit)
                {
                    _modeEdit = false;
                }
                if (!_modeEdit)
                {
                    bool chkData = await ValidationData(1);
                    if (chkData)
                    {
                        GenLotNo();
                    }
                    else
                    {
                        IsGenLotNo = false;
                    }
                }
                else if (_modeEdit)
                {
                    IsRePrint = true;
                    IsFirstDate = false;
                    var grouplot = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(g => g.LotNo_Group == _lotNo_Group);
                    var idbarcode = grouplot.Where(x => x.Id != 0).ToList();
                    if (idbarcode.Count == 0)
                    {
                        GenLotNo();
                    }
                }
            }
            else
            {
                IsTotalWeight = true;
                if (IsCancelEdit)
                {
                    _modeEdit = false;
                    MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>();
                }
                else
                {
                    try
                    {
                        if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode != null)
                        {
                            if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count != 0)
                            {
                                var bacodelst = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.state != Enumerations.StateData.State.Insert).ToList();
                                if (bacodelst.Count >= 1)
                                {
                                    if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode != null)
                                    {
                                        IsGenLotNo = true;
                                        await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                        "ต้องการลบรายการข้อมูล Lot:" + MaterialReceivePlanMst.receivePlanDtl[0].lotNo + " หรือไม่",
                                        async () =>
                                        {
                                            try
                                            {
                                                IsGenLotNo = false;
                                                var chk = await ValidationData(3);
                                                if (chk)
                                                {
                                                    var barcodedtl = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.LotNo_Group == LotNo_Group).ToList();
                                                    var dtl = await _materialAutoReceivingService.GetMaterialReceivePlanDtl(MaterialReceivePlanMst.Id, MaterialReceivePlanMst.receivePlanDtl[0].Id);
                                                    var lstbarcode = dtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                                                    foreach (var item in barcodedtl)
                                                    {
                                                        var selectbarcode = lstbarcode.Where(x => x.Id == item.Id).FirstOrDefault();
                                                        if (selectbarcode != null)
                                                        {
                                                            lstbarcode.Remove(selectbarcode);
                                                        }
                                                    }
                                                    if (barcodedtl.Count != 0)
                                                    {
                                                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                                                        barcodedtl.ForEach(x => x.state = Enumerations.StateData.State.Delete);
                                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = barcodedtl;
                                                        if (lstbarcode.Count < 1)
                                                        {
                                                            MaterialReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 1;
                                                        }
                                                        MaterialReceivePlanMst.receivePlanDtl[0].state = Enumerations.StateData.State.Delete;
                                                        MaterialReceivePlanMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                                                        MaterialReceivePlanMst.state = Enumerations.StateData.State.Update;
                                                        MaterialReceivePlanMst.receivePlanMstStatus = 2;
                                                        IsBusy = true;
                                                        if (MaterialReceivePlanMst != null)
                                                        {
                                                            MaterialReceivePlanMst = await _materialAutoReceivingService.SaveRecieving(MaterialReceivePlanMst);
                                                        }
                                                        IsBusy = false;
                                                        IsFirstDate = true;
                                                        LockBtn = true;
                                                        IsHideEdit = false;
                                                        IsRePrint = false;
                                                        ModeEdit = false;
                                                        if (MaterialReceivePlanMst.receivePlanDtl.Count == 0 || MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count == 0)
                                                        {
                                                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาทำรายการรับเข้าใหม่");
                                                            await _navigationService.NavigateBackAsync();
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        });
                                    }
                                    else
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.DoNothing);
                                    }
                                }
                                else
                                {
                                    var checkbar = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Select(x => x.Id).ToList();
                                    if (checkbar[0] != 0)
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.DoNothing);
                                    }
                                    else
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = null;
                                    }

                                }
                            }
                        }

                    }
                    catch (HttpRequestExceptionEx e)
                    {
                        _loggingService.Error(e.Message);
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                    }
                    catch (Exception e)
                    {
                        _loggingService.Error(e.ToString());
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message:" + e.ToString());
                    }
                }

            }
        }

        private async Task<bool> ValidationData(int step)
        {
            bool retval = true;

            if (step <= 2)
            {
                if (string.IsNullOrWhiteSpace(LotNo_Group))
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก หมายเลข Lot");
                }
                else if (Qty == 0)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก จำนวน");
                    MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = null;
                }
                else if (WeightPerUnit == 0)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก น้ำหนัก/หน่วยบรรจุ");
                }
                else if (SelectUnitCount == null)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือก หน่วย");
                }
            }

            if (step == 2) // Save
            {
                if (FirstDate.Date > DateTime.Now.Date)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือกวันที่รับเข้าครั้งแรกให้ถูกต้อง");
                }
                else if (ProductionDate.Date > DateTime.Now.Date)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือกวันที่ผลิตให้ถูกต้อง");
                }
                else if (isEditExpire && ExpireDate.Date < DateTime.Now.Date)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือกวันที่หมดอายุให้ถูกต้อง");
                }
                else if (SelectUnitCount == null)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาเลือก หน่วย");
                }
                else if (IsGenLotNo)
                {
                    foreach (var item in ReceivePlanDtlBarcode)
                    {
                        if (string.IsNullOrWhiteSpace(item.lotNo))
                        {
                            retval = false;
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก หมายเลข Lot");
                            break;
                        }
                        else if (item.qty == 0)
                        {
                            retval = false;
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก หมายเลข Lot");
                            break;
                        }
                    }
                }

                if (retval)
                {
                    var mst = MaterialReceivePlanMst;
                    var dtl = mst.receivePlanDtl;
                    var barcodelst = dtl[0].ReceivePlanBarcode != null ? dtl.SelectMany(x => x.ReceivePlanBarcode).ToList() : null;
                    if (barcodelst == null || barcodelst.Count == 0)
                    {
                        OnNogenbarcodeAsync();
                    }
                    var amount = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.lstPlanCheck == null).Count();
                    if (amount == MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count)
                    {
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();

                        retval = false;

                        await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                            "ทำการตรวจสอบวัตถุดิบเรียบร้อยแล้ว ใช่หรือไม่",
                            () =>
                            {
                                try { }
                                catch (Exception ex)
                                {
                                    foreach (var item in MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode)
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

                                };
                            });
                    }
                    else
                    {
                        var lstbar = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.lstPlanCheck == null);
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
            else if (step == 3) // Delete
            {
                var countbc = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count();
                var qastatus = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.StatusQA != null).ToList();
                if (qastatus.Count == 0)
                {
                    if (ReceivePlanDtlBarcode != null)
                    {
                        DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                        foreach (var item in ReceivePlanDtlBarcode)
                        {
                            if (item.barcode != null)
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
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                                    break;
                                }
                            }
                        }
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    }
                }
                else
                {
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถลบได้ \r\n QAD ทำการตรวจเช็คแล้ว");
                    if (countbc > 1)
                    {
                        IsGenLotNo = true;
                    }
                    else
                    {
                        IsGenLotNo = false;
                    }

                }
            }
            else if (step == 4) // Check Print
            {
                bool chkPrint = true;
                var lstdata = MaterialReceivePlanMst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                foreach (var itemBc in lstdata)
                {
                    int yearExpire = Convert.ToInt32(itemBc.expireDate.Substring(0, 4));
                    int monthExpire = Convert.ToInt32(itemBc.expireDate.Substring(4, 2));
                    int dayExpire = Convert.ToInt32(itemBc.expireDate.Substring(6, 2));
                    var expDate = new DateTime(yearExpire, monthExpire, dayExpire);
                    var word = itemBc.rmId.Substring(0, 1);

                    if (expDate <= DateTime.Now)
                    {
                        chkPrint = false;
                    }
                }

                if (!chkPrint)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่สามารถพิมม์บาร์โค้ดได้ เนื่องจากบาร์โค้ดหมดอายุแล้ว");
                }
            }
            else if (step == 5) // Check Edit
            {
                var amt = MaterialReceivePlanMst.receivePlanDtl.Where(x => x.ReceivePlanBarcode != null).Count();
                if (amt == 0)
                {
                    retval = false;
                }
            }
            else if (step == 6)//check null
            {

                if (string.IsNullOrWhiteSpace(LotNo_Group))
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก หมายเลข Lot");
                }
                else if (Qty == 0)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก จำนวน");
                }
                else if (WeightPerUnit == 0)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก น้ำหนัก/หน่วยบรรจุ");
                }
            }
            else if (step == 7) // weight >,<  Total weight
            {
                if (TotalWeight > MaterialReceivePlanMst.receivePlanDtl[0].receivingWeight)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                       "น้ำหนักรวมมากกว่าน้ำหนักที่ต้องการ จะทำการรับเข้าหรือไม่",
                       () =>
                       {
                           retval = true;
                       });
                }
                else if (TotalWeight < MaterialReceivePlanMst.receivePlanDtl[0].receivingWeight)
                {

                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    retval = false;
                    await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                     "น้ำหนักรวมน้อยกว่าน้ำหนักที่ต้องการ จะทำการรับเข้าหรือไม่",
                     () =>
                     {
                         retval = true;
                     });
                }

            }
            else if (step == 8)
            {
                var chkstatusqa = _materialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.StatusQA != null).Count();
                if (chkstatusqa > 1)
                {
                    retval = false;
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "รายการนี้ทำตรวจสอบเรียบร้อยแล้ว");
                }
            }

            return retval;
        }

        public void SumQty()
        {
            var a = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Select(x => x.qty).Sum();
        }

        public void OnChangeDate()
        {
            if (!_isswitch)
            {
                ExpireDate = ProductionDate.AddDays(Convert.ToDouble(MaterialReceivePlanMst.receivePlanDtl[0].keepTime));
            }
        }
        private void GenLotNo()
        {
            var mst = MaterialReceivePlanMst.Clone<MaterialReceivePlanMst>();
            var dtl = mst.receivePlanDtl.LastOrDefault();
            var lstBarcode = new ObservableCollection<MaterialReceivePlanDtlBarcode>();
            int keeptime = Convert.ToInt32(dtl.keepTime.ToString("0"));
            string expDate = IsEditExpire ? ExpireDate.ToString("yyyyMMdd") : ProductionDate.AddDays(keeptime).ToString("yyyyMMdd");

            if (dtl.ReceivePlanBarcode == null)
            {
                dtl.firstReceiveDate = _firstDate.ToString("yyyyMMdd");
                dtl.accountDate = _accountDate.ToString("yyyyMMdd");
                dtl.countQty = _qty;
                dtl.weightPerUnit = _weightPerUnit;

                for (int i = 1; i <= _qty; i++)
                {
                    lstBarcode.Add(new MaterialReceivePlanDtlBarcode()
                    {
                        state = Enumerations.StateData.State.Insert,
                        receivePlanMstId = mst.Id,
                        receivePlanDtlId = dtl.Id,
                        rmId = dtl.rmId,
                        rmName = dtl.rmName,
                        lotNo = _lotNo_Group + "-",
                        lotNonum = (i).ToString(),
                        LotNo_Group = _lotNo_Group,
                        qty = _weightPerUnit,
                        receivePlanDtlBarcodeStatus = 1,
                        receivePlanDtlBarcodeStatusNameThai = "รอ LRD ทำการตรวจสอบ",
                        receiveBy = _settingsService.UserSetting.UserId,
                        receiveByName = _settingsService.UserSetting.UserWindow,
                        productionDate = ProductionDate.ToString("yyyyMMdd"),
                        expireDate = expDate,
                        reviseBy = int.Parse(_settingsService.UserIdSetting),
                        receiveDate = DateTime.Now,
                        MaxlenlotNonum = Maxlenlot

                    });
                }
                ReceivePlanDtlBarcode = lstBarcode;
                dtl.receivePlanDtlStatus = 1;
                dtl.lotNo = _lotNo_Group;
                dtl.unitCode = SelectUnitCount.unitCode;
                dtl.reviseBy = int.Parse(_settingsService.UserIdSetting);
                dtl.ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanDtlBarcode);
                mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                mst.state = Enumerations.StateData.State.Update;
                MaterialReceivePlanMst = mst;
            }
            else
            {
                dtl.firstReceiveDate = _firstDate.ToString("yyyyMMdd");
                dtl.accountDate = _accountDate.ToString("yyyyMMdd");
                dtl.countQty = _qty;
                dtl.weightPerUnit = _weightPerUnit;
                for (int i = 1; i <= _qty; i++)
                {
                    lstBarcode.Add(new MaterialReceivePlanDtlBarcode()
                    {
                        state = Enumerations.StateData.State.Insert,
                        receivePlanMstId = mst.Id,
                        receivePlanDtlId = dtl.Id,
                        rmId = dtl.rmId,
                        rmName = dtl.rmName,
                        lotNo = _lotNo_Group + "-",
                        lotNonum = (i).ToString(),
                        LotNo_Group = _lotNo_Group,
                        qty = _weightPerUnit,
                        receivePlanDtlBarcodeStatus = 1,
                        receivePlanDtlBarcodeStatusNameThai = "รอ LRD ทำการตรวจสอบ",
                        receiveBy = _settingsService.UserSetting.UserId,
                        receiveByName = _settingsService.UserSetting.UserWindow,
                        productionDate = ProductionDate.ToString("yyyyMMdd"),
                        expireDate = expDate,
                        reviseBy = int.Parse(_settingsService.UserIdSetting),
                        receiveDate = DateTime.Now,
                        MaxlenlotNonum = Maxlenlot
                    });
                }
                ReceivePlanDtlBarcode = lstBarcode;
                dtl.lotNo = _lotNo_Group;
                dtl.unitCode = SelectUnitCount.unitCode;
                dtl.receivePlanDtlStatus = 1;
                dtl.reviseBy = int.Parse(_settingsService.UserIdSetting);
                dtl.ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanDtlBarcode);
                mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                mst.state = Enumerations.StateData.State.Update;
                MaterialReceivePlanMst = mst;
            }
        }
        private async void LoadDefault()
        {
            FirstDate = DateTime.Now;
            ProductionDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(Convert.ToDouble(MaterialReceivePlanMst.receivePlanDtl[0].keepTime));
            AccountDate = DateTime.Now;
            LockEditData = true;
            IsEditExpire = MaterialReceivePlanMst.forSetEXPDate;
            if (IsEditExpire)
            {
                IsSwitch = true;
            }
            try
            {
                IsBusy = true;
                await LoadUnitProfile();
                var dtl = _materialReceivePlanMst.receivePlanDtl[0];
                SelectUnitCount = lstUnitCount.Where(x => x.unitCode == dtl.unitCode).FirstOrDefault();
                WeightPerUnit = dtl.weightPerUnit;
                IsBusy = false;
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());


            }
        }
        private async Task LoadForEdit()
        {
            var dtl = _materialReceivePlanMst.receivePlanDtl[0];
            if (dtl.firstReceiveDate != null)
            {
                int yearFirst = Convert.ToInt32(dtl.firstReceiveDate.Substring(0, 4));
                int monthFirst = Convert.ToInt32(dtl.firstReceiveDate.Substring(4, 2));
                int dayFirst = Convert.ToInt32(dtl.firstReceiveDate.Substring(6, 2));
                FirstDate = new DateTime(yearFirst, monthFirst, dayFirst);
            }
            if (dtl.accountDate != null)
            {
                int yearFirst = Convert.ToInt32(dtl.accountDate.Substring(0, 4));
                int monthFirst = Convert.ToInt32(dtl.accountDate.Substring(4, 2));
                int dayFirst = Convert.ToInt32(dtl.accountDate.Substring(6, 2));
                AccountDate = new DateTime(yearFirst, monthFirst, dayFirst);
                IsLockAcc = false;
            }
            Qty = dtl.countQty;
            SelectUnitCount = lstUnitCount.Where(z => z.unitCode == dtl.unitCode).FirstOrDefault();
            WeightPerUnit = dtl.weightPerUnit;

            var chk = await ValidationData(5);
            if (chk)
            {
                var lstBarcode = MaterialReceivePlanMst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToList();
                foreach (var itemnum in lstBarcode)
                {
                    itemnum.lotNonum = itemnum.lotNo.Substring(itemnum.lotNo.LastIndexOf("-") + 1, itemnum.lotNo.Length - (itemnum.lotNo.LastIndexOf("-") + 1));
                    itemnum.MaxlenlotNonum = ((MaxlenLotGrroup + 5) - itemnum.LotNo_Group.Length) - 1;
                }
                if (dtl.ReceivePlanBarcode.Count > 0)
                {
                    var barcode = dtl.ReceivePlanBarcode[0];

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

                    if (string.IsNullOrWhiteSpace(barcode.LotNo_Group))
                    {
                        _modeEdit = false;
                        IsFirstDate = true;
                        LockEditData = true;
                    }
                    else
                    {
                        IsRePrint = true;
                        _modeEdit = true;
                        IsHideEdit = true;
                        IsFirstDate = false;
                        IsLockEntryEdit = false;
                        LockEditData = false;
                        //IsTotalWeight = false;
                        LotNo_Group = barcode.LotNo_Group;
                    }

                    if (dtl.ReceivePlanBarcode.Count > 1)
                    {
                        IsGenLotNo = true;
                    }
                    TotalWeight = (decimal)dtl.ReceivePlanBarcode.Sum(x => x.qty);
                    var keepdtl = dtl.Clone<MaterialReceivePlanDtl>();
                    KeepDtlPlan = new List<MaterialReceivePlanDtl>();
                    KeepDtlPlan.Add(keepdtl);
                    ReceivePlanDtlBarcode = dtl.ReceivePlanBarcode.ToObservableCollection();
                    if (KeepReceivePlanDtlBarcode == null)
                    {
                        KeepReceivePlanDtlBarcode = lstBarcode.Clone<List<MaterialReceivePlanDtlBarcode>>();
                    }
                    if (barcode.receivePlanDtlBarcodeStatus >= 2)
                    {
                        LockBtn = false;
                    }
                    else
                    {
                        LockBtn = true;
                    }
                    if (barcode.ReceivePlanRecheckCauseId != 0 && barcode.receivePlanDtlBarcodeStatus < 2)
                    {
                        IsCancelEdit = true;
                        IsRePrint = false;
                        IsEnCalcelEdit = false;
                        IsHideEdit = false;
                    }
                    if (barcode.StatusQA != null)
                    {
                        IsHideEdit = false;
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "QAD ทำการตรวจเช็คแล้ว ไม่สารถแก้ไขรายการนี้ได้ !!");
                    }
                }
                else
                {
                    _modeEdit = false;
                    IsFirstDate = true;
                    LockEditData = true;
                }
                foreach (var item in lstBarcode)
                {
                    try
                    {
                        IsBusy = true;
                        var chkuse = await _materialAutoReceivingService.GetWeightingByBarcodeAsync(item.barcode);
                        IsBusy = false;

                        if (chkuse)
                        {
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            IsHideEdit = false;
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "บาร์โค้ด " + item.barcode + " ได้ถูกนำไปใช้งานแล้ว");
                            break;
                        }
                    }
                    catch (HttpRequestExceptionEx e)
                    {
                        _loggingService.Error(e.Message);
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                        break;
                    }
                    catch (Exception e)
                    {
                        _loggingService.Error(e.ToString());
                        DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                        break;
                    }
                }
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            }
        }
        private void OnTextChanged(string value)
        {
            IsGenLotNo = false;
            var a = int.TryParse(value, out int retval);
            Qty = retval;
            TotalWeight = Qty * WeightPerUnit;
            if (IsCancelEdit)
            {
                MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>();
            }
        }
        private void OnWeightPerUnitChanged()
        {
            IsGenLotNo = false;
            TotalWeight = Qty * WeightPerUnit;
            if (IsCancelEdit)
            {
                MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>();
            }
        }
        private void OnTabChangeFrame()
        {
            if (IsCancelEdit)
            {
                if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count > 1)
                {
                    TotalWeight = (decimal)MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Sum(x => x.qty);
                }

            }
            else
            {
                if (ReceivePlanDtlBarcode != null)
                {
                    TotalWeight = (decimal)ReceivePlanDtlBarcode.Sum(x => x.qty);
                }
            }
        }
        private void OnEditDataBarcode()
        {
            EditLotNoAsync();
        }
        private void OnppChanged(object obj)
        {
            if (ReceivePlanDtlBarcode != null)
            {
                TotalWeight = (decimal)ReceivePlanDtlBarcode.Sum(x => x.qty);
            }
        }
        private async void OnSaveClicked()
        {
            SaveClicked();
        }
        private async Task SaveClicked()
        {
            var mst = new MaterialReceivePlanMst();
            var mstreturn = new MaterialReceivePlanMst();
            var chkData = await ValidationData(6);
            if (chkData)
            {
                var chkweight = await ValidationData(7);
                if (chkweight)
                {
                    var chkDatatest = await ValidationData(2);
                    if (chkDatatest)
                    {
                        var chkusedata = await ValidationData(3);
                        if (chkusedata)
                        {
                            try
                            {

                                if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count == 0)
                                {
                                    OnNogenbarcodeAsync();
                                }

                                var checkbarcode = MaterialReceivePlanMst.receivePlanDtl.Where(x => x.ReceivePlanBarcode != null).ToList();
                                if (checkbarcode.Count > 0)
                                {
                                    if (MaterialReceivePlanMst.state == Enumerations.StateData.State.DoNothing)
                                    {
                                        if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count == 1)
                                        {
                                            MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0].qty = TotalWeight;
                                        }
                                        MaterialReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 2;
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.Update);
                                        MaterialReceivePlanMst.receivePlanDtl[0].unitCode = SelectUnitCount.unitCode;
                                        MaterialReceivePlanMst.receivePlanDtl[0].countQty = _qty;
                                        MaterialReceivePlanMst.receivePlanDtl[0].state = Enumerations.StateData.State.Update;
                                        MaterialReceivePlanMst.receivePlanMstStatus = 2;
                                        MaterialReceivePlanMst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                                        MaterialReceivePlanMst.OperationSite = int.Parse(_settingsService.SiteIdSetting);
                                        MaterialReceivePlanMst.state = Enumerations.StateData.State.Update;
                                    }
                                    var checkid = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Where(x => x.Id == 0).Count();
                                    if (checkid > 0)
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.Insert);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.lotNo = x.lotNo + x.lotNonum);
                                        MaterialReceivePlanMst.receivePlanDtl[0].receivePlanDtlStatus = 2;
                                        MaterialReceivePlanMst.receivePlanMstStatus = 2;
                                        MaterialReceivePlanMst.OperationSite = int.Parse(_settingsService.SiteIdSetting);
                                    }
                                    else
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.lotNo = x.lotNo.Substring(0, x.lotNo.LastIndexOf("-") + 1) + x.lotNonum);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 2);
                                    }
                                    IsBusy = true;
                                    if (MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count == 1 && IsGenLotNo == true)
                                    {
                                        MaterialReceivePlanMst.receivePlanDtl[0].lotNo = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0].lotNo;
                                    }
                                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                                    if (IsCancelEdit)
                                    {
                                        KeepDtlPlan[0].state = Enumerations.StateData.State.Delete;
                                        KeepDtlPlan[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.Delete);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.Id = 0);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.barcode = null);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.piLotNo = null);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.receivePlanDtlBarcodeStatus = 2);
                                        MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.state = Enumerations.StateData.State.Insert);
                                        if (MaterialReceivePlanMst.ownerSite != MaterialReceivePlanMst.receiveSite)
                                        {
                                            foreach (var item in MaterialReceivePlanMst.receivePlanDtl)
                                            {
                                                MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.ForEach(x => x.rmId = item.rmId);
                                            }
                                        }
                                        MaterialReceivePlanMst.receivePlanDtl.AddRange(KeepDtlPlan);
                                    }
                                    MaterialReceivePlanMst.IseditStatus = false;
                                    if (MaterialReceivePlanMst != null)
                                    {
                                        mst = await _materialAutoReceivingService.SaveRecieving(MaterialReceivePlanMst);
                                    }

                                    IsBusy = false;

                                }
                            }
                            catch (HttpRequestExceptionEx e)
                            {
                                IsBusy = false;
                                _loggingService.Error(e.Message);
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                            }
                            catch (Exception ex)
                            {
                                IsBusy = false;
                                _loggingService.Error(ex.ToString());
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                if (ex.Message == "Connection reset")
                                {
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การเชื่อมต่ออินเตอร์เน็ตผิดพลาด กรุณาลองใหม่");
                                }
                                else
                                {
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + ex.ToString());
                                }
                            }
                            try
                            {
                                if (mst.receivePlanDtl != null)
                                {
                                    mstreturn = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(mst);
                                    if (mst != null)
                                    {
                                        if (mstreturn.receivePlanDtl.Count > 0)
                                        {
                                            var clone = mstreturn.Clone<MaterialReceivePlanMst>();
                                            MaterialReceivePlanMst = mst;
                                            var chkPrint = await ValidationData(4);
                                            if (chkPrint)
                                            {
                                                PrintLabel(clone);
                                            }
                                            int typeid = 1;
                                            MessagingCenter.Send(this, MessagingConstants.ChecklistType, typeid);
                                            await _navigationService.NavigateBackAsync();
                                        }
                                        else
                                        {
                                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "ข้อมูลผิดพลาด ไม่พบรายละเอียดของรายการพิมพ์บาร์โค้ด", async () => { await _navigationService.NavigateBackAsync(); });
                                        }
                                    }
                                }
                            }
                            catch (HttpRequestExceptionEx e)
                            {
                                IsBusy = false;
                                _loggingService.Error(e.Message);
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                                await _navigationService.NavigateBackAsync();
                            }
                            catch (Exception e)
                            {
                                IsBusy = false;
                                _loggingService.Error(e.ToString());
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การบันทึกข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());
                                await _navigationService.NavigateBackAsync();
                            }
                        }
                    }
                }

            }
        }

        private async void PrintLabel(MaterialReceivePlanMst mst)
        {
            try
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem,
                                               "ต้องการพิมพ์ลาเบลหรือไม่",
                                               async () =>
                                               {
                                                   try
                                                   {
                                                       ReceivePlanDtlBarcode = mst.receivePlanDtl.SelectMany(x => x.ReceivePlanBarcode).ToObservableCollection<MaterialReceivePlanDtlBarcode>();
                                                       var word = mst.receivePlanDtl[0].rmId.Substring(0, 1);

                                                       if (word == "P")
                                                       {
                                                           var color = mst.receivePlanDtl.Select(x => x.colorName).ToList();
                                                           DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                                           await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "กรุณาเปลี่ยนเครื่องพิมพ์เป็น A5 แล้วใช้กระดาษสี" + color[0]);
                                                           var dtlimg = mst.receivePlanDtl[0].Imagedtl;
                                                           var ms = new MemoryStream(dtlimg);
                                                           var printer = DependencyService.Get<IPrintService>();
                                                           var result = printer.PrintPdfFile(ms, MediaSize.A5);
                                                       }
                                                       else
                                                       {
                                                           try
                                                           {
                                                               var lstBarcode = ReceivePlanDtlBarcode.ToList();
                                                               foreach (var itemBc in lstBarcode)
                                                               {
                                                                   var GHSmst = await _materialAutoReceivingService.GetReportGHSByRM(itemBc.rmId, Convert.ToInt32(mst.ownerSite));
                                                                   itemBc.rmGHSlst = GHSmst;
                                                                   var printer = DependencyService.Get<IPrintService>();
                                                                   printer.CommandPrint(_settingsService.PrintPortFormSetting, _settingsService.PrintIPAdressFormSetting, itemBc);
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

                                                   }
                                                   catch (Exception ex)
                                                   {
                                                       throw ex;
                                                   }
                                               });
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + ex.ToString());
            }

        }

        private async void OnNogenbarcodeAsync()
        {
            try
            {
                if (_isgenLotNo == false)
                {
                    var mst = MaterialReceivePlanMst.Clone<MaterialReceivePlanMst>();
                    var statedtl = mst.receivePlanDtl[0].state == Enumerations.StateData.State.Update;
                    if (!statedtl)
                    {
                        var lstBarcode = new ObservableCollection<MaterialReceivePlanDtlBarcode>();
                        int keeptime = Convert.ToInt32(MaterialReceivePlanMst.receivePlanDtl[0].keepTime.ToString("0"));
                        string expDate = IsEditExpire ? ExpireDate.ToString("yyyyMMdd") : ProductionDate.AddDays(keeptime).ToString("yyyyMMdd");
                        if (mst.receivePlanDtl[0].ReceivePlanBarcode == null || mst.receivePlanDtl[0].ReceivePlanBarcode.Count == 0)
                        {
                            lstBarcode.Add(new MaterialReceivePlanDtlBarcode()
                            {
                                state = Enumerations.StateData.State.Insert,
                                receivePlanMstId = mst.Id,
                                receivePlanDtlId = mst.receivePlanDtl[0].Id,
                                rmId = mst.receivePlanDtl[0].rmId,
                                rmName = mst.receivePlanDtl[0].rmName,
                                lotNo = _lotNo_Group,
                                LotNo_Group = _lotNo_Group,
                                qty = TotalWeight,
                                receivePlanDtlBarcodeStatus = 1,
                                receivePlanDtlBarcodeStatusNameThai = "รอ LRD ทำการตรวจสอบ",
                                receiveBy = _settingsService.UserSetting.UserId,
                                receiveByName = _settingsService.UserSetting.UserWindow,
                                productionDate = ProductionDate.ToString("yyyyMMdd"),
                                expireDate = expDate,
                                reviseBy = int.Parse(_settingsService.UserIdSetting),
                                receiveDate = DateTime.Now
                            });
                            ReceivePlanDtlBarcode = lstBarcode;
                            mst.receivePlanDtl[0].firstReceiveDate = _firstDate.ToString("yyyyMMdd");
                            mst.receivePlanDtl[0].firstReceiveDate = _accountDate.ToString("yyyyMMdd");
                            mst.receivePlanDtl[0].countQty = _qty;
                            mst.receivePlanDtl[0].weightPerUnit = _weightPerUnit;
                            mst.receivePlanDtl[0].receivePlanDtlStatus = 2;
                            mst.receivePlanDtl[0].lotNo = _lotNo_Group;
                            mst.receivePlanDtl[0].unitCode = SelectUnitCount.unitCode;
                            mst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                            mst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>(ReceivePlanDtlBarcode);
                        }
                        mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                        mst.state = Enumerations.StateData.State.Update;
                        MaterialReceivePlanMst = mst;
                    }
                    else
                    {
                        mst.receivePlanDtl.ForEach(x => x.state = Enumerations.StateData.State.Update);
                        mst.receivePlanDtl.ForEach(x => x.reviseBy = int.Parse(_settingsService.UserIdSetting));
                        var lstBarcode = new ObservableCollection<MaterialReceivePlanDtlBarcode>();
                        int keeptime = Convert.ToInt32(mst.receivePlanDtl[0].keepTime.ToString("0"));
                        string expDate = IsEditExpire ? ExpireDate.ToString("yyyyMMdd") : ProductionDate.AddDays(keeptime).ToString("yyyyMMdd");
                        if (mst.receivePlanDtl[0].ReceivePlanBarcode == null || mst.receivePlanDtl[0].ReceivePlanBarcode.Count == 0)
                        {
                            mst.receivePlanDtl[0].firstReceiveDate = _firstDate.ToString("yyyyMMdd");
                            mst.receivePlanDtl[0].accountDate = _accountDate.ToString("yyyyMMdd");
                            mst.receivePlanDtl[0].countQty = _qty;
                            mst.receivePlanDtl[0].weightPerUnit = _weightPerUnit;
                            lstBarcode.Add(new MaterialReceivePlanDtlBarcode()
                            {
                                state = Enumerations.StateData.State.Insert,
                                receivePlanMstId = mst.Id,
                                receivePlanDtlId = mst.receivePlanDtl.LastOrDefault().Id,
                                rmId = mst.receivePlanDtl.LastOrDefault().rmId,
                                rmName = mst.receivePlanDtl.LastOrDefault().rmName,
                                lotNo = _lotNo_Group,
                                LotNo_Group = _lotNo_Group,
                                qty = TotalWeight,
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
                        mst.receivePlanDtl[0].receivePlanDtlStatus = 2;
                        mst.receivePlanDtl[0].lotNo = _lotNo_Group;
                        mst.receivePlanDtl[0].unitCode = SelectUnitCount.unitCode;
                        mst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                        mst.receivePlanDtl[0].ReceivePlanBarcode = lstBarcode.ToList();
                        mst.reviseBy = int.Parse(_settingsService.UserIdSetting);
                        mst.state = Enumerations.StateData.State.Update;
                        MaterialReceivePlanMst = mst;
                    }
                }
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "มีข้อผิดพลาดเกิดขึ้น กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
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
            Ischecklist = true;
            await _navigationService.NavigateToAsync<InternalReceivingBarcodeDetailViewModel>(mst);
        }

        private void OnProdDateSelected()
        {
            var dtl = MaterialReceivePlanMst.receivePlanDtl[0];
            DateTime exp = ProductionDate;
            int keeptime = Convert.ToInt32(dtl.keepTime.ToString("0"));
            if (isEditExpire)
            {
                ExpireDate = exp;
            }
            else
            {
                ExpireDate = exp.AddDays(keeptime);
            }
        }

        private async void OnBarcodeClickedAsync()
        {
            var barcodelst = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode;
            if (barcodelst == null || barcodelst.Count == 0)
            {
                var chkDataa = await ValidationData(6);
                if (chkDataa)
                {
                    OnNogenbarcodeAsync();
                    var obj = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0];
                    SetCheckList(obj);
                }
            }
            else
            {
                var chkDataa = await ValidationData(6);
                if (chkDataa)
                {
                    var obj = MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode[0];
                    SetCheckList(obj);
                }
            }

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
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลเช็คลิสต์ผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลเช็คลิสต์ผิดพลาด กรุณาติดต่อ ICT\r\nError Message :" + ex.ToString());
            }
        }
        public void OnEnableSwitch()
        {
            var EmojiPattern = @"[#*0-9]\uFE0F?\u20E3|©\uFE0F?|[®\u203C\u2049\u2122\u2139\u2194-\u2199\u21A9\u21AA]\uFE0F?|[\u231A\u231B]|[\u2328\u23CF]\uFE0F?|[\u23E9-\u23EC]|[\u23ED-\u23EF]\uFE0F?|\u23F0|[\u23F1\u23F2]\uFE0F?|\u23F3|[\u23F8-\u23FA\u24C2\u25AA\u25AB\u25B6\u25C0\u25FB\u25FC]\uFE0F?|[\u25FD\u25FE]|[\u2600-\u2604\u260E\u2611]\uFE0F?|[\u2614\u2615]|\u2618\uFE0F?|\u261D(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|[\u2620\u2622\u2623\u2626\u262A\u262E\u262F\u2638-\u263A\u2640\u2642]\uFE0F?|[\u2648-\u2653]|[\u265F\u2660\u2663\u2665\u2666\u2668\u267B\u267E]\uFE0F?|\u267F|\u2692\uFE0F?|\u2693|[\u2694-\u2697\u2699\u269B\u269C\u26A0]\uFE0F?|\u26A1|\u26A7\uFE0F?|[\u26AA\u26AB]|[\u26B0\u26B1]\uFE0F?|[\u26BD\u26BE\u26C4\u26C5]|\u26C8\uFE0F?|\u26CE|[\u26CF\u26D1\u26D3]\uFE0F?|\u26D4|\u26E9\uFE0F?|\u26EA|[\u26F0\u26F1]\uFE0F?|[\u26F2\u26F3]|\u26F4\uFE0F?|\u26F5|[\u26F7\u26F8]\uFE0F?|\u26F9(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?|\uFE0F(?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\u26FA\u26FD]|\u2702\uFE0F?|\u2705|[\u2708\u2709]\uFE0F?|[\u270A\u270B](?:\uD83C[\uDFFB-\uDFFF])?|[\u270C\u270D](?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|\u270F\uFE0F?|[\u2712\u2714\u2716\u271D\u2721]\uFE0F?|\u2728|[\u2733\u2734\u2744\u2747]\uFE0F?|[\u274C\u274E\u2753-\u2755\u2757]|\u2763\uFE0F?|\u2764(?:\u200D(?:\uD83D\uDD25|\uD83E\uDE79)|\uFE0F(?:\u200D(?:\uD83D\uDD25|\uD83E\uDE79))?)?|[\u2795-\u2797]|\u27A1\uFE0F?|[\u27B0\u27BF]|[\u2934\u2935\u2B05-\u2B07]\uFE0F?|[\u2B1B\u2B1C\u2B50\u2B55]|[\u3030\u303D\u3297\u3299]\uFE0F?|\uD83C(?:[\uDC04\uDCCF]|[\uDD70\uDD71\uDD7E\uDD7F]\uFE0F?|[\uDD8E\uDD91-\uDD9A]|\uDDE6\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF]|\uDDE7\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF]|\uDDE8\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF5\uDDF7\uDDFA-\uDDFF]|\uDDE9\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF]|\uDDEA\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA]|\uDDEB\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7]|\uDDEC\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE]|\uDDED\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA]|\uDDEE\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9]|\uDDEF\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5]|\uDDF0\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF]|\uDDF1\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE]|\uDDF2\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF]|\uDDF3\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF]|\uDDF4\uD83C\uDDF2|\uDDF5\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE]|\uDDF6\uD83C\uDDE6|\uDDF7\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC]|\uDDF8\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF]|\uDDF9\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF]|\uDDFA\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF]|\uDDFB\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA]|\uDDFC\uD83C[\uDDEB\uDDF8]|\uDDFD\uD83C\uDDF0|\uDDFE\uD83C[\uDDEA\uDDF9]|\uDDFF\uD83C[\uDDE6\uDDF2\uDDFC]|\uDE01|\uDE02\uFE0F?|[\uDE1A\uDE2F\uDE32-\uDE36]|\uDE37\uFE0F?|[\uDE38-\uDE3A\uDE50\uDE51\uDF00-\uDF20]|[\uDF21\uDF24-\uDF2C]\uFE0F?|[\uDF2D-\uDF35]|\uDF36\uFE0F?|[\uDF37-\uDF7C]|\uDF7D\uFE0F?|[\uDF7E-\uDF84]|\uDF85(?:\uD83C[\uDFFB-\uDFFF])?|[\uDF86-\uDF93]|[\uDF96\uDF97\uDF99-\uDF9B\uDF9E\uDF9F]\uFE0F?|[\uDFA0-\uDFC1]|\uDFC2(?:\uD83C[\uDFFB-\uDFFF])?|[\uDFC3\uDFC4](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDFC5\uDFC6]|\uDFC7(?:\uD83C[\uDFFB-\uDFFF])?|[\uDFC8\uDFC9]|\uDFCA(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDFCB\uDFCC](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?|\uFE0F(?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDFCD\uDFCE]\uFE0F?|[\uDFCF-\uDFD3]|[\uDFD4-\uDFDF]\uFE0F?|[\uDFE0-\uDFF0]|\uDFF3(?:\u200D(?:\u26A7\uFE0F?|\uD83C\uDF08)|\uFE0F(?:\u200D(?:\u26A7\uFE0F?|\uD83C\uDF08))?)?|\uDFF4(?:\u200D\u2620\uFE0F?|\uDB40\uDC67\uDB40\uDC62\uDB40(?:\uDC65\uDB40\uDC6E\uDB40\uDC67|\uDC73\uDB40\uDC63\uDB40\uDC74|\uDC77\uDB40\uDC6C\uDB40\uDC73)\uDB40\uDC7F)?|[\uDFF5\uDFF7]\uFE0F?|[\uDFF8-\uDFFF])|\uD83D(?:[\uDC00-\uDC07]|\uDC08(?:\u200D\u2B1B)?|[\uDC09-\uDC14]|\uDC15(?:\u200D\uD83E\uDDBA)?|[\uDC16-\uDC3A]|\uDC3B(?:\u200D\u2744\uFE0F?)?|[\uDC3C-\uDC3E]|\uDC3F\uFE0F?|\uDC40|\uDC41(?:\u200D\uD83D\uDDE8\uFE0F?|\uFE0F(?:\u200D\uD83D\uDDE8\uFE0F?)?)?|[\uDC42\uDC43](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC44\uDC45]|[\uDC46-\uDC50](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC51-\uDC65]|[\uDC66\uDC67](?:\uD83C[\uDFFB-\uDFFF])?|\uDC68(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|[\uDC68\uDC69]\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|\uD83E[\uDDAF-\uDDB3\uDDBC\uDDBD])|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFC-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFE]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?))?|\uDC69(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?[\uDC68\uDC69]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|\uDC69\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|\uD83E[\uDDAF-\uDDB3\uDDBC\uDDBD])|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF]|\uDC8B\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF])|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFC-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF]|\uDC8B\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF])|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB\uDFFD-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF]|\uDC8B\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF])|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF]|\uDC8B\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF])|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFD\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF]|\uDC8B\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFF])|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFE]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?))?|\uDC6A|[\uDC6B-\uDC6D](?:\uD83C[\uDFFB-\uDFFF])?|\uDC6E(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDC6F(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDC70\uDC71](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDC72(?:\uD83C[\uDFFB-\uDFFF])?|\uDC73(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDC74-\uDC76](?:\uD83C[\uDFFB-\uDFFF])?|\uDC77(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDC78(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC79-\uDC7B]|\uDC7C(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC7D-\uDC80]|[\uDC81\uDC82](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDC83(?:\uD83C[\uDFFB-\uDFFF])?|\uDC84|\uDC85(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC86\uDC87](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDC88-\uDC8E]|\uDC8F(?:\uD83C[\uDFFB-\uDFFF])?|\uDC90|\uDC91(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC92-\uDCA9]|\uDCAA(?:\uD83C[\uDFFB-\uDFFF])?|[\uDCAB-\uDCFC]|\uDCFD\uFE0F?|[\uDCFF-\uDD3D]|[\uDD49\uDD4A]\uFE0F?|[\uDD4B-\uDD4E\uDD50-\uDD67]|[\uDD6F\uDD70\uDD73]\uFE0F?|\uDD74(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|\uDD75(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?|\uFE0F(?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDD76-\uDD79]\uFE0F?|\uDD7A(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD87\uDD8A-\uDD8D]\uFE0F?|\uDD90(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|[\uDD95\uDD96](?:\uD83C[\uDFFB-\uDFFF])?|\uDDA4|[\uDDA5\uDDA8\uDDB1\uDDB2\uDDBC\uDDC2-\uDDC4\uDDD1-\uDDD3\uDDDC-\uDDDE\uDDE1\uDDE3\uDDE8\uDDEF\uDDF3\uDDFA]\uFE0F?|[\uDDFB-\uDE2D]|\uDE2E(?:\u200D\uD83D\uDCA8)?|[\uDE2F-\uDE34]|\uDE35(?:\u200D\uD83D\uDCAB)?|\uDE36(?:\u200D\uD83C\uDF2B\uFE0F?)?|[\uDE37-\uDE44]|[\uDE45-\uDE47](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDE48-\uDE4A]|\uDE4B(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDE4C(?:\uD83C[\uDFFB-\uDFFF])?|[\uDE4D\uDE4E](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDE4F(?:\uD83C[\uDFFB-\uDFFF])?|[\uDE80-\uDEA2]|\uDEA3(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDEA4-\uDEB3]|[\uDEB4-\uDEB6](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDEB7-\uDEBF]|\uDEC0(?:\uD83C[\uDFFB-\uDFFF])?|[\uDEC1-\uDEC5]|\uDECB\uFE0F?|\uDECC(?:\uD83C[\uDFFB-\uDFFF])?|[\uDECD-\uDECF]\uFE0F?|[\uDED0-\uDED2\uDED5-\uDED7]|[\uDEE0-\uDEE5\uDEE9]\uFE0F?|[\uDEEB\uDEEC]|[\uDEF0\uDEF3]\uFE0F?|[\uDEF4-\uDEFC\uDFE0-\uDFEB])|\uD83E(?:\uDD0C(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD0D\uDD0E]|\uDD0F(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD10-\uDD17]|[\uDD18-\uDD1C](?:\uD83C[\uDFFB-\uDFFF])?|\uDD1D|[\uDD1E\uDD1F](?:\uD83C[\uDFFB-\uDFFF])?|[\uDD20-\uDD25]|\uDD26(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDD27-\uDD2F]|[\uDD30-\uDD34](?:\uD83C[\uDFFB-\uDFFF])?|\uDD35(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDD36(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD37-\uDD39](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDD3A|\uDD3C(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDD3D\uDD3E](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDD3F-\uDD45\uDD47-\uDD76]|\uDD77(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD78\uDD7A-\uDDB4]|[\uDDB5\uDDB6](?:\uD83C[\uDFFB-\uDFFF])?|\uDDB7|[\uDDB8\uDDB9](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDDBA|\uDDBB(?:\uD83C[\uDFFB-\uDFFF])?|[\uDDBC-\uDDCB]|[\uDDCD-\uDDCF](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDDD0|\uDDD1(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1|[\uDDAF-\uDDB3\uDDBC\uDDBD]))|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFC-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB\uDFFD-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB-\uDFFD\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB-\uDFFE]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?))?|[\uDDD2\uDDD3](?:\uD83C[\uDFFB-\uDFFF])?|\uDDD4(?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|\uDDD5(?:\uD83C[\uDFFB-\uDFFF])?|[\uDDD6-\uDDDD](?:\u200D[\u2640\u2642]\uFE0F?|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F?)?)?|[\uDDDE\uDDDF](?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDDE0-\uDDFF\uDE70-\uDE74\uDE78-\uDE7A\uDE80-\uDE86\uDE90-\uDEA8\uDEB0-\uDEB6\uDEC0-\uDEC2\uDED0-\uDED6])";
            LotNo_Group = Regex.Replace(_lotNo_Group, EmojiPattern, "");
            Maxlenlot = ((MaxlenLotGrroup + 5) - LotNo_Group.Length) - 1;
            IsGenLotNo = false;
            if (string.IsNullOrEmpty(LotNo_Group))
            {
                IsSwitchLot = false;
            }
            else
            {
                IsSwitchLot = true;
            }

            if (IsCancelEdit)
            {
                MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>();
            }
        }
        public void OnChangeUnit()
        {
            IsGenLotNo = false;

            if (IsCancelEdit)
            {
                MaterialReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode = new List<MaterialReceivePlanDtlBarcode>();
            }
        }

        private void OnRePrintAsync()
        {
            RePrintAsync();
        }

        private async Task RePrintAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var mstlst = new MaterialReceivePlanMst();
                    DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    if (MaterialReceivePlanMst.receiveSite != MaterialReceivePlanMst.ownerSite)
                    {
                        MaterialReceivePlanMst = await _materialAutoReceivingService.SaveRecieving(MaterialReceivePlanMst);
                    }
                    mstlst = await _materialAutoReceivingService.GetReturnMaterialReceivePlanDtl(MaterialReceivePlanMst);
                    IsBusy = false;
                    var chkPrint = await ValidationData(4);
                    if (chkPrint)
                    {
                        PrintLabel(mstlst);
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
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<App, bool>(this, MessagingConstants.Closeapp,
               (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
            //MessagingCenter.Subscribe<ExtReceivingAddEditView, bool>(this, MessagingConstants.ReExternal,
            //   (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
        }
        public async void OnRefreshExPageAsync(bool expage)
        {
            if (Convert.ToInt32(_settingsService.ReceiveMstIdSetting) == MaterialReceivePlanMst.Id && Convert.ToInt32(_settingsService.ReceiveDtlIdSetting) == MaterialReceivePlanMst.receivePlanDtl[0].Id)
            {
                if ((expage == true && IsCancelEdit == true) && (Ischecklist == false))
                {
                    try
                    {
                        IsCancelEdit = false;
                        IsHideEdit = true;
                        IsRePrint = true;
                        IsLockEntryEdit = false;
                        LockBtn = false;
                        LockEditData = false;
                        Ischecklist = false;
                        _modeEdit = true;
                        var lstdatabarcode = new List<MaterialReceivePlanDtlBarcode>();
                        if (KeepDtlPlan.Count > 0)
                        {
                            LotNo_Group = KeepDtlPlan[0].lotNo;
                            Qty = KeepDtlPlan[0].countQty;
                            SelectUnitCount = lstUnitCount.Where(z => z.unitCode == KeepDtlPlan[0].unitCode).FirstOrDefault();
                            WeightPerUnit = KeepDtlPlan[0].weightPerUnit;
                            TotalWeight = (decimal)KeepDtlPlan[0].ReceivePlanBarcode.Sum(x => x.qty);
                        }
                        if (KeepReceivePlanDtlBarcode.Count > 0)
                        {
                            lstdatabarcode = KeepReceivePlanDtlBarcode.Clone<List<MaterialReceivePlanDtlBarcode>>();
                            ReceivePlanDtlBarcode = lstdatabarcode.ToObservableCollection();
                            if (ReceivePlanDtlBarcode.Count > 1)
                            {
                                IsGenLotNo = true;
                            }
                        }
                        if (MaterialReceivePlanMst != null)
                        {
                            MaterialReceivePlanMst.receivePlanDtl = KeepDtlPlan;
                            MaterialReceivePlanMst.IseditStatus = false;
                            await _materialAutoReceivingService.GetUpdatermInMstStatus(MaterialReceivePlanMst);
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "ยกเลิกการแก้ไขข้อมูลแล้ว");
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
                Ischecklist = false;
            }
        }

    }
}
