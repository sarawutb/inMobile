using Innovation.Mobile.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Contracts.Service.Data
{
    public interface IMaterialAutoReceivingService
    {
        Task<IEnumerable<MaterialReceivePlanMst>> GetMaterialReceivePlanMstAsync(DateTime beginDateReceive, DateTime endDateReceive);
        Task<IEnumerable<MaterialReceivePlanDtl>> GetMaterialReceivePlanDtl( int mstId , int dtlId );
        Task<IEnumerable<UnitCountVM>> GetUnitCountAsync();
        Task<IEnumerable<CheckList>> GetCheckList();
        Task<IEnumerable<ReceivePlanRecheckCauseVM>> GetRecheckCauseAsync();
        Task<IEnumerable<MaterialPickingListMstListVM>> GetMaterialPickingMstAsync(DateTime dateStart, DateTime dateEnd);
        Task<IEnumerable<MaterialPickingDtlVM>> GetMaterialPickingDtlAsync(int mstID,int? Status = null);
        Task<DefaultContAndUnit> GetDefaultContWeightAsync(string rmId, string suppCode);
        Task<RmBalVM> GetRMCompoundByBarcodeAsync(string barcode);
        Task<bool> GetPickingWeightingByBarcodeAsync(int pickingMstID, string barcode);
        Task SavePicking(MaterialPickingMstVM materialPickingMstVM);
        Task GetUpdatermInMstStatus(MaterialReceivePlanMst materialReceivePlanMstVM);
        Task InsertCheckList(ApiListData<PlanCheck> planCheck);
        Task<MaterialReceivePlanMst> GetReturnMaterialReceivePlanDtl(MaterialReceivePlanMst materialReceivePlanMstVM);
        //Task<MaterialReceivePlanMst> SaveRecievingCrossSite(MaterialReceivePlanMst materialReceivePlanMstVM);
        Task<MaterialReceivePlanMst> SaveRecieving(MaterialReceivePlanMst materialReceivePlanMstVM);
        Task<IEnumerable<RMCompaFIFOVM>> GetRMCompaFIFOAsync(string rmId);
        Task<IEnumerable<MaterialReceivePlanDtl>> GetRMChecklistByReceiveMstAndTypeIDAsync(int mstID, int typeID,int dtlId = 0);
        Task<IEnumerable<MaterialReceivePlanMst>> GetMaterialReceivePlanMstQAAsync(DateTime beginDateReceive, DateTime endDateReceive);
        Task<bool> GetWeightingByBarcodeAsync(string barcode);
        Task<String> GetVersionAppMobile(string Appcode);
        Task<bool> GetCheckPickRMPack(string inid, string Rmid, string Lotno, string pilot);
        Task<byte[]> GenerateBarcode(string barcode);
        Task<IEnumerable<LinefileVM>> GetLinefileAsync();
        Task<List<Printer_Profile>> GetPrinterProfiles(int SiteId);
        Task<MaterialPickingMstVM> GetPickingBarcodePrintAsync(MaterialPickingMstVM materialPickingMst);
        Task<MaterialPickingMstVM> GetPickingLabelPrint(MaterialPickingMstVM materialPickingMst);
        Task<IEnumerable<MaterialMatchingVM>> GetMaterialProductionCrossSiteAsync(string rmId, int ownerSite, int productionSite);
        Task CancelReceivePlanQualityCheck(CancelReceivePlanCheckVM cancelReceivePlanCheck);
        Task<MaterialReceivePlanMst> GetReceivePlanByBarcode(string barcode);
        Task<byte[]> GetDeliveryReport(DeliveryReportGetConditionVM reportGetCondition);
        Task<List<MaterialRequestMSTReferenceVM>> GetMaterialRequestMSTReferences(int requestMstId);
        Task<RmGHSMatchMstVM> GetReportGHSByRM(string rmid, int Operation_Site_ID);
    }
}
