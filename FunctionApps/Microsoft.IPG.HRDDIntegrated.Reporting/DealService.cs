namespace Microsoft.IPG.HRDDIntegrated.Reporting
{
    using Microsoft.Extensions.Logging;
    using Microsoft.IPG.HRDDIntegrated.Reporting.DatabaseHelpers;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Entities;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Enums;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Interface;
    using Microsoft.IPG.HRDDIntegrated.Reporting.StorageHelpers;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Deal Service Class
    /// </summary>
    internal class DealService
    {
        public async Task<bool> GenerateExcelReport(ExcelExportRequest exportRequest, ILogger log)
        {
            bool isUploaded = false;
            int recordCount = 0, pageIndex = 1;

            DealRepository dealRepository = new DealRepository();
            exportRequest.DealRequest.FilterTypeInput = await PrepareFilterTypeInput(exportRequest.DealRequest.FilterInput?.FilterType, exportRequest.DealRequest.FilterInput);

            IExcelExport excelBatchExport = new ExcelExport();
            string fileName = $"{exportRequest.JobId}.csv";
            bool isHeaderAppended = false;
            do
            {
                DataSet dsDealsInfo = await dealRepository.GetExcelReport(exportRequest.DealRequest, exportRequest.IncludeRevisions, pageIndex, log);
                var dtCloned = exportRequest.IncludeRevisions ? FormatAllDatatableColumns(dsDealsInfo.Tables[0], exportRequest.DealRequest.IsUnflaggedDeal) : FormatDatatableColumns(dsDealsInfo.Tables[0], exportRequest.DealRequest.IsUnflaggedDeal);

                recordCount = dsDealsInfo.Tables[0].Rows.Count;
                byte[] excelBatchWorkbook = excelBatchExport.ExportToCsv(dtCloned, isHeaderAppended);

                // Save to blob and return the blob url
                isUploaded = await StorageUtility.UploadFileAsync(Environment.GetEnvironmentVariable("ContainerName").ToString(), fileName, excelBatchWorkbook, pageIndex, log);
                pageIndex += 1;
                isHeaderAppended = true;
            } while (recordCount == int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            return isUploaded;
        }

        /// <summary>
        /// Get flagged and unflagged deals with all revisions
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<bool> GenerateExcelReportAllQuotes(ExcelExportRequest excelExportRequest, ILogger log)
        {
            bool isUploaded = false;
            int recordCount = 0, pageIndex = 1;

            DealRepository dealRepository = new DealRepository();
            IExcelExport excelBatchExport = new ExcelExport();
            string fileName = $"{excelExportRequest.JobId}.csv";
            bool isHeaderAppended = false;
            do
            {
                DataSet dsDealsInfo = await dealRepository.GetExcelReportAllQuotes(excelExportRequest, pageIndex, log);
                //Always send isUnflaggeddeals = false as the excel needs to have flagged deals template
                var dtCloned = excelExportRequest.IncludeRevisions ? FormatAllDatatableColumns(dsDealsInfo.Tables[0], false) : FormatDatatableColumns(dsDealsInfo.Tables[0], false);

                recordCount = dsDealsInfo.Tables[0].Rows.Count;
                byte[] excelBatchWorkbook = excelBatchExport.ExportToCsv(dtCloned, isHeaderAppended);

                // Save to blob and return the blob url
                isUploaded = await StorageUtility.UploadFileAsync(Environment.GetEnvironmentVariable("ContainerName").ToString(), fileName, excelBatchWorkbook, pageIndex, log);
                pageIndex += 1;
                isHeaderAppended = true;
            } while (recordCount == int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            return isUploaded;
        }

        /// <summary>
        /// Get Flagged and Unflagged failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<bool> GenerateExcelReportforMSQuoteFailedQuotes(ExcelExportRequest excelExportRequest, ILogger log)
        {
            bool isUploaded = false;
            int recordCount = 0, pageIndex = 1;
            bool isHeaderAppended = false;

            DealRepository dealRepository = new DealRepository();
            IExcelExport excelBatchExport = new ExcelExport();
            string fileName = $"{excelExportRequest.JobId}.csv";
            do
            {
                DataSet dsDealsInfo = await dealRepository.GetExportExcelForMSQuoteFailedQuotes(excelExportRequest, pageIndex, log);
                //Always send isUnflaggeddeals = false as the excel needs to have flagged deals template
                var dtCloned = excelExportRequest.IncludeRevisions ? FormatAllDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal) : FormatDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal);

                recordCount = dsDealsInfo.Tables[0].Rows.Count;
                byte[] excelBatchWorkbook = excelBatchExport.ExportToCsv(dtCloned, isHeaderAppended);

                // Save to blob and return the blob url
                isUploaded = await StorageUtility.UploadFileAsync(Environment.GetEnvironmentVariable("ContainerName").ToString(), fileName, excelBatchWorkbook, pageIndex, log);
                pageIndex += 1;
                isHeaderAppended = true;

            } while (recordCount == int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            return isUploaded;
        }

        /// <summary>
        /// Get CRM failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<bool> GenerateExcelReportforCRMFailedQuotes(ExcelExportRequest excelExportRequest, ILogger log)
        {
            bool isUploaded = false;
            int recordCount = 0, pageIndex = 1;
            bool isHeaderAppended = false;

            DealRepository dealRepository = new DealRepository();
            IExcelExport excelBatchExport = new ExcelExport();
            string fileName = $"{excelExportRequest.JobId}.csv";
            do
            {
                DataSet dsDealsInfo = await dealRepository.GetExportExcelForCRMFailedQuotes(excelExportRequest, pageIndex, log);
                //Always send isUnflaggeddeals = false as the excel needs to have flagged deals template
                var dtCloned = excelExportRequest.IncludeRevisions ? FormatAllDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal) : FormatDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal);

                recordCount = dsDealsInfo.Tables[0].Rows.Count;
                byte[] excelBatchWorkbook = excelBatchExport.ExportToCsv(dtCloned, isHeaderAppended);

                // Save to blob and return the blob url
                isUploaded = await StorageUtility.UploadFileAsync(Environment.GetEnvironmentVariable("ContainerName").ToString(), fileName, excelBatchWorkbook, pageIndex, log);
                pageIndex += 1;
                isHeaderAppended = true;

            } while (recordCount == int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            return isUploaded;
        }

        /// <summary>
        /// Get CRM failed Quotes
        /// </summary>
        /// <param name="excelExportRequest"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<bool> GenerateExcelReportforPilotQuotes(ExcelExportRequest excelExportRequest, ILogger log)
        {
            bool isUploaded = false;
            int recordCount = 0, pageIndex = 1;
            bool isHeaderAppended = false;

            DealRepository dealRepository = new DealRepository();
            IExcelExport excelBatchExport = new ExcelExport();
            string fileName = $"{excelExportRequest.JobId}.csv";
            do
            {
                DataSet dsDealsInfo = await dealRepository.GetExportExcelForPilotQuotes(excelExportRequest, pageIndex, log);
                //Always send isUnflaggeddeals = false as the excel needs to have flagged deals template
                var dtCloned = excelExportRequest.IncludeRevisions ? FormatAllDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal) : FormatDatatableColumns(dsDealsInfo.Tables[0], excelExportRequest.DealRequest.IsUnflaggedDeal);

                recordCount = dsDealsInfo.Tables[0].Rows.Count;
                byte[] excelBatchWorkbook = excelBatchExport.ExportToCsv(dtCloned, isHeaderAppended);

                // Save to blob and return the blob url
                isUploaded = await StorageUtility.UploadFileAsync(Environment.GetEnvironmentVariable("ContainerName").ToString(), fileName, excelBatchWorkbook, pageIndex, log);
                pageIndex += 1;
                isHeaderAppended = true;

            } while (recordCount == int.Parse(Environment.GetEnvironmentVariable("ExcelBatchProcess")));

            return isUploaded;
        }

        private async Task<FilterTypeInput> PrepareFilterTypeInput(int? filterType, FilterInput filterInput)
        {
            if (filterType != null && filterType.Value <= 0)
            {
                return new FilterTypeInput();
            }

            var filterTypeInput = new FilterTypeInput();

            filterTypeInput.AnalyticalFlagged = filterType == (int)QuoteHeaders.AnalyticallyFlagged;
            filterTypeInput.AssignedDeals = filterType == (int)QuoteHeaders.AllAssigned;
            filterTypeInput.UnAssignedDeals = filterType == (int)QuoteHeaders.AllUnAssigned;
            filterTypeInput.BothFlaggged = filterType == (int)QuoteHeaders.BothFlagged;
            filterTypeInput.ManualFlagged = filterType == (int)QuoteHeaders.ManuallyFlagged;
            filterTypeInput.IsMyDeals = filterType == (int)QuoteHeaders.MyTotalDeals;
            filterTypeInput.OnHold = filterType == (int)QuoteHeaders.OnHold;
            filterTypeInput.Trade = filterType == (int)QuoteHeaders.Trade;
            filterTypeInput.OneVet = filterType == (int)QuoteHeaders.OneVet;
            filterTypeInput.PostFinalFlagged = filterType == (int)QuoteHeaders.PostFinalFlagged;
            filterTypeInput.MyPostFinalFlagged = filterType == (int)QuoteHeaders.MyPostFinalFlagged;
            filterTypeInput.DNEQuotes = filterType == (int)QuoteHeaders.DNE;
            filterTypeInput.PAMQuotes = filterType == (int)QuoteHeaders.PAM;
            filterTypeInput.SOEQuotes = filterType == (int)QuoteHeaders.SOE;

            if (filterType == (int)QuoteHeaders.AllOpen
               || filterType == (int)QuoteHeaders.MyOpenDeals)
            {
                filterTypeInput.DealState = "open";
            }
            else if (filterType == (int)QuoteHeaders.AllClosed
                || filterType == (int)QuoteHeaders.MyClosedDeals)
            {
                filterTypeInput.DealState = "closed";
            }

            if (filterType == (int)QuoteHeaders.MyClosedDeals
                || filterType == (int)QuoteHeaders.MyOpenDeals || filterType == (int)QuoteHeaders.MyAnalyticallyFlagged
                || filterType == (int)QuoteHeaders.MyManuallyFlagged || filterType == (int)QuoteHeaders.MyBothFlagged
                || filterType == (int)QuoteHeaders.MyPostFinalFlagged)
            {
                filterTypeInput.IsMyDeals = true;
            }
            return await Task.FromResult(filterTypeInput);
        }

        /// <summary>
        /// Clones the datatable and returns the formatted datatable for excel report all data
        /// </summary>
        /// <param name="dealInfo"></param>
        /// <param name="unflaggedDeals"></param>
        /// <returns>DataTable</returns>
        private DataTable FormatAllDatatableColumns(DataTable dealInfo, bool unflaggedDeals)
        {
            // Clone datatable to change the column data type to string.
            DataTable dtCloned;
            dtCloned = dealInfo.Clone();
            dtCloned.Columns["DollarAmount"].DataType = typeof(string);
            dtCloned.Columns["FinalNetListPrice"].DataType = typeof(string);
            dtCloned.Columns["FinalNetSalesPrice"].DataType = typeof(string);
            dtCloned.Columns["DiscountAmount"].DataType = typeof(string);
            dtCloned.Columns["DiscountPercent"].DataType = typeof(string);
            dtCloned.Columns["FinalNetSalesPriceLC"].DataType = typeof(string);
            dtCloned.Columns["FinalNetListPriceLC"].DataType = typeof(string);
            dtCloned.Columns["AzureFinalNetSalesPrice"].DataType = typeof(string);

            if (!unflaggedDeals)
            {
                dtCloned.Columns["ExchangeRateForLocalCurrency"].DataType = typeof(string);
                dtCloned.Columns["FirstFlaggedDate"].DataType = typeof(string);
            }
            foreach (DataRow row in dealInfo.Rows)
            {
                dtCloned.ImportRow(row);
            }
            dtCloned.TableName = unflaggedDeals ? "UnflaggedDeals" : "FlaggedDeals";

            //Change the column names.
            dtCloned.Columns["QuoteID"].ColumnName = "Quote ID";
            dtCloned.Columns["MasterID"].ColumnName = "Master ID";
            dtCloned.Columns["RevisionID"].ColumnName = "Revision ID";
            dtCloned.Columns["QuoteDisplayID"].ColumnName = "Quote Display ID";
            dtCloned.Columns["RevisionNumber"].ColumnName = "Revision Number";
            dtCloned.Columns["CustomerName"].ColumnName = "Customer Name";
            dtCloned.Columns["PartnerName"].ColumnName = "Partner Name";
            dtCloned.Columns["SalesLocation"].ColumnName = "Sales Location";
            dtCloned.Columns["DollarAmount"].ColumnName = "Amount";
            dtCloned.Columns["DealSizeBucket"].ColumnName = "Deal Size Bucket";
            dtCloned.Columns["IsRenewal"].ColumnName = "Is Renewal";
            dtCloned.Columns["IsMSUserModified"].ColumnName = "Is MSUser Modified";
            dtCloned.Columns["HeaderCreatedDate"].ColumnName = "Header Created Date";
            dtCloned.Columns["CreatedDate"].ColumnName = "Create dDate";
            dtCloned.Columns["CreatedDateID"].ColumnName = "Created Date ID";
            dtCloned.Columns["CreatedBy"].ColumnName = "Created By";
            dtCloned.Columns["IsFinalizeRevision"].ColumnName = "Is Finalize Revision";
            dtCloned.Columns["DiscountingMode"].ColumnName = "Discounting Mode";
            dtCloned.Columns["LocaleCountryCode"].ColumnName = "Locale Country Code";
            dtCloned.Columns["FiscalYear"].ColumnName = "Fiscal Year";
            dtCloned.Columns["FiscalQuarter"].ColumnName = "Fiscal Quarter";
            dtCloned.Columns["CountryName"].ColumnName = "Country Name";
            dtCloned.Columns["RegionCode"].ColumnName = "Region Code";
            dtCloned.Columns["EmergingOrDeveloped"].ColumnName = "Emerging Or Developed";
            dtCloned.Columns["Category"].ColumnName = "Category";
            dtCloned.Columns["CodeValue"].ColumnName = "CodeValue";
            dtCloned.Columns["QuoteTypeCode"].ColumnName = "Quote Type Code";
            dtCloned.Columns["FrameworkID"].ColumnName = "Framework ID";
            dtCloned.Columns["FrameworkName"].ColumnName = "Framework Name";
            dtCloned.Columns["FrameworkCode"].ColumnName = "Framework Code";
            dtCloned.Columns["FrameworkDiscount"].ColumnName = "Framework Discount";
            dtCloned.Columns["FrameworkFlag"].ColumnName = "Framework Flag";
            dtCloned.Columns["BDSGScore"].ColumnName = "BDSG Score";
            dtCloned.Columns["CustomerPCN"].ColumnName = "Customer PCN";
            dtCloned.Columns["CustomerPreferredExternalName"].ColumnName = "Customer Preferred External Name";
            dtCloned.Columns["PartnerPCN"].ColumnName = "Partner PCN";
            dtCloned.Columns["PartnerPreferredExternalName"].ColumnName = "Partner Preferred External Name";
            dtCloned.Columns["MasterCountryName"].ColumnName = "Master Country Name";
            dtCloned.Columns["GeoRiskTier"].ColumnName = "Geo Risk Tier";
            dtCloned.Columns["RegionName"].ColumnName = "Region Name";
            dtCloned.Columns["MasterCustomerOneID"].ColumnName = "Master CustomerOne ID";
            dtCloned.Columns["MasterPartnerOneID"].ColumnName = "Master PartnerOne ID";
            dtCloned.Columns["GovernmentFlag"].ColumnName = "Government Flag";
            dtCloned.Columns["ChangeInPartner"].ColumnName = "Change In Partner";
            dtCloned.Columns["IsSOE"].ColumnName = "Is SOE";
            dtCloned.Columns["PartnerCount"].ColumnName = "Partner Count";
            dtCloned.Columns["PEPFlag"].ColumnName = "PEP Flag";
            dtCloned.Columns["DealSize"].ColumnName = "Deal Size";
            dtCloned.Columns["PoolName"].ColumnName = "Pool Name";
            dtCloned.Columns["LicenseTypeName"].ColumnName = "License Type Name";
            dtCloned.Columns["NumberOfRevisions"].ColumnName = "Number Of Revisions";
            dtCloned.Columns["LargeDiscountCustomerClusterID"].ColumnName = "Large Discount Customer ClusterID";
            dtCloned.Columns["LargeDiscountCustomerClusterScore"].ColumnName = "Large Discount Customer Cluster Score";
            dtCloned.Columns["LargeDiscountPartnerClusterID"].ColumnName = "Large Discount Partner ClusterID";
            dtCloned.Columns["LargeDiscountPartnerClusterScore"].ColumnName = "Large Discount Partner Cluster Score";
            dtCloned.Columns["LargeDiscountQuoteClusterID"].ColumnName = "Large Discount Quote ClusterID";
            dtCloned.Columns["LargeDiscountQuoteClusterScore"].ColumnName = "Large Discount Quote Cluster Score";
            dtCloned.Columns["TrendCustomerClusterID"].ColumnName = "TrendCustomerClusterID";
            dtCloned.Columns["TrendCustomerClusterScore"].ColumnName = "Trend Customer Cluster Score";
            dtCloned.Columns["TrendPartnerClusterID"].ColumnName = "TrendPartnerClusterID";
            dtCloned.Columns["TrendPartnerClusterScore"].ColumnName = "TrendPartnerClusterScore";
            dtCloned.Columns["ECIFEOQScore"].ColumnName = "ECIFEOQ Score";
            dtCloned.Columns["ECIFSameMonthSaleScore"].ColumnName = "ECIF Same Month Sale Score";
            dtCloned.Columns["ECIFEADealScore"].ColumnName = "ECIFE ADeal Score";
            dtCloned.Columns["ECIFProjectRankScore"].ColumnName = "ECIF Project Rank Score";
            dtCloned.Columns["ECIFSamePartnerScore"].ColumnName = "ECIF Same Partner Score";
            dtCloned.Columns["TimingScore1"].ColumnName = "Timing Score1";
            dtCloned.Columns["TimingScore2"].ColumnName = "Timing Score2";
            dtCloned.Columns["TimingScore3"].ColumnName = "Timing Score3";
            dtCloned.Columns["TimingScore4"].ColumnName = "Timing Score4";
            dtCloned.Columns["SalesAmount"].ColumnName = "Sales Amount";
            dtCloned.Columns["GeoScore"].ColumnName = "Geo Score";
            dtCloned.Columns["GeoScoreIndicator"].ColumnName = "Geo Score Indicator";
            dtCloned.Columns["PartnerScore"].ColumnName = "Partner Score";
            dtCloned.Columns["PartnerScoreIndicator"].ColumnName = "Partner Score Indicator";
            dtCloned.Columns["SOEScore"].ColumnName = "SOE Score";
            dtCloned.Columns["SOEScoreIndicator"].ColumnName = "SOE Score Indicator";
            dtCloned.Columns["DiscountScore"].ColumnName = "Discount Score";
            dtCloned.Columns["DiscountScoreIndicator"].ColumnName = "Discount Score Indicator";
            dtCloned.Columns["TrendScore"].ColumnName = "Trend Score";
            dtCloned.Columns["TrendScoreIndicator"].ColumnName = "Trend Score Indicator";
            dtCloned.Columns["ECIFScore"].ColumnName = "ECIF Score";
            dtCloned.Columns["ECIFScoreIndicator"].ColumnName = "ECIF Score Indicator";
            dtCloned.Columns["TimingScore"].ColumnName = "Timing Score";
            dtCloned.Columns["TimingScoreIndicator"].ColumnName = "Timing Score Indicator";
            dtCloned.Columns["FrameworkScore"].ColumnName = "Framework Score";
            dtCloned.Columns["FrameworkScoreIndicator"].ColumnName = "Framework Score Indicator";
            dtCloned.Columns["ServiceScore"].ColumnName = "Service Score";
            dtCloned.Columns["ServiceScoreIndicator"].ColumnName = "Service Score Indicator";
            dtCloned.Columns["WeightedGeographyScore"].ColumnName = "Weighted Geography Score";
            dtCloned.Columns["WeightedSOEScore"].ColumnName = "Weighted SOE Score";
            dtCloned.Columns["WeightedPartnerScore"].ColumnName = "Weighted Partner Score";
            dtCloned.Columns["WeightedTrendScore"].ColumnName = "Weighted Trend Score";
            dtCloned.Columns["WeightedLargeDiscountScore"].ColumnName = "Weighted Large Discount Score";
            dtCloned.Columns["WeightedFrameworkScore"].ColumnName = "Weighted Framework Score";
            dtCloned.Columns["WeightedServiceScore"].ColumnName = "Weighted Service Score";
            dtCloned.Columns["WeightedECIFScore"].ColumnName = "Weighted ECIF Score";
            dtCloned.Columns["WeightedTimingScore"].ColumnName = "Weighted Timing Score";
            dtCloned.Columns["PreviouslyFlagged"].ColumnName = "Previously Flagged";
            dtCloned.Columns["IsScoreIncreased"].ColumnName = "Is Score Increased";
            dtCloned.Columns["InsertedOn"].ColumnName = "Inserted On";
            dtCloned.Columns["NewLSSAliasName"].ColumnName = "New CE AliasName";
            dtCloned.Columns["LSSMappedOn"].ColumnName = "CE Mapped On";
            dtCloned.Columns["Segment"].ColumnName = "Segment";
            dtCloned.Columns["TotalScore"].ColumnName = "Risk Score FBI";
            dtCloned.Columns["USTTotalScore"].ColumnName = "Risk Score UST";
            dtCloned.Columns["DealLandingPrediction"].ColumnName = "Landing Probability";
            dtCloned.Columns["CPSRevisionNumber"].ColumnName = "CPS Revision Number";
            dtCloned.Columns["TrainingCycleID"].ColumnName = "Training Cycle ID";
            dtCloned.Columns["ScoringCycleID"].ColumnName = "Scoring Cycle ID";
            dtCloned.Columns["InsertDate"].ColumnName = "Insert Date";
            dtCloned.Columns["ModifiedDate"].ColumnName = "Modified Date";
            dtCloned.Columns["IsArchived"].ColumnName = "Is Archived";
            dtCloned.Columns["IsQuickQuote"].ColumnName = "Is Quick Quote";
            dtCloned.Columns["DealState"].ColumnName = "Deal State";
            dtCloned.Columns["AssignedTo"].ColumnName = "Assigned To";
            dtCloned.Columns["OpportunityID"].ColumnName = "Opportunity ID";
            dtCloned.Columns["ChannelModel"].ColumnName = "Channel Model";
            dtCloned.Columns["AzureConcession"].ColumnName = "Azure Concession";

            // Admin Global Settings Flag
            if (!unflaggedDeals)
            {
                dtCloned.Columns["OLCReviewFlag"].ColumnName = "OLC Review Flag";
                dtCloned.Columns["KeyLearningReviewFlag"].ColumnName = "Key Learning Review Flag";
                dtCloned.Columns["FeedbackToOperationsFlag"].ColumnName = "Feedback to Operations Flag";
                dtCloned.Columns["DealStatusUpdateDate"].ColumnName = "Deal Status Updated Date";
                dtCloned.Columns["CRMCaseID"].ColumnName = "CRM CaseID";
                dtCloned.Columns["ExchangeRateForLocalCurrency"].ColumnName = "Currency Exchange Rate (LC)";
                dtCloned.Columns["ReportingComment"].ColumnName = "Reporting Comments";
            }
            dtCloned.Columns["LSSDisplayName"].ColumnName = "CE Name";

            if (!unflaggedDeals)// columns for flagged deals
            {
                dtCloned.Columns["ManuallyFlaggedForReview"].ColumnName = "Manually Flagged For Review";
                dtCloned.Columns["FlaggedForReview"].ColumnName = "Flagged For Review";
                dtCloned.Columns["MRP"].ColumnName = "MRP (LC)";
                dtCloned.Columns["FirstFlaggedDate"].ColumnName = "First Flagged Date";
                dtCloned.Columns["QuarterUpdated"].ColumnName = "Quarter Updated";
                dtCloned.Columns["QuarterFirstFlagged"].ColumnName = "Quarter First Flagged";
                dtCloned.Columns["QuoteLevelFlagging"].ColumnName = "Quote Level Flagging";
                dtCloned.Columns["SystemFlagged-Date"].ColumnName = "System Flagged Date";
                dtCloned.Columns.Remove("PostFinalQuote");
            }
            dtCloned.Columns["QuoteStatus"].ColumnName = "Quote Status";
            dtCloned.Columns["QuoteStateCode"].ColumnName = "Quote State Code";
            dtCloned.Columns["QuoteState"].ColumnName = "Quote State";
            dtCloned.Columns["MOPETStatus"].ColumnName = "MOPET Status";
            dtCloned.Columns["IsFinal"].ColumnName = "Is Final";
            dtCloned.Columns["IsOpportunityValidated"].ColumnName = "Is Opportunity Validated";
            dtCloned.Columns["BDSGFlag"].ColumnName = "BDSG Flag";
            dtCloned.Columns["FinalNetListPrice"].ColumnName = "Net List Price (USD)";
            dtCloned.Columns["FinalNetSalesPrice"].ColumnName = "Net Sales Price (USD)";
            dtCloned.Columns["DiscountAmount"].ColumnName = "Discount Amount";
            dtCloned.Columns["DiscountPercent"].ColumnName = "Discount Percent";
            dtCloned.Columns["BillsInCurrencyCode"].ColumnName = "Bills in Currency Code";
            dtCloned.Columns["FinalNetSalesPriceLC"].ColumnName = "Net Sales Price LC";
            dtCloned.Columns["FinalNetListPriceLC"].ColumnName = "Net List Price LC";
            dtCloned.Columns.Remove("StageIdForSort");
            dtCloned.Columns.Remove("OneVetControl");
            dtCloned.Columns["Enrollment"].ColumnName = "Enrollment Number";
            dtCloned.Columns["EnrollmentStartDate"].ColumnName = "Agreement Start Date";
            dtCloned.Columns["ProposalID"].SetOrdinal(1);
            dtCloned.Columns["IsQuoteAzureInvolvedFlag"].ColumnName = "Quote Azure Involved Flag";
            dtCloned.Columns["AzureFinalNetSalesPrice"].ColumnName = "Azure Final Net Sales Price";
            if (dtCloned.Columns.Contains("DNEFlag"))
            {
                dtCloned.Columns["DNEFlag"].ColumnName = "TNA Flag";
            }

            foreach (DataRow drRow in dtCloned.Rows)
            {
                if (drRow.ExtractFieldValue<string>("Amount", "0") != "0")
                {
                    drRow["Amount"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Amount"])));
                }
                else
                {
                    drRow["Amount"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Net List Price (USD)", "0") != "0")
                {
                    drRow["Net List Price (USD)"] = string.Format("{0:$,#,#}", Math.Round(Convert.ToDouble(drRow["Net List Price (USD)"])));
                }
                else
                {
                    drRow["Net List Price (USD)"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Net Sales Price (USD)", "0") != "0")
                {
                    drRow["Net Sales Price (USD)"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Net Sales Price (USD)"])));
                }
                else
                {
                    drRow["Net Sales Price (USD)"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Discount Amount", "0") != "0")
                {
                    drRow["Discount Amount"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Discount Amount"])));
                }
                else
                {
                    drRow["Discount Amount"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Discount Percent", "0") != "0")
                {
                    drRow["Discount Percent"] = string.Format("{0}%", Math.Round(Convert.ToDouble(drRow["Discount Percent"])));
                }
                else
                {
                    drRow["Discount Percent"] = "0%";
                }
                if (drRow.ExtractFieldValue<string>("Net Sales Price LC", "0") != "0")
                {
                    drRow["Net Sales Price LC"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Net Sales Price LC"])));
                }
                else
                {
                    drRow["Net Sales Price LC"] = "0";
                }
                if (drRow.ExtractFieldValue<string>("Net List Price LC", "0") != "0")
                {
                    drRow["Net List Price LC"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Net List Price LC"])));
                }
                else
                {
                    drRow["Net List Price LC"] = "0";
                }

                if (drRow.ExtractFieldValue<string>("Azure Final Net Sales Price", "") != "" && drRow.ExtractFieldValue<string>("Azure Final Net Sales Price", "0") != "0")
                {
                    drRow["Azure Final Net Sales Price"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Azure Final Net Sales Price"])));
                }
                else
                {
                    drRow["Azure Final Net Sales Price"] = "0";
                }
                if (!unflaggedDeals)
                {
                    if (drRow.ExtractFieldValue<string>("Currency Exchange Rate (LC)", string.Empty) != string.Empty)
                    {
                        drRow["Currency Exchange Rate (LC)"] = string.Format("{0:0.##}", Convert.ToDecimal(drRow["Currency Exchange Rate (LC)"]));
                    }
                    else
                    {
                        drRow["Currency Exchange Rate (LC)"] = string.Empty;
                    }
                    drRow["First Flagged Date"] = !string.IsNullOrEmpty(drRow["First Flagged Date"].ToString()) ? ConvertToDateTime(drRow["First Flagged Date"]?.ToString()?.Trim()).ToShortDateString() : string.Empty;
                }
            }
            return dtCloned;
        }

        /// <summary>
        /// Clones the datatable and returns the formatted datatable for excel report
        /// </summary>
        /// <param name="dealInfo">The deal information.</param>
        /// <param name="unflaggedDeals">if set to <c>true</c> [unflagged deals].</param>
        /// <returns>
        /// DataTable
        /// </returns>
        private DataTable FormatDatatableColumns(DataTable dealInfo, bool unflaggedDeals)
        {
            // Clone datatable to change the column data type to string.
            DataTable dtCloned;
            dtCloned = dealInfo.Clone();
            dtCloned.Columns["DollarAmount"].DataType = typeof(string);
            dtCloned.Columns["FinalNetListPrice"].DataType = typeof(string);
            dtCloned.Columns["FinalNetSalesPrice"].DataType = typeof(string);
            dtCloned.Columns["DiscountAmount"].DataType = typeof(string);
            dtCloned.Columns["DiscountPercent"].DataType = typeof(string);
            dtCloned.Columns["FinalNetSalesPriceLC"].DataType = typeof(string);
            dtCloned.Columns["FinalNetListPriceLC"].DataType = typeof(string);
            dtCloned.Columns["AzureFinalNetSalesPrice"].DataType = typeof(string);

            if (!unflaggedDeals)
            {
                dtCloned.Columns["ExchangeRateForLocalCurrency"].DataType = typeof(string);
                dtCloned.Columns["FirstFlaggedDate"].DataType = typeof(string);
            }
            foreach (DataRow row in dealInfo.Rows)
            {
                dtCloned.ImportRow(row);
            }
            dtCloned.TableName = unflaggedDeals ? "UnflaggedDeals" : "FlaggedDeals";

            //Change the column names.
            dtCloned.Columns["QuoteID"].ColumnName = "Quote ID";
            dtCloned.Columns["ProposalID"].ColumnName = "Proposal ID";
            dtCloned.Columns["CustomerName"].ColumnName = "Customer Name";
            dtCloned.Columns["PartnerName"].ColumnName = "Partner Name";
            dtCloned.Columns["SalesLocation"].ColumnName = "Sales Location";
            dtCloned.Columns["DollarAmount"].ColumnName = "Amount";
            dtCloned.Columns["AverageRiskScore"].ColumnName = "Aggregate Risk Score";
            dtCloned.Columns["HighestRiskScore"].ColumnName = "Highest Risk Score";
            dtCloned.Columns["HighestVersionRiskScore"].ColumnName = "Highest Version Risk Score";
            dtCloned.Columns["StateName"].ColumnName = "State";
            dtCloned.Columns["AssignedTo"].ColumnName = "Assigned To";
            dtCloned.Columns["CustomerPCN"].ColumnName = "Customer PCN";
            dtCloned.Columns["PartnerPCN"].ColumnName = "Partner PCN";
            dtCloned.Columns["OpportunityID"].ColumnName = "Opportunity ID";

            // Admin Global Settings Flag
            if (!unflaggedDeals)
            {
                dtCloned.Columns["OLCReviewFlag"].ColumnName = "OLC Review Flag";
                dtCloned.Columns["KeyLearningReviewFlag"].ColumnName = "Key Learning Review Flag";
                dtCloned.Columns["FeedbackToOperationsFlag"].ColumnName = "Feedback to Operations Flag";
                dtCloned.Columns["DealStatusUpdateDate"].ColumnName = "Deal Status Updated Date";
                dtCloned.Columns["CRMCaseID"].ColumnName = "CRM CaseID";
                dtCloned.Columns["ExchangeRateForLocalCurrency"].ColumnName = "Currency Exchange Rate (LC)";
                dtCloned.Columns["ReportingComment"].ColumnName = "Reporting Comments";
            }

            // Deal and revision details
            dtCloned.Columns["FiscalQuarter"].ColumnName = "Fiscal Quarter";
            dtCloned.Columns["QuoteTitle"].ColumnName = "Quote Title";
            dtCloned.Columns["LSSName"].ColumnName = "CE Name";
            if (!unflaggedDeals)
            {
                dtCloned.Columns["DateFlagged"].ColumnName = "Flagged On";
                dtCloned.Columns["FlaggedForReview"].ColumnName = "Flagged For Review";
                dtCloned.Columns["MRP"].ColumnName = "MRP (LC)";
                dtCloned.Columns["FirstFlaggedDate"].ColumnName = "First Flagged Date";
                dtCloned.Columns["QuarterUpdated"].ColumnName = "Quarter Updated";
                dtCloned.Columns["QuarterFirstFlagged"].ColumnName = "Quarter First Flagged";
                dtCloned.Columns["QuoteLevelFlagging"].ColumnName = "Quote Level Flagging";
            }
            dtCloned.Columns["QuoteStatus"].ColumnName = "Quote Status";
            dtCloned.Columns["QuoteState"].ColumnName = "Quote State";
            dtCloned.Columns["MOPETStatus"].ColumnName = "MOPET Status";
            dtCloned.Columns["IsFinal"].ColumnName = "Is Final";
            dtCloned.Columns["IsOpportunityValidated"].ColumnName = "Is Opportunity Validated";
            dtCloned.Columns["BDSGFlag"].ColumnName = "BDSG Flag";
            dtCloned.Columns["FinalNetListPrice"].ColumnName = "Net List Price (USD)";
            dtCloned.Columns["FinalNetSalesPrice"].ColumnName = "Net Sales Price (USD)";
            dtCloned.Columns["DiscountAmount"].ColumnName = "Discount Amount";
            dtCloned.Columns["DiscountPercent"].ColumnName = "Discount Percent";
            dtCloned.Columns["BillsInCurrencyCode"].ColumnName = "Bills in Currency Code";
            dtCloned.Columns["FinalNetSalesPriceLC"].ColumnName = "Net Sales Price LC";
            dtCloned.Columns["FinalNetListPriceLC"].ColumnName = "Net List Price LC";
            dtCloned.Columns["SummaryStatus"].ColumnName = "Summary Status";
            dtCloned.Columns["SummaryStatusDetail"].ColumnName = "Summary Status Detail";
            dtCloned.Columns["ROCProcessingStatus"].ColumnName = "ROC Processing Status";
            dtCloned.Columns["ConvergenceStatus"].ColumnName = "Convergence Status";
            dtCloned.Columns["DealState"].ColumnName = "Deal State";
            dtCloned.Columns["TotalScore"].ColumnName = "Risk Score FBI";
            dtCloned.Columns["USTTotalScore"].ColumnName = "Risk Score UST";
            dtCloned.Columns["DealLandingPrediction"].ColumnName = "Landing Probability";
            dtCloned.Columns["Enrollment"].ColumnName = "Enrollment Number";
            dtCloned.Columns["EnrollmentStartDate"].ColumnName = "Agreement Start Date";
            dtCloned.Columns["IsQuoteAzureInvolvedFlag"].ColumnName = "Quote Azure Involved Flag";
            dtCloned.Columns["AzureFinalNetSalesPrice"].ColumnName = "Azure Final Net Sales Price";

            dtCloned.Columns.Remove("StageID");
            dtCloned.Columns.Remove("RevisionID");
            dtCloned.Columns.Remove("StageIdForSort");
            dtCloned.Columns.Remove("OneVetControl");
            dtCloned.Columns["Proposal ID"].SetOrdinal(1);
            if (dtCloned.Columns.Contains("DNEFlag"))
            {
                dtCloned.Columns["DNEFlag"].ColumnName = "TNA Flag";
            }

            // Round off DollarAmount value and AverageRiskScore,HighestRiskScore and Highest VersionRiskScore value before write to Excel.
            foreach (DataRow drRow in dtCloned.Rows)
            {
                drRow["Aggregate Risk Score"] = Math.Round(Convert.ToDouble(drRow["Aggregate Risk Score"]));
                drRow["Highest Risk Score"] = Math.Round(Convert.ToDouble(drRow["Highest Risk Score"]));
                drRow["Highest Version Risk Score"] = Math.Round(Convert.ToDouble(drRow["Highest Version Risk Score"]));
            }
            foreach (DataRow drRow in dtCloned.Rows)
            {
                if (drRow.ExtractFieldValue<string>("Amount", "0") != "0")
                {
                    drRow["Amount"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Amount"])));
                }
                else
                {
                    drRow["Amount"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Net List Price (USD)", "0") != "0")
                {
                    drRow["Net List Price (USD)"] = string.Format("{0:$,#,#}", Math.Round(Convert.ToDouble(drRow["Net List Price (USD)"])));
                }
                else
                {
                    drRow["Net List Price (USD)"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Net Sales Price (USD)", "0") != "0")
                {
                    drRow["Net Sales Price (USD)"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Net Sales Price (USD)"])));
                }
                else
                {
                    drRow["Net Sales Price (USD)"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Discount Amount", "0") != "0")
                {
                    drRow["Discount Amount"] = string.Format("{0:$,#,#.}", Math.Round(Convert.ToDouble(drRow["Discount Amount"])));
                }
                else
                {
                    drRow["Discount Amount"] = "$0";
                }
                if (drRow.ExtractFieldValue<string>("Discount Percent", "0") != "0")
                {
                    drRow["Discount Percent"] = string.Format("{0}%", Math.Round(Convert.ToDouble(drRow["Discount Percent"])));
                }
                else
                {
                    drRow["Discount Percent"] = "0%";
                }
                if (drRow.ExtractFieldValue<string>("Net Sales Price LC", "0") != "0")
                {
                    drRow["Net Sales Price LC"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Net Sales Price LC"])));
                }
                else
                {
                    drRow["Net Sales Price LC"] = "0";
                }
                if (drRow.ExtractFieldValue<string>("Net List Price LC", "0") != "0")
                {
                    drRow["Net List Price LC"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Net List Price LC"])));
                }
                else
                {
                    drRow["Net List Price LC"] = "0";
                }
                if (drRow.ExtractFieldValue<string>("Azure Final Net Sales Price", "") != "" && drRow.ExtractFieldValue<string>("Azure Final Net Sales Price", "0") != "0")
                {
                    drRow["Azure Final Net Sales Price"] = string.Format("{0:#,#.}", Math.Round(Convert.ToDouble(drRow["Azure Final Net Sales Price"])));
                }
                else
                {
                    drRow["Azure Final Net Sales Price"] = "0";
                }
                if (!unflaggedDeals)
                {
                    if (drRow.ExtractFieldValue<string>("Currency Exchange Rate (LC)", string.Empty) != string.Empty)
                    {
                        drRow["Currency Exchange Rate (LC)"] = string.Format("{0:0.##}", Convert.ToDecimal(drRow["Currency Exchange Rate (LC)"]));
                    }
                    else
                    {
                        drRow["Currency Exchange Rate (LC)"] = string.Empty;
                    }
                    drRow["First Flagged Date"] = !string.IsNullOrEmpty(drRow["First Flagged Date"].ToString()) ? ConvertToDateTime(drRow["First Flagged Date"]?.ToString()?.Trim()).ToShortDateString() : string.Empty;
                }
            }
            return dtCloned;
        }

        private DateTime ConvertToDateTime(string input)
        {
            DateTime.TryParse(input, out DateTime result);
            return result;
        }

    }
}
