using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class ExternalReceivingViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private  ISettingsService _setting;
        private readonly ILoggingService _loggingService;
        private MaterialReceivePlanMst _selectReceivePlan;
        private ObservableCollection<MaterialReceivePlanDtl> _lstReceiveDtl;
        private string _rmTitle;
        private bool _taplock;
        public ExternalReceivingViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService setting, ILoggingService loggingService, IMaterialAutoReceivingService materialAutoReceivingService) : base(connectionService, navigationService, dialogService)
        {
            _setting = setting;
            _loggingService=loggingService;
            _materialAutoReceivingService = materialAutoReceivingService;
            InitializeMessenger();
        }

        public ICommand RMTappedCommand => new Command<MaterialReceivePlanDtl>(OnRMTapped);

        public string RMTitle
        {
            get => _rmTitle;
            set
            {
                _rmTitle = value;
                OnPropertyChanged();
            }
        }
        public bool Taplock
        {
            get => _taplock;
            set
            {
                _taplock = value;
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
        public ObservableCollection<MaterialReceivePlanDtl> lstReceiveDtl
        {
            get => _lstReceiveDtl;
            set
            {
                _lstReceiveDtl = value;
                OnPropertyChanged();
            }
        }

        private async void OnRMTapped(MaterialReceivePlanDtl selectRM)
        {
            if (!_taplock)
            {
                MaterialReceivePlanMst mst = selectReceivePlan.Clone<MaterialReceivePlanMst>();
                var lstdtl = mst.receivePlanDtl.Where(x => x.rmId == selectRM.rmId && x.Id == selectRM.Id && x.keyLink == selectRM.keyLink).ToList();
                mst.receivePlanDtl = lstdtl;
                Taplock = true;
                await _navigationService.NavigateToAsync<ExtReceivingDtlViewModel>(mst);
                MessagingCenter.Send(this, MessagingConstants.ReExternal, true);
            }
            //else
            //{
            //    await _dialogService.ShowDialog(
            //       "กรุณาลองใหม่",
            //       "Error",
            //       "ok"
            //       );
            //}
        }

        public override async Task InitializeAsync(object ReceivePlan)
        {

            selectReceivePlan = (MaterialReceivePlanMst)ReceivePlan;
            _setting.ReceiveMstIdSetting = selectReceivePlan.Id.ToString();
            RMTitle = selectReceivePlan.documentRequestNo;
            GetReceivePlanDtl();
        }

        private async void GetReceivePlanDtl()
        {
            try
            {
                IsBusy = true;
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                var lstDtl = await _materialAutoReceivingService.GetMaterialReceivePlanDtl((int)selectReceivePlan.Id, 0);
                lstReceiveDtl = (from a in lstDtl
                                 group a by new
                                 {
                                     a.Id,
                                     a.keyLink,
                                     a.rmId,
                                     a.rmName,
                                     a.receivingWeight,
                                     //a.receivePlanDtlStatus
                                 } into grp
                                 select new MaterialReceivePlanDtl
                                 {
                                     Id = grp.Key.Id,
                                     keyLink = grp.Key.keyLink,
                                     rmId = grp.Key.rmId,
                                     rmName = grp.Key.rmName,
                                     receivingWeight = grp.Key.receivingWeight,
                                     ColorStatusCode = lstDtl.Where(x=>x.rmId == grp.Key.rmId && x.Id == grp.Key.Id).SelectMany(x=>x.ReceivePlanBarcode).Where(x=>x.receivePlanDtlBarcodeStatus >= 2).Count() > 0  ? "#00FF04" : "#FFFFFF"
                           }).ToObservableCollection();

                //lstReceiveDtl = lstDtl.GroupBy(x => x.rmId).Select(z => z.s);

                selectReceivePlan.receivePlanDtl = new List<MaterialReceivePlanDtl>(lstDtl);

                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                IsBusy = false;
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
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<ExtReceivingDtlView, bool>(this, MessagingConstants.ReCieve,
               (extReceivingDtlView, recheck) => OnRefreshPage(recheck));
        }
        public void OnRefreshPage(bool repage)
        {
            if(Convert.ToInt32(_setting.ReceiveMstIdSetting) == selectReceivePlan.Id)
            {
                if (repage)
                {
                    GetReceivePlanDtl();
                    Taplock = false;
                }
            }
        }
    }
}
