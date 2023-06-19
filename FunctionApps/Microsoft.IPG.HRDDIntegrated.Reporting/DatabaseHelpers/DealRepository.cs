namespace Microsoft.IPG.HRDDIntegrated.Reporting.DatabaseHelpers
{
    using Microsoft.Extensions.Logging;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Entities;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Enums;
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Deal Repository class
    /// </summary>
    internal class DealRepository
    {
        /// <summary>
        /// Get Excel Report
        /// </summary>
        /// <param name="dealRequest"></param>
        /// <param name="includeAllRevisions"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<DataSet> GetExcelReport(DealRequest dealRequest, bool includeAllRevisions, int pageIndex, ILogger log)
        {
            var parameters = new SqlParameterList();
            parameters.AddVarcharParameter("@alias", dealRequest.CurrentUser.Alias);

            parameters.AddBooleanParameter("@FlaggedDeal", (dealRequest.FilterTypeInput.DNEQuotes || dealRequest.FilterTypeInput.PAMQuotes || dealRequest.FilterTypeInput.SOEQuotes) || dealRequest.IsUnflaggedDeal ? false : true);
            parameters.AddBooleanParameter("@AzureInvolvedFlagged", (dealRequest.FilterInput.FilterType == (int)QuoteHeaders.AzureInvolvedFlagged || dealRequest.FilterInput.FilterType == (int)QuoteHeaders.AzureInvolvedUnflagged) ? true : false);
            parameters.AddStructuredParameter("@FiltersInfo", ConvertToDataTable(dealRequest.FilterInput), "PRTHRDD.FiltersData");
            parameters.AddBooleanParameter("@AnalyticFlagged", dealRequest.FilterTypeInput.AnalyticalFlagged);
            parameters.AddBooleanParameter("@ManualFlagged", dealRequest.FilterTypeInput.ManualFlagged);
            parameters.AddBooleanParameter("@BothFlagged", dealRequest.FilterTypeInput.BothFlaggged);
            parameters.AddBooleanParameter("@AssignedDeal", dealRequest.FilterTypeInput.AssignedDeals);
            parameters.AddBooleanParameter("@UnAssignedDeal", dealRequest.FilterTypeInput.UnAssignedDeals);
            parameters.AddBooleanParameter("@MyDeals", dealRequest.FilterTypeInput.IsMyDeals);
            parameters.AddVarcharParameter("@DealState", dealRequest.FilterTypeInput.DealState == null ? string.Empty : dealRequest.FilterTypeInput.DealState);
            parameters.AddBooleanParameter("@Trade", dealRequest.FilterTypeInput.Trade);
            parameters.AddBooleanParameter("@OneVet", dealRequest.FilterTypeInput.OneVet);
            parameters.AddBooleanParameter("@OnHold", dealRequest.FilterTypeInput.OnHold);
            parameters.AddBooleanParameter("@PostFinalFlagged", dealRequest.FilterTypeInput.PostFinalFlagged);
            parameters.AddBooleanParameter("@My_PostFinalFlagged", dealRequest.FilterTypeInput.MyPostFinalFlagged);
            parameters.AddBooleanParameter("@DNE", dealRequest.FilterTypeInput.DNEQuotes);
            parameters.AddBooleanParameter("@PAM", dealRequest.FilterTypeInput.PAMQuotes);
            parameters.AddBooleanParameter("@SOE", dealRequest.FilterTypeInput.SOEQuotes);
            parameters.AddIntParameter("@PageIndex", pageIndex);
            parameters.AddIntParameter("@PageSize", int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            if (dealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotY)
            {
                parameters.AddVarcharParameter("@Pilot", "Yes");
            }
            else if (dealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotN)
            {
                parameters.AddVarcharParameter("@Pilot", "No");
            }
            string storedProcedure = includeAllRevisions ? "PRTHRDD.usp_GetExcelReportAllData" : "PRTHRDD.usp_GetExcelReport";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet dsDeals = await SqlHelper.ExecuteDataset(Environment.GetEnvironmentVariable("EADBConnection"),
                CommandType.StoredProcedure,
                storedProcedure,
                parameters.ToArray());

            stopwatch.Stop();
            log.LogInformation($"GetExcelReport - Time taken to get {pageIndex} batch records from DB : {stopwatch.Elapsed.TotalMinutes} Mins");

            return dsDeals;
        }

        /// <summary>
        /// Fetch flagged and unflagged quotes with all revisions 
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<DataSet> GetExcelReportAllQuotes(ExcelExportRequest excelExportRequest, int pageIndex, ILogger log)
        {
            var parameters = new SqlParameterList();
            parameters.AddVarcharParameter("@alias", excelExportRequest.DealRequest.CurrentUser.Alias);
            parameters.AddStructuredParameter("@FiltersInfo", ConvertToDataTable(excelExportRequest.DealRequest.FilterInput), "PRTHRDD.FiltersData");
            parameters.AddIntParameter("@PageIndex", pageIndex);
            parameters.AddIntParameter("@PageSize", int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));
            //For online getexcel , when count>10k call GetAllDealsExcelReport sp
            string storedProcedure = excelExportRequest.IncludeRevisions ? "PRTHRDD.usp_GetExcelReportAllDataForFlaggedAndUnFlagged" : "PRTHRDD.usp_GetAllDealsExcelReport";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet dsDeals = await SqlHelper.ExecuteDataset(Environment.GetEnvironmentVariable("EADBConnection"),
                CommandType.StoredProcedure,
                storedProcedure,
                parameters.ToArray());
            
            stopwatch.Stop();
            log.LogInformation($"GetExcelReportAllQuotes - Time taken to get {pageIndex} batch records from DB : {stopwatch.Elapsed.TotalMinutes} Mins");

            return dsDeals;
        }

        /// <summary>
        /// Get Export Excel for Failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<DataSet> GetExportExcelForMSQuoteFailedQuotes(ExcelExportRequest excelExportRequest, int pageIndex, ILogger log)
        {
            var parameters = new SqlParameterList();
            parameters.AddVarcharParameter("@alias", excelExportRequest.DealRequest.CurrentUser.Alias);
            parameters.AddBooleanParameter("@FailFlag", !excelExportRequest.DealRequest.IsUnflaggedDeal);
            parameters.AddBooleanParameter("@MyDeals", excelExportRequest.DealRequest.FilterTypeInput.IsMyDeals);
            parameters.AddStructuredParameter("@FiltersInfo", ConvertToDataTable(excelExportRequest.DealRequest.FilterInput), "PRTHRDD.FiltersData");
            parameters.AddIntParameter("@PageIndex", pageIndex);
            parameters.AddIntParameter("@PageSize", int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            string storedProcedure = "PRTHRDD.usp_GetExcelReportFailedDealsAllData";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet dsDeals = await SqlHelper.ExecuteDataset(Environment.GetEnvironmentVariable("EADBConnection"),
                CommandType.StoredProcedure,
                storedProcedure,
                parameters.ToArray());

            stopwatch.Stop();
            log.LogInformation($"GetExportExcelForMSQuoteFailedQuotes - Time taken to get {pageIndex} batch records from DB : {stopwatch.Elapsed.TotalMinutes} Mins");

            return dsDeals;
        }

        /// <summary>
        /// Get Export to Excel for CRM failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<DataSet> GetExportExcelForCRMFailedQuotes(ExcelExportRequest excelExportRequest, int pageIndex, ILogger log)
        {
            var parameters = new SqlParameterList();
            parameters.AddVarcharParameter("@alias", excelExportRequest.DealRequest.CurrentUser.Alias);
            parameters.AddBooleanParameter("@MyDeals", excelExportRequest.DealRequest.FilterTypeInput.IsMyDeals);
            parameters.AddStructuredParameter("@FiltersInfo", ConvertToDataTable(excelExportRequest.DealRequest.FilterInput), "PRTHRDD.FiltersData");
            parameters.AddIntParameter("@PageIndex", pageIndex);
            parameters.AddIntParameter("@PageSize", int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));
            string storedProcedure = "PRTHRDD.usp_GetExcelReportCRMFailedDealsAllData";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet dsDeals = await SqlHelper.ExecuteDataset(Environment.GetEnvironmentVariable("EADBConnection"),
                CommandType.StoredProcedure,
                storedProcedure,
                parameters.ToArray());

            stopwatch.Stop();
            log.LogInformation($"GetExportExcelForCRMFailedQuotes - Time taken to get {pageIndex} batch records from DB : {stopwatch.Elapsed.TotalMinutes} Mins");

            return dsDeals;
        }

        /// <summary>
        /// Get Export to Excel for CRM failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<DataSet> GetExportExcelForPilotQuotes(ExcelExportRequest excelExportRequest, int pageIndex, ILogger log)
        {
            var parameters = new SqlParameterList();
            parameters.AddVarcharParameter("@alias", excelExportRequest.DealRequest.CurrentUser.Alias);
            parameters.AddBooleanParameter("@MyDeals", excelExportRequest.DealRequest.FilterTypeInput.IsMyDeals);
            parameters.AddStructuredParameter("@FiltersInfo", ConvertToDataTable(excelExportRequest.DealRequest.FilterInput), "PRTHRDD.FiltersData");
            parameters.AddVarcharParameter("@Pilot", excelExportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotY ? "Yes" : "No");
            parameters.AddIntParameter("@PageIndex", pageIndex);
            parameters.AddIntParameter("@PageSize", int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));
            string storedProcedure = excelExportRequest.IncludeRevisions ? "[PRTHRDD].[usp_GetExcelReportPilotDealsAllData]" : "[PRTHRDD].[usp_GetExcelReportPilotDeals]";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataSet dsDeals = await SqlHelper.ExecuteDataset(Environment.GetEnvironmentVariable("EADBConnection"),
                CommandType.StoredProcedure,
                storedProcedure,
                parameters.ToArray());

            stopwatch.Stop();
            log.LogInformation($"GetExportExcelForPilotQuotes - Time taken to get {pageIndex} batch records from DB : {stopwatch.Elapsed.TotalMinutes} Mins");

            return dsDeals;
        }

        private DataTable ConvertToDataTable(FilterInput filterInput)
        {
            DataTable filterInputData = new DataTable();
            filterInputData.Columns.Add("filterTypeInputlterName");
            filterInputData.Columns.Add("filterTypeInputlterData");
            if (filterInput.QuoteID != null && filterInput.QuoteID != 0)
            {
                filterInputData.Rows.Add(new object[] { "QuoteID", filterInput.QuoteID });
            }
            if (!string.IsNullOrEmpty(filterInput.SelectedArea))
            {
                filterInputData.Rows.Add(new object[] { "Area", filterInput.SelectedArea });
            }
            if (!string.IsNullOrEmpty(filterInput.SelectedSalesLocation))
            {
                filterInputData.Rows.Add(new object[] { "SalesLocation", filterInput.SelectedSalesLocation });
            }
            if (!string.IsNullOrEmpty(filterInput.CustomerName))
            {
                filterInputData.Rows.Add(new object[] { "CustomerName", filterInput.CustomerName });
            }
            if (!string.IsNullOrEmpty(filterInput.PartnerName))
            {
                filterInputData.Rows.Add(new object[] { "PartnerName", filterInput.PartnerName });
            }
            if (!string.IsNullOrEmpty(filterInput.SelectedState))
            {
                filterInputData.Rows.Add(new object[] { "StageID", filterInput.SelectedState });
            }
            if (!string.IsNullOrEmpty(filterInput.AssignedTo))
            {
                filterInputData.Rows.Add(new object[] { "AssignedTo", filterInput.AssignedTo });
            }
            if (!string.IsNullOrEmpty(filterInput.MlPrediction))
            {
                filterInputData.Rows.Add(new object[] { "MlPrediction", filterInput.MlPrediction });
            }
            if (!string.IsNullOrEmpty(filterInput.QuarterFirstFlagged))
            {
                filterInputData.Rows.Add(new object[] { "QuarterFirstFlagged", filterInput.QuarterFirstFlagged });
            }
            if (!string.IsNullOrEmpty(filterInput.TradeScreenStatus))
            {
                filterInputData.Rows.Add(new object[] { "TradeScreeningStatus", filterInput.TradeScreenStatus });
            }
            if (!string.IsNullOrEmpty(filterInput.ManuallyFlaggedCategory))
            {
                filterInputData.Rows.Add(new object[] { "ManuallyFlaggedCategory", filterInput.ManuallyFlaggedCategory });
            }
            if (!string.IsNullOrEmpty(filterInput.RiskScoreFbi))
            {
                filterInputData.Rows.Add(new object[] { "TotalScore", filterInput.RiskScoreFbi });
            }
            if (!string.IsNullOrEmpty(filterInput.RiskScoreUst))
            {
                filterInputData.Rows.Add(new object[] { "USTTotalScore", filterInput.RiskScoreUst });
            }
            if (!string.IsNullOrEmpty(filterInput.LandingProbability))
            {
                filterInputData.Rows.Add(new object[] { "DealLandingPrediction", filterInput.LandingProbability });
            }
            if (!string.IsNullOrEmpty(filterInput.Segmentation))
            {
                filterInputData.Rows.Add(new object[] { "Segmentation", filterInput.Segmentation });
            }
            if (!string.IsNullOrEmpty(filterInput.InScope))
            {
                filterInputData.Rows.Add(new object[] { "InScope", filterInput.InScope });
            }
            return filterInputData;
        }
    }
}
