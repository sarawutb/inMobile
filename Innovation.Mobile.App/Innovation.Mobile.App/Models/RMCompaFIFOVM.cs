using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class RMCompaFIFOVM
    {
        public string RMID { get; set; }
        public string RM_Name { get; set; }
        public string Barcode { get; set; }
        public string BatchNo { get; set; }
        public string LotNo { get; set; }
        public decimal Balance { get; set; }
        public string ExpireDate { get; set; }
        public string WH_Name { get; set; }
        public string Location_Name { get; set; }
        public bool Lock { get; set; }
        public string QA_Status { get; set; }
    }
}
