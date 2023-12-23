using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Exceptions;
using Innovation.Mobile.App.Interfase.Service;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository.Interface.Service;
using Innovation.Mobile.App.Service.Genaral;
using Innovation.Mobile.App.Templates;
using Innovation.Mobile.App.ViewModels.Base;
using Innovation.Mobile.App.Views.Widget.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;


namespace Innovation.Mobile.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IMaterialAutoReceivingService _autoReceivingService;
        private readonly ILoggingService _loggingService;
        private IList<SiteProfile> _siteUser;
        private List<Printer_Profile> _printprofile;
        private Printer_Profile _printprofiles;
        private SiteProfile _sites;
        private string _userName, _versionname, _appcode, _packageName;
        private string _password;
        private int _site_ID;
        private bool lockprint, locklogin, _isupdate;

        public LoginViewModel(
            IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISettingsService settingsService,
            IAuthenticationService authenticationService,
            IMaterialAutoReceivingService autoReceivingService,
            ILoggingService loggingService)
            : base(connectionService, navigationService, dialogService)
        {
            _autoReceivingService = autoReceivingService;
            _loggingService = loggingService;
            _authenticationService = authenticationService;
            _settingsService = settingsService;
            _dialogService = dialogService;
            _settingsService.CurrentFormSetting = typeof(LoginViewModel).Name;
            LoadLastUserLogin();
            _packageName = DependencyService.Get<IAutoUpdateVersion>().PackageNameApp();
            _versionname = DependencyService.Get<IAutoUpdateVersion>().VersionName();
            _appcode = DependencyService.Get<IXmlLoadData>().LoadData();
            //GetVersion();
            locklogin = false;
            LockPrint = false;
        }

        public ICommand UserUnfocusedCommand => new Command(OnGetSite);
        public ICommand LoginCommand => new Command(OnLogin);
        public ICommand UpdateCommand => new Command(GetVersion);
        public ICommand UserFocusedCommand => new Command(OnUserFocused);
        public ICommand UserSelectedIndex => new Command(SlectSite);
        private void OnUserFocused(object obj)
        {
            if (obj is FocusEventArgs eventArgs)
            {
                if (eventArgs.VisualElement is Entry entry)
                {
                    entry.CursorPosition = 0;
                    entry.SelectionLength = entry.Text.Length;
                    IsBusy = false;
                    lockprint = false;
                }
            }
        }
        public bool LockPrint
        {
            get => lockprint;
            set
            {
                lockprint = value;
                OnPropertyChanged();
            }

        }
        public bool LockLogin
        {
            get => locklogin;
            set
            {
                locklogin = value;
                OnPropertyChanged();
            }

        }
        public bool IsUpdate
        {
            get => _isupdate;
            set
            {
                _isupdate = value;
                OnPropertyChanged();
            }
        }
        public string UserName
        {
            get => _userName = "sarawutb";
            set
            {
                _userName = value;
                ValidateLogin();
                OnPropertyChanged();
            }
        }
        public string PackageNameApp
        {
            get => _packageName;
            set
            {
                _packageName = value;
                OnPropertyChanged();
            }
        }
        public string VersionName
        {
            get => _versionname;
            set
            {
                _versionname = value;
                OnPropertyChanged();
            }
        }
        public string Appcode
        {
            get => _appcode;
            set
            {
                _appcode = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password = "Rang7754";
            set
            {
                _password = value;
                ValidateLogin();
                OnPropertyChanged();
            }
        }
        public IList<SiteProfile> SiteUser
        {
            get => _siteUser;
            set
            {
                _siteUser = value;
                OnPropertyChanged();

            }
        }
        public List<Printer_Profile> Printer_Profiles
        {
            get => _printprofile;
            set
            {
                _printprofile = value;
                OnPropertyChanged();
            }
        }
        public Printer_Profile Printer_Profile
        {
            get => _printprofiles;
            set
            {
                _printprofiles = value;
                OnPropertyChanged();
            }
        }
        public SiteProfile Sites
        {
            get => _sites;
            set
            {
                _sites = value;
                OnPropertyChanged();

            }
        }
        public int Site_ID
        {
            get => _site_ID;
            set
            {
                _site_ID = value;
                OnPropertyChanged();
            }
        }

        private async void OnGetSite()
        {
            if (!string.IsNullOrEmpty(UserName))
                await GetSite();
        }
        public async void GetVersion()
        {
            try
            {
                var CurrentVersion = await _autoReceivingService.GetVersionAppMobile(Appcode);
                if (CurrentVersion != VersionName)
                {
                    await _dialogService.DialogYesOrNo(
                        "แจ้งเตือนระบบ",
                                            "ต้องการอัปเดทเวอร์ชั่น " + CurrentVersion + " หรือไม่",
                                            async () =>
                                            {
                                                try
                                                {
                                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning,
                                                                                          "1.แอปจะทำการลบตัวติดตั้งเวอร์ชั่นเก่า \r\n" +
                                                                                          "2.ดาวน์โหลดไฟล์(.zip)ผ่านเบราว์เซอร์ของแทปเล็ต \r\n" +
                                                                                          "3.กด'OPEN'ไฟล์(.zip)เข้าไปเพื่อติดตั้งไฟล์(.apk)เวอร์ชั่นใหม่ \r\n" +
                                                                                          "*อัปเดทไม่สำเร็จติดต่อ IT");

                                                    //await _dialogService.ShowDialog(
                                                    //          "1.แอปจะทำการลบตัวติดตั้งเวอร์ชั่นเก่า \r\n" +
                                                    //          "2.ดาวน์โหลดไฟล์(.zip)ผ่านเบราว์เซอร์ของแทปเล็ต \r\n" +
                                                    //          "3.กด'OPEN'ไฟล์(.zip)เข้าไปเพื่อติดตั้งไฟล์(.apk)เวอร์ชั่นใหม่ \r\n" +
                                                    //          "*อัปเดทไม่สำเร็จติดต่อ IT",
                                                    //          "ขั้นตอนการอัปเดทเวอร์ชั่น",
                                                    //          "ตกลง");
                                                    var urldowload = ApiConstants.BaseApiUrlDowload + CurrentVersion + ApiConstants.FilePath;
                                                    DependencyService.Get<IAutoUpdateVersion>().GetVersion(urldowload, PackageNameApp);
                                                    LockLogin = false;
                                                    IsUpdate = true;
                                                }
                                                catch (Exception ex)
                                                {
                                                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                                                }
                                            });
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลเวอร์ชั่นผิดพลาด\r\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "การโหลดข้อมูลเวอร์ชั่นผิดพลาด\r\nError Message: " + e.ToString());
            }

        }
        private async Task GetSite()
        {
            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาตรวจสอบการเชื่อมต่ออินเตอร์เน็ตหรือ ติดต่อ SWD.");
            if (!IsBusy)
            {
                try
                {
                    if (_connectionService.IsConnected)
                    {
                        IsBusy = true;
                        //DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                        var authenticationResponse = await _authenticationService.GetSiteAsync(UserName);
                        IsBusy = false;
                        Thread.Sleep(250);
                        //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        if (authenticationResponse != null)
                        {
                            _settingsService.UserSetting = authenticationResponse;
                            SiteUser = authenticationResponse.UserOperationSite;
                            LockLogin = true;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(UserName))
                            {
                                Thread.Sleep(250);
                                //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบข้อมูลชื่อผู้ใช้");
                            }
                            _settingsService.UserIdSetting = null;
                            SiteUser = null;
                            LockLogin = false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(250);
                        //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        SiteUser = null;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาตรวจสอบการเชื่อมต่ออินเตอร์เน็ตหรือ ติดต่อ SWD.");
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    Thread.Sleep(250);
                    //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    SiteUser = null;
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ท่านยังไม่ได้รับสิทธิ์ในการใช้งาน\r\nError Message : " + e.Message);
                    IsBusy = false;
                }
                catch (Exception e)
                {
                    Thread.Sleep(250);
                    //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    SiteUser = null;
                    _loggingService.Error(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ท่านยังไม่ได้รับสิทธิ์ในการใช้งาน\r\nError Message : " + e.ToString());
                    IsBusy = false;
                }
            }
            if (!IsBusy)
            {
                try
                {
                    if (_connectionService.IsConnected)
                    {
                        IsBusy = true;
                        //DependencyService.Get<ILoadPageAndroid>().ShowLoadingPageAsync();
                        var authenticationResponse = await _authenticationService.GetSiteAsync(UserName);
                        IsBusy = false;
                        Thread.Sleep(250);
                        //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        if (authenticationResponse != null)
                        {
                            _settingsService.UserSetting = authenticationResponse;
                            SiteUser = authenticationResponse.UserOperationSite;
                            LockLogin = true;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(UserName))
                            {
                                Thread.Sleep(250);
                                //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "ไม่พบข้อมูลชื่อผู้ใช้");
                            }
                            _settingsService.UserIdSetting = null;
                            SiteUser = null;
                            LockLogin = false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(250);
                        //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                        SiteUser = null;
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณาตรวจสอบการเชื่อมต่ออินเตอร์เน็ตหรือ ติดต่อ SWD.");
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    Thread.Sleep(250);
                    //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    SiteUser = null;
                    _loggingService.Error(e.Message);
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ท่านยังไม่ได้รับสิทธิ์ในการใช้งาน\r\nError Message : " + e.Message);
                    IsBusy = false;
                }
                catch (Exception e)
                {
                    Thread.Sleep(250);
                    //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    SiteUser = null;
                    _loggingService.Error(e.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ท่านยังไม่ได้รับสิทธิ์ในการใช้งาน\r\nError Message : " + e.ToString());
                    IsBusy = false;
                }
            }
        }

        public void SlectSite()
        {
            SelectPrinterAsync();
        }
        public async Task SelectPrinterAsync()
        {
            try
            {
                if (Sites != null)
                {
                    Printer_Profiles = await _autoReceivingService.GetPrinterProfiles(Sites.Site_ID);
                    if (Printer_Profiles.Count > 0)
                        LockPrint = true;
                }
            }
            catch (HttpRequestExceptionEx e)
            {
                _loggingService.Error(e.Message);
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่สามารถดึงข้อมูล Printer ได้\\r\\nError Message : " + e.Message);
            }
            catch (Exception e)
            {
                _loggingService.Error(e.ToString());
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "ไม่สามารถดึงข้อมูล Printer ได้\\r\\nError Message : " + e.ToString());
            }

        }

        private async void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก ชื่อผู้ใช้");
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Warning, "กรุณากรอก รหัสผ่าน");
            }
            else
            {
                try
                {
                    IsBusy = true;
                    try
                    {
                        if (_connectionService.IsConnected)
                        {
                            var a = _connectionService.IsConnected;
                            var LoginVM = new LoginVm
                            {
                                Username = UserName,
                                Password = Password,
                            };


                            var UserSetting = await DependencyService.Get<ILoadingService>().Loading<ApplicationUser>(await _authenticationService.CheckPermissionAsync(LoginVM), TimeoutLoading: 3000);
                            if (UserSetting != null)
                            {
                                try
                                {
                                    _settingsService.UserSetting = new ApplicationUser
                                    {
                                        UserOperationSite = UserSetting.UserOperationSite,
                                    };
                                    _settingsService.TokenSetting = UserSetting.token;
                                    var SiteAndPrinter = await _dialogService.DialogSiteAndPrinter(true);
                                    if (SiteAndPrinter != null)
                                    {
                                        _settingsService.UserSetting = UserSetting;
                                        _settingsService.UserIdSetting = _settingsService.UserSetting.UserId.ToString();
                                        _settingsService.PasswordSetting = Password;
                                        _settingsService.UserNameSetting = _settingsService.UserSetting.UserWindow;
                                        _settingsService.UserFullNameSetting = _settingsService.UserSetting.UserFullName;
                                        _settingsService.UserSetting.token = _settingsService.UserSetting.token;
                                        _settingsService.SiteIdSetting = SiteAndPrinter.siteProfile.Site_ID.ToString();
                                        _settingsService.SiteNameSetting = SiteAndPrinter.siteProfile.Site_Name;
                                        _settingsService.PrintIPAdressFormSetting = SiteAndPrinter.printerProfile.Printer_IP_Address;
                                        _settingsService.PrintPortFormSetting = SiteAndPrinter.printerProfile.Printer_Port;
                                        _settingsService.PrintNameFormSetting = SiteAndPrinter.printerProfile.Printer_Name;
                                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Success, "สำเร็จ");
                                        await _navigationService.NavigateToAsync<MainViewModel>();
                                        ResetFormLogin();
                                    }
                                    IsBusy = false;
                                }
                                catch (Exception ex)
                                {
                                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, ex.ToString());
                                }
                            }
                            else
                            {
                                await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error,
                                "ไม่พบชื่อผู้ใช้นี้! \nเกิดข้อผิดพลาดในการ Login");
                            }
                        }
                        else
                        {
                            await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "กรุณาตรวจสอบการเชื่อมต่อ WIFI!");
                        }
                    }
                    catch (Exception e)
                    {
                        _loggingService.Error(e.ToString());
                        await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "เกิดข้อผิดพลาดในการ Login\\r\\nError Message : " + e.ToString());

                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }
                catch (Exception ex)
                {
                    //DependencyService.Get<ILoadPageAndroid>().HideLoadingPage();
                    _loggingService.Error(ex.ToString());
                    await _dialogService.DialogOK(MessagingConstants.NotifySystem, IconDialog.Error, "Error Message : " + ex.ToString());
                }
            }
        }

        private void LoadLastUserLogin()
        {
            if (_settingsService.UserNameSetting != null)
            {
                UserName = _settingsService.UserNameSetting;
            }
        }

        private void ResetFormLogin()
        {
            UserName = string.Empty;
            Password = string.Empty;
            LockLogin = false;
        }

        void ValidateLogin()
        {
            if (IsValid(UserName) && IsValid(Password))
            {
                LockLogin = true;
            }
            else
            {
                LockLogin = false;
            }
        }

        bool IsValid(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

    }
}
