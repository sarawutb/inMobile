using Akavache;
using Innovation.Mobile.App.Bootstrap;
using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Repository;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Innovation.Mobile.App.Service.Data
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISettingsService _settingsService;
        public string programCode = DependencyService.Get<IXmlLoadData>().LoadData();
        public AuthenticationService(IGenericRepository genericRepository, ISettingsService settingsService, IBlobCache cache = null)
            : base(cache)
        {
            _settingsService = settingsService;
            _genericRepository = genericRepository;
        }

        public async Task<String> CheckPermissionAsync(string Username, string Password, int site_id)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.AuthenticateEndpoint + "/ValidateUserPassword?username=" + Username + "&password=" + Password + "&ProgramCode=" + programCode;
            var token = await _genericRepository.GetAsync<string>(apiurl);
            return token;
            //var LoginVm = await _genericRepository.GetAsync<LoginVm>(apiurl);
            //return LoginVm.Token;
        }

        public async Task<ApplicationUser> CheckPermissionAsync(LoginVm loginVm)
        {
            var URL = ApiConstants.AuthenticationMobile + "/Login";
            var User = await _genericRepository.PostAsync<LoginVm, ApplicationUser>(URL, loginVm);
            return User;
        }

        public async Task<ApplicationUser> GetSiteAsync(string userName)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.AuthenticateEndpoint + "/GetAutherizedDataWithUserAndProgramcode?username=" + userName + "&ProgramCode=" + programCode;

            //UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            //{
            //    Path = $"{ApiConstants.AuthenticateEndpoint}/GetAutherizedDataWithUserAndProgramcode?username={userName}&ProgramCode={programCode}"
            //};

            var ApplicationUserResponse = await _genericRepository.GetAsync<ApplicationUser>(apiurl);

            //var ApplicationUserResponse = await _genericRepository.GetAsync<ApplicationUser>(builder.ToString());

            return ApplicationUserResponse;
        }

        public bool IsUserAuthenticated()
        {
            _settingsService.GetTokenAsync();
            return !string.IsNullOrEmpty(_settingsService.TokenSetting);
        }

        public async Task<ApplicationUser> ValidateUserWIthToken(string token)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.AuthenticateEndpoint + "/ValidateUserWIthToken";

            var result = await _genericRepository.PostAsync<string, ApplicationUser>(apiurl, token);

            return result;
        }

        public async Task<List<Printer_Profile>> GetPrinterProfiles()
        {
            var URL = ApiConstants.AuthenticationMobile + "/GetPrinterProfile";
            var PrinterProfile = await _genericRepository.GetAsync<List<Printer_Profile>>(URL);
            return PrinterProfile;
        }

    }
}
