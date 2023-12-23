using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialReceiveGroupLotNoVM
    {
        public bool IsCheck { get; set; }
        public string rmId { get; set; }
        public string rmIdRecieve { get; set; }
        public string rmName { get; set; }
        public string firstReceiveDate { get; set; }
        public string LotNo_Group { get; set; }
        public string PI_Lot_NO { get; set; }
        public int countQty { get; set; }
        public decimal weightPerUnit { get; set; }
        public decimal SumWeight { get; set; }
        public string  LabelColor { get; set; }
    }
}
