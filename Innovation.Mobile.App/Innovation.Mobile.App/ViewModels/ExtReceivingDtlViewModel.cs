using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Views;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Repository.Interface.Service;

namespace Innovation.Mobile.App.ViewModels
{
    public class ExtReceivingDtlViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private MaterialReceivePlanMst _selectReceivePlanMst;
        private List<MaterialReceiveGroupLotNoVM> _lstLotNoByRM;
        private List<MaterialReceivePlanDtlBarcode> _receivePlanDtlBarcode;
        private string _rmTitle;
        public ExtReceivingDtlViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService)
        {
            _settingsService = settingsService;
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService=loggingService;
            InitializeMessenger();
        }

        public ICommand RMLotNoTappedCommand => new Command<MaterialReceiveGroupLotNoVM>(OnRMLotNoTapped);
        public ICommand AddRMLotNoCommand => new Command(OnAddRMLotNoAsync);
        
        public MaterialReceivePlanMst selectReceivePlanMst
        {
            get => _selectReceivePlanMst;
            set
            {
                _selectReceivePlanMst = value;
                OnPropertyChanged();
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

        public string RMTitle
        {
            get => _rmTitle;
            set
            {
                _rmTitle = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object ReceivePlanMst)
        {
            try
            {
                selectReceivePlanMst = (MaterialReceivePlanMst)ReceivePlanMst;
                _settingsService.ReceiveDtlIdSetting = selectReceivePlanMst.receivePlanDtl[0].Id.ToString();
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                RMTitle = selectReceivePlanMst.receivePlanDtl[0].rmId + " " + selectReceivePlanMst.receivePlanDtl[0].rmName;
                GetLotNoByRMID();
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            }
            catch(Exception e)
            {
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
            
        }

        private void GetLotNoByRMID()
        {
            var checkbarcode = _selectReceivePlanMst.receivePlanDtl.Where(x => x.ReceivePlanBarcode.Count > 0).ToList();
            if(checkbarcode.Count > 0)
            {
                var dtllst = _selectReceivePlanMst.receivePlanDtl.FirstOrDefault();
                var word = dtllst.rmId.Substring(0, 1);
                if(word == "I"||word =="S")
                {
                    lstLotNoByRM = (from a in _selectReceivePlanMst.receivePlanDtl
                                    select new MaterialReceiveGroupLotNoVM()
                                    {
                                        rmId = a.rmId,
                                        rmName = a.rmName,
                                        firstReceiveDate = a.ReceivePlanBarcode.Count > 0 ? a.firstReceiveDate : null,
                                        LotNo_Group = a.lotNo != null ? a.lotNo : "",
                                        countQty = a.ReceivePlanBarcode.Count == 0 ? a.countQty : a.ReceivePlanBarcode.Count(),
                                        weightPerUnit = a.weightPerUnit,
                                        SumWeight = (decimal)(a.countQty > 0 ? a.ReceivePlanBarcode.Sum(x => x.qty) : 0),
                                        IsCheck = !string.IsNullOrWhiteSpace(_selectReceivePlanMst.inId),
                                        LabelColor = a.colorCode,
                                        PI_Lot_NO = a.ReceivePlanBarcode[0].piLotNo
                                    }).ToList();
                }
                else
                {
                    lstLotNoByRM = (from a in _selectReceivePlanMst.receivePlanDtl
                                    select new MaterialReceiveGroupLotNoVM()
                                    {
                                        rmId = a.rmId,
                                        rmName = a.rmName,
                                        firstReceiveDate = a.ReceivePlanBarcode.Count > 0 ? a.firstReceiveDate : null,
                                        LotNo_Group = a.lotNo != null ? a.lotNo : "",
                                        countQty = a.ReceivePlanBarcode.Count == 1 ? a.countQty : a.ReceivePlanBarcode.Count(),
                                        weightPerUnit = a.weightPerUnit,
                                        SumWeight = (decimal)(a.countQty > 0 ? a.ReceivePlanBarcode.Sum(x => x.qty) : 0),
                                        IsCheck = !string.IsNullOrWhiteSpace(_selectReceivePlanMst.inId),
                                        LabelColor = a.colorCode,
                                    }).ToList();
                }
            }
            else
            {
                lstLotNoByRM = null;
            }
        }
        private async Task<bool> ValidationData(int mode)
        {
            bool retval = true;
            List<int> sitecheck = new List<int>() 
            {
               1,5
            };
            if (mode == 1)
            {
                if(selectReceivePlanMst != null)
                {
                    if (selectReceivePlanMst.ownerSite.Value != selectReceivePlanMst.receiveSite)
                    {
                        //if((selectReceivePlanMst.ownerSite.Value != 1 &&  selectReceivePlanMst.receiveSite != 5)
                        //    ||((selectReceivePlanMst.ownerSite.Value != 5 && selectReceivePlanMst.receiveSite != 1)))
                        if(!sitecheck.Contains(selectReceivePlanMst.ownerSite.Value) && !sitecheck.Contains(selectReceivePlanMst.receiveSite))
                        {
                            try
                            {
                                var maprm = (await _materialAutoReceivingService.GetMaterialProductionCrossSiteAsync(selectReceivePlanMst.receivePlanDtl[0].rmId, selectReceivePlanMst.ownerSite.Value, selectReceivePlanMst.receiveSite)).ToList();
                                if (maprm.Count < 1)
                                {
                                    retval = false;
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบการ Map วัตถุดิบ โค้ด: " + selectReceivePlanMst.suppCode + "(RM Supplier) ที่ฝั่งผลิต กรุณา Map วัตถุดิบก่อน");
                                }
                            }
                            catch (HttpRequestExceptionEx e)
                            {
                                retval = false;
                                _loggingService.Error(e.Message);
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่พบข้อมูลรายการ กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                            }
                            catch (Exception e)
                            {
                                retval = false;
                                _loggingService.Error(e.ToString());
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่พบข้อมูลรายการ กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                            }
                        }
                       
                    }
                }
                else
                {
                    retval = false;
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดรายการผิดพลาด กรุณาทำรายการเพิ่มรับเข้าอีกครั้ง");
                }

            }

            return retval;
        }
        private async void OnRMLotNoTapped(MaterialReceiveGroupLotNoVM selectLotNo)
        {
            var mst = _selectReceivePlanMst.Clone<MaterialReceivePlanMst>();
            var dtl = new List<MaterialReceivePlanDtl>(mst.receivePlanDtl);
            var lstdtl = dtl.Where(x=>x.lotNo == selectLotNo.LotNo_Group).ToList();
            if(lstdtl.Count > 0)
            {
                mst.receivePlanDtl = lstdtl;
                var wrod = dtl.Select(x => x.rmId).ToList();
                var substrword = wrod[0].Substring(0, 1);
                if (substrword == "I" || substrword == "S")
                {
                    var dtlbar  = mst.receivePlanDtl.SelectMany(x=>x.ReceivePlanBarcode).ToList();
                    var selectdtl = dtlbar.Where(x => x.piLotNo == selectLotNo.PI_Lot_NO).ToList();
                    var dtllst = mst.receivePlanDtl.Where(x => x.ReceivePlanBarcode.Count == selectdtl.Count).FirstOrDefault();
                    dtllst.ReceivePlanBarcode = selectdtl;
                    var dtllsts = new List<MaterialReceivePlanDtl>();
                    dtllsts.Add(dtllst);
                    mst.receivePlanDtl = dtllsts;
                   await _navigationService.NavigateToAsync<ExtReceivingCompDtlViewModel>(mst);
                }
                else
                {
                    await _navigationService.NavigateToAsync<ExtReceivingAddEditViewModel>(mst);
                }
            }
            else
            {
                 DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่พบข้อมูลรายการ");
            }
            
        }
        private async void OnAddRMLotNoAsync()
        {
            var chkData = await ValidationData(1);
            if (chkData)
            {
                if (_selectReceivePlanMst != null)
                {
                    var lstbarcode = _selectReceivePlanMst.receivePlanDtl[0].ReceivePlanBarcode.Count != 0;
                    var wrod = _selectReceivePlanMst.receivePlanDtl.Select(x => x.rmId).ToList();
                    var substrword = wrod[0].Substring(0, 1);
                    if (lstbarcode)
                    {
                        var mstdata = _selectReceivePlanMst.Clone<MaterialReceivePlanMst>();
                        var dtldata = new List<MaterialReceivePlanDtl>();
                        dtldata.Add(new MaterialReceivePlanDtl()
                        {
                            Id = _selectReceivePlanMst.receivePlanDtl[0].Id,
                            state = Enumerations.StateData.State.Insert,
                            rmId = _selectReceivePlanMst.receivePlanDtl[0].rmId,
                            rmName = _selectReceivePlanMst.receivePlanDtl[0].rmName,
                            reviseBy = Convert.ToInt32(_settingsService.UserIdSetting),
                            receivePlanMstId = _selectReceivePlanMst.Id,
                            accountDate = _selectReceivePlanMst.receivePlanDtl[0].accountDate,
                            typeRM = _selectReceivePlanMst.receivePlanDtl[0].typeRM,
                            RequestDtlID = _selectReceivePlanMst.receivePlanDtl[0].RequestDtlID,
                            receivingWeight = _selectReceivePlanMst.receivePlanDtl[0].receivingWeight,
                            unitCode = _selectReceivePlanMst.receivePlanDtl[0].unitCode,
                            weightPerUnit = _selectReceivePlanMst.receivePlanDtl[0].weightPerUnit,
                            keepTime = _selectReceivePlanMst.receivePlanDtl[0].keepTime
                        });
                        mstdata.receivePlanDtl = dtldata;
                        if (substrword == "I" || substrword == "S")
                        {
                            await _navigationService.NavigateToAsync<ExtReceivingCompDtlViewModel>(mstdata);
                        }
                        else
                        {
                            await _navigationService.NavigateToAsync<ExtReceivingAddEditViewModel>(mstdata);
                        }
                    }
                    else
                    {
                        var selectmst = _selectReceivePlanMst.Clone<MaterialReceivePlanMst>();
                        selectmst.receivePlanDtl[0].state = Enumerations.StateData.State.Update;
                        selectmst.receivePlanDtl[0].reviseBy = int.Parse(_settingsService.UserIdSetting);
                        if (substrword == "I" || substrword == "S")
                        {
                            await _navigationService.NavigateToAsync<ExtReceivingCompDtlViewModel>(selectmst);
                        }
                        else
                        {
                            await _navigationService.NavigateToAsync<ExtReceivingAddEditViewModel>(selectmst);
                        }
                    }
                }
                else
                {
                    DialogshowErrorAsync();
                }
            }
        }
        public async void DialogshowErrorAsync()
        {
            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลผิดพลาด กรุณาเข้ารายการใหม่");
            await _navigationService.NavigateBackAsync();
        }
        public void InitializeMessenger()
        {
            //MessagingCenter.Subscribe<InternalReceivingViewModel, bool>(this, MessagingConstants.ReExternal,
            //    (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
            //MessagingCenter.Subscribe<InternalReceivingView, bool>(this, MessagingConstants.ReExternal,
            //   (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
            MessagingCenter.Subscribe<ExtReceivingAddEditView, bool>(this, MessagingConstants.ReExternal,
               (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
            MessagingCenter.Subscribe<ExternalReceivingViewModel, bool>(this, MessagingConstants.ReExternal,
              (internalReceivingViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
            MessagingCenter.Subscribe<ExtReceivingCompDtlView, bool>(this, MessagingConstants.ReExternal,
              (extReceivingCompDtlViewModel, repageExternal) => OnRefreshExPageAsync(repageExternal));
        }
        public async void OnRefreshExPageAsync(bool expage)
        {
           if(Convert.ToInt32(_settingsService.ReceiveMstIdSetting) == selectReceivePlanMst.Id && Convert.ToInt32(_settingsService.ReceiveDtlIdSetting) == selectReceivePlanMst.receivePlanDtl[0].Id)
            {
                if (expage)
                {
                    if (_selectReceivePlanMst.typeSuplierId != 1)
                    {
                        try
                        {
                            var dtllstdata = new List<MaterialReceivePlanDtl>();
                            DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                            dtllstdata = (await _materialAutoReceivingService.GetMaterialReceivePlanDtl(selectReceivePlanMst.Id, selectReceivePlanMst.receivePlanDtl[0].Id)).ToList();
                            if (dtllstdata.Count > 0)
                            {
                                _selectReceivePlanMst.receivePlanDtl = dtllstdata;
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                GetLotNoByRMID();
                            }
                            else
                            {
                                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "โหลดข้อมูลผิดพลาด กรุณาลองใหม่");
                            }
                        }
                        catch (HttpRequestExceptionEx e)
                        {
                            _loggingService.Error(e.Message);
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                        }
                        catch (Exception e)
                        {
                            _loggingService.Error(e.ToString());
                            DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                        }

                    }
                }
            }
        }
    }
}
