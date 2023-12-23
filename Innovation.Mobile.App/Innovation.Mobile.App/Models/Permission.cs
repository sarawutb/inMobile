using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string ComponentName { get; set; }
        public Boolean Authorized { get; set; }
    }
}
