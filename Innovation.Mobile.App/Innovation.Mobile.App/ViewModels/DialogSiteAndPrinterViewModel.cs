using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Data;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views.Widget.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Innovation.Mobile.App.ViewModels
{
    public class SiteAndPrinter
    {
        public Printer_Profile printerProfile { get; set; }
        public SiteProfile siteProfile { get; set; }
    }
    public class DialogSiteAndPrinterViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private SiteProfile _SiteProfile;
        private List<Printer_Profile> _LstPrinterProfile = new List<Printer_Profile>();
        private Printer_Profile _PrinterProfile;

        private SiteAndPrinter _siteAndPrinter;
        public DialogSiteAndPrinterViewModel(
            IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            IAuthenticationService authenticationService) : base(connectionService, navigationService, dialogService)
        {
            _authenticationService = authenticationService;
        }

        public event EventHandler<SiteAndPrinter> OnReturnSiteAndPrinterAsync;

        public SiteAndPrinter siteAndPrinter
        {
            get => _siteAndPrinter; set
            {
                _siteAndPrinter = value;
            }
        }
        public ICommand GetSiteAndPrinterCommand => new Command(OnRetrunModelAsync);
        public ICommand SelectSiteCommand => new Command(OnSelectSite);

        public DialogSiteAndPrinter _dialogSiteAndPrinter;
        public DialogSiteAndPrinter dialogSiteAndPrinter
        {
            get => _dialogSiteAndPrinter;
            set
            {
                _dialogSiteAndPrinter = value;
                OnPropertyChanged();
            }
        }

        public async void OnSelectSite()
        {
            try
            {
                LstPrinterProfile.Clear();
                if (_SiteProfile != null)
                {
                    LstPrinterProfile = await DependencyService.Get<ILoadingService>().Loading(_authenticationService.GetPrinterProfiles(_SiteProfile.Site_ID.ToString()));
                    if (LstPrinterProfile.Count < 1)
                        PrinterProfile = null;
                }
            }
            catch (Exception ex)
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, ex.Message);
            }
        }
        public List<SiteProfile> LstSiteProfile
        {
            get => _settingsService.UserSetting.UserOperationSite;
        }
        public SiteProfile SiteProfile
        {
            get => _SiteProfile;
            set
            {
                _SiteProfile = value;
                IsValid();
                OnPropertyChanged();
            }
        }
        public List<Printer_Profile> LstPrinterProfile
        {
            get => _LstPrinterProfile;
            set
            {
                _LstPrinterProfile = value.OrderBy(p => p.Printer_Name).ToList();
                OnPropertyChanged(nameof(LstPrinterProfile));
            }
        }
        public Printer_Profile PrinterProfile
        {
            get => _PrinterProfile;
            set
            {
                _PrinterProfile = value;
                IsValid();
                OnPropertyChanged();
            }
        }
        public bool _lsCommit = false;
        public bool IsCommit
        {
            get => _lsCommit;
            set
            {
                _lsCommit = value;
                OnPropertyChanged();
            }
        }
        public async void OnRetrunModelAsync()
        {
            await Close();
            OnReturnSiteAndPrinterAsync?.Invoke(this, new SiteAndPrinter
            {
                printerProfile = PrinterProfile,
                siteProfile = SiteProfile
            });
        }
        public Task<SiteAndPrinter> GetOnReturnSiteAndPrinterHandlerAsync()
        {
            var tcs = new TaskCompletionSource<SiteAndPrinter>();
            EventHandler<SiteAndPrinter> handler = null;
            handler = (sender, result) =>
            {
                OnReturnSiteAndPrinterAsync -= handler;
                tcs.SetResult(result);
            };
            OnReturnSiteAndPrinterAsync += handler;
            return tcs.Task;
        }
        public async Task<SiteAndPrinter> Show(DialogSiteAndPrinter page, bool IsRefresh)
        {
            dialogSiteAndPrinter = page;
            await _navigationService.NavigateModalAsync(dialogSiteAndPrinter, false);
            dialogSiteAndPrinter.IsRefresh(IsRefresh);
            return await GetOnReturnSiteAndPrinterHandlerAsync();
        }
        protected async Task Close()
        {
            await _navigationService.PopModalAsync();
        }
        void IsValid()
        {
            if (SiteProfile == null || PrinterProfile == null)
            {
                IsCommit = false;
            }
            else
            {
                IsCommit = true;
            }
        }
        public async Task SetCurrentSite()
        {
            await Task.Run(() =>
            {
                if (LstSiteProfile.Count > 0)
                    SiteProfile = LstSiteProfile.FirstOrDefault(p => p.Site_ID.ToString() == _settingsService.SiteIdSetting);
                if (LstPrinterProfile.Count > 0)
                    PrinterProfile = LstPrinterProfile.FirstOrDefault(p => p.Printer_IP_Address == _settingsService.PrintIPAdressFormSetting && p.Printer_Port == _settingsService.PrintPortFormSetting && p.Printer_Name == _settingsService.PrintNameFormSetting);
            });
        }
    }
}
