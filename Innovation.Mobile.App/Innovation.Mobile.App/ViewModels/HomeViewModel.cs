using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class HomeViewModel : MenuViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMaterialAutoReceivingService _materialAutoReceiving;
        private readonly ILoggingService _loggingService;

        public HomeViewModel(IMaterialAutoReceivingService materialAutoReceiving, IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingService, IAuthenticationService authenticationService, ILoggingService loggingService) : base(connectionService, navigationService, dialogService, settingService, authenticationService, loggingService)
        {
            _materialAutoReceiving = materialAutoReceiving;
            _loggingService = loggingService;
            _authenticationService = authenticationService;
            _settingsService.CurrentFormSetting = typeof(HomeViewModel).Name;
        }

        public ICommand SelectSite => new Command(SelectSiteAsync);
        public async void SelectSiteAsync()
        {
            try
            {
                var SiteAndPrinter = await _dialogService.DialogSiteAndPrinter(false);
                if (SiteAndPrinter != null)
                {
                    _settingsService.SiteIdSetting = SiteAndPrinter.siteProfile.Site_ID.ToString();
                    _settingsService.SiteNameSetting = SiteAndPrinter.siteProfile.Site_Name.ToString();
                    _settingsService.PrintPortFormSetting = SiteAndPrinter.printerProfile.Printer_Port.ToString();
                    _settingsService.PrintIPAdressFormSetting = SiteAndPrinter.printerProfile.Printer_IP_Address.ToString(); ;
                    _settingsService.PrintNameFormSetting = SiteAndPrinter.printerProfile.Printer_Name.ToString();
                    SetDisplayPrintertMenu(SiteAndPrinter);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "บันทึกแล้ว");
                    AppContainer.Resolve<MainViewModel>().MenuViewModel = AppContainer.Resolve<MenuViewModel>();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, ex.Message);
            }
        }
        private void SetDisplayPrintertMenu(SiteAndPrinter siteAndPrinter)
        {
            PrinterName = siteAndPrinter.printerProfile.Printer_Name.ToString();
            SiteName = siteAndPrinter.siteProfile.Site_Name.ToString();
        }
    }
}
