using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class DeliveryReportGetConditionVM
    {
        public string OutId { get; set; }
        public int RequestMstId { get; set; }
        public int RequestMstTransferSite { get; set; }
        public int RequestMstReceiveSite { get; set; }
        public string AppId { get; set; }
        public int ReportId { get; set; }
        public string CompId { get; set; }
        public int UserId { get; set; }
    }
}
