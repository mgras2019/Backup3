using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.ApiClient
{
    public class ApiResponse<TResult>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TResult Value { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        public ApiError Error { get; set; }
    }
}
