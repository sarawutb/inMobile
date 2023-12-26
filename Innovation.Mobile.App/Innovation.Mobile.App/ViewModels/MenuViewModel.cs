using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Enumerations;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MainMenuItem> _menuItems, _menuHomeItems;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggingService _loggingService;

        public MenuViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService
            , ISettingsService settingService, IAuthenticationService authenticationService,
            ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _authenticationService = authenticationService;
            _loggingService = loggingService;
            MenuItems = new ObservableCollection<MainMenuItem>();
            MenuHomeItems = new ObservableCollection<MainMenuItem>();
            Task.Run(async () =>
            {
                await LoadMenuItemsAsync();
            });
        }

        private string _UserName;
        public string UserName
        {
            get => _settingsService.UserFullNameSetting;
            set
            {
                _UserName = value;
                OnPropertyChanged();
            }
        }
        private string _SiteName;
        public string SiteName
        {
            get => _settingsService.SiteNameSetting;
            set
            {
                _SiteName = value;
                OnPropertyChanged(nameof(SiteName));
            }
        }
        private string _PrinterName;
        public string PrinterName
        {
            get => _settingsService.PrintNameFormSetting;
            set
            {
                _PrinterName = value;
                OnPropertyChanged(nameof(PrinterName));
            }
        }
        public ICommand MenuItemTappedCommand => new Command(OnMenuItemTapped);
        public ICommand LogoutCommand => new Command(OnLogout);

        private void OnLogout()
        {
            _dialogService.DialogYesOrNo(MessagingConstants.NotifySystem, "แน่ใจว่าต้องการออกจากระบบ ?", () =>
            {
                UserLogoutAsync();
            });
        }

        private async Task UserLogoutAsync()
        {
            _settingsService.UserIdSetting = null;
            _settingsService.UserNameSetting = null;
            _settingsService.SiteIdSetting = null;
            _settingsService.SiteNameSetting = null;
            _settingsService.PrintIPAdressFormSetting = null;
            _settingsService.PrintPortFormSetting = null;
            _settingsService.PrintNameFormSetting = null;
            _settingsService.TokenSetting = null;
            Device.BeginInvokeOnMainThread(() =>
            {
                _navigationService.ClearBackStack();
            });

            await DependencyService.Get<ILoadingService>().Loading(() => { }, false, TimeoutLoading: 250);
            await _navigationService.NavigateToAsync<LoginViewModel>();
        }

        public ObservableCollection<MainMenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MainMenuItem> MenuHomeItems
        {
            get => _menuHomeItems;
            set
            {
                _menuHomeItems = value;
                OnPropertyChanged();
            }
        }

        private void OnMenuItemTapped(object menuItemTappedEventArgs)
        {
            var menuItem = ((menuItemTappedEventArgs as ItemTappedEventArgs)?.Item as MainMenuItem);

            if (menuItem != null && menuItem.MenuText == "Log out")
            {
                _settingsService.UserIdSetting = null;
                _settingsService.UserNameSetting = null;
                _settingsService.SiteIdSetting = null;
                _settingsService.SiteNameSetting = null;
                _settingsService.TokenSetting = null;
                _navigationService.ClearBackStack();
            }
            else
            {
                MessagingCenter.Send(this, MessagingConstants.ReAgainPage, true);
            }

            var type = menuItem?.ViewModelToLoad;
            _navigationService.NavigateToAsync(type);
        }

        public async Task LoadMenuItemsAsync()
        {
            await CheckmenudataAsync();
            MenuItems.Insert(0, new MainMenuItem
            {
                MenuText = "Home",
                ViewModelToLoad = typeof(MainViewModel),
                MenuItemType = MenuItemType.Home,
                MenuIcon = MenuItemModel.Home.IconName
            });

            //MenuItems.Add(new MainMenuItem
            //{
            //    MenuText = "Log out",
            //    ViewModelToLoad = typeof(LoginViewModel),
            //    MenuItemType = MenuItemType.Logout,
            //    MenuIcon = MenuItemModel.LogOut.IconName
            //});

        }
        private async Task CheckmenudataAsync()
        {
            try
            {

                if (!string.IsNullOrEmpty(_settingsService.UserNameSetting) && !string.IsNullOrEmpty(_settingsService.PasswordSetting))
                {
                    if (_settingsService.UserSetting == null)
                    {
                        IsBusy = true;
                        var LoginVM = new LoginVm
                        {
                            Username = _settingsService.UserNameSetting,
                            Password = _settingsService.PasswordSetting,
                        };
                        _settingsService.UserSetting = await _authenticationService.CheckPermissionAsync(LoginVM);
                        IsBusy = false;
                    }
                }
                else
                {
                    UserLogoutAsync();
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การดาวน์โหลดข้อมูลผิดพลาด กรุณาติดต่อ ICT\r\nError Message: " + e.ToString());

            }
            if (_settingsService.UserSetting != null)
            {
                if (_settingsService.UserSetting.ProgramPermission != null)
                {
                    var programList = _settingsService.UserSetting.ProgramPermission.Modules;
                    programList.ForEach(menu =>
                    {
                        var MenuModel = GetViewModelFromName(menu.ModuleCode);
                        if (MenuModel != null)
                        {
                            MenuItems.Add(MenuModel);
                            MenuHomeItems.Add(MenuModel);
                        }
                    });
                }
            }
        }

        protected MainMenuItem GetViewModelFromName(string nameView)
        {
            switch (nameView)
            {
                case nameof(ReceivingListViewModel):
                    return new MainMenuItem
                    {
                        MenuText = MenuItemModel.Receiving.Name,
                        MenuIcon = MenuItemModel.Receiving.IconName,
                        ViewModelToLoad = typeof(ReceivingListViewModel),
                        MenuItemType = MenuItemType.Receiving
                    };
                case nameof(PickingListViewModel):
                    return new MainMenuItem
                    {
                        MenuText = MenuItemModel.Picking.Name,
                        MenuIcon = MenuItemModel.Picking.IconName,
                        ViewModelToLoad = typeof(PickingListViewModel),
                        MenuItemType = MenuItemType.Picking
                    };
                case nameof(QualityCheckListViewModel):
                    return new MainMenuItem
                    {
                        MenuText = MenuItemModel.QualityCheck.Name,
                        MenuIcon = MenuItemModel.QualityCheck.IconName,
                        ViewModelToLoad = typeof(QualityCheckListViewModel),
                        MenuItemType = MenuItemType.QualityCheck
                    };
                default: return null;
            }
        }
    }
}
