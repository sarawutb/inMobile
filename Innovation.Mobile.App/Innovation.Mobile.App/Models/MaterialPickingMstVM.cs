using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialPickingMstVM : StateData
    {
        public int Id { get; set; }
        public int MaterialRequestMstId { get; set; }
        public string RequestDocNo { get; set; }
        public int? ReviseNo { get; set; }
        public int? OwnerSite { get; set; }
        public string OwnerSiteName { get; set; }
        public int? ReceiveSite { get; set; }
        public string ReceiveSiteName { get; set; }
        public int TransferToSite { get; set; }
        public string TransferToSiteName { get; set; }
        public string Description { get; set; }
        public int PickingStatusId { get; set; }
        public string PickingStatusName { get; set; }
        public DateTime pickingDate { get; set; }
        public DateTime? expectDate { get; set; }
        public int pickingPurposeId { get; set; }
        public string pickingPurposeName { get; set; }
        public bool? isChangeRm { get; set; }
        public List<MaterialPickingDtlVM> pickingDtl { get; set; }
        public int reviseBy { get; set; }
        public int reviseNo { get; set; }
        public string ColorCode { get; set; }
        public string colorName { get; set; }
    };

    public class MaterialPickingListMstListVM
    { 
        public int Id { get; set; }
        public int MaterialRequestMstId { get; set; }
        public string RequestDocNo { get; set; }
        public int? ReviseNo { get; set; }
        public int? OwnerSite { get; set; }
        public string OwnerSiteName { get; set; }
        public int? ReceiveSite { get; set; }
        public string ReceiveSiteName { get; set; }
        public string SuppName { get; set; }
        public string ColorCode { get; set; }
        public DateTime expectDate { get; set; }
        public string pickingPurposeName { get; set; }
    }
}
