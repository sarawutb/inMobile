using Akavache.Sqlite3.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class LoginVm
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("programCode")]
        public const string programCode = "MOB005";
        public bool IsLogin { get; set; }
        public string Token { get; set; }
    }
}
