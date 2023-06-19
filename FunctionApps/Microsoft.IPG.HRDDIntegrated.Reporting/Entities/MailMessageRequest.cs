namespace Microsoft.IPG.HRDDIntegrated.Reporting.Entities
{
    using Microsoft.IPG.HRDDIntegrated.Reporting.Enums;
    using System.Collections.Generic;

    public class MailMessageRequest
    {
        /// <summary>
        /// To recipients
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// CC recipients
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// Subject of the mail
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Priority of the mail
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// Template name
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// TemplateData
        /// </summary>
        public string TemplateData { get; set; }
    }
}
