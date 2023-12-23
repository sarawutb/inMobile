namespace Innovation.Mobile.App.Models
{
    public class RmBalVM
    {
        public string IN_ID { get; set; }
        public string RM_ID { get; set; }
        public string PI_LOT_NO { get; set; }
        public string BARCODE { get; set; }
        public string BatchNo { get; set; }
        public string RMName { get; set; }
        public string RmSuppCode { get; set; }
        public string LOT_NO { get; set; }
        public string TYPE { get; set; }
        public decimal? QTY { get; set; }
        public string EXP_DATE { get; set; }
        public bool? LOCK { get; set; }
        public int? Site_ID { get; set; }
        public string QA_LOCK { get; set; }
        public bool? WH_LOCK { get; set; }
        public int? Owner_Site_ID { get; set; }
    }
}
