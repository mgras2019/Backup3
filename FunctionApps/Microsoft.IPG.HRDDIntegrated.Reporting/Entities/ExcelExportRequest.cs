namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    using System;

    /// <summary>
    /// Excel Export Request
    /// </summary>
    public class ExcelExportRequest
    {
        /// <summary>
        /// The Job Id
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// Include All Revisions
        /// </summary>
        public bool IncludeRevisions { get; set; }

        /// <summary>
        /// Deal Request
        /// </summary>
        public DealRequest DealRequest { get; set; }
    }
}
