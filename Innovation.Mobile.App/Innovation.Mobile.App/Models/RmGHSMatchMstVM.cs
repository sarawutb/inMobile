using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class RmGHSMatchMstVM
    {
        public string RMID { get; set; }
        public string DangerDetail { get; set; }
        public string WarningDetail { get; set; }
        public bool Notify { get; set; }
        public bool IsActive { get; set; }
        public int ReviseBy { get; set; }
        public DateTime ReviseDate { get; set; }
        public List<RmGHSMatchDtlVM> RmGHSDtlLst { get; set; }
        public List<DataReportFileCenter> DataReportFileCenter { get; set; }

    }
}
