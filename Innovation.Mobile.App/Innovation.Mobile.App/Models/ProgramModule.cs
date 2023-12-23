using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class ProgramModule
    {
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int ChildOfModuleId { get; set; }
        public List<Permission> Permissions { get; set; }
        public Boolean Autherized { get; set; }

        public ProgramModule()
        {
            Permissions = new List<Permission>();
        }
    }
}
