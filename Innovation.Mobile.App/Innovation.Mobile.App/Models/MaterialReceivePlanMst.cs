using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialReceivePlanMst : StateData
    {
        public int Id { get; set; }
        public int? materialRequestMstId { get; set; }
        public string PONO { get; set; }
        public string documentRequestNo { get; set; }
        public int? receivePlanMstStatus { get; set; }
        public int typeSuplierId { get; set; }
        public string typeSuplierNameEng { get; set; }
        public string typeSuplierNameThai { get; set; }
        public string receivePlanMstStatusNameThai { get; set; }
        public string receivePlanMstStatusNameEng { get; set; }
        public int OperationSite { get; set; }
        public string OperationSiteName { get; set; }
        public DateTime? requestReceiveDate { get; set; }
        public int? ownerSite { get; set; }
        public string ownerCode { get; set; }
        public string ownerSiteName { get; set; }
        public int receiveSite { get; set; }
        public string receiveLocation { get; set; }
        public string receiveLocationName { get; set; }
        public int? createBy { get; set; }
        public string createByName { get; set; }
        public DateTime? createDate { get; set; }
        public string typeRM { get; set; }
        public string suppCode { get; set; }
        public string suppName { get; set; }
        public string ColorCode { get; set; }
        public string colorName { get; set; }
        public string inId { get; set; }
        public string docId { get; set; }
        public bool IseditStatus { get; set; }
        public int reviseBy { get; set; }
        public int reviseNo { get; set; }
        public bool forSetEXPDate { get; set; }
        public string receiveSiteShortName { get; set; }
        public string suppNameEng { get; set; }
        public string PoNoCheckSuppName => PONO == suppNameEng ? "" : " : " + PONO;
        public string DocNoMobileShow => suppNameEng + " Sent To " + receiveSiteShortName + PoNoCheckSuppName + " : " + documentRequestNo;
        public List<MaterialReceivePlanDtl> receivePlanDtl { get; set; }
    }
}
