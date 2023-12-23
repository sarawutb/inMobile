using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialRequestMSTReferenceVM
    {
        public int ID { get; set; }
        public int DocRefTypeId { get; set; }
        public string DocRefValue { get; set; }
        public int MaterialRequestMstId { get; set; }
        public int ItemGroupId { get; set; }
        public int Site_ID { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
