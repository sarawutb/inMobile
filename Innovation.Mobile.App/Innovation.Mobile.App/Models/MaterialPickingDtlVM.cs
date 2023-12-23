using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class MaterialPickingDtlVM : StateData
    {
        public int Id { get; set; }
        public int MaterialPickinglistMstId { get; set; }
        public string Barcode { get; set; }
        public string RmId { get; set; }
        public string RmName { get; set; }
        public string ChangeCodeRM { get; set; }
        public string Request_LotNo { get; set; }
        public int Package_QTY { get; set; }
        public string PILotNo { get; set; }
        public decimal RequestWeight { get; set; }
        public decimal Weight_Per_Package { get; set; }
        public string LotNo { get; set; }
        public decimal Total_Weight { get; set; }
        public decimal? Request_Residue_Weight { get; set; }
        public decimal? Residue_Weight { get; set; }
        public decimal Net_Weight { get; set; }
        public string BatchNo { get; set; }
        public int ARP_Material_PickingList_Weighing_Status_ID { get; set; }
        public bool IsWeight { get; set; }
        public int ARP_Material_PickingList_DTL_Status_ID { get; set; }
        public string PickingDtlStatusNameTH { get; set; }
        public int Picking_By { get; set; }
        public string Picking_By_Name { get; set; }
        public DateTime Picking_Date { get; set; }
        public byte[] PicBarcode { get; set; }
        public int Station_ID { get; set; }

        //--For show on display
        public bool IsSelect { get; set; }
        public bool CanPrint { get; set; }
        public bool CanDelete { get; set; }
        public int Package_QTY_Show { get; set; }
        public string colorCode { get; set; }
        public string colorName { get; set; }
        public string colorStatus { get; set; }

        //--For Barcode
        public string CompanyName { get; set; }
        public string BarcodeData { get; set; }
        public string RmCode { get; set; }
        public string RmNameBC { get; set; }
        public string BOI { get; set; }
        public string PILotno { get; set; }
        public string OwnerShortName { get; set; }
        public string Lotno { get; set; }
        public string BatchNoBC { get; set; }
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

    }
}
