using Innovation.Mobile.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Data
{
    public interface IAuthenticationService
    {
        Task<ApplicationUser> GetSiteAsync(string userName);
        Task<String> CheckPermissionAsync(string Username, string Password, int site_id);
        Task<ApplicationUser> ValidateUserWIthToken(string token);
        bool IsUserAuthenticated();

        //new
        Task<ApplicationUser> CheckPermissionAsync(LoginVm loginVm);
        Task<List<Printer_Profile>> GetPrinterProfiles();
    }
}
