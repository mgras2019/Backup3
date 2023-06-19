namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    using System.Collections.Generic;

    public class ApplicationEmailMessageRequest
    {
        /// <summary>
        /// To recipients
        /// </summary>
        public string ApplicationName { get { return "PRTHRDD"; } }
        /// <summary>
        /// CC recipients
        /// </summary>
        public List<MailMessageRequest> EmailMessages { get; set; }
    }
}