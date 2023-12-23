using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialPickingListWeightingDtlVM
    {
        public int Id { get; set; }
        public int PickingListWeightingMstId { get; set; }
        public string Barcode { get; set; }
        public string RmId { get; set; }
        public string BatchNo { get; set; }
        public string PiLotNo { get; set; }
        public int? PickingBy { get; set; }
        public DateTime? PickingDate { get; set; }
        public decimal? NetWeight { get; set; }
        public int reviseBy { get; set; }
        public int reviseNo { get; set; }
    }
}
