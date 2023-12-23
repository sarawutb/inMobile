using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Repository;
using Innovation.Mobile.App.Contracts.Service.Data;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Service.Data
{
    public class MaterialAutoReceivingService : IMaterialAutoReceivingService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISettingsService _settingsService;
        public MaterialAutoReceivingService(IGenericRepository genericRepository, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _genericRepository = genericRepository;
        }

        public async Task<DefaultContAndUnit> GetDefaultContWeightAsync(string rmId, string suppCode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetDefaultContWeight?rmId=" + rmId
                + "&suppCode=" + suppCode + "&siteID=" + _settingsService.SiteIdSetting;

            var deCont = await _genericRepository.GetAsync<DefaultContAndUnit>(apiurl);

            return deCont;
        }

        public async Task<IEnumerable<MaterialPickingDtlVM>> GetMaterialPickingDtlAsync(int mstID, int? Status = null)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialPickingDtl?mstID=" + mstID.ToString() + "&Status=" + Status.ToString() + "&siteID=" + _settingsService.SiteIdSetting;
            //string apiurl = ApiConstants.TestApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialPickingDtl?mstID=" + mstID.ToString() + "&siteID=" + _settingsService.SiteIdSetting;
            var lstPickingDtl = await _genericRepository.GetAsync<List<MaterialPickingDtlVM>>(apiurl);

            return lstPickingDtl;
        }

        public async Task<IEnumerable<MaterialPickingListMstListVM>> GetMaterialPickingMstAsync(DateTime dateStart, DateTime dateEnd)
        {
            string startDate = dateStart.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string endDate = dateEnd.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));

            var URL = ApiConstants.MaterialTransfer + "/GetMaterialPickingListPlanMst?beginDateReceive=" + startDate + "&endDateReceive=" + endDate
                   + "&docNo=" + _settingsService.SiteIdSetting + "&operateSite=" + _settingsService.SiteIdSetting;

            var lstPickingPlan = await _genericRepository.GetAsync<List<MaterialPickingListMstListVM>>(URL);

            return lstPickingPlan;
        }

        public async Task<IEnumerable<MaterialReceivePlanMst>> GetMaterialReceivePlanMstAsync(DateTime beginDateReceive, DateTime endDateReceive)
        {
            string strbeginDate = beginDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string strendDate = endDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            var URL = ApiConstants.MaterialReceive + "/GetMaterialReceivePlanMst?beginDateReceive=" + strbeginDate + "&endDateReceive=" + strendDate + "&siteID=" + _settingsService.SiteIdSetting;
            var lstReceivePlan = await _genericRepository.GetAsync<List<MaterialReceivePlanMst>>(URL);
            return lstReceivePlan;
        }
        public async Task<IEnumerable<MaterialReceivePlanMst>> GetMaterialReceivePlanMstAsyncOld(DateTime beginDateReceive, DateTime endDateReceive)
        {
            string strbeginDate = beginDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string strendDate = endDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialReceivePlanMst?beginDateReceive=" + strbeginDate + "&endDateReceive=" + strendDate + "&siteID=" + _settingsService.SiteIdSetting;
            var lstReceivePlan = await _genericRepository.GetAsync<List<MaterialReceivePlanMst>>(apiurl);
            return lstReceivePlan;
        }

        public async Task<RmBalVM> GetRMCompoundByBarcodeAsync(string barcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetRMCompoundByBarcode?barcode=" + barcode + "&siteID=" + _settingsService.SiteIdSetting;
            var RMCompAData = await _genericRepository.GetAsync<RmBalVM>(apiurl);
            return RMCompAData;
        }

        public async Task<IEnumerable<UnitCountVM>> GetUnitCountAsync()
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetUnitCount?siteID=" + _settingsService.SiteIdSetting;
            var lstUnitCount = await _genericRepository.GetAsync<List<UnitCountVM>>(apiurl);
            return lstUnitCount;
        }

        public async Task<bool> GetPickingWeightingByBarcodeAsync(int pickingMstID, string barcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetPickingWeightingByBarcode?pickingMstID=" + pickingMstID.ToString()
                + "&barcode=" + barcode + "&siteID=" + _settingsService.SiteIdSetting;

            var wtData = await _genericRepository.GetAsync<bool>(apiurl);

            return wtData;
        }

        public async Task SavePicking(MaterialPickingMstVM materialPickingMstVM)
        {
            ApiData<MaterialPickingMstVM> data = new ApiData<MaterialPickingMstVM>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialPickingMstVM
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/UpdateMaterialPickinglistMst";

            await _genericRepository.PostAsync<ApiData<MaterialPickingMstVM>, MaterialPickingMstVM>(apiurl, data);
        }

        public async Task CancelReceivePlanQualityCheck(CancelReceivePlanCheckVM cancelReceivePlanCheck)
        {
            var data = new ApiData<CancelReceivePlanCheckVM>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = cancelReceivePlanCheck
            };

            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/CancelReceivePlanQualityCheck";
            await _genericRepository.PostAsync<ApiData<CancelReceivePlanCheckVM>, CancelReceivePlanCheckVM>(apiurl, data);
        }

        public async Task InsertCheckList(ApiListData<PlanCheck> planCheck)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/InsertCheckList";// wait APi
            await _genericRepository.PostAsync<ApiListData<PlanCheck>, PlanCheck>(apiurl, planCheck);
        }
        public async Task GetUpdatermInMstStatus(MaterialReceivePlanMst materialReceivePlanMstVM)
        {
            ApiData<MaterialReceivePlanMst> data = new ApiData<MaterialReceivePlanMst>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialReceivePlanMstVM
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetUpdatermInMstStatus";

            await _genericRepository.PostAsync<ApiData<MaterialReceivePlanMst>, PlanCheck>(apiurl, data);
        }
        public async Task<MaterialReceivePlanMst> SaveRecieving(MaterialReceivePlanMst materialReceivePlanMstVM)
        {
            ApiData<MaterialReceivePlanMst> data = new ApiData<MaterialReceivePlanMst>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialReceivePlanMstVM
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/UpdateMaterialReceivePlanMst";// wait APi
            var result = await _genericRepository.PostAsync<ApiData<MaterialReceivePlanMst>, MaterialReceivePlanMst>(apiurl, data);

            return result;
        }
        //public async Task<MaterialReceivePlanMst> SaveRecievingCrossSite(MaterialReceivePlanMst materialReceivePlanMstVM)
        //{
        //    ApiData<MaterialReceivePlanMst> data = new ApiData<MaterialReceivePlanMst>()
        //    {
        //        Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
        //        MyData = materialReceivePlanMstVM
        //    };
        //    string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/UpdateMaterialReceivePlanMstCrossSite";// wait APi
        //    var result = await _genericRepository.PostAsync<ApiData<MaterialReceivePlanMst>, MaterialReceivePlanMst>(apiurl, data);

        //    return result;
        //}
        public async Task<MaterialReceivePlanMst> GetReturnMaterialReceivePlanDtl(MaterialReceivePlanMst materialReceivePlanMstVM)
        {
            ApiData<MaterialReceivePlanMst> data = new ApiData<MaterialReceivePlanMst>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialReceivePlanMstVM
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetReturnMaterialReceivePlanDtl";// wait APi
            var result = await _genericRepository.PostAsync<ApiData<MaterialReceivePlanMst>, MaterialReceivePlanMst>(apiurl, data);

            return result;
        }
        public async Task<IEnumerable<RMCompaFIFOVM>> GetRMCompaFIFOAsync(string rmId)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetRMCompaFIFO?rmId=" + rmId + "&siteID=" + _settingsService.SiteIdSetting;

            var fifoData = await _genericRepository.GetAsync<List<RMCompaFIFOVM>>(apiurl);

            return fifoData;
        }

        public async Task<IEnumerable<MaterialReceivePlanDtl>> GetRMChecklistByReceiveMstAndTypeIDAsync(int mstID, int typeID, int dtlId = 0)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetRMChecklistByReceiveMstAndTypeID?mstId=" + mstID.ToString()
                + "&typeId=" + typeID.ToString() + "&dtlId=" + dtlId.ToString() + "&siteId=" + _settingsService.SiteIdSetting;

            var rmChecklist = await _genericRepository.GetAsync<List<MaterialReceivePlanDtl>>(apiurl);

            return rmChecklist;
        }
        public async Task<bool> GetCheckPickRMPack(string inid, string Rmid, string Lotno, string pilot)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetCheckPickRMPack?inid=" + inid + "&Rmid=" + Rmid + "&Lotno=" + Lotno + "&pilot=" + pilot + "&siteid=" + _settingsService.SiteIdSetting;
            var chkispack = await _genericRepository.GetAsync<bool>(apiurl);
            return chkispack;
        }

        public async Task<IEnumerable<MaterialReceivePlanDtl>> GetMaterialReceivePlanDtl(int mstId, int dtlId)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialReceivePlanDtl?siteID=" + _settingsService.SiteIdSetting + "&mstId=" + mstId + "&dtlId=" + dtlId;
            var dtl = await _genericRepository.GetAsync<List<MaterialReceivePlanDtl>>(apiurl);
            return dtl;
        }

        public async Task<IEnumerable<MaterialReceivePlanMst>> GetMaterialReceivePlanMstQAAsync(DateTime beginDateReceive, DateTime endDateReceive)
        {
            string strbeginDate = beginDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string strendDate = endDateReceive.ToString("yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialReceivePlanMstQA?beginDateReceive=" + strbeginDate + "&endDateReceive=" + strendDate + "&siteID=" + _settingsService.SiteIdSetting;

            var lstReceivePlan = await _genericRepository.GetAsync<List<MaterialReceivePlanMst>>(apiurl);

            return lstReceivePlan;
        }

        public async Task<bool> GetWeightingByBarcodeAsync(string barcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetWeightingByBarcode?siteID=" + _settingsService.SiteIdSetting + "&barcode=" + barcode;
            var chk = await _genericRepository.GetAsync<bool>(apiurl);
            return chk;
        }
        public async Task<String> GetVersionAppMobile(string Appcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetVersionAppMobile?Appcode=" + Appcode;
            var result = await _genericRepository.GetAsync<string>(apiurl);
            return result;
        }

        public async Task<IEnumerable<CheckList>> GetCheckList()
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetCheckList?siteID=" + _settingsService.SiteIdSetting;
            var dtl = await _genericRepository.GetAsync<List<CheckList>>(apiurl);
            return dtl;
        }

        public async Task<byte[]> GenerateBarcode(string barcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GenerateBarcode?siteID=" + _settingsService.SiteIdSetting + "&barcode=" + barcode;
            var bacodegen = await _genericRepository.GetAsync<byte[]>(apiurl);
            return bacodegen;
        }

        public async Task<IEnumerable<LinefileVM>> GetLinefileAsync()
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetLinefile?siteID=" + _settingsService.SiteIdSetting;
            var linefiles = await _genericRepository.GetAsync<List<LinefileVM>>(apiurl);
            return linefiles;
        }

        public async Task<MaterialPickingMstVM> GetPickingBarcodePrintAsync(MaterialPickingMstVM materialPickingMst)
        {
            ApiData<MaterialPickingMstVM> data = new ApiData<MaterialPickingMstVM>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialPickingMst
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetPickingBarcodePrint";// wait APi
            var mst = await _genericRepository.PostAsync<ApiData<MaterialPickingMstVM>, MaterialPickingMstVM>(apiurl, data);
            return mst;
        }
        public async Task<MaterialPickingMstVM> GetPickingLabelPrint(MaterialPickingMstVM materialPickingMst)
        {
            ApiData<MaterialPickingMstVM> data = new ApiData<MaterialPickingMstVM>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = materialPickingMst
            };
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetPickingLabelPrint";
            var mst = await _genericRepository.PostAsync<ApiData<MaterialPickingMstVM>, MaterialPickingMstVM>(apiurl, data);
            return mst;
        }
        public async Task<List<Printer_Profile>> GetPrinterProfiles(int SiteId)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetPrinterProfile?siteID=" + SiteId;
            var print = await _genericRepository.GetAsync<List<Printer_Profile>>(apiurl);
            return print;
        }
        public async Task<IEnumerable<ReceivePlanRecheckCauseVM>> GetRecheckCauseAsync()
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetRecheckCause?siteID=" + _settingsService.SiteIdSetting;
            var dtl = await _genericRepository.GetAsync<List<ReceivePlanRecheckCauseVM>>(apiurl);
            return dtl;
        }

        public async Task<IEnumerable<MaterialMatchingVM>> GetMaterialProductionCrossSiteAsync(string rmId, int ownerSite, int productionSite)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialProductionCrossSite?rmId=" + rmId + "&ownerSite=" + ownerSite + "&productionSite=" + productionSite;
            var datamodel = await _genericRepository.GetAsync<List<MaterialMatchingVM>>(apiurl);
            return datamodel;
        }

        public async Task<MaterialReceivePlanMst> GetReceivePlanByBarcode(string barcode)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetReceivePlanByBarcode?siteId=" + _settingsService.SiteIdSetting
                + "&barcode=" + barcode;

            var data = await _genericRepository.GetAsync<MaterialReceivePlanMst>(apiurl);

            return data;
        }

        public async Task<byte[]> GetDeliveryReport(DeliveryReportGetConditionVM reportGetCondition)
        {
            var data = new ApiData<DeliveryReportGetConditionVM>()
            {
                Site_ID = Convert.ToInt32(_settingsService.SiteIdSetting),
                MyData = reportGetCondition
            };

            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetDeliveryReport";
            string json = JsonConvert.SerializeObject(data);
            var reportBytes = await _genericRepository.PostAsync<ApiData<DeliveryReportGetConditionVM>, byte[]>(apiurl, data);
            return reportBytes;
        }

        public async Task<List<MaterialRequestMSTReferenceVM>> GetMaterialRequestMSTReferences(int requestMstId)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/GetMaterialRequestMSTReferences?mstid=" + requestMstId
                + "&siteId=" + _settingsService.SiteIdSetting;
            return await _genericRepository.GetAsync<List<MaterialRequestMSTReferenceVM>>(apiurl);
        }

        public async Task<RmGHSMatchMstVM> GetReportGHSByRM(string rmid, int Operation_Site_ID)
        {
            string apiurl = ApiConstants.BaseApiUrl + ApiConstants.MaterialAutoReceivingEndpoint + "/getReportGHSByRM?rmid=" + rmid + "&Operation_Site_ID=" + Operation_Site_ID; ;
            var GHSmst = await _genericRepository.GetAsync<RmGHSMatchMstVM>(apiurl);
            return GHSmst;
        }
    }
}
