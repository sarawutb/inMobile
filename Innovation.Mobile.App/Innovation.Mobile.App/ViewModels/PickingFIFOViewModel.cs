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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class PickingFIFOViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private bool _isShowFIFO, _isRefreshing;
        private MaterialPickingDtlVM _materialPickingDtl;
        private string _title;
        private ObservableCollection<RMCompaFIFOVM> _lstRMCompaFIFO;
        private List<RMCompaFIFOVM> _lstRMFIFO;
        public PickingFIFOViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService,
            IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService=loggingService;
        }
        public ICommand RefreshCommand => new Command(OnRefresh);
        public override async Task InitializeAsync(object PickingDtl)
        {
            MaterialPickingDtl = (MaterialPickingDtlVM)PickingDtl;
            Title = _materialPickingDtl.RmId + " " + _materialPickingDtl.RmName;

            try
            {
                IsBusy = true;
                DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();

                lstRMCompaFIFO = (await _materialAutoReceivingService.GetRMCompaFIFOAsync(_materialPickingDtl.RmId)).ToObservableCollection();
                ConvertDateFIFO();

                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                IsBusy = false;
            }
            catch (HttpRequestExceptionEx e)
            {
                IsBusy = false;
                _loggingService.Error(e.Message);
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);

            }
            catch (Exception e)
            {
                IsBusy = false;
                _loggingService.Error(e.ToString());
                DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
            }
        }

        private void ConvertDateFIFO()
        {
            List<RMCompaFIFOVM> retval = new List<RMCompaFIFOVM>();
            foreach (var item in _lstRMCompaFIFO)
            {
                retval.Add(new RMCompaFIFOVM()
                {
                    RMID = item.RMID,
                    RM_Name = item.RM_Name,
                    Barcode = item.Barcode,
                    BatchNo = item.BatchNo,
                    LotNo = item.LotNo,
                    ExpireDate = item.ExpireDate.Substring(6, 2) + "/" + item.ExpireDate.Substring(4, 2) + "/" + item.ExpireDate.Substring(0, 4),
                    Balance = item.Balance,
                    WH_Name = item.WH_Name,
                    Location_Name = item.Location_Name,
                    QA_Status = item.Lock ? "ล็อค" : "ไม่ล็อค"
                });
            }
            lstRMFIFO = retval;
        }
        private async void OnRefresh(object obj)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    IsRefreshing = true;
                    lstRMCompaFIFO = (await _materialAutoReceivingService.GetRMCompaFIFOAsync(_materialPickingDtl.RmId)).ToObservableCollection();
                    ConvertDateFIFO();
                    IsRefreshing = false;
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
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
        public bool IsShowFIFO
        {
            get => _isShowFIFO;
            set
            {
                _isShowFIFO = value;
                OnPropertyChanged();
            }
        }
        public MaterialPickingDtlVM MaterialPickingDtl
        {
            get => _materialPickingDtl;
            set
            {
                _materialPickingDtl = value;
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
        public ObservableCollection<RMCompaFIFOVM> lstRMCompaFIFO
        {
            get => _lstRMCompaFIFO;
            set
            {
                _lstRMCompaFIFO = value;
                OnPropertyChanged();
            }
        }
        public List<RMCompaFIFOVM> lstRMFIFO
        {
            get => _lstRMFIFO;
            set
            {
                _lstRMFIFO = value;
                OnPropertyChanged();
            }
        }
    }
}
