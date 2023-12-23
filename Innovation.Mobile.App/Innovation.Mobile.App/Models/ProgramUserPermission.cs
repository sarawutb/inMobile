using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class ProgramUserPermission
    {
        public List<int> ProgramId { get; set; }
        public List<string> ProgramCode { get; set; }
        public List<string> ProgramName { get; set; }
        public List<ProgramModule> Modules { get; set; }
        public ProgramUserPermission()
        {
            Modules = new List<ProgramModule>();
            ProgramId = new List<int>();
            ProgramCode = new List<string>();
            ProgramName = new List<string>();
        }
    }
}
