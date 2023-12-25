using Akavache;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Innovation.Mobile.App.Service.Data;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Innovation.Mobile.App.Constants;

namespace Innovation.Mobile.App.Service.Genaral
{
    public class SettingsService : BaseService, ISettingsService
    {
        private readonly ISettings _settings;
        private const string UserName = "UserName";
        private const string _password = "Password";
        private const string UserFullName = "UserFullName";
        private const string UserId = "UserId";
        private const string SiteId = "SiteId";
        private const string SiteName = "SiteName";
        private const string Token = "Token";
        private string Barcode;
        private string _Token;
        private const string PrintIPAdress = "PrintIPAdress";
        private const string PrintPort = "PrintPort";
        private const string PrintName = "PrintName";
        private const string CurrentForm = "CurrentForm";
        private string _MstId;
        private string _DtlId;
        private string _BaseApiUrlBySite = ApiConstants.BaseApiUrlNew;

        private ApplicationUser User;
        public SettingsService(IBlobCache cache = null) : base(cache)
        {
            _settings = CrossSettings.Current;
        }

        public void AddItem(string key, string value)
        {
            _settings.AddOrUpdateValue(key, value);
        }

        public string GetItem(string key)
        {
            return _settings.GetValueOrDefault(key, string.Empty);
        }

        public void GetTokenAsync()
        {
            string tokenFormcach = GetFromCache<string>(Token);
            if (tokenFormcach != null)//loaded from cache
            {
                _Token = tokenFormcach;
            }
            else
            {
                _Token = null;
            }
        }

        public void SetTokenAsync(string key, string value)
        {
            Cache.InsertObject(key, value, DateTimeOffset.Now.AddHours(1));
        }
        public void SetGetorCreatToken(string key, string value)
        {
            Cache.GetOrCreateObject(key, () => value, DateTimeOffset.Now.AddSeconds(10));
        }

        public string UserNameSetting
        {
            get => GetItem(UserName);
            set => AddItem(UserName, value);
        }
        public string PasswordSetting
        {
            get => GetItem(_password);
            set => AddItem(_password, value);
        }
        public string UserIdSetting
        {
            get => GetItem(UserId);
            set => AddItem(UserId, value);
        }
        public ApplicationUser UserSetting
        {
            get => User;
            set => User = value;
        }
        public string SiteIdSetting
        {
            get => GetItem(SiteId);
            set => AddItem(SiteId, value);
        }
        public string TokenSetting
        {
            get => _Token;
            set
            {
                _Token = value;
                SetTokenAsync(Token, _Token);
            }
        }

        public string SiteNameSetting
        {
            get => GetItem(SiteName);
            set => AddItem(SiteName, value);
        }
        public string UserFullNameSetting
        {
            get => GetItem(UserFullName);
            set => AddItem(UserFullName, value);
        }
        public string BaseApiUrlBySite
        {
            get =>  string.IsNullOrEmpty(GetItem(_BaseApiUrlBySite)) ? ApiConstants.BaseApiCenter : GetItem(_BaseApiUrlBySite);
            set => AddItem(_BaseApiUrlBySite, value);
        }
        public string BarcodeSetting
        {
            get => Barcode;
            set => Barcode = value;
        }
        public string ReceiveMstIdSetting
        { 
            get => _MstId; 
            set => _MstId = value; 
        }
        public string CurrentFormSetting
        {
            get => GetItem(CurrentForm);
            set => AddItem(CurrentForm, value);
        }
        public string PrintIPAdressFormSetting { get => GetItem(PrintIPAdress); set => AddItem(PrintIPAdress, value); }
        public string PrintPortFormSetting { get => GetItem(PrintPort); set => AddItem(PrintPort, value); }
        public string PrintNameFormSetting { get => GetItem(PrintName); set => AddItem(PrintName, value); }
        public string ReceiveDtlIdSetting { get => _DtlId; set => _DtlId = value; }
    }
}
