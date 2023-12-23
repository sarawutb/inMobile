using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class ApplicationUser
    {
        public int UserId { get; set; }
        [JsonProperty("fullName")]
        public string UserFullName { get; set; }
        [JsonProperty("userName")]
        public string UserWindow { get; set; }
        public int DepartmentID { get; set; }
        public int? CompanyID { get; set; }
        public string token { get; set; }
        public bool IsAutherized { get; set; }
        public string Remark { get; set; }
        [JsonProperty("roles")]
        public List<String> Roles { get; set; }
        public List<SiteProfile> UserOperationSite { get; set; }
        public List<SiteProfile> UserOwnerSite { get; set; }
        public ProgramUserPermission ProgramPermission { get; set; }

        public ApplicationUser()
        {
            Roles = new List<String>();
            UserOperationSite = new List<SiteProfile>();
            ProgramPermission = new ProgramUserPermission();
            UserOwnerSite = new List<SiteProfile>();
        }
    }
}
