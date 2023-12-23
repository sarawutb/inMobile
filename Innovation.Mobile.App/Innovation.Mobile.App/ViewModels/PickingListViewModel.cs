using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Extensions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class PickingListViewModel : ViewModelBase
    {
        private readonly IMaterialAutoReceivingService _materialAutoReceivingService;
        private readonly ILoggingService _loggingService;
        private string _requestno;
        private DateTime _dateStrat, _dateEnd; 
        private ObservableCollection<MaterialPickingListMstListVM> _pickingList;
        private bool _isRefreshing;
        private List<InventoryPurposeVM> _inventoryPurposeVMs;
        public PickingListViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService,
            IMaterialAutoReceivingService materialAutoReceivingService, ILoggingService loggingService)
             : base(connectionService, navigationService, dialogService)
        {
            _materialAutoReceivingService = materialAutoReceivingService;
            _loggingService=loggingService;
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            _pickingList = new ObservableCollection<MaterialPickingListMstListVM>();
            InitializeMessenger();
            InitializeViewModel();
            OnGetPickingPlan();
        }

        private void InitializeViewModel()
        {
            InventoryPurposeVMs = new List<InventoryPurposeVM>()
            {
                new InventoryPurposeVM()
                {
                    Id = 0,
                    Name_Local = "All"
                },
                new InventoryPurposeVM()
                {
                    Id = 1,
                    Name_Local = "Transfer To Production"
                },
                new InventoryPurposeVM()
                {
                    Id = 2,
                    Name_Local = "Transfer Location"
                },
            };
        }

        public ICommand SearchCommand => new Command(OnGetPickingPlan);
        public ICommand PickingPlanTappedCommand => new Command<MaterialPickingListMstListVM>(PickingPlanTapped);
        public ICommand RefreshCommand => new Command(OnRefresh);
        public ICommand PlanItemSelectedCommand => new Command<MaterialPickingListMstListVM>(PlanItemSelected);

        private void PlanItemSelected(MaterialPickingListMstListVM obj)
        {
            _navigationService.NavigateToAsync<PickingListDtlViewModel>(obj);
        }

        public DateTime DateStart
        {
            get => _dateStrat;
            set
            {
                _dateStrat = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateEnd
        {
            get => _dateEnd;
            set
            {
                _dateEnd = value;
                OnPropertyChanged();
            }
        }
        public string RquestNo
        {
            get => _requestno;
            set
            {
                _requestno = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<MaterialPickingListMstListVM> PickingList
        {
            get => _pickingList;
            set
            {
                _pickingList = value;
                OnPropertyChanged();
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

        public List<InventoryPurposeVM> InventoryPurposeVMs
        {
            get => _inventoryPurposeVMs;
            set
            {
                if (Equals(value, _inventoryPurposeVMs)) return;
                _inventoryPurposeVMs = value;
                OnPropertyChanged();
            }
        }

        private async void OnGetPickingPlan()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                    PickingList = (await _materialAutoReceivingService.GetMaterialPickingMstAsync(DateStart, DateEnd))
                        .ToObservableCollection();
                    await DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error,
                        "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error,
                        "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void PickingPlanTapped(MaterialPickingListMstListVM selectPickingMst)
        {
            _navigationService.NavigateToAsync<PickingListDtlViewModel>(selectPickingMst);
        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<PickingListDtlView, bool>(this, MessagingConstants.RePick,
               (pickingListDtlView, recheckPage) => OnRefreshPage(recheckPage));
            MessagingCenter.Subscribe<MenuViewModel, bool>(this, MessagingConstants.ReAgainPage,
               (MenuViewModel, recheckPage) => OnRefreshPage(recheckPage));
        }
        public void OnRefreshPage(bool repage)
        {
            if (repage)
            {
                OnGetPickingPlan();
            }
        }

        private async void OnRefresh(object obj)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    IsRefreshing = true;

                    PickingList = (await _materialAutoReceivingService.GetMaterialPickingMstAsync(DateStart, DateEnd)).ToObservableCollection();
                    
                    IsRefreshing = false;
                    IsBusy = false;
                }
                catch (HttpRequestExceptionEx e)
                {
                    _loggingService.Error(e.Message);
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.Message);
                }
                catch (Exception e)
                {
                    _loggingService.Error(e.ToString());
                    DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลรายการรับเข้าผิดพลาด กรุณาติดต่อ ICT\r\nError Message : " + e.ToString());
                }
            }
        }
    }
}
