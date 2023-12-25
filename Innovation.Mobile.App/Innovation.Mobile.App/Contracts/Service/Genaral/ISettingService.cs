using Innovation.Mobile.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Genaral
{
    public interface ISettingsService
    {
        void AddItem(string key, string value);
        string GetItem(string key);
        string BarcodeSetting { get; set; }
        string ReceiveMstIdSetting { get; set; }
        string ReceiveDtlIdSetting { get; set; }
        string UserNameSetting { get; set; }
        string PasswordSetting { get; set; }
        string UserFullNameSetting { get; set; }
        string UserIdSetting { get; set; }
        string SiteIdSetting { get; set; }
        string SiteNameSetting { get; set; }
        string TokenSetting { get; set; }
        string PrintIPAdressFormSetting { get; set; }
        string PrintPortFormSetting { get; set; }
        string PrintNameFormSetting { get; set; }
        void SetTokenAsync(string key,string value);
        ApplicationUser UserSetting { get; set; }
        void GetTokenAsync();
        string CurrentFormSetting { get; set; } 
        string BaseApiUrlBySite { get; set; } 
    }
}
