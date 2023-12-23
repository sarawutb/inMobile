using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class CheckList
    {
        public int ID { get; set; }
        public int Revise_No { get; set; }
        public string Code { get; set; }
        public string Name_TH { get; set; }
        public string Name_ENG { get; set; }
        public int Order_NO { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsCheck { get; set; }
        public bool IsOpenEnt { get; set; }
        public int CheckTypeId { get; set; }
    }

    public class CheckType
    {
        public int ID { get; set; }
        public int Revise_No { get; set; }
        public string Code { get; set; }
        public string Name_TH { get; set; }
        public string Name_ENG { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }

    public class PlanCheck : StateData
    {
        public int ID { get; set; }
        public int BarcodeId { get; set; }
        public int CheckListId { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string CheckListName { get; set; }
        public bool IsCheck { get; set; }
        public int CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }

    }
}
