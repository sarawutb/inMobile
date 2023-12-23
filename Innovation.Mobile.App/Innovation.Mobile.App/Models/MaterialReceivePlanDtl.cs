using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialReceivePlanDtl : StateData
    {
        public int Id { get; set; }
        public int? receivePlanMstId { get; set; }
        public string rmId { get; set; }
        public string rmName { get; set; }
        public int? receivePlanDtlStatus { get; set; }
        public string receivePlanDtlStatusNameEng { get; set; }
        public string receivePlanDtlStatusNameThai { get; set; }
        public int countQty { get; set; }
        public string typeRM { get; set; }
        public decimal receivingWeight { get; set; }
        public decimal weightPerUnit { get; set; }
        public string unitCode { get; set; }
        public string lineId { get; set; }
        public string firstReceiveDate { get; set; }
        public string accountDate { get; set; }
        public decimal keepTime { get; set; }
        public int reviseBy { get; set; }
        public int reviseNo { get; set; }
        public string keyLink { get; set; }
        public int RequestDtlID { get; set; }
        public string lotNo { get; set; }
        public string colorCode { get; set; }
        public string colorName { get; set; }
        public string ColorStatusCode { get; set; }
        public string rmIdReceive { get; set; }
        public byte[] Imagedtl { get; set; }
        public List<MaterialReceivePlanDtlBarcode> ReceivePlanBarcode { get; set; }
    }
}
