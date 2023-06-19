using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.ApiClient
{
    public class ApiSuccess
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApiSuccess"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the code received.
        /// </summary>
        /// <value>
        /// The code received.
        /// </value>
        public int CodeReceived { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}
