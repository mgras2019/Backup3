namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    public class DealRequest
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The curren user
        /// </value>
        public CurrentUser CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the isunflaggde deal.
        /// </summary>
        /// <value>
        /// Is unflagged deal.
        /// </value>
        public bool IsUnflaggedDeal { get; set; }


        /// <summary>
        /// Gets or sets the filter input.
        /// </summary>
        /// <value>
        /// The filter input.
        /// </value>
        public FilterInput FilterInput { get; set; }

        /// <summary>
        /// Gets or sets the filter input type.
        /// </summary>
        /// <value>
        /// The filter type input.
        /// </value>
        public FilterTypeInput FilterTypeInput { get; set; }

    }
}
