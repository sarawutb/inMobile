using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialReceivePlanDtlBarcode : StateData
    {
        public int Id { get; set; }
        public int? receivePlanMstId { get; set; }
        public int? receivePlanDtlId { get; set; }
        public string rmId { get; set; }
        public string rmName { get; set; }
        public string barcode { get; set; }
        public string batchNo { get; set; }
        public string lotNo { get; set; }
        public string lotNonum { get; set; }
        public int MaxlenlotNonum { get; set; }
        public string LotNo_Group { get; set; }
        public string piLotNo { get; set; }
        public decimal? qty { get; set; }
        public int? receivePlanDtlBarcodeStatus { get; set; }
        public string receivePlanDtlBarcodeStatusNameEng { get; set; }
        public string receivePlanDtlBarcodeStatusNameThai { get; set; }
        public int? receiveBy { get; set; }
        public string receiveByName { get; set; }
        public string productionDate { get; set; }
        public string expireDate { get; set; }
        public DateTime? receiveDate { get; set; }
        public byte[] BarcodeImage { get; set; }
        public int reviseBy { get; set; }
        public int reviseNo { get; set; }
        public int ReceivePlanRecheckCauseId { get; set; }
        public string ReceivePlanRecheckCauseNameTH { get; set; }
        public string ReceivePlanRecheckCauseNameENG { get; set; }
        public bool? StatusQA { get; set; }
        public List<PlanCheck> lstPlanCheck { get; set; }
        public List<PlanCheck> lstPlanCheckQA { get; set; }
        public bool IsSelect { get; set; }
        ///
        public string CompanyName { get; set; }
        public string RmCode { get; set; }
        public string RmName { get; set; }
        public string BOI { get; set; }
        public string OwnerShortName { get; set; }
        public string OwnerName { get; set; }
        public string Weight { get; set; }
        public string LotWeight { get; set; }
        public string RecordDate { get; set; }
        public string ExpireDate { get; set; }
        public string DARNO { get; set; }
        public string ISO { get; set; }
        public string SoCRM { get; set; }
        public string RmTemp { get; set; }
        public string RmDG { get; set; }
        public string colorName { get; set; }
        public string ColorStatusCode { get; set; }

        public RmGHSMatchMstVM rmGHSlst { get; set; }

    }
}
