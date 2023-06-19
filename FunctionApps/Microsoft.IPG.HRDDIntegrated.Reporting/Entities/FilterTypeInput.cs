namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    /// <summary>
    /// Filter Type Input
    /// </summary>
    public class FilterTypeInput
    {
        /// <summary>
        /// Gets or sets a value indicating whether [analytical flagged].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [analytical flagged]; otherwise, <c>false</c>.
        /// </value>
        public bool AnalyticalFlagged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [manual flagged].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manual flagged]; otherwise, <c>false</c>.
        /// </value>
        public bool ManualFlagged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [both flaggged].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [both flaggged]; otherwise, <c>false</c>.
        /// </value>
        public bool BothFlaggged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [assigned deals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [assigned deals]; otherwise, <c>false</c>.
        /// </value>
        public bool AssignedDeals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [un assigned deals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [un assigned deals]; otherwise, <c>false</c>.
        /// </value>
        public bool UnAssignedDeals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [my deals].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [my deals]; otherwise, <c>false</c>.
        /// </value>
        public bool IsMyDeals { get; set; }

        /// <summary>
        /// Gets or sets the state of the deal.
        /// </summary>
        /// <value>
        /// The state of the deal.
        /// </value>
        public string DealState { get; set; }
        /// <summary>
        /// Gets or sets the state of the deal.
        /// </summary>
        /// <value>
        /// The state of the deal.
        /// </value>
        public bool Trade { get; set; }
        /// <summary>
        /// Gets or sets the state of the deal.
        /// </summary>
        /// <value>
        /// The state of the deal.
        /// </value>
        public bool OnHold { get; set; }
        /// <summary>
        /// Gets or sets the state of the deal.
        /// </summary>
        /// <value>
        /// The state of the deal.
        /// </value>
        public bool OneVet { get; set; }
        /// <summary>
        /// Gets or sets the PostFinalFlagged.
        /// </summary>
        /// <value>
        /// ThePostFinalFlagged.
        /// </value>
        public bool PostFinalFlagged { get; set; }
        /// <summary>
        /// Gets or sets the PostFinalFlagged.
        /// </summary>
        /// <value>
        /// ThePostFinalFlagged.
        /// </value>
        public bool MyPostFinalFlagged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [DNE Quotes]. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [DNE Quotes]; otherwise, <c>false</c>.
        /// </value>
        public bool DNEQuotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [PAM Quotes]. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [PAM Quotes]; otherwise, <c>false</c>.
        /// </value>
        public bool PAMQuotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SOE Quotes]. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [SOE Quotes]; otherwise, <c>false</c>.
        /// </value>
        public bool SOEQuotes { get; set; }
    }
}